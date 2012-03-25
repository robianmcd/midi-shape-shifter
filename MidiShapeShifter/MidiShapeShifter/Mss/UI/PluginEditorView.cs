using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;

using Ninject;
using MidiShapeShifter.Ioc;

using MidiShapeShifter.CSharpUtil;
using MidiShapeShifter.Framework;
using MidiShapeShifter.Mss;
using MidiShapeShifter.Mss.Mapping;
using MidiShapeShifter.Mss.Mapping.MssMsgRangeEntryMetadataTypes;
using MidiShapeShifter.Mss.Generator;
using MidiShapeShifter.Mss.Relays;
using MidiShapeShifter.Mss.MssMsgInfoTypes;
using MidiShapeShifter.Mss.Programs;
using MidiShapeShifter.Mss.Evaluation;


using LBSoft.IndustrialCtrls.Knobs;

using ZedGraph;

//Winforms and ZedGraphs both define Label
using Label = System.Windows.Forms.Label;

namespace MidiShapeShifter.Mss.UI
{
    public partial class PluginEditorView : UserControl
    {
        public const int NUM_VARIABLE_PARAMS = 4;
        public const int NUM_PRESET_PARAMS = 4;

        public const int NUM_GRAPH_POINTS = 128;

        public const int NUM_DECIMALS_IN_CONTROL_POINT = 3;

        protected const string GRAPH_DEFAULT_OUTPUT_LABEL = "Output";
        protected const string GRAPH_CONTROL_POINTS_LABEL = "ControlPoints";
        protected const string EQUATION_CURVE_LABEL_PREFIX = "Curve";

        protected string graphOutputLabelText = GRAPH_DEFAULT_OUTPUT_LABEL;

        public TwoWayDictionary<MssParameterID, LBKnob> ParameterValueKnobControlDict = new TwoWayDictionary<MssParameterID, LBKnob>();
        public TwoWayDictionary<MssParameterID, Label> ParameterValueLabelControlDict = new TwoWayDictionary<MssParameterID, Label>();
        public TwoWayDictionary<MssParameterID, Label> ParameterNameControlDict = new TwoWayDictionary<MssParameterID, Label>();

        protected IEvaluator evaluator;

        protected MssParameters mssParameters;
        protected MappingManager mappingMgr;
        protected GeneratorMappingManager genMappingMgr;
        protected IDryMssEventOutputPort dryMssEventOutputPort;

        protected Factory_MssMsgRangeEntryMetadata msgMetadataFactory;
        protected IFactory_MssMsgInfo msgInfoFactory;

        protected MssProgramMgr programMgr;

        protected SerializablePluginEditorInfo persistantInfo;

        protected List<MssMsgDataField> DataFieldsInGraphInputCombo;

        protected bool ignoreEquationTextBoxChangeHandlers;
        protected bool ignoreInputTypeComboSelectionChangeHandlers;

        public IMappingEntry ActiveGraphableEntry 
        {
            get
            {
                if (this.persistantInfo.activeGraphableEntryIndex < 0)
                {
                    return null;
                }
                else
                {
                    IMappingEntry activeEntry;

                    if (this.persistantInfo.activeGraphableEntryType == GraphableEntryType.Mapping) 
                    {
                        activeEntry = this.mappingMgr.GetMappingEntry(
                                this.persistantInfo.activeGraphableEntryIndex);
                    }
                    else if (this.persistantInfo.activeGraphableEntryType == GraphableEntryType.Generator)
                    {
                        activeEntry = this.genMappingMgr.GetGenMappingEntryByIndex(
                                this.persistantInfo.activeGraphableEntryIndex);
                    }
                    else
                    {
                        //Unknown MappingType
                        Debug.Assert(false);
                        return null;
                    }

                    return activeEntry;
                }
            }
        }

        public PluginEditorView()
        {
            InitializeComponent();
            PopulateControlDictionaries();

            this.DataFieldsInGraphInputCombo = new List<MssMsgDataField>();
            this.evaluator = IocMgr.Kernel.Get<IEvaluator>();
            this.msgMetadataFactory = new Factory_MssMsgRangeEntryMetadata();
            this.msgInfoFactory = new Factory_MssMsgInfo();

            this.ignoreEquationTextBoxChangeHandlers = false;
            this.ignoreInputTypeComboSelectionChangeHandlers = false;
        }

        public void Init(MssParameters mssParameters, 
                         MappingManager mappingMgr, 
                         GeneratorMappingManager genMappingMgr,
                         MssProgramMgr programMgr,
                         IDryMssEventOutputPort dryMssEventOutputPort,
                         SerializablePluginEditorInfo serializablePluginEditorInfo)
        {
            InitiaizeGraph();

            this.mssParameters = mssParameters;
            this.mappingMgr = mappingMgr;
            this.genMappingMgr = genMappingMgr;
            this.programMgr = programMgr;
            this.dryMssEventOutputPort = dryMssEventOutputPort;
            this.persistantInfo = serializablePluginEditorInfo;

            this.mssParameters.ParameterValueChanged += new ParameterValueChangedEventHandler(MssParameters_ValueChanged);
            this.mssParameters.ParameterNameChanged += new ParameterNameChangedEventHandler(MssParameters_NameChanged);
            this.mssParameters.ParameterMinValueChanged += new ParameterMinValueChangedEventHandler(MssParameters_MinValueChanged);
            this.mssParameters.ParameterMaxValueChanged += new ParameterMaxValueChangedEventHandler(MssParameters_MaxValueChanged);

            //Set parameters from MssParameters
            foreach(MssParameterID paramId in Enum.GetValues(typeof(MssParameterID)))
            {
                UpdateInfoForParameter(paramId);                
            }

            this.msgMetadataFactory.Init(genMappingMgr);
            this.msgInfoFactory.Init(genMappingMgr);

            RefreshMappingListView();
            RefreshGeneratorListView();

            repopulateProgramList();

            //TODO: once the TransformController is done see if this call is really nessary. The
            //Only time this won't get called by repopulateProgramList() is when a blank program
            //is loaded.
            OnActiveGraphableEntryChanged();
        }

        protected void OnDispose()
        {
            this.mssParameters.ParameterValueChanged -= new ParameterValueChangedEventHandler(MssParameters_ValueChanged);
            this.mssParameters.ParameterNameChanged -= new ParameterNameChangedEventHandler(MssParameters_NameChanged);
            this.mssParameters.ParameterMinValueChanged -= new ParameterMinValueChangedEventHandler(MssParameters_MinValueChanged);
            this.mssParameters.ParameterMaxValueChanged -= new ParameterMaxValueChangedEventHandler(MssParameters_MaxValueChanged);
        }

        protected void PopulateControlDictionaries()
        {
            ParameterValueKnobControlDict.Add(MssParameterID.VariableA, this.variableAKnob);
            ParameterValueKnobControlDict.Add(MssParameterID.VariableB, this.variableBKnob);
            ParameterValueKnobControlDict.Add(MssParameterID.VariableC, this.variableCKnob);
            ParameterValueKnobControlDict.Add(MssParameterID.VariableD, this.variableDKnob);
            ParameterValueKnobControlDict.Add(MssParameterID.Preset1, this.presetParam1Knob);
            ParameterValueKnobControlDict.Add(MssParameterID.Preset2, this.presetParam2Knob);
            ParameterValueKnobControlDict.Add(MssParameterID.Preset3, this.presetParam3Knob);
            ParameterValueKnobControlDict.Add(MssParameterID.Preset4, this.presetParam4Knob);

            ParameterValueLabelControlDict.Add(MssParameterID.VariableA, this.variableAValue);
            ParameterValueLabelControlDict.Add(MssParameterID.VariableB, this.variableBValue);
            ParameterValueLabelControlDict.Add(MssParameterID.VariableC, this.variableCValue);
            ParameterValueLabelControlDict.Add(MssParameterID.VariableD, this.variableDValue);
            ParameterValueLabelControlDict.Add(MssParameterID.Preset1, this.presetParam1Value);
            ParameterValueLabelControlDict.Add(MssParameterID.Preset2, this.presetParam2Value);
            ParameterValueLabelControlDict.Add(MssParameterID.Preset3, this.presetParam3Value);
            ParameterValueLabelControlDict.Add(MssParameterID.Preset4, this.presetParam4Value);

            ParameterNameControlDict.Add(MssParameterID.VariableA, this.variableATitle);
            ParameterNameControlDict.Add(MssParameterID.VariableB, this.variableBTitle);
            ParameterNameControlDict.Add(MssParameterID.VariableC, this.variableCTitle);
            ParameterNameControlDict.Add(MssParameterID.VariableD, this.variableDTitle);
            ParameterNameControlDict.Add(MssParameterID.Preset1, this.presetParam1Title);
            ParameterNameControlDict.Add(MssParameterID.Preset2, this.presetParam2Title);
            ParameterNameControlDict.Add(MssParameterID.Preset3, this.presetParam3Title);
            ParameterNameControlDict.Add(MssParameterID.Preset4, this.presetParam4Title);
        }

        protected void UpdateGraphableEntryButtonsEnabledStatus()
        {
            bool EnableMappingButtons;
            bool EnableGeneratorButtons;

            if (this.persistantInfo.activeGraphableEntryIndex < 0) 
            {
                EnableMappingButtons = false;
                EnableGeneratorButtons = false;
            }
            else if (this.persistantInfo.activeGraphableEntryType == GraphableEntryType.Mapping)
            {
                EnableMappingButtons = true;
                EnableGeneratorButtons = false;
            }
            else if (this.persistantInfo.activeGraphableEntryType == GraphableEntryType.Generator)
            {
                EnableMappingButtons = false;
                EnableGeneratorButtons = true;
            }
            else
            {
                //Unexpected GraphableEntryType
                Debug.Assert(false);
                return;
            }

            this.deleteMappingBtn.Enabled = EnableMappingButtons;
            this.editMappingBtn.Enabled = EnableMappingButtons;
            this.moveMappingUpBtn.Enabled = EnableMappingButtons && 
                    this.persistantInfo.activeGraphableEntryIndex > 0;
            this.moveMappingDownBtn.Enabled = EnableMappingButtons && 
                    this.persistantInfo.activeGraphableEntryIndex < this.mappingMgr.GetNumEntries() - 1;

            this.deleteGeneratorBtn.Enabled = EnableGeneratorButtons;
            this.editGeneratorBtn.Enabled = EnableGeneratorButtons;
        }

        protected void RefreshMappingListView()
        {
            this.mappingListView.Items.Clear();

            for (int i = 0; i < this.mappingMgr.GetNumEntries(); i++)
            {
                this.mappingListView.Items.Add(this.mappingMgr.GetListViewRow(i));
            }

            if (this.persistantInfo.activeGraphableEntryType == GraphableEntryType.Mapping &&
                this.persistantInfo.activeGraphableEntryIndex > -1)
            {
                this.mappingListView.Items[this.persistantInfo.activeGraphableEntryIndex].Selected = true;
            }
        }

        protected void RefreshGeneratorListView()
        {
            this.generatorListView.Items.Clear();

            for (int i = 0; i < this.genMappingMgr.GetNumEntries(); i++)
            {
                this.generatorListView.Items.Add(this.genMappingMgr.GetListViewRow(i));
            }

            if (this.persistantInfo.activeGraphableEntryType == GraphableEntryType.Generator && 
                this.persistantInfo.activeGraphableEntryIndex > -1)
            {
                this.generatorListView.Items[this.persistantInfo.activeGraphableEntryIndex].Selected = true;
            }
        }

        private void lbKnob_KnobChangeValue(object sender, LBKnobEventArgs e) {
            LBKnob knob = (LBKnob)sender;
            MssParameterID paramId;
            ParameterValueKnobControlDict.TryGetLeftByRight(out paramId, knob);


            Label parameterValueDisplay;
            if (ParameterValueLabelControlDict.TryGetRightByLeft(paramId, out parameterValueDisplay) == true)
            {
                string valueString = FormatRawParameterValue((double)knob.Value);
                if (valueString != parameterValueDisplay.Text)
                {
                    parameterValueDisplay.Text = valueString;
                }
            }
            

            if (this.mssParameters.GetParameterValue(paramId) != knob.Value)
            {
                this.mssParameters.SetParameterValue(paramId, knob.Value);
            }
        }

        protected string FormatRawParameterValue(double value)
        {
            return System.Math.Round(value, 2).ToString();
        }

        protected void OnActiveGraphableEntryChanged()
        {
            UpdateCurveShapeControls();
            UpdateGraphableEntryButtonsEnabledStatus();
        }

        protected void UpdateCurveShapeControls()
        {
            bool activeEntryExists = (this.ActiveGraphableEntry != null);

            curvePresetCombo.Enabled = activeEntryExists;
            presetParam1Knob.Enabled = activeEntryExists;
            presetParam2Knob.Enabled = activeEntryExists;
            presetParam3Knob.Enabled = activeEntryExists;
            presetParam4Knob.Enabled = activeEntryExists;

            UpdateGraphInputCombo();

            if (activeEntryExists)
            {
                CurveShapeInfo curveInfo = this.ActiveGraphableEntry.CurveShapeInfo;

            //Update preset controls
                this.curvePresetCombo.SelectedIndex = ActiveGraphableEntry.CurveShapeInfo.PresetIndex;

            //Update Graph controls (the equation curve is not updated until the end)
                SetGraphOutputLabelText(this.ActiveGraphableEntry.OutMssMsgRange.MsgInfo.Data3Name);

            //Update the buttons under the graph
                if (curveInfo.SelectedEquationType == EquationType.Curve)
                {
                    this.prevEquationBtn.Enabled = (curveInfo.SelectedEquationIndex > 0);
                    this.nextEquationBtn.Enabled = (curveInfo.SelectedEquationIndex <= curveInfo.PointEquations.Count - 1);
                }
                else if (curveInfo.SelectedEquationType == EquationType.Point)
                {
                    this.prevEquationBtn.Enabled = true;
                    this.nextEquationBtn.Enabled = true;
                }
                else
                {
                    //Unknown equation type
                    Debug.Assert(false);
                }
                this.resetGraphBtn.Enabled = true;

            //Update equations controls
                EnableAppropriateEquationControls(curveInfo.SelectedEquationType);
                if (curveInfo.SelectedEquationType == EquationType.Curve)
                {
                    ignoreEquationTextBoxChangeHandlers = true;
                    this.curveEquationTextBox.Text = curveInfo.CurveEquations[curveInfo.SelectedEquationIndex];
                    ignoreEquationTextBoxChangeHandlers = false;
                }
                else if (curveInfo.SelectedEquationType == EquationType.Point)
                {
                    ignoreEquationTextBoxChangeHandlers = true;
                    this.pointXEquationTextBox.Text = curveInfo.PointEquations[curveInfo.SelectedEquationIndex].X;
                    this.pointYEquationTextBox.Text = curveInfo.PointEquations[curveInfo.SelectedEquationIndex].Y;
                    ignoreEquationTextBoxChangeHandlers = false;
                }
                else
                {
                    //Unknown equation type
                    Debug.Assert(false);
                }

            //Update Parameter controls
                this.presetParam1Knob.Value = (float)ActiveGraphableEntry.CurveShapeInfo.PresetParamValues[0];
                this.presetParam2Knob.Value = (float)ActiveGraphableEntry.CurveShapeInfo.PresetParamValues[1];
                this.presetParam3Knob.Value = (float)ActiveGraphableEntry.CurveShapeInfo.PresetParamValues[2];
                this.presetParam4Knob.Value = (float)ActiveGraphableEntry.CurveShapeInfo.PresetParamValues[3];
                this.presetParam1Value.Text = FormatRawParameterValue(ActiveGraphableEntry.CurveShapeInfo.PresetParamValues[0]);
                this.presetParam2Value.Text = FormatRawParameterValue(ActiveGraphableEntry.CurveShapeInfo.PresetParamValues[1]);
                this.presetParam3Value.Text = FormatRawParameterValue(ActiveGraphableEntry.CurveShapeInfo.PresetParamValues[2]);
                this.presetParam4Value.Text = FormatRawParameterValue(ActiveGraphableEntry.CurveShapeInfo.PresetParamValues[3]);
            }
            else //Active mapping does not exsist
            {
            //Update preset controls
                this.curvePresetCombo.SelectedIndex = -1;

            //Update Graph controls (the equation curve is not updated until the end)
                SetGraphOutputLabelText(GRAPH_DEFAULT_OUTPUT_LABEL);

            //Update the buttons under the graph
                this.prevEquationBtn.Enabled = false;
                this.nextEquationBtn.Enabled = false;
                this.resetGraphBtn.Enabled = false;

            //Update equations controls
                EnableAppropriateEquationControls(EquationType.Curve);
                this.curveEquationTextBox.Text = "";
                this.curveEquationTextBox.Enabled = false;

            }

            UpdateEquationCurve();
        }

        protected void EnableAppropriateEquationControls(EquationType equationType)
        {
            bool isCurveEquation;

            if (equationType == EquationType.Curve)
            {
                isCurveEquation = true;
            }
            else if (equationType == EquationType.Point)
            {
                isCurveEquation = false;
            }
            else
            {
                //Unknown equation type
                Debug.Assert(false);
                isCurveEquation = true;
            }

            this.curveEquationLabel.Visible = isCurveEquation;
            this.curveEquationTextBox.Visible = isCurveEquation;
            this.curveEquationTextBox.Enabled = isCurveEquation;

            this.pointXEquationLabel.Visible = ! isCurveEquation;
            this.pointXEquationTextBox.Visible = ! isCurveEquation;
            this.pointYEquationLabel.Visible = ! isCurveEquation;
            this.pointYEquationTextBox.Visible = ! isCurveEquation;
            this.pointXEquationTextBox.Enabled = !isCurveEquation;
            this.pointYEquationTextBox.Enabled = !isCurveEquation;
        }

        protected void SetGraphOutputLabelText(string outputText)
        {
            this.graphOutputLabelText = outputText;

            Graphics outputTypeGraphics = this.graphOutputTypeImg.CreateGraphics();
            DrawGraphOutputLabel(outputTypeGraphics, outputText);
            outputTypeGraphics.Dispose();
        }

        protected void UpdateGraphInputCombo()
        {
            this.DataFieldsInGraphInputCombo.Clear();
            this.graphInputTypeCombo.Items.Clear();

            if (this.ActiveGraphableEntry != null)
            {
                MssMsgInfo inputInfo = this.ActiveGraphableEntry.InMssMsgRange.MsgInfo;
                MssMsgDataField[] dataFieldArray = (MssMsgDataField[])Enum.GetValues(typeof(MssMsgDataField));

                foreach(MssMsgDataField dataField in dataFieldArray)
                {
                    string dataFieldName = inputInfo.GetDataFieldName(dataField);
                    if (dataFieldName != MssMsgInfo.DATA_NAME_UNUSED)
                    {
                        this.DataFieldsInGraphInputCombo.Add(dataField);
                        this.graphInputTypeCombo.Items.Add(dataFieldName);

                        if (this.ActiveGraphableEntry.CurveShapeInfo.PrimaryInputSource == dataField)
                        {
                            ignoreInputTypeComboSelectionChangeHandlers = true;
                            //Select the item that was just added.
                            this.graphInputTypeCombo.SelectedIndex = 
                                this.graphInputTypeCombo.Items.Count - 1;
                            ignoreInputTypeComboSelectionChangeHandlers = false;
                        }
                    }
                }
            }
        }

        protected void DrawGraphOutputLabel(System.Drawing.Graphics graphics, string text)
        {
            graphics.Clear(Color.White);

            System.Drawing.Font drawFont = new System.Drawing.Font(
                    this.graphInputLable.Font.FontFamily, this.graphInputLable.Font.Size);
            System.Drawing.SolidBrush drawBrush = new 
                System.Drawing.SolidBrush(System.Drawing.Color.Black);
            float x = this.graphOutputTypeImg.Width / 2;
            float y = this.graphOutputTypeImg.Height / 2;

            StringFormat drawFormat = new StringFormat();
            drawFormat.Alignment = StringAlignment.Center;
            drawFormat.LineAlignment = StringAlignment.Center;
            drawFormat.FormatFlags = StringFormatFlags.DirectionVertical;

            graphics.RotateTransform(180);
            //Multiply X and Y by -1 becasue we rotated by 180 degrees.
            graphics.DrawString(text, drawFont, drawBrush, x * -1, y * -1, drawFormat);
            drawFont.Dispose();
            drawBrush.Dispose();
        }

        protected bool IgnoreGraphableEntrySelectionChangedHandler = false;

        protected void GraphableEntrySelectionChanged(
            ListView modifiedListView, 
            GraphableEntryType mappingType,
            ListViewItemSelectionChangedEventArgs eventArgs)
        {
            if (IgnoreGraphableEntrySelectionChangedHandler)
            {
                return;
            }

            if (eventArgs.IsSelected == false)
            {
                IgnoreGraphableEntrySelectionChangedHandler = true;
                eventArgs.Item.Selected = true;
                IgnoreGraphableEntrySelectionChangedHandler = false;

                return;
            }
            else
            {
                IgnoreGraphableEntrySelectionChangedHandler = true;

                for (int i = 0; i < this.mappingListView.Items.Count; i++)
                {
                    if (this.mappingListView.Items[i].Selected == true)
                    {
                        this.mappingListView.Items[i].Selected = false;
                    }
                }

                for (int i = 0; i < this.generatorListView.Items.Count; i++)
                {
                    if (this.generatorListView.Items[i].Selected == true)
                    {
                        this.generatorListView.Items[i].Selected = false;
                    }
                }

                eventArgs.Item.Selected = true;
                IgnoreGraphableEntrySelectionChangedHandler = false;
            }

            this.persistantInfo.activeGraphableEntryType = mappingType;
            this.persistantInfo.activeGraphableEntryIndex = modifiedListView.SelectedItems[0].Index;
            OnActiveGraphableEntryChanged();
        }

        protected void UpdateInfoForParameter(MssParameterID paramID)
        {
            MssParameters_NameChanged(paramID, this.mssParameters.GetParameterName(paramID));
            MssParameters_ValueChanged(paramID, this.mssParameters.GetParameterValue(paramID));
            MssParameters_MaxValueChanged(paramID, this.mssParameters.GetParameterMaxValue(paramID));
            MssParameters_MinValueChanged(paramID, this.mssParameters.GetParameterMinValue(paramID));
        }

        private void addMappingBtn_Click(object sender, System.EventArgs e)
        {
            MappingDlg mapDlg = new MappingDlg();
            mapDlg.Init(new MappingEntry(), false, this.msgMetadataFactory, this.msgInfoFactory);

            if (mapDlg.ShowDialog(this) == DialogResult.OK)
            {
                this.mappingMgr.AddMappingEntry(mapDlg.mappingEntry);

                int newestEntryIndex = this.mappingMgr.GetNumEntries() - 1;

                this.persistantInfo.activeGraphableEntryType = GraphableEntryType.Mapping;
                this.persistantInfo.activeGraphableEntryIndex = newestEntryIndex;

                this.mappingListView.Items.Add(this.mappingMgr.GetListViewRow(newestEntryIndex));
                this.mappingListView.Items[newestEntryIndex].Selected = true;
            }
        }

        private void editMappingBtn_Click(object sender, System.EventArgs e)
        {
            if (ActiveGraphableEntry == null || 
                this.persistantInfo.activeGraphableEntryType != GraphableEntryType.Mapping)
            {
                //The edit button should be disabled if there is no ActiveGraphableEntry or if the active 
                //mapping is not in the mapping list view.
                Debug.Assert(false);
                return;
            }

            MappingDlg mapDlg = new MappingDlg();
            mapDlg.Init(ActiveGraphableEntry, true, this.msgMetadataFactory, this.msgInfoFactory);
            

            if (mapDlg.ShowDialog(this) == DialogResult.OK)
            {
                RefreshMappingListView();
                //The equation curve needs to be updated incase the equation uses data1 or data2 
                //and the input range for these has changed.
                UpdateEquationCurve();
            }
        }

        private void deleteMappingBtn_Click(object sender, EventArgs e)
        {
            if (ActiveGraphableEntry == null ||
                this.persistantInfo.activeGraphableEntryType != GraphableEntryType.Mapping)
            {
                //The delete button should be disabled if there is no ActiveGraphableEntry or if the 
                //active mapping is not in the mapping list view.
                Debug.Assert(false);
                return;
            }

            this.mappingMgr.RemoveMappingEntry(this.persistantInfo.activeGraphableEntryIndex);
            this.mappingListView.Items[this.persistantInfo.activeGraphableEntryIndex].Remove();

            if (this.mappingListView.Items.Count > this.persistantInfo.activeGraphableEntryIndex)
            {
                //don't need to call ActiveGraphableEntryChanged() because selecting an item 
                //will trigger it.
                this.mappingListView.Items[this.persistantInfo.activeGraphableEntryIndex].Selected = true;
            }
            else
            {
                this.persistantInfo.activeGraphableEntryIndex = -1;
                OnActiveGraphableEntryChanged();
            }
        }

        private void mappingListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            GraphableEntrySelectionChanged((ListView)sender, GraphableEntryType.Mapping, e);
        }

        private void addGeneratorBtn_Click(object sender, System.EventArgs e)
        {
            GeneratorDlg genDlg = new GeneratorDlg();
            GenEntryConfigInfo genInfo = new GenEntryConfigInfo();
            genInfo.InitWithDefaultValues();
            genDlg.Init(genInfo);

            if (genDlg.ShowDialog() == DialogResult.OK)
            {
                //Creates a new mapping entry, adds it the the generator mapping manager and sets
                //it as the active mapping
                this.genMappingMgr.CreateAndAddEntryFromGenInfo(genDlg.GenInfoResult);
                this.persistantInfo.activeGraphableEntryType = GraphableEntryType.Generator;
                this.persistantInfo.activeGraphableEntryIndex = this.genMappingMgr.GetNumEntries() - 1;

                this.generatorListView.Items.Add(
                    this.genMappingMgr.GetListViewRow(this.persistantInfo.activeGraphableEntryIndex));
                this.generatorListView.Items[this.persistantInfo.activeGraphableEntryIndex].Selected = true;
            }
        }

        private void editGeneratorBtn_Click(object sender, EventArgs e)
        {
            if (ActiveGraphableEntry == null || 
                this.persistantInfo.activeGraphableEntryType != GraphableEntryType.Generator)
            {
                //The edit button should be disabled if there is no ActiveGraphableEntry or if the active 
                //mapping is not in the generator list view.
                Debug.Assert(false);
                return;
            }
            IGeneratorMappingEntry activeGenMapping = 
                this.genMappingMgr.GetGenMappingEntryByIndex(this.persistantInfo.activeGraphableEntryIndex);

            GeneratorDlg genDlg = new GeneratorDlg();
            genDlg.Init(activeGenMapping.GenConfigInfo);


            if (genDlg.ShowDialog(this) == DialogResult.OK)
            {
                this.genMappingMgr.UpdateEntryWithNewGenInfo(genDlg.GenInfoResult);
                RefreshGeneratorListView();
            }
        }

        private void MssParameters_NameChanged(MssParameterID paramId, string name)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<MssParameterID, string>(MssParameters_NameChanged), paramId, name);
                return;
            }

            Label paramNameLabel;
            if (ParameterNameControlDict.TryGetRightByLeft(paramId, out paramNameLabel) == true)
            {
                if (paramNameLabel.Text != name)
                {
                    paramNameLabel.Text = name;
                }
            }
        }

        private void MssParameters_ValueChanged(MssParameterID paramId, double value)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<MssParameterID, double>(MssParameters_ValueChanged), paramId, value);
                return;
            }

            LBKnob knob;
            if (ParameterValueKnobControlDict.TryGetRightByLeft(paramId, out knob) == true)
            {
                if (knob.Value != value)
                {
                    knob.Value = (float)value;
                }
            }

            UpdateEquationCurve();
        }

        private void MssParameters_MinValueChanged(MssParameterID paramId, int minValue)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<MssParameterID, int>(MssParameters_MinValueChanged), paramId, minValue);
                return;
            }

            LBKnob knob;
            if (ParameterValueKnobControlDict.TryGetRightByLeft(paramId, out knob) == true)
            {
                knob.MinValue = minValue;
            }
        }

        private void MssParameters_MaxValueChanged(MssParameterID paramId, int maxValue)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<MssParameterID, int>(MssParameters_MaxValueChanged), paramId, maxValue);
                return;
            }

            LBKnob knob;
            if (ParameterValueKnobControlDict.TryGetRightByLeft(paramId, out knob) == true)
            {
                knob.MaxValue = maxValue;
            }
        }

        private void curveEquationTextBox_TextChanged(object sender, System.EventArgs e)
        {
            if (ignoreEquationTextBoxChangeHandlers == false && this.ActiveGraphableEntry != null)
            {
                CurveShapeInfo curveInfo = this.ActiveGraphableEntry.CurveShapeInfo;
                Debug.Assert(curveInfo.SelectedEquationType == EquationType.Curve);

                string expressionString = this.curveEquationTextBox.Text;
                int curCurve = curveInfo.SelectedEquationIndex;
                curveInfo.CurveEquations[curCurve] = expressionString;

                UpdateEquationCurve();
            }
        }

        private void pointEquationTextBox_TextChanged(object sender, System.EventArgs e)
        {
            if (ignoreEquationTextBoxChangeHandlers == false && this.ActiveGraphableEntry != null)
            {
                CurveShapeInfo curveInfo = this.ActiveGraphableEntry.CurveShapeInfo;
                Debug.Assert(curveInfo.SelectedEquationType == EquationType.Point);

                string xExpressionString = this.pointXEquationTextBox.Text;
                string yExpressionString = this.pointYEquationTextBox.Text;
                int curCurve = curveInfo.SelectedEquationIndex;
                curveInfo.PointEquations[curCurve].X = xExpressionString;
                curveInfo.PointEquations[curCurve].Y = yExpressionString;

                UpdateEquationCurve();
            }
        }

        protected void UpdateEquationCurve()
        {
            if (this.ActiveGraphableEntry != null)
            {
                CurveShapeInfo curveInfo = this.ActiveGraphableEntry.CurveShapeInfo;

                List<XyPoint<double>> pointList;
                double[] curveYValues;

                bool expressionIsValid = this.evaluator.SampleExpressionWithDefaultInputValues(
                        NUM_GRAPH_POINTS,
                        this.mssParameters,
                        this.ActiveGraphableEntry,
                        out pointList,
                        out curveYValues);
                if (expressionIsValid)
                {
                    GraphPane graphPane = this.mainGraphControl.GraphPane;


                    LineItem pointsCurve = (LineItem)graphPane.CurveList.Find(
                            curveItem => curveItem.Label.Text == GRAPH_CONTROL_POINTS_LABEL);
                    if (pointsCurve == null)
                    {
                        pointsCurve = EqGraphConfig.CreadControlPointsCurve(GRAPH_CONTROL_POINTS_LABEL);
                        graphPane.CurveList.Add(pointsCurve);
                    }
                    IPointListEdit pointsCurveEdit = (IPointListEdit)pointsCurve.Points;

                    //Update modified points and add new points
                    for(int i = 0; i < pointList.Count; i++)
                    {
                        XyPoint<double> curPoint = pointList[i];

                        if (i <= pointsCurve.Points.Count - 1)
                        {
                            pointsCurve.Points[i].X = curPoint.X;
                            pointsCurve.Points[i].Y = curPoint.Y;
                            pointsCurve.Points[i].ColorValue = 0;
                        }
                        else
                        {
                            pointsCurveEdit.Add(curPoint.X, curPoint.Y);
                        }
                    }
                    //Remove points. Itterate backwards so removing a point does not affect the 
                    //index of the next point.
                    for (int i = pointsCurve.Points.Count - 1; i >= pointList.Count; i--)
                    {
                        pointsCurveEdit.RemoveAt(i);
                    }

                    if(curveInfo.SelectedEquationType == EquationType.Point)
                    {
                        pointsCurve.Points[curveInfo.SelectedEquationIndex].ColorValue = 1;
                    }

                    bool highlightCurve = false;

                    if (curveInfo.SelectedEquationType == EquationType.Curve &&
                                curveInfo.SelectedEquationIndex == 0)
                    {
                        highlightCurve = true;
                    }
                    LineItem curCurve =
                        EqGraphConfig.CreateEqCurve(EQUATION_CURVE_LABEL_PREFIX + "-0", highlightCurve);
                    IPointListEdit curCurveEdit = (IPointListEdit)curCurve.Points;


                    RemoveEquationCurvesFromGraph();
                    int nextPointIndex = 0;

                    for (int i = 0; i < curveYValues.Length; i++)
                    {
                        double curXVal = (double)i / ((double)NUM_GRAPH_POINTS - 1.0);

                        bool startNewCurve = false;
                        bool skipCurXVal = false;
                        //If Y values are outside of the range 0 to 1 then they will not be mapped
                        //So they should not be shown on the graph.
                        //Note: values can be NaN
                        if (!(curveYValues[i] >= 0 && curveYValues[i] <= 1))
                        {
                            startNewCurve = true;
                            skipCurXVal = true;
                        }

                        if (nextPointIndex <= pointList.Count - 1 &&  curXVal > pointList[nextPointIndex].X)
                        {
                            startNewCurve = true;
                            nextPointIndex++;
                        }

                        if (startNewCurve == true)
                        {
                            PointPair nextStartingPoint = null;

                            if (curCurve.Points.Count > 0)
                            {
                                graphPane.CurveList.Add(curCurve);

                                if (skipCurXVal == false)
                                {
                                    nextStartingPoint = curCurve[curCurve.Points.Count - 1];
                                }
                            }

                            if (curveInfo.SelectedEquationType == EquationType.Curve &&
                                curveInfo.SelectedEquationIndex == nextPointIndex)
                            {
                                highlightCurve = true;
                            }
                            else
                            {
                                highlightCurve = false;
                            }
                            curCurve = EqGraphConfig.CreateEqCurve(
                                EQUATION_CURVE_LABEL_PREFIX + "-" + curXVal.ToString(),
                                highlightCurve);
                            curCurveEdit = (IPointListEdit)curCurve.Points;

                            if (nextStartingPoint != null)
                            {
                                curCurve.AddPoint(nextStartingPoint);
                            }
                        }

                        if (skipCurXVal == true)
                        {
                            continue;
                        }

                        curCurveEdit.Add(curXVal, curveYValues[i]);
                    }

                    if (curCurve.Points.Count > 0)
                    {
                        graphPane.CurveList.Add(curCurve);
                    }

                    this.mainGraphControl.Invalidate();
                }
                else
                {
                    //TODO: show that the line is not using the current formula.
                }
            }
            else //this.ActiveGraphableEntry is null
            {
                this.mainGraphControl.GraphPane.CurveList.Clear();
                this.mainGraphControl.Invalidate();
            }
        }

        protected void RemoveEquationCurvesFromGraph()
        {
            GraphPane pane = this.mainGraphControl.GraphPane;
            //Itterate through this list backwards so that removing an element wont affect the 
            //index of the next element.
            for(int i = pane.CurveList.Count - 1; i >= 0; i--)
            {
                CurveItem curve = pane.CurveList[i];

                if (curve.Label.Text.StartsWith(EQUATION_CURVE_LABEL_PREFIX))
                {
                    pane.CurveList.Remove(curve);
                }
            }
        }

        protected void InitiaizeGraph()
        {
            EqGraphConfig.ConfigureEqGraph(this.mainGraphControl);
            this.mainGraphControl.ContextMenuBuilder +=
                new ZedGraphControl.ContextMenuBuilderEventHandler(ZedGraphContextMenuBuilder);
        }

        protected void repopulateProgramList()
        {
            this.programList.Text = 
                CustomStringUtil.CreateStringWithMaxWidth(this.programMgr.ActiveProgram.Name, 
                                                          this.programList.Width - 10,
                                                          this.programList.Font);
            this.programList.DropDownItems.Clear();

            populateDropDownItemsFromProgramTreeNode(this.programList.DropDownItems, 
                                                     this.programMgr.ProgramTree);
        }

        protected void populateDropDownItemsFromProgramTreeNode(ToolStripItemCollection dropDownItems, 
                                                                MssProgramTreeNode programTreeNode)
        {
            foreach (MssProgramInfo program in programTreeNode.ChildProgramsList)
            {
                var programMenuItem = new ToolStripMenuItem(program.Name);
                programMenuItem.Tag = program;
                programMenuItem.Click += new EventHandler(onProgramClicked);
                dropDownItems.Add(programMenuItem);
            }

            foreach (MssProgramTreeNode childNode in programTreeNode.ChildTreeNodesList)
            {
                var programFolder = new ToolStripMenuItem(childNode.NodeName);
                dropDownItems.Add(programFolder);
                populateDropDownItemsFromProgramTreeNode(programFolder.DropDownItems, childNode);
            }
        }

        private void onProgramClicked(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
            this.programMgr.ActivateProgramByMssProgramInfo((MssProgramInfo) menuItem.Tag);
        }

        private void generatorListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            GraphableEntrySelectionChanged((ListView)sender, GraphableEntryType.Generator, e);
        }

        private void deleteBtn_EnabledChanged(object sender, EventArgs e)
        {
            Button delBtn = (Button)sender;

            if (delBtn.Enabled == true)
            {
                delBtn.BackgroundImage = 
                    global::MidiShapeShifter.Properties.Resources.imgDeleteBlue;
            }
            else
            {
                delBtn.BackgroundImage =
                    global::MidiShapeShifter.Properties.Resources.imgDeleteGrey;
            }
        }

        private void editBtn_EnabledChanged(object sender, EventArgs e)
        {
            Button editBtn = (Button)sender;

            if (editBtn.Enabled == true)
            {
                editBtn.BackgroundImage =
                    global::MidiShapeShifter.Properties.Resources.imgEditBlue;
            }
            else
            {
                editBtn.BackgroundImage =
                    global::MidiShapeShifter.Properties.Resources.imgEditGrey;
            }
        }

        private void moveUpBtn_EnabledChanged(object sender, EventArgs e)
        {
            Button moveUpBtn = (Button)sender;

            if (moveUpBtn.Enabled == true)
            {
                moveUpBtn.BackgroundImage = 
                    global::MidiShapeShifter.Properties.Resources.imgUpBlue;
            }
            else
            {
                moveUpBtn.BackgroundImage =
                    global::MidiShapeShifter.Properties.Resources.imgUpGrey;
            }
        }

        private void moveDownBtn_EnabledChanged(object sender, EventArgs e)
        {
            Button moveDownBtn = (Button)sender;

            if (moveDownBtn.Enabled == true)
            {
                moveDownBtn.BackgroundImage =
                    global::MidiShapeShifter.Properties.Resources.imgDownBlue;
            }
            else
            {
                moveDownBtn.BackgroundImage =
                    global::MidiShapeShifter.Properties.Resources.imgDownGrey;
            }
        }

        private void deleteGeneratorBtn_Click(object sender, EventArgs e)
        {
            if (ActiveGraphableEntry == null ||
                this.persistantInfo.activeGraphableEntryType != GraphableEntryType.Generator)
            {
                //The delete button should be disabled if there is no ActiveGraphableEntry or if the 
                //active mapping is not in the mapping list view.
                Debug.Assert(false);
                return;
            }

            this.genMappingMgr.RemoveGenMappingEntry(this.persistantInfo.activeGraphableEntryIndex);
            this.generatorListView.Items[this.persistantInfo.activeGraphableEntryIndex].Remove();

            if (this.generatorListView.Items.Count > this.persistantInfo.activeGraphableEntryIndex)
            {
                //don't need to call ActiveGraphableEntryChanged() because selecting an item 
                //will trigger it.
                this.generatorListView.Items[this.persistantInfo.activeGraphableEntryIndex].Selected = true;
            }
            else
            {
                this.persistantInfo.activeGraphableEntryIndex = -1;
                OnActiveGraphableEntryChanged();
            }
        }

        private void moveMappingUpBtn_Click(object sender, EventArgs e)
        {
            if (ActiveGraphableEntry == null ||
                this.persistantInfo.activeGraphableEntryType != GraphableEntryType.Mapping ||
                this.persistantInfo.activeGraphableEntryIndex <= 0)
            {
                //The move up button should be disabled if there is no ActiveGraphableEntry, 
                //if the ActiveGraphableEntry is not in the mapping list view or if the 
                //ActiveGraphableEntry cannot be moved up.
                Debug.Assert(false);
                return;
            }

            this.mappingMgr.MoveEntryUp(this.persistantInfo.activeGraphableEntryIndex);
            this.persistantInfo.activeGraphableEntryIndex--;
            RefreshMappingListView();
        }

        private void moveMappingDownBtn_Click(object sender, EventArgs e)
        {
            if (ActiveGraphableEntry == null ||
                this.persistantInfo.activeGraphableEntryType != GraphableEntryType.Mapping ||
                this.persistantInfo.activeGraphableEntryIndex >= this.mappingMgr.GetNumEntries() - 1)
            {
                //The move up button should be disabled if there is no ActiveGraphableEntry, 
                //if the ActiveGraphableEntry is not in the mapping list view or if the 
                //ActiveGraphableEntry cannot be moved down.
                Debug.Assert(false);
                return;
            }

            this.mappingMgr.MoveEntryDown(this.persistantInfo.activeGraphableEntryIndex);
            this.persistantInfo.activeGraphableEntryIndex++;
            RefreshMappingListView();
        }

        private void saveProgram_Click(object sender, EventArgs e)
        {
            this.programMgr.SaveActiveProgram();
            repopulateProgramList();
        }

        private void saveProgramAs_Click(object sender, EventArgs e)
        {
            this.programMgr.SaveActiveProgramAsNewProgram();
            repopulateProgramList();
        }

        private void openProgram_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = MssProgramInfo.MSS_PROGRAM_FILE_FILTER;
            dlg.InitialDirectory = MssFileSystemLocations.UserProgramsFolder;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                this.programMgr.ActivateProgramByPath(dlg.FileName);
            }
        }

        private void graphOutputTypeImg_Paint(object sender, PaintEventArgs e)
        {
            DrawGraphOutputLabel(e.Graphics, this.graphOutputLabelText);
        }

        private void mainGraphControl_MouseClick(object sender, MouseEventArgs e)
        {
            //TODO: Remove
        }

        private bool mainGraphControl_MouseDownEvent(ZedGraphControl sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                //Return false so that ZedGraphs keeps processing the mouse event.
                return false;
            }

            if (this.ActiveGraphableEntry != null)
            {
                Point mousePt = new Point(e.X, e.Y);

                GraphPane pane = this.mainGraphControl.GraphPane;
                CurveShapeInfo curveInfo = this.ActiveGraphableEntry.CurveShapeInfo;

                RectangleF chartRect = pane.Chart.Rect;

                XyPoint<double> newPoint = new XyPoint<double>();
                newPoint.X = (e.X - chartRect.Left) / (double)chartRect.Width;
                newPoint.Y = (chartRect.Height - (e.Y - chartRect.Top)) / (double)chartRect.Height;

                LineItem controlPointsCurve;
                int nearestPointIndex;

                //If the click was close enough to an exsisting control point then start dragging 
                //it. The distances that is deemed "close enough" is defined in 
                //EqGraphConfig.ConfigureEqGraph.
                if (GetClickedControlPoint(mousePt, out controlPointsCurve, out nearestPointIndex))
                {
                    this.mainGraphControl.StartEditing(pane, mousePt, controlPointsCurve, nearestPointIndex);
                    return true;
                }

                //If the click event occured in the chart
                if (newPoint.X >= 0 && newPoint.X <= 1 && newPoint.Y >= 0 && newPoint.Y <= 1)
                {
                    int pointBeforeNewPointIndex = -1;
                    int pointAfterNewPointIndex = -1;

                    Debug.Assert(controlPointsCurve.Points.Count == this.ActiveGraphableEntry.CurveShapeInfo.PointEquations.Count);
                    for (int i = 0; i < controlPointsCurve.Points.Count; i++)
                    {
                        if (controlPointsCurve.Points[i].X > newPoint.X)
                        {
                            pointBeforeNewPointIndex = i - 1;
                            pointAfterNewPointIndex = i;
                            break;
                        }

                    }

                    if (pointAfterNewPointIndex == -1)
                    {
                        pointBeforeNewPointIndex = controlPointsCurve.Points.Count - 1;
                    }

                    //Adjust the new point's Y value so that it is on the equation line.
                    EvaluationCurveInput evalInput = new EvaluationCurveInput();
                    evalInput.Init(newPoint.X, newPoint.X, newPoint.X,
                        this.mssParameters.GetParameterValue(MssParameterID.VariableA),
                        this.mssParameters.GetParameterValue(MssParameterID.VariableB),
                        this.mssParameters.GetParameterValue(MssParameterID.VariableC),
                        this.mssParameters.GetParameterValue(MssParameterID.VariableD),
                        this.ActiveGraphableEntry);
                    ReturnStatus<double> evalReturnStatus = this.evaluator.Evaluate(evalInput);
                    if (evalReturnStatus.IsValid == false)
                    {
                        return true;
                    }
                    newPoint.Y = evalReturnStatus.Value;

                    //Copy curve equation from before the new point and assign it to the new line
                    //segment after the new point
                    string equationToDuplicate = curveInfo.CurveEquations[pointBeforeNewPointIndex + 1];
                    curveInfo.CurveEquations.Insert(pointBeforeNewPointIndex + 1, equationToDuplicate);
                    //If the new equation is being inserted before the one that is currently 
                    //selected then the index of the currenly selected equation must be incremented.
                    if (curveInfo.SelectedEquationType == EquationType.Curve &&
                        curveInfo.SelectedEquationIndex > pointBeforeNewPointIndex + 1)
                    {
                        curveInfo.SelectedEquationIndex++;
                    }

                    XyPoint<string> newPointEquation = new XyPoint<string>();
                    newPointEquation.X = Math.Round(newPoint.X, NUM_DECIMALS_IN_CONTROL_POINT).ToString();
                    newPointEquation.Y = Math.Round(newPoint.Y, NUM_DECIMALS_IN_CONTROL_POINT).ToString();

                    //Add the new point to curveInfo
                    if (pointAfterNewPointIndex != -1)
                    {
                        curveInfo.PointEquations.Insert(pointAfterNewPointIndex, newPointEquation);
                        //If the new point equation is being inserted before the point that is 
                        //currently selected then the index of the currenly selected point 
                        //equation must be incremented.
                        if (curveInfo.SelectedEquationType == EquationType.Point &&
                        curveInfo.SelectedEquationIndex >= pointAfterNewPointIndex)
                        {
                            curveInfo.SelectedEquationIndex++;
                        }
                    }
                    else
                    {
                        curveInfo.PointEquations.Add(newPointEquation);
                    }

                    UpdateCurveShapeControls();

                    this.mainGraphControl.StartEditing(pane, mousePt, controlPointsCurve, pointBeforeNewPointIndex + 1);
                }

            }

            return true;
        }

        private bool mainGraphControl_EditDragEvent(ZedGraphControl sender, 
                                                    PointPair newPointPosition, 
                                                    int pointBeingEditedIndex, 
                                                    CurveItem curveBeingEdited)
        {
            if (this.ActiveGraphableEntry == null)
            {
                //This event shouldn't fire when there is no active entry
                Debug.Assert(false);
                return false;
            }
            
            if (pointBeingEditedIndex > 0 && 
                    newPointPosition.X <= curveBeingEdited[pointBeingEditedIndex - 1].X)
            {
                newPointPosition.X = curveBeingEdited[pointBeingEditedIndex - 1].X;
            }
            else if (pointBeingEditedIndex < curveBeingEdited.Points.Count - 1 &&
                    newPointPosition.X >= curveBeingEdited[pointBeingEditedIndex + 1].X)
            {
                newPointPosition.X = curveBeingEdited[pointBeingEditedIndex + 1].X;
            }
            else if (newPointPosition.X < 0)
            {
                newPointPosition.X = 0;
            }
            else if (newPointPosition.X > 1)
            {
                newPointPosition.X = 1;
            }

            if (newPointPosition.Y < 0)
            {
                newPointPosition.Y = 0;
            }
            else if (newPointPosition.Y > 1)
            {
                newPointPosition.Y = 1;
            }

            CurveShapeInfo activeCurveInfo = this.ActiveGraphableEntry.CurveShapeInfo;
            activeCurveInfo.PointEquations[pointBeingEditedIndex].X = 
                Math.Round(newPointPosition.X, NUM_DECIMALS_IN_CONTROL_POINT).ToString();
            activeCurveInfo.PointEquations[pointBeingEditedIndex].Y = 
                Math.Round(newPointPosition.Y, NUM_DECIMALS_IN_CONTROL_POINT).ToString();
            
            UpdateCurveShapeControls();

            return true;
        }

        private void ZedGraphContextMenuBuilder(ZedGraphControl control,
            ContextMenuStrip menuStrip, Point mousePt,
            ZedGraphControl.ContextMenuObjectState objState)
        {
            menuStrip.Items.Clear();

            LineItem controlPointCurve;
            int nearestPointIndex;

            //If the click was close enough to an exsisting control point then start dragging 
            //it. The distances that is deemed "close enough" is defined in 
            //EqGraphConfig.ConfigureEqGraph.
            bool controlPointClicked = 
                GetClickedControlPoint(mousePt, out controlPointCurve, out nearestPointIndex);


            // create a new menu item
            ToolStripMenuItem item = new ToolStripMenuItem();
            item.Name = "ZedMenuDeletePoint";
            // This is the text that will show up in the menu
            item.Text = "Delete Point";
            item.Enabled = controlPointClicked;

            DeletePointParams deletePointParams = new DeletePointParams();
            deletePointParams.pointIndex = nearestPointIndex;
            item.Tag = deletePointParams;

            // Add a handler that will respond when that menu item is selected
            item.Click += new System.EventHandler(ZedGraph_DeletePoint);
            menuStrip.Items.Add(item);
        }

        private bool GetClickedControlPoint(Point mousePt, 
            out LineItem controlPointCurve, out int controlPointIndex)
        {
            GraphPane pane = this.mainGraphControl.GraphPane;
            controlPointCurve = (LineItem)pane.CurveList.Find(curveItem => curveItem.Label.Text == GRAPH_CONTROL_POINTS_LABEL);

            CurveItem nearestCurve;

            return pane.FindNearestPoint(mousePt, controlPointCurve,
                    out nearestCurve, out controlPointIndex);
        }

        private void ZedGraph_DeletePoint(object sender, EventArgs e)
        {
            if (this.ActiveGraphableEntry == null)
            {
                //This menu should be be possible to click when there is not active graphable 
                //entry.
                Debug.Assert(false);
                return;
            }
            
            ToolStripDropDownItem item = (ToolStripDropDownItem)sender;
            DeletePointParams deletePointParams = (DeletePointParams)item.Tag;

            CurveShapeInfo curveInfo = this.ActiveGraphableEntry.CurveShapeInfo;
            curveInfo.PointEquations.RemoveAt(deletePointParams.pointIndex);
            curveInfo.CurveEquations.RemoveAt(deletePointParams.pointIndex + 1);

            if (curveInfo.SelectedEquationIndex > deletePointParams.pointIndex)
            {
                curveInfo.SelectedEquationIndex--;
            }
            else if (curveInfo.SelectedEquationIndex == deletePointParams.pointIndex &&
                    curveInfo.SelectedEquationType == EquationType.Point)
            {
                curveInfo.SelectedEquationType = EquationType.Curve;                
            }


            UpdateCurveShapeControls();
        }

        private void nextEquationBtn_Click(object sender, EventArgs e)
        {
            if (this.ActiveGraphableEntry != null)
            {
                CurveShapeInfo curveInfo = this.ActiveGraphableEntry.CurveShapeInfo;
                if (curveInfo.SelectedEquationType == EquationType.Curve)
                {
                    curveInfo.SelectedEquationType = EquationType.Point;
                }
                else if (curveInfo.SelectedEquationType == EquationType.Point)
                {
                    curveInfo.SelectedEquationType = EquationType.Curve;
                    curveInfo.SelectedEquationIndex++;
                }
                else
                {
                    //Unknown equation type
                    Debug.Assert(false);
                }

                UpdateCurveShapeControls();
            }
            else
            {
                //This button should be disabled when there is no ActiveGraphableEntry
                Debug.Assert(false);
            }
        }

        private void prevEquationBtn_Click(object sender, EventArgs e)
        {
            if (this.ActiveGraphableEntry != null)
            {
                CurveShapeInfo curveInfo = this.ActiveGraphableEntry.CurveShapeInfo;
                if (curveInfo.SelectedEquationType == EquationType.Curve)
                {
                    curveInfo.SelectedEquationType = EquationType.Point;
                    curveInfo.SelectedEquationIndex--;
                }
                else if (curveInfo.SelectedEquationType == EquationType.Point)
                {
                    curveInfo.SelectedEquationType = EquationType.Curve;
                }
                else
                {
                    //Unknown equation type
                    Debug.Assert(false);
                }

                UpdateCurveShapeControls();
            }
            else
            {
                //This button should be disabled when there is no ActiveGraphableEntry
                Debug.Assert(false);
            }
        }

        private void prevEquationBtn_EnabledChanged(object sender, EventArgs e)
        {
            Button prevBtn = (Button)sender;

            if (prevBtn.Enabled == true)
            {
                prevBtn.BackgroundImage =
                    global::MidiShapeShifter.Properties.Resources.imgLeftBlue;
            }
            else
            {
                prevBtn.BackgroundImage =
                    global::MidiShapeShifter.Properties.Resources.imgLeftGrey;
            }
        }

        private void nextEquationBtn_EnabledChanged(object sender, EventArgs e)
        {
            Button nextBtn = (Button)sender;

            if (nextBtn.Enabled == true)
            {
                nextBtn.BackgroundImage =
                    global::MidiShapeShifter.Properties.Resources.imgRightBlue;
            }
            else
            {
                nextBtn.BackgroundImage =
                    global::MidiShapeShifter.Properties.Resources.imgRightGrey;
            }
        }

        private void resetGraphBtn_EnabledChanged(object sender, EventArgs e)
        {
            Button resetBtn = (Button)sender;

            if (resetBtn.Enabled == true)
            {
                resetBtn.BackgroundImage =
                    global::MidiShapeShifter.Properties.Resources.imgTrashBlue;
            }
            else
            {
                resetBtn.BackgroundImage =
                    global::MidiShapeShifter.Properties.Resources.imgTrashGrey;
            }
        }

        private class DeletePointParams
        {
            public int pointIndex;
        }

        private void resetGraphBtn_Click(object sender, EventArgs e)
        {
            if (this.ActiveGraphableEntry == null)
            {
                //This button should not be clickable when there is no active graphable entry
                Debug.Assert(false);
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you to reset all equations and control points for this mapping?", 
                MssConstants.APP_NAME, 
                MessageBoxButtons.YesNoCancel);
            if (result == DialogResult.Yes)
            {
                CurveShapeInfo curveInfo = this.ActiveGraphableEntry.CurveShapeInfo;
                curveInfo.CurveEquations.Clear();

                curveInfo.CurveEquations.Add(CurveShapeInfo.DEFAULT_EQUATION);
                curveInfo.PointEquations.Clear();

                curveInfo.SelectedEquationIndex = 0;
                curveInfo.SelectedEquationType = EquationType.Curve;

                UpdateCurveShapeControls();
            }
        }

        private void graphInputTypeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ignoreInputTypeComboSelectionChangeHandlers == false &&
                this.ActiveGraphableEntry != null)
            {
                ComboBox inputTypeCombo = (ComboBox)sender;
                this.ActiveGraphableEntry.CurveShapeInfo.PrimaryInputSource =
                    this.DataFieldsInGraphInputCombo[inputTypeCombo.SelectedIndex];

                UpdateCurveShapeControls();
            }
        }

    }
}

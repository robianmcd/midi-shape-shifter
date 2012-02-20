using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;

using MidiShapeShifter.CSharpUtil;
using MidiShapeShifter.Framework;
using MidiShapeShifter.Mss;
using MidiShapeShifter.Mss.Mapping;
using MidiShapeShifter.Mss.Mapping.MssMsgRangeEntryMetadataTypes;
using MidiShapeShifter.Mss.Generator;
using MidiShapeShifter.Mss.Relays;
using MidiShapeShifter.Mss.MssMsgInfoTypes;
using MidiShapeShifter.Mss.Programs;

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
        public static readonly double[] GRAPH_X_VAULES = new double[NUM_GRAPH_POINTS];

        protected const string GRAPH_MAIN_CURVE_LABEL = "Main Curve";

        public TwoWayDictionary<MssParameterID, LBKnob> ParameterValueKnobControlDict = new TwoWayDictionary<MssParameterID, LBKnob>();
        public TwoWayDictionary<MssParameterID, Label> ParameterValueLabelControlDict = new TwoWayDictionary<MssParameterID, Label>();
        public TwoWayDictionary<MssParameterID, TextBox> ParameterMaxValueControlDict = new TwoWayDictionary<MssParameterID, TextBox>();
        public TwoWayDictionary<MssParameterID, TextBox> ParameterMinValueControlDict = new TwoWayDictionary<MssParameterID, TextBox>();
        public TwoWayDictionary<MssParameterID, Label> ParameterNameControlDict = new TwoWayDictionary<MssParameterID, Label>();

        protected MssEvaluator evaluator;

        protected MssParameters mssParameters;
        protected MappingManager mappingMgr;
        protected GeneratorMappingManager genMappingMgr;
        protected IDryMssEventOutputPort dryMssEventOutputPort;

        protected Factory_MssMsgRangeEntryMetadata msgMetadataFactory;
        protected IFactory_MssMsgInfo msgInfoFactory;

        protected MssProgramMgr programMgr;

        protected SerializablePluginEditorInfo persistantInfo;

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

            this.evaluator = new MssEvaluator();
            this.msgMetadataFactory = new Factory_MssMsgRangeEntryMetadata();
            this.msgInfoFactory = new Factory_MssMsgInfo();
        }

        //static constructor
        static PluginEditorView()
        {
            for (int i = 0; i < NUM_GRAPH_POINTS; i++)
            {
                GRAPH_X_VAULES[i] = (1.0 / (NUM_GRAPH_POINTS - 1)) * i;
            }
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

            ActiveGraphableEntryChanged();
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

            ParameterMinValueControlDict.Add(MssParameterID.VariableA, this.variableAMin);
            ParameterMinValueControlDict.Add(MssParameterID.VariableB, this.variableBMin);
            ParameterMinValueControlDict.Add(MssParameterID.VariableC, this.variableCMin);
            ParameterMinValueControlDict.Add(MssParameterID.VariableD, this.variableDMin);

            ParameterMaxValueControlDict.Add(MssParameterID.VariableA, this.variableAMax);
            ParameterMaxValueControlDict.Add(MssParameterID.VariableB, this.variableBMax);
            ParameterMaxValueControlDict.Add(MssParameterID.VariableC, this.variableCMax);
            ParameterMaxValueControlDict.Add(MssParameterID.VariableD, this.variableDMax);

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

        //TODO: add handlers like this for changes to max and min value text boxes
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

        protected void ActiveGraphableEntryChanged()
        {
            UpdateCurveShapeControls();
            UpdateGraphableEntryButtonsEnabledStatus();
        }

        protected void UpdateCurveShapeControls()
        {
            if (this.ActiveGraphableEntry == null)
            {
                this.curveShapeEquationRadio.Checked = false;
                this.curveShapePresetRadio.Checked = false;

                this.curveShapeEquationRadio.Enabled = false;
                this.curveShapePresetRadio.Enabled = false;

                this.curveEquationTextBox.Text = "";
                this.curvePresetCombo.SelectedIndex = -1;
            }
            else
            {
                this.curveShapeEquationRadio.Enabled = true;
                this.curveShapePresetRadio.Enabled = true;
                
                if (this.ActiveGraphableEntry.CurveShapeInfo.EqInputMode == EquationInputMode.Text)
                {
                    this.curveShapeEquationRadio.Checked = true;
                }
                else if (this.ActiveGraphableEntry.CurveShapeInfo.EqInputMode == EquationInputMode.Preset)
                {
                    this.curveShapePresetRadio.Checked = true;
                }
                else
                {
                    //Unknown EquationInputMode
                    Debug.Assert(false);
                }

                this.curveEquationTextBox.Text = ActiveGraphableEntry.CurveShapeInfo.Equation;
                this.curvePresetCombo.SelectedIndex = ActiveGraphableEntry.CurveShapeInfo.PresetIndex;

                this.presetParam1Knob.Value = (float)ActiveGraphableEntry.CurveShapeInfo.PresetParamValues[0];
                this.presetParam2Knob.Value = (float)ActiveGraphableEntry.CurveShapeInfo.PresetParamValues[1];
                this.presetParam3Knob.Value = (float)ActiveGraphableEntry.CurveShapeInfo.PresetParamValues[2];
                this.presetParam4Knob.Value = (float)ActiveGraphableEntry.CurveShapeInfo.PresetParamValues[3];
                this.presetParam1Value.Text = FormatRawParameterValue(ActiveGraphableEntry.CurveShapeInfo.PresetParamValues[0]);
                this.presetParam2Value.Text = FormatRawParameterValue(ActiveGraphableEntry.CurveShapeInfo.PresetParamValues[1]);
                this.presetParam3Value.Text = FormatRawParameterValue(ActiveGraphableEntry.CurveShapeInfo.PresetParamValues[2]);
                this.presetParam4Value.Text = FormatRawParameterValue(ActiveGraphableEntry.CurveShapeInfo.PresetParamValues[3]);
            }
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
            ActiveGraphableEntryChanged();
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
                ActiveGraphableEntryChanged();
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
                this.Invoke(new Action<MssParameterID, string>(MssParameters_NameChanged), paramId, name);
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
                this.Invoke(new Action<MssParameterID, double>(MssParameters_ValueChanged), paramId, value);
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
        }

        private void MssParameters_MinValueChanged(MssParameterID paramId, int minValue)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<MssParameterID, int>(MssParameters_MinValueChanged), paramId, minValue);
                return;
            }

            TextBox minValueTextBox;
            if (ParameterMinValueControlDict.TryGetRightByLeft(paramId, out minValueTextBox) == true)
            {
                if (minValueTextBox.Text != minValue.ToString())
                {
                    minValueTextBox.Text = minValue.ToString();
                }
            }
        }

        private void MssParameters_MaxValueChanged(MssParameterID paramId, int maxValue)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<MssParameterID, int>(MssParameters_MaxValueChanged), paramId, maxValue);
                return;
            }

            TextBox maxValueTextBox;
            if (ParameterMaxValueControlDict.TryGetRightByLeft(paramId, out maxValueTextBox) == true)
            {
                if (maxValueTextBox.Text != maxValue.ToString())
                {
                    maxValueTextBox.Text = maxValue.ToString();
                }
            }
        }

        private void CurveShapeRadio_CheckedChanged(object sender, System.EventArgs e)
        {
            bool equationInputMode = this.curveShapeEquationRadio.Checked;

            curveEquationTextBox.Enabled = equationInputMode;


            bool presetInputMode = this.curveShapePresetRadio.Checked;

            curvePresetCombo.Enabled = presetInputMode;
            presetParam1Knob.Enabled = presetInputMode;
            presetParam2Knob.Enabled = presetInputMode;
            presetParam3Knob.Enabled = presetInputMode;
            presetParam4Knob.Enabled = presetInputMode;


            if (this.ActiveGraphableEntry != null)
            {
                EquationInputMode newInputMode;

                if (this.curveShapeEquationRadio.Checked == true)
                {
                    newInputMode = EquationInputMode.Text;
                }
                else if (this.curveShapePresetRadio.Checked == true)
                {
                    newInputMode = EquationInputMode.Preset;
                }
                else
                {
                    //Unknown input mode
                    Debug.Assert(false);
                    return;
                }

                ActiveGraphableEntry.CurveShapeInfo.EqInputMode = newInputMode;
            }
        }

        private void curveEquationTextBox_TextChanged(object sender, System.EventArgs e)
        {
            string expressionString = ((TextBox)sender).Text;
            ReturnStatus<double[]> evalReturnStatus = 
                this.evaluator.SampleExpressionWithDefaultInputValues(expressionString, NUM_GRAPH_POINTS);
            if (evalReturnStatus.IsValid == true)
            {
                double[] GraphYValues = evalReturnStatus.ReturnVal;

                //If values are outside of the range 0 to 1 then they will not be mapped
                //So they should not be shown on the graph.
                for (int i = 0; i < GraphYValues.Length; i++ )
                {
                    if (GraphYValues[i] < 0 || GraphYValues[i] > 1)
                    {
                        GraphYValues[i] = -1;
                    }
                }

                ActiveGraphableEntry.CurveShapeInfo.Equation = expressionString;

                LineItem mainCurve = GetMainCurve();
                mainCurve.Points = new PointPairList(GRAPH_X_VAULES, GraphYValues);

                this.mainGraphControl.Invalidate();
            }
            else
            {
               if (this.ActiveGraphableEntry == null)
                {
                    GetMainCurve().Points = new PointPairList();
                    this.mainGraphControl.Invalidate();
                }
                else
                {
                    //TODO: show that the line is not using the current formula.
                }
            }

        }

        protected void InitiaizeGraph()
        {
            EqGraphConfig.ConfigureEqGraph(this.mainGraphControl);

            LineItem mainCurve =  EqGraphConfig.CreateEqCurve(GRAPH_MAIN_CURVE_LABEL);
            
            // get a reference to the GraphPane
            GraphPane pane = this.mainGraphControl.GraphPane;
            pane.CurveList.Add(mainCurve);
        }

        protected LineItem GetMainCurve()
        {
            GraphPane pane = this.mainGraphControl.GraphPane;
            return (LineItem) pane.CurveList.Find(curveItem => curveItem.Label.Text == GRAPH_MAIN_CURVE_LABEL);
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
                ActiveGraphableEntryChanged();
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

    }
}

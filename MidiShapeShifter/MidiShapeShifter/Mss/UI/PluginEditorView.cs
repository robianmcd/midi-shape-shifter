using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;

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
using MidiShapeShifter.Mss.Evaluation;
using MidiShapeShifter.Mss.Parameters;


using LBSoft.IndustrialCtrls.Knobs;

using ZedGraph;

//Winforms and ZedGraphs both define Label
using Label = System.Windows.Forms.Label;

namespace MidiShapeShifter.Mss.UI
{
    public partial class PluginEditorView : UserControl
    {
        public enum EditorCommandId { Generic, UpdateCurveShapeControls, UpdateEquationCurve }

        public const int NUM_VARIABLE_PARAMS = 4;
        public const int NUM_PRESET_PARAMS = 4;

        public const int NUM_GRAPH_POINTS = 256;

        public const int NUM_DECIMALS_IN_CONTROL_POINT = 3;

        protected const string GRAPH_DEFAULT_OUTPUT_LABEL = "Output";
        protected const string GRAPH_CONTROL_POINTS_LABEL = "ControlPoints";
        protected const string EQUATION_CURVE_LABEL_PREFIX = "Curve";

        private static readonly Color KNOB_COLOR_TRANSFORM = Color.FromArgb(164, 193, 216); //H207 S75 V88
        private static readonly Color KNOB_COLOR_DISABLED = Color.Silver;

        protected string graphOutputLabelText = GRAPH_DEFAULT_OUTPUT_LABEL;

        protected readonly AutoResetEvent idleProcessingSignal;

        public TwoWayDictionary<MssParameterID, LBKnob> ParameterValueKnobControlDict = new TwoWayDictionary<MssParameterID, LBKnob>();
        public TwoWayDictionary<MssParameterID, Label> ParameterValueLabelControlDict = new TwoWayDictionary<MssParameterID, Label>();
        public TwoWayDictionary<MssParameterID, Label> ParameterNameControlDict = new TwoWayDictionary<MssParameterID, Label>();
        public Dictionary<Control, MssParameterID> ParameterAllControlsDict = new Dictionary<Control, MssParameterID>();

        protected IEvaluator evaluator;

        protected MssParameters mssParameters;
        protected IMappingManager mappingMgr;
        protected TransformPresetMgr transformPresetMgr;
        protected IGeneratorMappingManager genMappingMgr;
        protected CommandQueue<EditorCommandId> commandQueue;

        protected IDryMssEventOutputPort dryMssEventOutputPort;
        protected IHostInfoOutputPort hostInfoOutputPort;

        protected Factory_MssMsgRangeEntryMetadata msgMetadataFactory;
        protected IFactory_MssMsgInfo msgInfoFactory;

        protected MssProgramMgr programMgr;

        protected ActiveMappingInfo activeMappingInfo;

        protected List<MssMsgDataField> DataFieldsInGraphInputCombo;

        protected bool ignoreEquationTextBoxChangeHandlers;
        protected bool ignoreInputTypeComboSelectionChangeHandlers;
        protected bool IgnoreGraphableEntrySelectionChangedHandler;

        public PluginEditorView()
        {
            InitializeComponent();
            PopulateControlDictionaries();

            this.DataFieldsInGraphInputCombo = new List<MssMsgDataField>();
            this.evaluator = IocMgr.Kernel.Get<IEvaluator>();
            this.msgMetadataFactory = new Factory_MssMsgRangeEntryMetadata();
            this.msgInfoFactory = new Factory_MssMsgInfo();
            this.idleProcessingSignal = new AutoResetEvent(false);
            this.commandQueue = new CommandQueue<EditorCommandId>();

            this.ignoreEquationTextBoxChangeHandlers = false;
            this.ignoreInputTypeComboSelectionChangeHandlers = false;
            this.IgnoreGraphableEntrySelectionChangedHandler = false;
        }

        public void Init(MssParameters mssParameters, 
                         IMappingManager mappingMgr, 
                         GeneratorMappingManager genMappingMgr,
                         MssProgramMgr programMgr,
                         TransformPresetMgr transformPresetMgr,
                         IDryMssEventOutputPort dryMssEventOutputPort,
                         IHostInfoOutputPort hostInfoOutputPort,
                         ActiveMappingInfo activeMappingInfo)
        {
            InitiaizeGraph();

            this.mssParameters = mssParameters;
            this.mappingMgr = mappingMgr;
            this.transformPresetMgr = transformPresetMgr;
            this.genMappingMgr = genMappingMgr;
            this.programMgr = programMgr;
            
            this.dryMssEventOutputPort = dryMssEventOutputPort;
            this.hostInfoOutputPort = hostInfoOutputPort;

            this.activeMappingInfo = activeMappingInfo;

            this.commandQueue.Init(EditorCommandId.Generic);

            this.mssParameters.ParameterValueChanged += new ParameterValueChangedEventHandler(MssParameters_ValueChanged);
            this.mssParameters.ParameterNameChanged += new ParameterNameChangedEventHandler(MssParameters_NameChanged);
            this.mssParameters.ParameterMinValueChanged += new ParameterMinValueChangedEventHandler(MssParameters_MinValueChanged);
            this.mssParameters.ParameterMaxValueChanged += new ParameterMaxValueChangedEventHandler(MssParameters_MaxValueChanged);

            this.hostInfoOutputPort.DoIdleProcessing += new DoIdleProcessingEventHandler(OnIdleProcessing);
            //Don't forget to unregister any events added here in OnDispose()


            //Set parameters from MssParameters
            foreach(MssParameterID paramId in Enum.GetValues(typeof(MssParameterID)))
            {
                UpdateInfoForParameter(paramId);                
            }

            this.msgMetadataFactory.Init(genMappingMgr, (IMssParameterViewer)mssParameters);
            this.msgInfoFactory.Init(genMappingMgr, (IMssParameterViewer)mssParameters);

            RefreshMappingListView();
            RefreshGeneratorListView();

            //Populate the program list
            repopulateProgramsList();
            //Populate the transform preset list
            repopulateTransformPresetList();

            //The only time this won't get called by repopulateProgramList() is when a blank
            //program is loaded.
            OnActiveGraphableEntryChanged();
        }

        protected void OnDispose()
        {
            this.mssParameters.ParameterValueChanged -= new ParameterValueChangedEventHandler(MssParameters_ValueChanged);
            this.mssParameters.ParameterNameChanged -= new ParameterNameChangedEventHandler(MssParameters_NameChanged);
            this.mssParameters.ParameterMinValueChanged -= new ParameterMinValueChangedEventHandler(MssParameters_MinValueChanged);
            this.mssParameters.ParameterMaxValueChanged -= new ParameterMaxValueChangedEventHandler(MssParameters_MaxValueChanged);

            this.hostInfoOutputPort.DoIdleProcessing -= new DoIdleProcessingEventHandler(OnIdleProcessing);
        }

        protected void PopulateControlDictionaries()
        {
            ParameterValueKnobControlDict.Add(MssParameterID.VariableA, this.variableAKnob);
            ParameterValueKnobControlDict.Add(MssParameterID.VariableB, this.variableBKnob);
            ParameterValueKnobControlDict.Add(MssParameterID.VariableC, this.variableCKnob);
            ParameterValueKnobControlDict.Add(MssParameterID.VariableD, this.variableDKnob);
            ParameterValueKnobControlDict.Add(MssParameterID.VariableE, this.variableEKnob);
            ParameterValueKnobControlDict.Add(MssParameterID.Preset1, this.presetParam1Knob);
            ParameterValueKnobControlDict.Add(MssParameterID.Preset2, this.presetParam2Knob);
            ParameterValueKnobControlDict.Add(MssParameterID.Preset3, this.presetParam3Knob);
            ParameterValueKnobControlDict.Add(MssParameterID.Preset4, this.presetParam4Knob);
            ParameterValueKnobControlDict.Add(MssParameterID.Preset5, this.presetParam5Knob);

            ParameterValueLabelControlDict.Add(MssParameterID.VariableA, this.variableAValue);
            ParameterValueLabelControlDict.Add(MssParameterID.VariableB, this.variableBValue);
            ParameterValueLabelControlDict.Add(MssParameterID.VariableC, this.variableCValue);
            ParameterValueLabelControlDict.Add(MssParameterID.VariableD, this.variableDValue);
            ParameterValueLabelControlDict.Add(MssParameterID.VariableE, this.variableEValue);
            ParameterValueLabelControlDict.Add(MssParameterID.Preset1, this.presetParam1Value);
            ParameterValueLabelControlDict.Add(MssParameterID.Preset2, this.presetParam2Value);
            ParameterValueLabelControlDict.Add(MssParameterID.Preset3, this.presetParam3Value);
            ParameterValueLabelControlDict.Add(MssParameterID.Preset4, this.presetParam4Value);
            ParameterValueLabelControlDict.Add(MssParameterID.Preset5, this.presetParam5Value);

            ParameterNameControlDict.Add(MssParameterID.VariableA, this.variableATitle);
            ParameterNameControlDict.Add(MssParameterID.VariableB, this.variableBTitle);
            ParameterNameControlDict.Add(MssParameterID.VariableC, this.variableCTitle);
            ParameterNameControlDict.Add(MssParameterID.VariableD, this.variableDTitle);
            ParameterNameControlDict.Add(MssParameterID.VariableE, this.variableETitle);
            ParameterNameControlDict.Add(MssParameterID.Preset1, this.presetParam1Title);
            ParameterNameControlDict.Add(MssParameterID.Preset2, this.presetParam2Title);
            ParameterNameControlDict.Add(MssParameterID.Preset3, this.presetParam3Title);
            ParameterNameControlDict.Add(MssParameterID.Preset4, this.presetParam4Title);
            ParameterNameControlDict.Add(MssParameterID.Preset5, this.presetParam5Title);

            foreach (LBKnob paramKnob in ParameterValueKnobControlDict.RightKeys)
            {
                MssParameterID paramId;
                ParameterValueKnobControlDict.TryGetLeftByRight(out paramId, paramKnob);
                ParameterAllControlsDict[paramKnob] = paramId;
            }

            foreach (Label paramValue in ParameterValueLabelControlDict.RightKeys)
            {
                MssParameterID paramId;
                ParameterValueLabelControlDict.TryGetLeftByRight(out paramId, paramValue);
                ParameterAllControlsDict[paramValue] = paramId;
            }

            foreach (Label paramName in ParameterNameControlDict.RightKeys)
            {
                MssParameterID paramId;
                ParameterNameControlDict.TryGetLeftByRight(out paramId, paramName);
                ParameterAllControlsDict[paramName] = paramId;
            }
        }

        protected void UpdateGraphableEntryButtonsEnabledStatus()
        {
            bool EnableMappingButtons;
            bool EnableGeneratorButtons;

            if (this.activeMappingInfo.ActiveGraphableEntryId < 0) 
            {
                EnableMappingButtons = false;
                EnableGeneratorButtons = false;
            }
            else if (this.activeMappingInfo.ActiveGraphableEntryType == GraphableEntryType.Mapping)
            {
                EnableMappingButtons = true;
                EnableGeneratorButtons = false;
            }
            else if (this.activeMappingInfo.ActiveGraphableEntryType == GraphableEntryType.Generator)
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

            int activeMappingIndex = -1;
            if (EnableMappingButtons) {
                activeMappingIndex = this.mappingMgr.GetMappingEntryIndexById(this.activeMappingInfo.ActiveGraphableEntryId);
            }

            this.moveMappingUpBtn.Enabled = EnableMappingButtons &&
                    activeMappingIndex > 0;
            this.moveMappingDownBtn.Enabled = EnableMappingButtons &&
                    activeMappingIndex < this.mappingListView.Items.Count - 1;

            this.deleteGeneratorBtn.Enabled = EnableGeneratorButtons;
            this.editGeneratorBtn.Enabled = EnableGeneratorButtons;
        }

        protected void RefreshMappingListView()
        {
            RefreshGraphableMappingListView(this.mappingListView, this.mappingMgr, GraphableEntryType.Mapping, GetMappingListViewRow);
        }

        protected void RefreshGeneratorListView()
        {
            RefreshGraphableMappingListView(this.generatorListView, this.genMappingMgr, GraphableEntryType.Generator, GetGeneratorListViewRow);
        }


        protected void RefreshGraphableMappingListView<MappingEntryType>(ListView listView, IGraphableMappingManager<MappingEntryType> graphableEntryMgr, GraphableEntryType graphableType, Func<MappingEntryType, ListViewItem> getListViewRow)
             where MappingEntryType : IMappingEntry
        {
            int topItemIndex = 0;
            ListViewItem prevTopItem = listView.TopItem;
            if (prevTopItem != null)
            {
                topItemIndex = prevTopItem.Index;
            }

            listView.Items.Clear();

            List<MappingEntryType> mappingEntryList = graphableEntryMgr.GetCopyOfMappingEntryList();

            foreach (MappingEntryType entry in mappingEntryList)
            {
                listView.Items.Add(getListViewRow(entry));
            }

            int selectedIndex = -1;
            if (this.activeMappingInfo.ActiveGraphableEntryType == graphableType &&
                this.activeMappingInfo.ActiveGraphableEntryId > -1)
            {
                selectedIndex = mappingEntryList.FindIndex(entry => entry.Id == this.activeMappingInfo.ActiveGraphableEntryId);

                this.IgnoreGraphableEntrySelectionChangedHandler = true;
                listView.Items[selectedIndex].Selected = true;
                //Need to set focused here because of some winforms weirdness. See these posts for more info
                //http://stackoverflow.com/questions/7363777/arrow-keys-dont-work-after-programmatically-setting-listview-selecteditem
                //http://stackoverflow.com/questions/165424/how-does-itemcontainergenerator-containerfromitem-work-with-a-grouped-list/169123#169123
                listView.Items[selectedIndex].Focused = true;
                this.IgnoreGraphableEntrySelectionChangedHandler = false;

            }


            if (listView.Items.Count - 1 >= topItemIndex)
            {
                listView.TopItem = listView.Items[topItemIndex];
            }
            //This could be the case when an entry is being deleted.
            else if (listView.Items.Count - 1 == topItemIndex - 1 && listView.Items.Count > 0)
            {
                listView.TopItem = listView.Items[topItemIndex - 1];
            }

            if (selectedIndex != -1)
            {
                listView.EnsureVisible(selectedIndex);
            }
        }

        /// <summary>
        ///     Creates a ListViewItem based on the MappingEntry specified by <paramref name="id"/>.
        /// </summary>
        /// <returns>The ListViewItem representation of a MappingEntry</returns>
        public ListViewItem GetMappingListViewRow(IMappingEntry entry)
        {
            ListViewItem mappingItem = new ListViewItem(entry.GetReadableMsgType(IoType.Input));
            mappingItem.Tag = entry;
            mappingItem.SubItems.Add(entry.InMssMsgRange.GetData1RangeStr(this.msgInfoFactory));
            mappingItem.SubItems.Add(entry.InMssMsgRange.GetData2RangeStr(this.msgInfoFactory));

            mappingItem.SubItems.Add(entry.GetReadableMsgType(IoType.Output));
            mappingItem.SubItems.Add(entry.OutMssMsgRange.GetData1RangeStr(this.msgInfoFactory));
            mappingItem.SubItems.Add(entry.OutMssMsgRange.GetData2RangeStr(this.msgInfoFactory));

            mappingItem.SubItems.Add(entry.GetReadableOverrideDuplicates());

            return mappingItem;
        }

        public int getIndexForGenIdInListView(int id) {
            int index = 0;
            foreach(ListViewItem item in this.generatorListView.Items) {
                var genEntry = (IGeneratorMappingEntry)item.Tag;
                if (genEntry.Id == id) {
                    return index;
                }

                index++;
            }

            return -1;
        }

        /// <summary>
        ///     Creates a ListViewItem based on the GeneratorMappingEntry specified by 
        ///     <paramref name="id"/>. This ListViewItem is intended to be used in the 
        ///     PluginEditorView's generator list box.
        /// </summary>
        /// <returns>The ListViewItem representation of a GeneratorMappingEntry</returns>
        public ListViewItem GetGeneratorListViewRow(IGeneratorMappingEntry entry)
        {
            ListViewItem genMappingItem = new ListViewItem(entry.GenConfigInfo.Name);
            genMappingItem.Tag = entry;
            genMappingItem.SubItems.Add(entry.GetReadablePeriod());
            genMappingItem.SubItems.Add(entry.GetReadableLoopStatus());
            genMappingItem.SubItems.Add(entry.GetReadableEnabledStatus());

            return genMappingItem;
        }
                
        private void lbKnob_KnobChangeValue(object sender, LBKnobEventArgs e) {
            LBKnob knob = (LBKnob)sender;
            MssParameterID paramId;
            ParameterValueKnobControlDict.TryGetLeftByRight(out paramId, knob);
            MssParamInfo paramInfo = mssParameters.GetParameterInfoCopy(paramId);

            double knobValue = GetRoundedKnobValueAsDouble(knob);

            if (paramInfo.RawValue != knobValue)
            {
                this.mssParameters.SetParameterRawValue(paramId, knobValue);
            }
        }

        //Knob values are stored as floats which are less percise than doubles. The max and min value for a parameter 
        //are stored as doubles so if the knob is set to 0.01f and the min value for a parameter is set to 0.01 then 
        //then knob value will actually be less than the min value. To avoid this we round the knob value so that it 
        //should end up being the same as a double value as long as the double value is less than 6 digits.
        protected double GetRoundedKnobValueAsDouble(LBKnob knob) 
        {
            return Math.Round((double)knob.Value, 6);
        }

        protected void OnActiveGraphableEntryChanged()
        {
            OnActiveCurveShapeChanged();
            UpdateGraphableEntryButtonsEnabledStatus();

            RefreshMappingListView();
            RefreshGeneratorListView();
        }

        protected void OnActiveCurveShapeChanged()
        {
            this.commandQueue.EnqueueCommandOverwriteDups(
                EditorCommandId.UpdateCurveShapeControls, () => UpdateCurveShapeControls());
        }

        protected void UpdateCurveShapeControls()
        {
            //Disable redraw while updating the transformation group box.
            DrawingControl.SuspendDrawing(this.curveGroup);

            //Enable/disable controls in the transformation group box.
            foreach (Control curveControl in this.curveGroup.Controls)
            {
                curveControl.Enabled = this.activeMappingInfo.GetActiveMappingExists();
            }

            UpdateGraphInputCombo();


            //Update Parameter controls
            foreach (MssParameterID curId in MssParameters.PRESET_PARAM_ID_LIST)
            {
                LBKnob curKnob;
                Label curValueLabel;
                bool knobFound;
                bool labelFound;
                knobFound = this.ParameterValueKnobControlDict.TryGetRightByLeft(curId, out curKnob);
                labelFound = this.ParameterValueLabelControlDict.TryGetRightByLeft(curId, out curValueLabel);

                if (knobFound == false || labelFound == false)
                {
                    //Could not find a control. They should have been added to the dictionaries
                    //in PopulateControlDictionaries().
                    Debug.Assert(false);
                    break;
                }

                if (this.activeMappingInfo.GetActiveMappingExists())
                {
                    UpdateInfoForParameter(curId);
                    curKnob.KnobColor = KNOB_COLOR_TRANSFORM;
                }
                else
                {
                    curKnob.KnobColor = KNOB_COLOR_DISABLED;                    
                }
            }

            if (this.activeMappingInfo.GetActiveMappingExists())
            {
                IMappingEntry activeMapping = this.activeMappingInfo.GetActiveMappingCopy();
                CurveShapeInfo curveInfo = activeMapping.CurveShapeInfo;

                Logger.HighVolume(2, string.Format(
                    "Updating curve shape controls. Curve equations: {0}. Point equations: {1}. Equation type: {2}", 
                    EnumerableUtils.ToString(curveInfo.CurveEquations), EnumerableUtils.ToString(curveInfo.PointEquations), curveInfo.SelectedEquationType.ToString()));


                IMssMsgInfo outMsgInfo =
                    this.msgInfoFactory.Create(activeMapping.OutMssMsgRange.MsgType);

            //Update preset controls
                repopulateTransformPresetList();

            //Update Graph controls (the equation curve is not updated until the end)
                SetGraphOutputLabelText(outMsgInfo.Data3Name);

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

            //Update equations controls
                MakeAppropriateEquationControlsVisable(curveInfo.SelectedEquationType);
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

            }
            else //Active mapping does not exsist
            {
            //Update preset controls
                repopulateTransformPresetList();

            //Update Graph controls (the equation curve is not updated until the end)
                SetGraphOutputLabelText(GRAPH_DEFAULT_OUTPUT_LABEL);

            //Update the buttons under the graph
                //Nothing to do.

            //Update equations controls
                MakeAppropriateEquationControlsVisable(EquationType.Curve);
                this.curveEquationTextBox.Text = "";
            }

            UpdateEquationCurve();

            //Resume drawing once updating the transformation group box is funished.
            DrawingControl.ResumeDrawing(this.curveGroup);
        }

        protected void MakeAppropriateEquationControlsVisable(EquationType equationType)
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

            this.pointXEquationLabel.Visible = ! isCurveEquation;
            this.pointXEquationTextBox.Visible = ! isCurveEquation;
            this.pointYEquationLabel.Visible = ! isCurveEquation;
            this.pointYEquationTextBox.Visible = ! isCurveEquation;
        }

        protected void SetGraphOutputLabelText(string outputText)
        {
            //The following code will cause the output type label to flicker so it should be 
            //avoided whenever possible.
            if (this.graphOutputLabelText != outputText)
            {
                this.graphOutputLabelText = outputText;

                Graphics outputTypeGraphics = this.graphOutputTypeImg.CreateGraphics();
                DrawGraphOutputLabel(outputTypeGraphics, outputText);
                outputTypeGraphics.Dispose();
            }
        }

        protected void UpdateGraphInputCombo()
        {
            this.DataFieldsInGraphInputCombo.Clear();
            this.graphInputTypeCombo.Items.Clear();

            if (this.activeMappingInfo.GetActiveMappingExists())
            {
                this.graphInputTypeCombo.Enabled = true;

                MssMsgType inMsgType = (MssMsgType) (-1);
                MssMsgDataField primaryInputSource = (MssMsgDataField) (-1);
                this.activeMappingInfo.GetActiveGraphableEntryManager().RunFuncOnMappingEntry(this.activeMappingInfo.ActiveGraphableEntryId,
                    mappingEntry =>
                    {
                        inMsgType = mappingEntry.InMssMsgRange.MsgType;
                        primaryInputSource = mappingEntry.PrimaryInputSource;
                    });

                IMssMsgInfo inputInfo =
                    this.msgInfoFactory.Create(inMsgType);
                MssMsgDataField[] dataFieldArray = 
                    (MssMsgDataField[])Enum.GetValues(typeof(MssMsgDataField));

                foreach (MssMsgDataField dataField in dataFieldArray)
                {
                    string dataFieldName = inputInfo.GetDataFieldName(dataField);
                    if (dataFieldName != StaticMssMsgInfo.DATA_NAME_UNUSED)
                    {
                        this.DataFieldsInGraphInputCombo.Add(dataField);
                        this.graphInputTypeCombo.Items.Add(dataFieldName);

                        if (primaryInputSource == dataField)
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
            else
            {
                this.graphInputTypeCombo.Enabled = false;
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
                eventArgs.Item.Focused = true;
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
                eventArgs.Item.Focused = true;
                IgnoreGraphableEntrySelectionChangedHandler = false;
            }

            this.activeMappingInfo.ActiveGraphableEntryType = mappingType;

            this.activeMappingInfo.ActiveGraphableEntryId = this.activeMappingInfo.GetActiveGraphableEntryManager().GetMappingEntryIdByIndex(modifiedListView.SelectedItems[0].Index);
            OnActiveGraphableEntryChanged();
        }

        protected void UpdateInfoForParameter(MssParameterID paramID)
        {
            if (MssParameters.PRESET_PARAM_ID_LIST.Contains(paramID) && this.mssParameters.GetActiveMappingExists() == false) {
                return;
            }

            MssParamInfo paramInfo = this.mssParameters.GetParameterInfoCopy(paramID);
            MssParameters_NameChanged(paramID, paramInfo.Name);
            MssParameters_ValueChanged(paramID, paramInfo.GetValue());
            MssParameters_MaxValueChanged(paramID, paramInfo.MaxValue);
            MssParameters_MinValueChanged(paramID, paramInfo.MinValue);
        }

        protected void OnIdleProcessing()
        {
            this.commandQueue.DoAllCommands();

            //Signal anything blocking the GUI thread to do its processing.
            this.idleProcessingSignal.Set();
        }

        private void addMappingBtn_Click(object sender, System.EventArgs e)
        {
            //Wait for the host to be idle before proceeding.
            this.idleProcessingSignal.WaitOne();

            using (MappingDlg mapDlg = new MappingDlg())
            {
                mapDlg.Init(new MappingEntry(),
                            false,
                            this.msgMetadataFactory,
                            this.msgInfoFactory,
                            this.dryMssEventOutputPort);

                if (mapDlg.ShowDialog(this) == DialogResult.OK)
                {
                    int newId = this.mappingMgr.AddMappingEntry(mapDlg.mappingEntry);

                    this.activeMappingInfo.ActiveGraphableEntryType = GraphableEntryType.Mapping;
                    this.activeMappingInfo.ActiveGraphableEntryId = newId;

                    OnActiveGraphableEntryChanged();
                }
            }
        }

        private void editMappingBtn_Click(object sender, System.EventArgs e)
        {
            //Wait for the host to be idle before proceeding.
            this.idleProcessingSignal.WaitOne();

            IMappingEntry activeMappingEntryCopy = this.activeMappingInfo.GetActiveMappingCopy();

            using (MappingDlg mapDlg = new MappingDlg())
            {
                mapDlg.Init(activeMappingEntryCopy,
                            true,
                            this.msgMetadataFactory,
                            this.msgInfoFactory,
                            this.dryMssEventOutputPort);

                if (mapDlg.ShowDialog(this) == DialogResult.OK)
                {
                    this.activeMappingInfo.GetActiveMappingManager().ReplaceMappingEntry(activeMappingEntryCopy);

                    RefreshMappingListView();
                    //The equation curve needs to be updated incase the equation uses data1 or data2 
                    //and the input range for these has changed.
                    this.commandQueue.EnqueueCommandOverwriteDups(
                        EditorCommandId.UpdateEquationCurve, () => UpdateEquationCurve());
                }
            }
        }

        private void deleteMappingBtn_Click(object sender, EventArgs e)
        {
            if (this.activeMappingInfo.GetActiveMappingExists() == false ||
                this.activeMappingInfo.ActiveGraphableEntryType != GraphableEntryType.Mapping)
            {
                //The delete button should be disabled if there is no ActiveGraphableEntry or if the 
                //active mapping is not in the mapping list view.
                Debug.Assert(false);
                return;
            }

            deleteGraphableMappingEntry(this.mappingMgr, this.mappingListView);
        }

        private void deleteGeneratorBtn_Click(object sender, EventArgs e)
        {
            if (this.activeMappingInfo.GetActiveMappingExists() == false ||
                this.activeMappingInfo.ActiveGraphableEntryType != GraphableEntryType.Generator)
            {
                //The delete button should be disabled if there is no ActiveGraphableEntry or if the 
                //active mapping is not in the mapping list view.
                Debug.Assert(false);
                return;
            }

            deleteGraphableMappingEntry(this.genMappingMgr, this.generatorListView);
            
        }

        private void deleteGraphableMappingEntry(IBaseGraphableMappingManager graphableMappingManager, ListView graphableEntryListView)
        {
            int activeEntryId = this.activeMappingInfo.ActiveGraphableEntryId;

            int activeEntryIndex = graphableMappingManager.GetMappingEntryIndexById(activeEntryId);

            graphableMappingManager.RemoveMappingEntry(activeEntryId);
            graphableEntryListView.Items[activeEntryIndex].Remove();

            if (graphableEntryListView.Items.Count > activeEntryIndex)
            {
                this.activeMappingInfo.ActiveGraphableEntryId = graphableMappingManager.GetMappingEntryIdByIndex(activeEntryIndex);
            }
            else if (graphableEntryListView.Items.Count > 0)
            {
                this.activeMappingInfo.ActiveGraphableEntryId = graphableMappingManager.GetMappingEntryIdByIndex(graphableEntryListView.Items.Count - 1);
            }
            else
            {
                this.activeMappingInfo.ActiveGraphableEntryId = -1;
            }

            OnActiveGraphableEntryChanged();
        }

        private void mappingListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            GraphableEntrySelectionChanged((ListView)sender, GraphableEntryType.Mapping, e);
        }

        private void addGeneratorBtn_Click(object sender, System.EventArgs e)
        {
            //Wait for the host to be idle before proceeding.
            this.idleProcessingSignal.WaitOne();

            using (GeneratorDlg genDlg = new GeneratorDlg())
            {
                GenEntryConfigInfo genInfo = new GenEntryConfigInfo();
                genInfo.InitWithDefaultValues();
                genDlg.Init(genInfo);

                if (genDlg.ShowDialog() == DialogResult.OK)
                {
                    //Creates a new mapping entry, adds it the the generator mapping manager and sets
                    //it as the active mapping
                    int entryId = this.genMappingMgr.CreateAndAddEntryFromGenInfo(genDlg.GenInfoResult);
                    this.activeMappingInfo.ActiveGraphableEntryType = GraphableEntryType.Generator;
                    this.activeMappingInfo.ActiveGraphableEntryId = entryId;

                    OnActiveGraphableEntryChanged();
                }
            }
        }

        private void editGeneratorBtn_Click(object sender, EventArgs e)
        {
            //Wait for the host to be idle before proceeding.
            this.idleProcessingSignal.WaitOne();

            int activeId = this.activeMappingInfo.ActiveGraphableEntryId;

            Debug.Assert(activeId >= 0 && this.activeMappingInfo.ActiveGraphableEntryType == GraphableEntryType.Generator);

            IReturnStatus<IGeneratorMappingEntry> activeMappingStatus =
                this.genMappingMgr.GetCopyOfMappingEntryById(activeId);

            if (activeMappingStatus.IsValid == false)
            {
                Debug.Assert(false);
                return;
            }
            IGeneratorMappingEntry activeMapping = activeMappingStatus.Value;

            using (GeneratorDlg genDlg = new GeneratorDlg())
            {
                genDlg.Init(activeMapping.GenConfigInfo);

                if (genDlg.ShowDialog(this) == DialogResult.OK)
                {
                    this.genMappingMgr.UpdateEntryWithNewGenInfo(genDlg.GenInfoResult, activeMapping.Id);
                    RefreshGeneratorListView();
                }
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
                    paramNameLabel.Text = CustomStringUtil.CreateStringWithMaxWidth(
                        name, paramNameLabel.Width, paramNameLabel.Font);
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

            MssParamInfo paramInfo = mssParameters.GetParameterInfoCopy(paramId);

            //Update the knob
            LBKnob knob;
            if (ParameterValueKnobControlDict.TryGetRightByLeft(paramId, out knob) == true)
            {
                double knobValue = GetRoundedKnobValueAsDouble(knob);


                if (knobValue != paramInfo.RawValue)
                {
                    knob.Value = (float)value;
                }
            }

            //Update the value label
            Label parameterValueDisplay;
            if (ParameterValueLabelControlDict.TryGetRightByLeft(paramId, out parameterValueDisplay) == true)
            {
                string valueString = paramInfo.GetValueAsString();
                if (valueString != parameterValueDisplay.Text)
                {
                    parameterValueDisplay.Text = CustomStringUtil.CreateStringWithMaxWidth(
                        valueString, parameterValueDisplay.Width, parameterValueDisplay.Font);
                }
            }

            //Regenerate the curve to reflect the changed value
            this.commandQueue.EnqueueCommandOverwriteDups(
                EditorCommandId.UpdateEquationCurve, () => UpdateEquationCurve());
        }

        private void MssParameters_MinValueChanged(MssParameterID paramId, double minValue)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<MssParameterID, double>(MssParameters_MinValueChanged), paramId, minValue);
                return;
            }

            LBKnob knob;
            if (ParameterValueKnobControlDict.TryGetRightByLeft(paramId, out knob) == true)
            {
                knob.MinValue = (float)minValue;
            }
        }

        private void MssParameters_MaxValueChanged(MssParameterID paramId, double maxValue)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<MssParameterID, double>(MssParameters_MaxValueChanged), paramId, maxValue);
                return;
            }

            LBKnob knob;
            if (ParameterValueKnobControlDict.TryGetRightByLeft(paramId, out knob) == true)
            {
                knob.MaxValue = (float)maxValue;
            }
        }

        private void curveEquationTextBox_TextChanged(object sender, System.EventArgs e)
        {
            if (ignoreEquationTextBoxChangeHandlers == false && this.activeMappingInfo.GetActiveMappingExists())
            {
                int activeId = this.activeMappingInfo.ActiveGraphableEntryId;
                IBaseGraphableMappingManager activeManager = this.activeMappingInfo.GetActiveGraphableEntryManager();
                activeManager.RunFuncOnMappingEntry(activeId, (mappingEntry) => 
                {
                    CurveShapeInfo curveInfo = mappingEntry.CurveShapeInfo;

                    Debug.Assert(curveInfo.SelectedEquationType == EquationType.Curve);

                    string expressionString = this.curveEquationTextBox.Text;
                    int curCurve = curveInfo.SelectedEquationIndex;
                    curveInfo.CurveEquations[curCurve] = expressionString;                
                });

                this.commandQueue.EnqueueCommandOverwriteDups(
                    EditorCommandId.UpdateEquationCurve, () => UpdateEquationCurve());
            }
        }

        private void pointEquationTextBox_TextChanged(object sender, System.EventArgs e)
        {
            if (ignoreEquationTextBoxChangeHandlers == false && this.activeMappingInfo.GetActiveMappingExists())
            {
                int activeId = this.activeMappingInfo.ActiveGraphableEntryId;
                IBaseGraphableMappingManager activeManager = this.activeMappingInfo.GetActiveGraphableEntryManager();
                activeManager.RunFuncOnMappingEntry(activeId, (mappingEntry) =>
                {
                    CurveShapeInfo curveInfo = mappingEntry.CurveShapeInfo;

                    string xExpressionString = this.pointXEquationTextBox.Text;
                    string yExpressionString = this.pointYEquationTextBox.Text;
                    int curCurve = curveInfo.SelectedEquationIndex;
                    curveInfo.PointEquations[curCurve].X = xExpressionString;
                    curveInfo.PointEquations[curCurve].Y = yExpressionString;
                });

                this.commandQueue.EnqueueCommandOverwriteDups(
                    EditorCommandId.UpdateEquationCurve, () => UpdateEquationCurve());
            }
        }

        protected void UpdateEquationCurve()
        {
            if (this.activeMappingInfo.GetActiveMappingExists())
            {
                var mappingManager = this.activeMappingInfo.GetActiveGraphableEntryManager();
                int activeMappingId = this.activeMappingInfo.ActiveGraphableEntryId;

                Logger.HighVolume(3, string.Format("Updating equation curve for mapping with id: {0}", activeMappingId));

                IMappingEntry activeMappingEntryCopy = mappingManager.GetCopyOfMappingEntryById(activeMappingId).Value;
                CurveShapeInfo curveInfoCopy = activeMappingEntryCopy.CurveShapeInfo;

                Logger.HighVolume(22, string.Format(
                    "Updating equation curve. Curve equations: {0}. Point equations: {1}. Equation type: {2}",
                    EnumerableUtils.ToString(curveInfoCopy.CurveEquations), EnumerableUtils.ToString(curveInfoCopy.PointEquations), curveInfoCopy.SelectedEquationType.ToString()));


                GraphPane graphPane = this.mainGraphControl.GraphPane;

                LineItem pointsCurve = (LineItem)graphPane.CurveList.Find(
                        curveItem => curveItem.Label.Text == GRAPH_CONTROL_POINTS_LABEL);
                if (pointsCurve == null)
                {
                    Logger.Verbose(4, "Points curve not found! Creating one.");
                    pointsCurve = EqGraphConfig.CreadControlPointsCurve(GRAPH_CONTROL_POINTS_LABEL);
                    graphPane.CurveList.Add(pointsCurve);
                }
                IPointListEdit pointsCurveEdit = (IPointListEdit)pointsCurve.Points;

                List<XyPoint<double>> pointList;
                List<List<XyPoint<double>>> curvePointsByCurveList;
                HashSet<int> erroneousControlPointIndexSet;
                HashSet<int> erroneousCurveIndexSet;

                bool allEquationsAreValid = this.evaluator.SampleExpressionWithDefaultInputValues(
                        1.0 / ((double)NUM_GRAPH_POINTS - 1.0),
                        this.mssParameters.GetVariableParamInfoList(),
                        activeMappingEntryCopy,
                        out pointList,
                        out curvePointsByCurveList,
                        out erroneousControlPointIndexSet,
                        out erroneousCurveIndexSet);

                if (curveInfoCopy.AllEquationsAreValid != allEquationsAreValid) {
                    curveInfoCopy.AllEquationsAreValid = allEquationsAreValid;
                    mappingManager.RunFuncOnMappingEntry(activeMappingId, (mappingEntry) =>
                    {
                        Logger.Verbose(5, String.Format("AllEquationsAreValid changed to: {0}", allEquationsAreValid));
                        mappingEntry.CurveShapeInfo.AllEquationsAreValid = allEquationsAreValid;
                    });
                }

                if (pointsCurve.Points == null)
                {
                    Logger.Error(6, "pointsCurve.Points is null.");
                    return;
                }
                if (pointList == null)
                {
                    Logger.Error(7, "pointList is null.");
                    return;
                }
                Logger.HighVolume(8, string.Format("Num existing control points: {0}. Num control points after evaluation: {1}", pointsCurve.Points.Count, pointList.Count));

                if (curveInfoCopy.AllEquationsAreValid)
                {
                    //Update modified points and add new points
                    for(int i = 0; i < pointList.Count; i++)
                    {
                        XyPoint<double> curControlPoint = pointList[i];

                        if (i <= pointsCurve.Points.Count - 1)
                        {
                            Logger.HighVolume(9, string.Format("overriding point at index: {0}", i));
                            pointsCurve.Points[i].X = curControlPoint.X;
                            pointsCurve.Points[i].Y = curControlPoint.Y;
                            pointsCurve.Points[i].ColorValue = 0;
                        }
                        else
                        {
                            Logger.HighVolume(10, string.Format("Adding point at index: {0}", i));
                            pointsCurveEdit.Add(curControlPoint.X, curControlPoint.Y);
                        }
                    }
                    //Remove points. Itterate backwards so removing a point does not affect the 
                    //index of the next point.
                    for (int i = pointsCurve.Points.Count - 1; i >= pointList.Count; i--)
                    {
                        Logger.HighVolume(11, string.Format("Removing point at index: {0}", pointsCurve.Points.Count - 1 - i));
                        pointsCurveEdit.RemoveAt(i);
                    }

                    if(curveInfoCopy.SelectedEquationType == EquationType.Point)
                    {
                        pointsCurve.Points[curveInfoCopy.SelectedEquationIndex].ColorValue = 1;
                    }


                    RemoveEquationCurvesFromGraph();

                    for(int curveIndex = 0; curveIndex < curvePointsByCurveList.Count; curveIndex++)  {
                        List<XyPoint<double>> curCurvePoints = curvePointsByCurveList[curveIndex];

                        LineItem curCurve = EqGraphConfig.CreateEqCurve(EQUATION_CURVE_LABEL_PREFIX + "-" + curveIndex);
                        IPointListEdit curCurveEdit = (IPointListEdit)curCurve.Points;
                        

                        for (int curPointIndex = 0; curPointIndex < curCurvePoints.Count; curPointIndex++)
                        {
                            XyPoint<double> curPoint = curCurvePoints[curPointIndex];

                            //If Y values are outside of the range 0 to 1 then they will not be mapped
                            //So they should not be shown on the graph.
                            //Note: values can be NaN
                            if (!(curPoint.Y >= 0 && curPoint.Y <= 1))
                            {
                                if (curCurveEdit.Count > 0)
                                {
                                    graphPane.CurveList.Add(curCurve);
                                    curCurve = EqGraphConfig.CreateEqCurve(EQUATION_CURVE_LABEL_PREFIX + "-" + curveIndex + "-" + curPointIndex);
                                    curCurveEdit = (IPointListEdit)curCurve.Points;
                                }
                            }

                            curCurveEdit.Add(curPoint.X, curPoint.Y);
                        }

                        if (curCurveEdit.Count > 0)
                        {
                            graphPane.CurveList.Add(curCurve);
                        }

                    }

                }

                for (int i = 0; i < pointsCurve.Points.Count; i++)
                {
                    GraphSegmentColorStausFlags curPointColorStatus = GraphSegmentColorStausFlags.None;

                    if (curveInfoCopy.SelectedEquationType == EquationType.Point &&
                        i == curveInfoCopy.SelectedEquationIndex)
                    {
                        curPointColorStatus |= GraphSegmentColorStausFlags.Selected;
                    }

                    if (curveInfoCopy.AllEquationsAreValid) 
                    {
                        curPointColorStatus |= GraphSegmentColorStausFlags.Enabled;
                    }

                    if (erroneousControlPointIndexSet.Contains(i)) 
                    {
                        curPointColorStatus |= GraphSegmentColorStausFlags.Erroneous;
                    }

                    EqGraphConfig.SetControlPointColorStatus(pointsCurve.Points[i], curPointColorStatus);

                }


                List<CurveItem> eqCurveList = graphPane.CurveList.FindAll(curveItem => curveItem.Label.Text.StartsWith(EQUATION_CURVE_LABEL_PREFIX));
                foreach (CurveItem curEqCurve in eqCurveList)
                {
                    int curIndex = int.Parse(curEqCurve.Label.Text.Split('-')[1]);

                    GraphSegmentColorStausFlags curEqCurveColorStatus = GraphSegmentColorStausFlags.None;

                    if (curveInfoCopy.SelectedEquationType == EquationType.Curve &&
                        curIndex == curveInfoCopy.SelectedEquationIndex)
                    {
                        curEqCurveColorStatus |= GraphSegmentColorStausFlags.Selected;
                    }

                    if (curveInfoCopy.AllEquationsAreValid)
                    {
                        curEqCurveColorStatus |= GraphSegmentColorStausFlags.Enabled;
                    }

                    if (erroneousCurveIndexSet.Contains(curIndex))
                    {
                        curEqCurveColorStatus |= GraphSegmentColorStausFlags.Erroneous;
                    }

                    EqGraphConfig.SetEqCurveColorStatus((LineItem)curEqCurve, curEqCurveColorStatus);
                }

            }
            else //this.ActiveGraphableEntry is null
            {
                this.mainGraphControl.GraphPane.CurveList.Clear();
            }

            this.mainGraphControl.Invalidate();
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

        protected void repopulateProgramsList()
        {
            repopulateSettingsFileList(this.programList,
                                       this.programMgr,
                                       onProgramClicked);
        }

        protected void repopulateTransformPresetList()
        {
            repopulateSettingsFileList(this.curvePresetList,
                                       this.transformPresetMgr,
                                       onTransformPresetClicked);
        }

        protected void repopulateSettingsFileList(
                ToolStripDropDownButton settingsList, 
                BaseSettingsFileMgr settingsFileMgr,
                EventHandler itemClickedHandler)
        {
            if (settingsFileMgr.ActiveSettingsFileName != null)
            {
                settingsList.Text = CustomStringUtil.CreateStringWithMaxWidth(
                        settingsFileMgr.ActiveSettingsFileName,
                        settingsList.Width - 10,
                        settingsList.Font);
            }
            else
            {
                settingsList.Text = "";
            }
            settingsList.DropDownItems.Clear();

            populateSettingsFileList(settingsList.DropDownItems, 
                                     settingsFileMgr.SettingsFileTree, 
                                     itemClickedHandler);
        }

        protected void populateSettingsFileList(
                ToolStripItemCollection dropDownItems,
                FileTreeFolderNode<SettingsFileInfo> programTreeNode,
                EventHandler itemClickedHandler)
        {
            foreach (SettingsFileInfo program in programTreeNode.ChildFileList)
            {
                var programMenuItem = new ToolStripMenuItem(program.Name);
                programMenuItem.Tag = program;
                programMenuItem.Click += new EventHandler(itemClickedHandler);
                dropDownItems.Add(programMenuItem);
            }

            foreach (FileTreeFolderNode<SettingsFileInfo> childNode in programTreeNode.ChildFolderList)
            {
                var programFolder = new ToolStripMenuItem(childNode.NodeName);
                dropDownItems.Add(programFolder);
                populateSettingsFileList(programFolder.DropDownItems, childNode, itemClickedHandler);
            }
        }

        private void onProgramClicked(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
            this.programMgr.LoadAndActivateSettingsFromSettingsFileInfo((SettingsFileInfo)menuItem.Tag);
        }

        private void onTransformPresetClicked(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
            this.transformPresetMgr.LoadAndActivateSettingsFromSettingsFileInfo((SettingsFileInfo)menuItem.Tag);
            OnActiveCurveShapeChanged();
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

        private void moveMappingUpBtn_Click(object sender, EventArgs e)
        {
            if (this.activeMappingInfo.GetActiveMappingExists() == false ||
                this.activeMappingInfo.ActiveGraphableEntryType != GraphableEntryType.Mapping ||
                this.activeMappingInfo.GetActiveMappingManager().GetMappingEntryIndexById(this.activeMappingInfo.ActiveGraphableEntryId) <= 0)
            {
                //The move up button should be disabled if there is no ActiveGraphableEntry, 
                //if the ActiveGraphableEntry is not in the mapping list view or if the 
                //ActiveGraphableEntry cannot be moved up.
                Debug.Assert(false);
                return;
            }

            this.mappingMgr.MoveEntryUp(this.activeMappingInfo.ActiveGraphableEntryId);
            RefreshMappingListView();
            UpdateGraphableEntryButtonsEnabledStatus();
        }

        private void moveMappingDownBtn_Click(object sender, EventArgs e)
        {
            if (this.activeMappingInfo.GetActiveMappingExists() == false ||
                this.activeMappingInfo.ActiveGraphableEntryType != GraphableEntryType.Mapping)
            {
                //The move up button should be disabled if there is no ActiveGraphableEntry, 
                //or if the ActiveGraphableEntry is not in the mapping list view.
                Debug.Assert(false);
                return;
            }

            this.mappingMgr.MoveEntryDown(this.activeMappingInfo.ActiveGraphableEntryId);
            RefreshMappingListView();
            UpdateGraphableEntryButtonsEnabledStatus();
        }

        private void saveProgram_Click(object sender, EventArgs e)
        {
            this.programMgr.SaveActiveSettingsFile();
            repopulateProgramsList();
        }

        private void saveProgramAs_Click(object sender, EventArgs e)
        {
            this.programMgr.SaveAsActiveSettingsFile();
            repopulateProgramsList();
        }

        private void openProgram_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = this.programMgr.GetSettingsFileFilter();
            dlg.InitialDirectory = MssFileSystemLocations.UserProgramsFolder;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                this.programMgr.LoadAndActivateSettingsFromPath(dlg.FileName);
            }
        }

        private void saveTransformPreset_Click(object sender, EventArgs e)
        {
            this.transformPresetMgr.SaveActiveSettingsFile();
            repopulateTransformPresetList();
        }

        private void saveTransformPresetAs_Click(object sender, EventArgs e)
        {
            this.transformPresetMgr.SaveAsActiveSettingsFile();
            repopulateTransformPresetList();
        }

        private void openTransformPreset_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = this.transformPresetMgr.GetSettingsFileFilter();
            dlg.InitialDirectory = MssFileSystemLocations.UserTransformPresetFolder;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                this.transformPresetMgr.LoadAndActivateSettingsFromPath(dlg.FileName);
                OnActiveCurveShapeChanged();

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

            Logger.Info(12, string.Format("Equation graph clicked at ({0}, {1}).", e.X, e.Y));
            


            IBaseGraphableMappingManager activeMappingManager = this.activeMappingInfo.GetActiveGraphableEntryManager();
            int activeId = this.activeMappingInfo.ActiveGraphableEntryId;

            activeMappingManager.RunFuncOnMappingEntry(activeId, (activeMapping) =>
            {
                Logger.Verbose(13, string.Format("Clicked on mapping with id: {0}", activeId));
                
                CurveShapeInfo curveInfo = activeMapping.CurveShapeInfo;

                if (this.activeMappingInfo.GetActiveMappingExists() && curveInfo.AllEquationsAreValid)
                {
                    Point mousePt = new Point(e.X, e.Y);

                    GraphPane pane = this.mainGraphControl.GraphPane;


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
                        if (controlPointsCurve == null || controlPointsCurve.Points == null)
                        {
                            Logger.Error(20, "Found existing point but controlPointsCurve was null");
                            return;
                        }

                        Logger.Info(14, string.Format("Editing existing point. Num control points: {0}. Clicked control point index: {1}", controlPointsCurve.Points.Count, nearestPointIndex));
                        this.mainGraphControl.StartEditing(pane, mousePt, controlPointsCurve, nearestPointIndex);
                        return;
                    }

                    //If the click event occured in the chart
                    if (newPoint.X >= 0 && newPoint.X <= 1 && newPoint.Y >= 0 && newPoint.Y <= 1)
                    {
                        int pointBeforeNewPointIndex = -1;
                        int pointAfterNewPointIndex = -1;

                        Debug.Assert(controlPointsCurve.Points.Count == curveInfo.PointEquations.Count);
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
                            this.mssParameters.GetVariableParamInfoList(),
                            activeMapping);
                        ReturnStatus<double> evalReturnStatus = this.evaluator.Evaluate(evalInput);

                        //The returned value could be nan if the equation is something like "ignore"
                        if (evalReturnStatus.IsValid == false || double.IsNaN(evalReturnStatus.Value))
                        {
                            Logger.Info(15, "Cannot add contrl point. Could be clicking on a function like 'ignore'.");
                            return;
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
                        Logger.StartLoggingHighVolume(16, string.Format(
                            "Calling UpdateCurveShapeControls. Num curve equations: {0}. Num point equations: {1}. Num control points: {2}", 
                            curveInfo.CurveEquations.Count, curveInfo.PointEquations.Count, controlPointsCurve.Points.Count));
                        
                        UpdateCurveShapeControls();
                        
                        Logger.StopLoggingHighVolume(17, string.Format(
                            "Returning from UpdateCurveShapeControls. Num curve equations: {0}. Num point equations: {1}. Num control points: {2}", 
                            curveInfo.CurveEquations.Count, curveInfo.PointEquations.Count, controlPointsCurve.Points.Count));

                        //Ensures that UpdateCurveShapeControls() added the required control point.
                        if (controlPointsCurve.Points.Count <= pointBeforeNewPointIndex + 1)
                        {
                            Logger.Error(18, string.Format("Trying to edit point at {0} but only {1} point(s) exist.", pointBeforeNewPointIndex + 1, controlPointsCurve.Points.Count));
                            Debug.Assert(false);
                            return;
                        }

                        Logger.Info(19, "Editing new control point");
                        this.mainGraphControl.StartEditing(pane, mousePt, controlPointsCurve, pointBeforeNewPointIndex + 1);
                    }
                }
            });

            return true;
        }

        private bool mainGraphControl_EditDragEvent(ZedGraphControl sender, 
                                                    PointPair newPointPosition, 
                                                    int pointBeingEditedIndex, 
                                                    CurveItem curveBeingEdited)
        {
            if (this.activeMappingInfo.GetActiveMappingExists() == false)
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

            this.activeMappingInfo.GetActiveGraphableEntryManager().RunFuncOnMappingEntry(this.activeMappingInfo.ActiveGraphableEntryId,
                (mappingEntry) =>
                {
                    CurveShapeInfo curveInfo = mappingEntry.CurveShapeInfo;
                    curveInfo.PointEquations[pointBeingEditedIndex].X =
                        Math.Round(newPointPosition.X, NUM_DECIMALS_IN_CONTROL_POINT).ToString();
                    curveInfo.PointEquations[pointBeingEditedIndex].Y =
                        Math.Round(newPointPosition.Y, NUM_DECIMALS_IN_CONTROL_POINT).ToString();
                });

            IPointListEdit pointsList = (IPointListEdit)curveBeingEdited.Points;
            pointsList[pointBeingEditedIndex] = newPointPosition;

            this.commandQueue.EnqueueCommandOverwriteDups(
                EditorCommandId.UpdateCurveShapeControls, () => UpdateCurveShapeControls());

            //Returning false tells zedgraphs that we have already handled the drag event. We 
            //handel it in this method instead of letting zedgraphs handel it because zedgraphs 
            //would immeadateally force a redraw which we don't want to do until the host is idle.
            return false;
        }

        private void ZedGraphContextMenuBuilder(ZedGraphControl control,
            ContextMenuStrip menuStrip, Point mousePt,
            ZedGraphControl.ContextMenuObjectState objState)
        {
            menuStrip.Items.Clear();

            if (this.activeMappingInfo.GetActiveMappingExists())
            {
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

                this.activeMappingInfo.GetActiveGraphableEntryManager().RunFuncOnMappingEntry(this.activeMappingInfo.ActiveGraphableEntryId,
                    (mappingEntry) => item.Enabled = controlPointClicked && mappingEntry.CurveShapeInfo.AllEquationsAreValid);

                DeletePointParams deletePointParams = new DeletePointParams();
                deletePointParams.pointIndex = nearestPointIndex;
                item.Tag = deletePointParams;

                // Add a handler that will respond when that menu item is selected
                item.Click += new System.EventHandler(ZedGraph_DeletePoint);
                menuStrip.Items.Add(item);
            }
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
            if (this.activeMappingInfo.GetActiveMappingExists() == false)
            {
                //This menu should be be possible to click when there is not active graphable 
                //entry.
                Debug.Assert(false);
                return;
            }
            
            ToolStripDropDownItem item = (ToolStripDropDownItem)sender;
            DeletePointParams deletePointParams = (DeletePointParams)item.Tag;

            IBaseGraphableMappingManager activeMappingManager = this.activeMappingInfo.GetActiveGraphableEntryManager();
            int activeMappingId = this.activeMappingInfo.ActiveGraphableEntryId;

            activeMappingManager.RunFuncOnMappingEntry(activeMappingId, (activeMappingEntry) => {
                CurveShapeInfo curveInfo = activeMappingEntry.CurveShapeInfo;

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
            });            

            this.commandQueue.EnqueueCommandOverwriteDups(
                EditorCommandId.UpdateCurveShapeControls, () => UpdateCurveShapeControls());
        }

        private void nextEquationBtn_Click(object sender, EventArgs e)
        {
            if (this.activeMappingInfo.GetActiveMappingExists())
            {
                IBaseGraphableMappingManager activeMappingManager = this.activeMappingInfo.GetActiveGraphableEntryManager();
                int activeMappingId = this.activeMappingInfo.ActiveGraphableEntryId;

                activeMappingManager.RunFuncOnMappingEntry(activeMappingId, (activeMappingEntry) => {
                    CurveShapeInfo curveInfo = activeMappingEntry.CurveShapeInfo;

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
                });

                this.commandQueue.EnqueueCommandOverwriteDups(
                    EditorCommandId.UpdateCurveShapeControls, () => UpdateCurveShapeControls());
            }
            else
            {
                //This button should be disabled when there is no ActiveGraphableEntry
                Debug.Assert(false);
            }
        }

        private void prevEquationBtn_Click(object sender, EventArgs e)
        {
            if (this.activeMappingInfo.GetActiveMappingExists())
            {
                IBaseGraphableMappingManager activeMappingManager = this.activeMappingInfo.GetActiveGraphableEntryManager();
                int activeMappingId = this.activeMappingInfo.ActiveGraphableEntryId;

                activeMappingManager.RunFuncOnMappingEntry(activeMappingId, (activeMappingEntry) => {
                    CurveShapeInfo curveInfo = activeMappingEntry.CurveShapeInfo;

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
                });

                this.commandQueue.EnqueueCommandOverwriteDups(
                    EditorCommandId.UpdateCurveShapeControls, () => UpdateCurveShapeControls());
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
            if (this.activeMappingInfo.GetActiveMappingExists() == false)
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
                IBaseGraphableMappingManager activeMappingManager = this.activeMappingInfo.GetActiveGraphableEntryManager();
                int activeMappingId = this.activeMappingInfo.ActiveGraphableEntryId;

                activeMappingManager.RunFuncOnMappingEntry(activeMappingId, (activeMappingEntry) =>
                {
                    CurveShapeInfo curveInfo = activeMappingEntry.CurveShapeInfo;

                    curveInfo.CurveEquations.Clear();

                    curveInfo.CurveEquations.Add(CurveShapeInfo.DEFAULT_EQUATION);
                    curveInfo.PointEquations.Clear();

                    curveInfo.SelectedEquationIndex = 0;
                    curveInfo.SelectedEquationType = EquationType.Curve;
                });

                this.commandQueue.EnqueueCommandOverwriteDups(
                    EditorCommandId.UpdateCurveShapeControls, () => UpdateCurveShapeControls());
            }
        }

        private void graphInputTypeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ignoreInputTypeComboSelectionChangeHandlers == false &&
                this.activeMappingInfo.GetActiveMappingExists())
            {
                ComboBox inputTypeCombo = (ComboBox)sender;

                this.activeMappingInfo.GetActiveMappingManager().RunFuncOnMappingEntry(this.activeMappingInfo.ActiveGraphableEntryId, 
                    (mappingEntry) => mappingEntry.PrimaryInputSource = this.DataFieldsInGraphInputCombo[inputTypeCombo.SelectedIndex]);

                this.commandQueue.EnqueueCommandOverwriteDups(
                    EditorCommandId.UpdateCurveShapeControls, () => UpdateCurveShapeControls());
            }
        }

        private void editParamMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripDropDownItem editItem = (ToolStripDropDownItem)sender;
            ContextMenuStrip contextMenu = (ContextMenuStrip)editItem.Owner;
            Control paramControl = contextMenu.SourceControl;

            MssParameterID paramId = this.ParameterAllControlsDict[paramControl];
            MssParamInfo paramInfo = this.mssParameters.GetParameterInfoCopy(paramId);

            ParameterEditor paramEditorDlg = new ParameterEditor();
            paramEditorDlg.Init(paramInfo);


            if (paramEditorDlg.ShowDialog(this) == DialogResult.OK)
            {
                this.mssParameters.SetParamInfo(paramId, paramEditorDlg.resultParamInfo);

                //The mapping ListView needs to be updated as it may display parameter names.
                RefreshMappingListView();
            }
        }

        private void aboutPage_Click(object sender, EventArgs e)
        {
            AboutPage aboutPage = new AboutPage();
            aboutPage.ShowDialog(this);
        }

        private void EquationTextBox_Enter(object sender, EventArgs e)
        {
            RunEquationEditor((TextBox)sender, "");
        }
        
        protected void RunEquationEditor(TextBox equationBox, string insertCharacter)
        {
            //Need to run the dialog on another thread so that a keypress can be supressed:
            //http://stackoverflow.com/questions/9326508/in-keydown-a-showdialog-makes-suppresskeypress-not-work
            this.BeginInvoke(new Action(() => {
                using (EquationEditorDlg equationDlg = new EquationEditorDlg())
                {
                    equationDlg.Init(equationBox.Text, equationBox.SelectionStart, insertCharacter, this);
                    if (equationDlg.ShowDialog() == DialogResult.OK)
                    {
                        equationBox.Text = equationDlg.equation;
                        equationBox.SelectionStart = equationDlg.cursorPosition;
                    }
                }
            }));
        }

        private void curveEquationTextBox_Click(object sender, EventArgs e)
        {
            //If the textbox is not focused then the Enter event handler will handel this.
            if (((TextBox)sender).Focused == true) 
            {
                RunEquationEditor((TextBox)sender, "");
            }
        }

        private void EquationTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey)
            {
                RunEquationEditor((TextBox)sender, "");
            }
        }

        private void EquationTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            //backspace triggers keypress but delete does not so we just let backspace 
            //be handeled automatically.
            if (e.KeyChar != '\b')
            {
                e.Handled = true;
                RunEquationEditor((TextBox)sender, e.KeyChar.ToString());
            }
        }

        private void onlineHelp_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://code.google.com/p/midi-shape-shifter/wiki/OnlineDocumentation");
        }

    }
}

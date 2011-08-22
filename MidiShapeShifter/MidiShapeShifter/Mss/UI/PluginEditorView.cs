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
using MidiShapeShifter.Mss.Generator;
using MidiShapeShifter.Mss.Relays;

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

        //Injected dependencies
        protected MssParameters mssParameters;
        protected MappingManager mappingMgr;
        protected GeneratorMappingManager genMappingMgr;
        protected IDryMssEventOutputPort dryMssEventOutputPort;

        protected enum GraphableEntryType {Mapping, Generator}

        protected int ActiveGraphableEntryIndex = -1;
        protected GraphableEntryType ActiveGraphableEntryType;

        public MappingEntry ActiveGraphableEntry 
        {
            get
            {
                if (this.ActiveGraphableEntryIndex < 0)
                {
                    return null;
                }
                else
                {
                    MappingEntry activeEntry;

                    if (this.ActiveGraphableEntryType == GraphableEntryType.Mapping) 
                    {
                        activeEntry = this.mappingMgr.GetMappingEntry(this.ActiveGraphableEntryIndex);
                    }
                    else if (this.ActiveGraphableEntryType == GraphableEntryType.Generator)
                    {
                        activeEntry = this.genMappingMgr.GetGenMappingEntryByIndex(this.ActiveGraphableEntryIndex);
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

            evaluator = new MssEvaluator();
        }

        //static constructor
        static PluginEditorView()
        {
            for (int i = 0; i < NUM_GRAPH_POINTS; i++)
            {
                GRAPH_X_VAULES[i] = (1.0 / (NUM_GRAPH_POINTS - 1)) * i;
            }
        }

        public void Init(MssParameters mssParameters, MappingManager mappingMgr, GeneratorMappingManager genMappingMgr, IDryMssEventOutputPort dryMssEventOutputPort)
        {
            this.mssParameters = mssParameters;
            this.mappingMgr = mappingMgr;
            this.genMappingMgr = genMappingMgr;
            this.dryMssEventOutputPort = dryMssEventOutputPort;

            this.mssParameters.ParameterValueChanged += new ParameterValueChangedEventHandler(MssParameters_ValueChanged);
            this.mssParameters.ParameterNameChanged += new ParameterNameChangedEventHandler(MssParameters_NameChanged);
            this.mssParameters.ParameterMinValueChanged += new ParameterMinValueChangedEventHandler(MssParameters_MinValueChanged);
            this.mssParameters.ParameterMaxValueChanged += new ParameterMaxValueChangedEventHandler(MssParameters_MaxValueChanged);

            UpdateGraphableEntryButtonsEnabledStatus();
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

            if (this.ActiveGraphableEntryIndex < 0) 
            {
                EnableMappingButtons = false;
                EnableGeneratorButtons = false;
            }
            else if (this.ActiveGraphableEntryType == GraphableEntryType.Mapping)
            {
                EnableMappingButtons = true;
                EnableGeneratorButtons = false;
            }
            else if (this.ActiveGraphableEntryType == GraphableEntryType.Generator)
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

            //TODO: Enable.disable buttons
            this.deleteMappingBtn.Enabled = EnableMappingButtons;
            this.editMappingBtn.Enabled = EnableMappingButtons;
            this.moveMappingUpBtn.Enabled = EnableMappingButtons;
            this.moveMappingDownBtn.Enabled = EnableMappingButtons;

            this.deleteGeneratorBtn.Enabled = EnableGeneratorButtons;
            this.editGeneratorBtn.Enabled = EnableGeneratorButtons;
        }

        protected void RefreshMappingListView()
        {
            int selectionBackup = -1;

            if (this.mappingListView.SelectedIndices.Count > 0) 
            {
                selectionBackup = this.mappingListView.SelectedIndices[0];
            }

            this.mappingListView.Items.Clear();

            for (int i = 0; i < this.mappingMgr.GetNumEntries(); i++)
            {
                this.mappingListView.Items.Add(this.mappingMgr.GetListViewRow(i));
            }

            if (selectionBackup > -1)
            {
                this.mappingListView.Items[selectionBackup].Selected = true;
            }
        }

        protected void RefreshGeneratorListView()
        {
            int selectionBackup = -1;

            if (this.generatorListView.SelectedIndices.Count > 0)
            {
                selectionBackup = this.generatorListView.SelectedIndices[0];
            }

            this.generatorListView.Items.Clear();

            for (int i = 0; i < this.genMappingMgr.GetNumEntries(); i++)
            {
                this.generatorListView.Items.Add(this.genMappingMgr.GetListViewRow(i));
            }

            if (selectionBackup > -1)
            {
                this.generatorListView.Items[selectionBackup].Selected = true;
            }
        }

        //TODO: add handlers like this for changes to max and min value text boxes
        private void lbKnob_KnobChangeValue(object sender, LBSoft.IndustrialCtrls.Knobs.LBKnobEventArgs e) {
            LBKnob knob = (LBKnob)sender;
            MssParameterID paramId;
            ParameterValueKnobControlDict.TryGetLeftByRight(out paramId, knob);


            Label parameterValueDisplay;
            if (ParameterValueLabelControlDict.TryGetRightByLeft(paramId, out parameterValueDisplay) == true)
            {
                parameterValueDisplay.Text = FormatRawParameterValue((double)knob.Value);
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
            if (ActiveGraphableEntry.CurveShapeInfo.EqInputMode == EquationInputMode.Text)
            {
                this.curveShapeEquationRadio.Checked = true;
            }
            else if (ActiveGraphableEntry.CurveShapeInfo.EqInputMode == EquationInputMode.Preset)
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

            UpdateGraphableEntryButtonsEnabledStatus();
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

            ActiveGraphableEntryType = mappingType;
            ActiveGraphableEntryIndex = modifiedListView.SelectedItems[0].Index;
            ActiveGraphableEntryChanged();
        }

        private void addMappingBtn_Click(object sender, System.EventArgs e)
        {
            MappingDlg mapDlg = new MappingDlg();
            mapDlg.Init(new MappingEntry(), false);

            if (mapDlg.ShowDialog(this) == DialogResult.OK)
            {
                this.mappingMgr.AddMappingEntry(mapDlg.mappingEntry);

                int newestEntryIndex = this.mappingMgr.GetNumEntries() - 1;

                this.ActiveGraphableEntryType = GraphableEntryType.Mapping;
                this.ActiveGraphableEntryIndex = newestEntryIndex;

                this.mappingListView.Items.Add(this.mappingMgr.GetListViewRow(newestEntryIndex));
                this.mappingListView.Items[newestEntryIndex].Selected = true;
            }
        }

        private void editMappingBtn_Click(object sender, System.EventArgs e)
        {
            if (ActiveGraphableEntry == null || this.ActiveGraphableEntryType != GraphableEntryType.Mapping)
            {
                //The edit button should be disabled if there is no ActiveMapping or if the active 
                //mapping is not in the mapping list view.
                Debug.Assert(false);
                return;
            }

            MappingDlg mapDlg = new MappingDlg();
            mapDlg.Init(ActiveGraphableEntry, true);
            

            if (mapDlg.ShowDialog(this) == DialogResult.OK)
            {
                RefreshMappingListView();
            }
        }


        private void mappingListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            GraphableEntrySelectionChanged((ListView)sender, GraphableEntryType.Mapping, e);
        }

        private void addGeneratorBtn_Click(object sender, System.EventArgs e)
        {
            GeneratorDlg genDlg = new GeneratorDlg();
            GeneratorMappingEntryInfo genInfo = new GeneratorMappingEntryInfo();
            genInfo.InitWithDefaultValues();
            genDlg.Init(genInfo);

            if (genDlg.ShowDialog() == DialogResult.OK)
            {
                //Creates a new mapping entry, adds it the the generator mapping manager and sets
                //it as the active mapping
                this.genMappingMgr.CreateAndAddEntryFromGenInfo(genDlg.GenInfoResult);
                this.ActiveGraphableEntryType = GraphableEntryType.Generator;
                this.ActiveGraphableEntryIndex = this.genMappingMgr.GetNumEntries() - 1;

                this.generatorListView.Items.Add(this.genMappingMgr.GetListViewRow(this.ActiveGraphableEntryIndex));
                this.generatorListView.Items[this.ActiveGraphableEntryIndex].Selected = true;
            }
        }

        private void editGeneratorBtn_Click(object sender, EventArgs e)
        {
            if (ActiveGraphableEntry == null || this.ActiveGraphableEntryType != GraphableEntryType.Generator)
            {
                //The edit button should be disabled if there is no ActiveMapping or if the active 
                //mapping is not in the generator list view.
                Debug.Assert(false);
                return;
            }
            GeneratorMappingEntry activeGenMapping = 
                this.genMappingMgr.GetGenMappingEntryByIndex(this.ActiveGraphableEntryIndex);

            GeneratorDlg genDlg = new GeneratorDlg();
            genDlg.Init(activeGenMapping.GeneratorInfo);


            if (genDlg.ShowDialog(this) == DialogResult.OK)
            {
                this.genMappingMgr.UpdateEntryWithNewGenInfo(genDlg.GenInfoResult);
                RefreshGeneratorListView();
            }
        }

        private void MssParameters_NameChanged(MssParameterID paramId, string name)
        {
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
            LBKnob knob;
            if (ParameterValueKnobControlDict.TryGetRightByLeft(paramId, out knob) == true)
            {
                if (knob.Value != value)
                {
                    knob.Value = (float)value;
                }
            }

            Label parameterValueLabel;
            if (ParameterValueLabelControlDict.TryGetRightByLeft(paramId, out parameterValueLabel) == true)
            {
                string valueAsString = FormatRawParameterValue(value);
                if (parameterValueLabel.Text != valueAsString)
                {
                    parameterValueLabel.Text = valueAsString;
                }
            }
        }

        private void MssParameters_MinValueChanged(MssParameterID paramId, int minValue)
        {
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

            bool equationInputMode = this.curveShapeEquationRadio.Checked;

            curveEquationTextBox.Enabled = equationInputMode;


            bool presetInputMode = this.curveShapePresetRadio.Checked;

            curvePresetCombo.Enabled = presetInputMode;
            presetParam1Knob.Enabled = presetInputMode;
            presetParam2Knob.Enabled = presetInputMode;
            presetParam3Knob.Enabled = presetInputMode;
            presetParam4Knob.Enabled = presetInputMode;


            ActiveGraphableEntry.CurveShapeInfo.EqInputMode = newInputMode;
        }

        private void curveEquationTextBox_TextChanged(object sender, System.EventArgs e)
        {
            string expressionString = ((TextBox)sender).Text;
            ReturnStatus<double[]> evalReturnStatus = this.evaluator.EvaluateMultipleInputValues(expressionString, GRAPH_X_VAULES);
            if (evalReturnStatus.IsValid == true)
            {
                double[] GraphYValues = evalReturnStatus.ReturnVal;

                ActiveGraphableEntry.CurveShapeInfo.Equation = expressionString;

                LineItem mainCurve = GetMainCurve();
                mainCurve.Points = new PointPairList(GRAPH_X_VAULES, GraphYValues);

                this.mainGraphControl.Invalidate();
            }
            else
            {

            }

        }

        private void PluginEditorView_Load(object sender, EventArgs e)
        {
            InitiaizeGraph();
        }

        protected void InitiaizeGraph()
        {
            // get a reference to the GraphPane
            GraphPane pane = this.mainGraphControl.GraphPane;

            // Set the Titles
            pane.Title.Text = "";

            pane.XAxis.Title.Text = "Input";
            pane.XAxis.Scale.Min = 0;
            pane.XAxis.Scale.Max = 1;

            pane.YAxis.Title.Text = "Output";
            pane.YAxis.Scale.Min = 0;
            pane.YAxis.Scale.Max = 1;


            LineItem mainCurve = new LineItem(GRAPH_MAIN_CURVE_LABEL);
            mainCurve.Color = Color.Blue;
            mainCurve.Label.IsVisible = false;
            mainCurve.Symbol.IsVisible = false;

            pane.CurveList.Add(mainCurve);
        }

        protected LineItem GetMainCurve()
        {
            GraphPane pane = this.mainGraphControl.GraphPane;
            return (LineItem) pane.CurveList.Find(curveItem => curveItem.Label.Text == GRAPH_MAIN_CURVE_LABEL);
        }

        private void generatorListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            GraphableEntrySelectionChanged((ListView)sender, GraphableEntryType.Generator, e);
        }

        private void deleteMappingBtn_Click(object sender, EventArgs e)
        {

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
    }
}

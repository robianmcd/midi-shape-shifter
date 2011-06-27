using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

using MidiShapeShifter.CSharpUtil;
using MidiShapeShifter.Mss;
using MidiShapeShifter.Framework;
using MidiShapeShifter.Mss.Mapping;
using MidiShapeShifter.Mss.Generator;

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

        protected MssComponentHub mssHub;

        public MappingEntry ActiveMapping = null;

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

        public void Init(MssComponentHub mssHub)
        {
            this.mssHub = mssHub;
            
            this.mssHub.MssParameters.ParameterValueChanged += new ParameterValueChangedEventHandler(MssParameters_ValueChanged);
            this.mssHub.MssParameters.ParameterNameChanged += new ParameterNameChangedEventHandler(MssParameters_NameChanged);
            this.mssHub.MssParameters.ParameterMinValueChanged += new ParameterMinValueChangedEventHandler(MssParameters_MinValueChanged);
            this.mssHub.MssParameters.ParameterMaxValueChanged += new ParameterMaxValueChangedEventHandler(MssParameters_MaxValueChanged);

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

        protected void RefreshMappingListView()
        {
            this.mappingListView.Items.Clear();

            MappingManager mappingMgr = this.mssHub.MappingMgr;

            for (int i = 0; i < mappingMgr.GetNumEntries(); i++)
            {
                this.mappingListView.Items.Add(mappingMgr.GetListViewRow(i));

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
            

            if (this.mssHub.MssParameters.GetParameterValue(paramId) != knob.Value)
            {
                this.mssHub.MssParameters.SetParameterValue(paramId, knob.Value);
            }
        }
        
        protected string FormatRawParameterValue(double value)
        {
            return System.Math.Round(value, 2).ToString();
        }

        protected void ActiveMappingChanged()
        {
            if (ActiveMapping.CurveShapeInfo.EqInputMode == EquationInputMode.Text)
            {
                this.curveShapeEquationRadio.Checked = true;
            }
            else if (ActiveMapping.CurveShapeInfo.EqInputMode == EquationInputMode.Preset)
            {
                this.curveShapePresetRadio.Checked = true;
            }
            else
            {
                //Unknown EquationInputMode
                Debug.Assert(false);
            }

            this.curveEquationTextBox.Text = ActiveMapping.CurveShapeInfo.Equation;
            this.curvePresetCombo.SelectedIndex = ActiveMapping.CurveShapeInfo.PresetIndex;

            this.presetParam1Knob.Value = (float)ActiveMapping.CurveShapeInfo.PresetParamValues[0];
            this.presetParam2Knob.Value = (float)ActiveMapping.CurveShapeInfo.PresetParamValues[1];
            this.presetParam3Knob.Value = (float)ActiveMapping.CurveShapeInfo.PresetParamValues[2];
            this.presetParam4Knob.Value = (float)ActiveMapping.CurveShapeInfo.PresetParamValues[3];
            this.presetParam1Value.Text = FormatRawParameterValue(ActiveMapping.CurveShapeInfo.PresetParamValues[0]);
            this.presetParam2Value.Text = FormatRawParameterValue(ActiveMapping.CurveShapeInfo.PresetParamValues[1]);
            this.presetParam3Value.Text = FormatRawParameterValue(ActiveMapping.CurveShapeInfo.PresetParamValues[2]);
            this.presetParam4Value.Text = FormatRawParameterValue(ActiveMapping.CurveShapeInfo.PresetParamValues[3]);
        }

        private void addMappingBtn_Click(object sender, System.EventArgs e)
        {
            MappingDlg mapDlg = new MappingDlg();
            mapDlg.Init(new MappingEntry(), false);

            if (mapDlg.ShowDialog(this) == DialogResult.OK)
            {
                ActiveMapping = mapDlg.mappingEntry;
                MappingManager mappingMgr = this.mssHub.MappingMgr;

                mappingMgr.AddMappingEntry(mapDlg.mappingEntry);
                int newestEntryIndex = mappingMgr.GetNumEntries() - 1;
                this.mappingListView.Items.Add(mappingMgr.GetListViewRow(newestEntryIndex));
            }
        }

        private void editMappingBtn_Click(object sender, System.EventArgs e)
        {
            if (ActiveMapping == null)
            {
                return;
            }

            MappingDlg mapDlg = new MappingDlg();
            mapDlg.Init(ActiveMapping, true);
            

            if (mapDlg.ShowDialog(this) == DialogResult.OK)
            {
                RefreshMappingListView();
            }
        }

        private void mappingListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (mappingListView.SelectedItems.Count == 0)
            {
                //TODO: nothing is selected
                return;
            }
            
            ActiveMapping = this.mssHub.MappingMgr.GetMappingEntry(mappingListView.SelectedItems[0].Index);
            ActiveMappingChanged();
            
        }

        private void addGeneratorBtn_Click(object sender, System.EventArgs e)
        {
            GeneratorDlg genDlg = new GeneratorDlg();
            if (genDlg.ShowDialog() == DialogResult.OK)
            { 
                
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


            ActiveMapping.CurveShapeInfo.EqInputMode = newInputMode;
        }

        private void curveEquationTextBox_TextChanged(object sender, System.EventArgs e)
        {
            string expressionString = ((TextBox)sender).Text;
            double[] GraphYValues;

            if (this.evaluator.EvaluateMultipleInputValues(expressionString, GRAPH_X_VAULES, out GraphYValues) == true)
            {

                ActiveMapping.CurveShapeInfo.Equation = expressionString;

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

    }
}

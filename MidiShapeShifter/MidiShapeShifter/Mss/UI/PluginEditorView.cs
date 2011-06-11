using System.Windows.Forms;
using System.Collections.Generic;
using System.Diagnostics;

using Jacobi.Vst.Framework;

using LBSoft.IndustrialCtrls.Knobs;

using MidiShapeShifter.CSharpUtil;
using MidiShapeShifter.Mss;
using MidiShapeShifter.Framework;
using MidiShapeShifter.Mss.Mapping;
using MidiShapeShifter.Mss.Generator;

namespace MidiShapeShifter.Mss.UI
{
    public partial class PluginEditorView : UserControl
    {
        public const int NUM_VARIABLE_PARAMS = 4;
        public const int NUM_PRESET_PARAMS = 4;

        public TwoWayDictionary<MssParameterID, LBKnob> ParameterValueKnobControlDict = new TwoWayDictionary<MssParameterID, LBKnob>();
        public TwoWayDictionary<MssParameterID, Label> ParameterValueLabelControlDict = new TwoWayDictionary<MssParameterID, Label>();
        public TwoWayDictionary<MssParameterID, TextBox> ParameterMaxValueControlDict = new TwoWayDictionary<MssParameterID, TextBox>();
        public TwoWayDictionary<MssParameterID, TextBox> ParameterMinValueControlDict = new TwoWayDictionary<MssParameterID, TextBox>();
        public TwoWayDictionary<MssParameterID, Label> ParameterNameControlDict = new TwoWayDictionary<MssParameterID, Label>();

        protected MssComponentHub mssHub;

        public MappingEntry ActiveMapping = null;

        public PluginEditorView()
        {
            InitializeComponent();
            PopulateControlDictionaries();

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

        protected void AddEntryToMappingListView(MappingEntry entry) 
        {
            ListViewItem mappingItem = new ListViewItem(entry.GetReadableMsgType(IoType.Input));
            mappingItem.SubItems.Add(entry.InMssMsgInfo.Field1);
            mappingItem.SubItems.Add(entry.InMssMsgInfo.Field2);

            mappingItem.SubItems.Add(entry.GetReadableMsgType(IoType.Output));
            mappingItem.SubItems.Add(entry.OutMssMsgInfo.Field1);
            mappingItem.SubItems.Add(entry.OutMssMsgInfo.Field2);

            mappingItem.SubItems.Add(entry.GetReadableOverrideDuplicates());

            this.mappingListView.Items.Add(mappingItem);
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
            if (ActiveMapping.CurveShapeEntryInfo.EqInputMode == EquationInputMode.Text)
            {
                this.curveShapeEquationRadio.Checked = true;
            }
            else if (ActiveMapping.CurveShapeEntryInfo.EqInputMode == EquationInputMode.Preset)
            {
                this.curveShapePresetRadio.Checked = true;
            }
            else
            {
                //Unknown EquationInputMode
                Debug.Assert(false);
            }

            this.curveEquationTextBox.Text = ActiveMapping.CurveShapeEntryInfo.Equation;
            this.curvePresetCombo.SelectedIndex = ActiveMapping.CurveShapeEntryInfo.PresetIndex;

            this.presetParam1Knob.Value = (float)ActiveMapping.CurveShapeEntryInfo.PresetParamValues[0];
            this.presetParam2Knob.Value = (float)ActiveMapping.CurveShapeEntryInfo.PresetParamValues[1];
            this.presetParam3Knob.Value = (float)ActiveMapping.CurveShapeEntryInfo.PresetParamValues[2];
            this.presetParam4Knob.Value = (float)ActiveMapping.CurveShapeEntryInfo.PresetParamValues[3];
            this.presetParam1Value.Text = FormatRawParameterValue(ActiveMapping.CurveShapeEntryInfo.PresetParamValues[0]);
            this.presetParam2Value.Text = FormatRawParameterValue(ActiveMapping.CurveShapeEntryInfo.PresetParamValues[1]);
            this.presetParam3Value.Text = FormatRawParameterValue(ActiveMapping.CurveShapeEntryInfo.PresetParamValues[2]);
            this.presetParam4Value.Text = FormatRawParameterValue(ActiveMapping.CurveShapeEntryInfo.PresetParamValues[3]);
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
            bool equationInputMode = this.curveShapeEquationRadio.Checked;

            curveEquationTextBox.Enabled = equationInputMode;


            bool presetInputMode = this.curveShapePresetRadio.Checked;

            curvePresetCombo.Enabled = presetInputMode;
            presetParam1Knob.Enabled = presetInputMode;
            presetParam2Knob.Enabled = presetInputMode;
            presetParam3Knob.Enabled = presetInputMode;
            presetParam4Knob.Enabled = presetInputMode;

        }

        private void curveEquationTextBox_TextChanged(object sender, System.EventArgs e)
        {

        }

    }
}

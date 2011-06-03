using System.Windows.Forms;
using Jacobi.Vst.Framework;
using System.Collections.Generic;
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

        public struct VariableParamControls { 
            public LBKnob[] knobs;
            public TextBox[] maxTextBoxes;
            public TextBox[] minTextBoxes;
            public Label[] valueDisplays;
        }

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

        //private void BindParameter(VstParameterManager paramMgr)
        //{
        //    // NOTE: This code works best with integer parameter values.
        //    if (paramMgr.ParameterInfo.IsStepIntegerValid)
        //    {
        //        knob.StepValue
        //        knob.LargeChange = paramMgr.ParameterInfo.LargeStepInteger;
        //        knob.SmallChange = paramMgr.ParameterInfo.StepInteger;
        //    }

        //    if (paramMgr.ParameterInfo.IsMinMaxIntegerValid)
        //    {
        //        knob.Minimum = paramMgr.ParameterInfo.MinInteger;
        //        knob.Maximum = paramMgr.ParameterInfo.MaxInteger;
        //    }

        //     use databinding for VstParameter/Manager changed notifications.
        //    knob.DataBindings.Add("Value", paramMgr, "ActiveParameter.Value");
        //    knob.ValueChanged += new System.EventHandler(TrackBar_ValueChanged);
        //    knob.Tag = paramMgr;
        //}

        protected void AddEntryToMappingListView(MappingEntry entry) 
        {
            ListViewItem mappingItem = new ListViewItem(entry.GetReadableMsgType(MappingEntry.IO.Input));
            mappingItem.SubItems.Add(entry.InMssMsgInfo.Field1);
            mappingItem.SubItems.Add(entry.InMssMsgInfo.Field2);

            mappingItem.SubItems.Add(entry.GetReadableMsgType(MappingEntry.IO.Output));
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
                parameterValueDisplay.Text = FormatRawValue((double)knob.Value);
            }
            

            if (this.mssHub.MssParameters.GetParameterValue(paramId) != knob.Value)
            {
                this.mssHub.MssParameters.SetParameterValue(paramId, knob.Value);
            }
        }
        
        protected string FormatRawValue(double value)
        {
            return System.Math.Round(value, 2).ToString();
        }

        private void addMappingBtn_Click(object sender, System.EventArgs e)
        {
            MappingDlg mapDlg = new MappingDlg(new MappingEntry());
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

            MappingDlg mapDlg = new MappingDlg(ActiveMapping);
            
            mapDlg.useMappingEntryForDefaultValues = true;

            if (mapDlg.ShowDialog(this) == DialogResult.OK)
            {

            }
        }

        private void mappingListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            /*if (mappingListView.SelectedItems.Count != 1)
            {
                return;
            }

            mappingListView.SelectedItems[0].Index;*/
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
                string valueAsString = FormatRawValue(value);
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

    }
}

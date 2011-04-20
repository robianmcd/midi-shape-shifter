using System.Windows.Forms;
using Jacobi.Vst.Framework;
using System.Collections.Generic;
using LBSoft.IndustrialCtrls.Knobs;
using MidiShapeShifter;
namespace MidiShapeShifter.UI
{
    public partial class PluginEditorView : UserControl
    {
        public const int NUM_VARIABLE_PARAMS = 4;
        public const int NUM_PRESET_PARAMS = 4; 

        public struct VariableParamsInfo { 
            public LBKnob[] knobs;
            public TextBox[] maxTextBoxes;
            public TextBox[] minTextBoxes;
            public Label[] valueDisplays;
        }

        public VariableParamsInfo variableParamsInfo;
        public MappingEntry ActiveMapping = null;

        public PluginEditorView()
        {
            InitializeComponent();

            variableParamsInfo.knobs = new LBKnob[NUM_VARIABLE_PARAMS] { variableAKnob, variableBKnob, variableCKnob, variableDKnob };
            variableParamsInfo.maxTextBoxes = new TextBox[NUM_VARIABLE_PARAMS] { variableAMax, variableBMax, variableCMax, variableDMax };
            variableParamsInfo.minTextBoxes = new TextBox[NUM_VARIABLE_PARAMS] { variableAMin, variableBMin, variableCMin, variableDMin };
            variableParamsInfo.valueDisplays = new Label[NUM_VARIABLE_PARAMS] { variableAValue, variableBValue, variableCValue, variableDValue };
        }

        internal bool InitializeVariableParameters(List<VstParameterManager> parameters)
        {
            if (parameters == null || parameters.Count < NUM_VARIABLE_PARAMS)
            {
                return false;
            }

            for (int i = 0; i < NUM_VARIABLE_PARAMS; i++ )
            {
                //TODO: Need a 
                variableParamsInfo.knobs[i].DataBindings.Add("Value", parameters[i], "ActiveParameter.Value");
                variableParamsInfo.knobs[i].KnobChangeValue += new LBSoft.IndustrialCtrls.Knobs.KnobChangeValue(lbKnob_KnobChangeValue);
                variableParamsInfo.knobs[i].Tag = parameters[i];
            }

            return true;
        }

        private void BindParameter(VstParameterManager paramMgr)
        {
            // NOTE: This code works best with integer parameter values.
            if (paramMgr.ParameterInfo.IsStepIntegerValid)
            {
                //knob.StepValue
//                knob.LargeChange = paramMgr.ParameterInfo.LargeStepInteger;
  //              knob.SmallChange = paramMgr.ParameterInfo.StepInteger;
            }

            if (paramMgr.ParameterInfo.IsMinMaxIntegerValid)
            {
    //            knob.Minimum = paramMgr.ParameterInfo.MinInteger;
      //          knob.Maximum = paramMgr.ParameterInfo.MaxInteger;
            }

            // use databinding for VstParameter/Manager changed notifications.
        //    knob.DataBindings.Add("Value", paramMgr, "ActiveParameter.Value");
          //  knob.ValueChanged += new System.EventHandler(TrackBar_ValueChanged);
            //knob.Tag = paramMgr;
        }

        protected void AddEntryToListView(MappingEntry entry) 
        {
            ListViewItem inTypeItem = new ListView();
            //TODO: finish this
        }

        private void lbKnob_KnobChangeValue(object sender, LBSoft.IndustrialCtrls.Knobs.LBKnobEventArgs e) {
            var knob = (LBKnob)sender;
            var paramMgr = (VstParameterManager)knob.Tag;

            paramMgr.ActiveParameter.Value = knob.Value;
        }
            
        private void presetParam1Knob_KnobChangeValue(object sender, LBSoft.IndustrialCtrls.Knobs.LBKnobEventArgs e)
        {
            presetParam1Value.Text = System.Math.Round((double) presetParam1Knob.Value, 2).ToString();
        }

        private void addBtn_Click(object sender, System.EventArgs e)
        {
            if (ActiveMapping == null)
            {
                return;
            }

            MappingDlg mapDlg = new MappingDlg();
            if (mapDlg.ShowDialog(this) == DialogResult.OK)
            {
                ActiveMapping = mapDlg.mappingEntry.Clone();
                ActiveMapping.priority = mappingListView.Items.Count;
                AddEntryToListView(ActiveMapping);
            }
        }

        private void editBtn_Click(object sender, System.EventArgs e)
        {
            if (ActiveMapping == null)
            {
                return;
            }

            MappingDlg mapDlg = new MappingDlg();
            
            mapDlg.mappingEntry = ActiveMapping.Clone();
            mapDlg.useMappingEntryForDefaultValues = true;

            if (mapDlg.ShowDialog(this) == DialogResult.OK)
            {
                ActiveMapping = mapDlg.mappingEntry.Clone();
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
    }
}

using System.Windows.Forms;
using Jacobi.Vst.Framework;
using System.Collections.Generic;
using LBSoft.IndustrialCtrls.Knobs;
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

        private void lbKnob_KnobChangeValue(object sender, LBSoft.IndustrialCtrls.Knobs.LBKnobEventArgs e) {
            var trackBar = (LBKnob)sender;
            var paramMgr = (VstParameterManager)trackBar.Tag;

            paramMgr.ActiveParameter.Value = trackBar.Value;
        }
            
        private void presetParam1Knob_KnobChangeValue(object sender, LBSoft.IndustrialCtrls.Knobs.LBKnobEventArgs e)
        {
            presetParam1Value.Text = System.Math.Round((double) presetParam1Knob.Value, 2).ToString();
        }
    }
}

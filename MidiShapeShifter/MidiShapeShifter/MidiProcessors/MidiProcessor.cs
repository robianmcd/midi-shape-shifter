using Jacobi.Vst.Core;
using Jacobi.Vst.Framework;
using MidiShapeShifter.Framework;

namespace MidiShapeShifter.MidiProcessors
{
    internal sealed class MidiProcessor
    {
        private static readonly string VARIABLE_PARAM_CATEGORY_NAME = "Variable";

        internal VstParameterManager VariableAMgr { get; private set; }
        internal VstParameterManager VariableBMgr { get; private set; }
        internal VstParameterManager VariableCMgr { get; private set; }
        internal VstParameterManager VariableDMgr { get; private set; }

        private Plugin _plugin;

        public MidiProcessor(Plugin plugin) {
            _plugin = plugin;

            InitializeAllParams();

            _plugin.Opened += new System.EventHandler(Plugin_Opened);
        }

        public void InitializeAllParams() {
            VstParameterInfo paramInfo;
            paramInfo = InitializeVariableParam("A");
            VariableAMgr = new VstParameterManager(paramInfo);

            paramInfo = InitializeVariableParam("B");
            VariableBMgr = new VstParameterManager(paramInfo);

            paramInfo = InitializeVariableParam("C");
            VariableCMgr = new VstParameterManager(paramInfo);

            paramInfo = InitializeVariableParam("D");
            VariableDMgr = new VstParameterManager(paramInfo);
        }

        // This method initializes the "Variable" plugin parameters.
        private VstParameterInfo InitializeVariableParam(string name)
        {
            // all parameter definitions are added to a central list.
            VstParameterInfoCollection parameterInfos = _plugin.PluginPrograms.ParameterInfos;

            // retrieve the category for all variable parameters.
            VstParameterCategory paramCategory =
                _plugin.PluginPrograms.GetParameterCategory(VARIABLE_PARAM_CATEGORY_NAME);

            // Variable parameter
            VstParameterInfo paramInfo = new VstParameterInfo();
            paramInfo.Category = paramCategory;
            paramInfo.CanBeAutomated = true;
            paramInfo.Name = name;
            paramInfo.Label = "";
            paramInfo.ShortLabel = "";
            paramInfo.MinInteger = 0;
            paramInfo.MaxInteger = 1;
            paramInfo.LargeStepFloat = 0.125f;
            paramInfo.SmallStepFloat = 0.0078125f;
            paramInfo.StepFloat = 0.03125f;
            paramInfo.DefaultValue = 0f;
            VstParameterNormalizationInfo.AttachTo(paramInfo);

            parameterInfos.Add(paramInfo);

            return paramInfo;
        }

        private void Plugin_Opened(object sender, System.EventArgs e)
        {
            //Host automation must be set up in this even handler and not in the constructor becasue _plugin.Host is 
            //null when the constructor is called.
            VariableAMgr.HostAutomation = _plugin.Host.GetInstance<IVstHostAutomation>();
            VariableBMgr.HostAutomation = _plugin.Host.GetInstance<IVstHostAutomation>();
            VariableCMgr.HostAutomation = _plugin.Host.GetInstance<IVstHostAutomation>();
            VariableDMgr.HostAutomation = _plugin.Host.GetInstance<IVstHostAutomation>();

            _plugin.Opened -= new System.EventHandler(Plugin_Opened);
        }

        internal VstMidiEvent ProcessEvent(VstMidiEvent midiEvent)
        {
            throw new System.NotImplementedException();
        }
    }
}

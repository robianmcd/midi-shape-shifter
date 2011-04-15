using Jacobi.Vst.Core;
using Jacobi.Vst.Framework;

namespace MidiShapeShifter.MidiProcessors
{
    class MidiProcessor
    {
        private static readonly string PARAM_CATEGORY_NAME = "Variable";

        internal VstParameterManager VariableAMgr { get; private set; }
        internal VstParameterManager VariableBMgr { get; private set; }
        internal VstParameterManager VariableCMgr { get; private set; }
        internal VstParameterManager VariableDMgr { get; private set; }

        private Plugin m_plugin;

        public MidiProcessor(Plugin plugin) {
            m_plugin = plugin;

            VstParameterInfo paramInfo;
            paramInfo = InitializeVariableParam("Variable A");
            VariableAMgr = new VstParameterManager(paramInfo);
            
            paramInfo = InitializeVariableParam("Variable B");
            VariableBMgr = new VstParameterManager(paramInfo);
            
            paramInfo = InitializeVariableParam("Variable C");
            VariableCMgr = new VstParameterManager(paramInfo);
            
            paramInfo = InitializeVariableParam("Variable D");
            VariableDMgr = new VstParameterManager(paramInfo);
        }

        // This method initializes the plugin parameters this processing component owns.
        private VstParameterInfo InitializeVariableParam(string name)
        {
            // all parameter definitions are added to a central list.
            VstParameterInfoCollection parameterInfos = m_plugin.PluginPrograms.ParameterInfos;

            // retrieve the category for all variable parameters.
            VstParameterCategory paramCategory =
                m_plugin.PluginPrograms.GetParameterCategory(PARAM_CATEGORY_NAME);

            // Variable parameter
            VstParameterInfo paramInfo = new VstParameterInfo();
            paramInfo.Category = paramCategory;
            paramInfo.CanBeAutomated = true;
            paramInfo.Name = name;
            paramInfo.Label = "units";
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


        internal VstMidiEvent ProcessEvent(VstMidiEvent midiEvent)
        {
            throw new System.NotImplementedException();
        }
    }
}

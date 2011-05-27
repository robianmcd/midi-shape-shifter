using System.Collections.Generic;
using Jacobi.Vst.Core;
using Jacobi.Vst.Framework;
using MidiShapeShifter.Framework;
using MidiShapeShifter.Mapping;

namespace MidiShapeShifter.MidiProcessors
{
    internal class MidiProcessor
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

        internal List<MssMsg> ProcessMssMsg(MssMsg mssMsg)
        {
            IEnumerable<MappingEntry> mappingEntries = _plugin.mappingManager.GetAssociatedEntries(mssMsg);

            List<MssMsg> outMessages = new List<MssMsg>();

            foreach (MappingEntry entry in mappingEntries)
            {
                //TODO: map mssMsg.Data3 to equation
                int mappedData3 = 100;

                if (mappedData3 >= 0 && mappedData3 <= 127)
                {
                    int mappedData1 = CalculateLinearMapping(entry.InMssMsgInfo.Data1RangeBottom, 
                                                             entry.InMssMsgInfo.Data1RangeTop,
                                                             entry.OutMssMsgInfo.Data1RangeBottom,
                                                             entry.OutMssMsgInfo.Data1RangeTop,
                                                             mssMsg.Data1);
                    int mappedData2 = CalculateLinearMapping(entry.InMssMsgInfo.Data2RangeBottom,
                                                             entry.InMssMsgInfo.Data2RangeTop,
                                                             entry.OutMssMsgInfo.Data2RangeBottom,
                                                             entry.OutMssMsgInfo.Data2RangeTop,
                                                             mssMsg.Data2);

                    MssMsg outMsg = new MssMsg(entry.OutMssMsgInfo.mssMsgType, mappedData1, mappedData2, mappedData3);
                    outMessages.Add(outMsg);
                }
            }

            return outMessages;
        }

        protected int CalculateLinearMapping(int inRangeBottom, int inRangeTop, 
                                             int outRangeBottom, int outRangeTop, 
                                             int ValueToMap)
        {
            //if the in range is just a single value then treating it as a range would result in a divide by zero error. 
            //So this case must be handled seperateally.
            if (inRangeBottom == inRangeTop)
            {
                return outRangeBottom;
            }
            else
            {
                double percentIntoRange = (ValueToMap - inRangeBottom) / (inRangeTop - inRangeBottom);
                int outRangeSize = outRangeTop - outRangeBottom;

                //maybe we should round here
                int outRangeOffset = (int) percentIntoRange * outRangeSize;

                return outRangeBottom + outRangeOffset;
            }
        }
    }
}

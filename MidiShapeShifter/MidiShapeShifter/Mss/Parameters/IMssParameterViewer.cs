using System.Collections.Generic;

namespace MidiShapeShifter.Mss.Parameters
{
    public interface IMssParameterViewer
    {
        List<MssParamInfo> GetVariableParamInfoList();
        List<MssParamInfo> GetPresetParamInfoList();

        MssParamInfo GetParameterInfoCopy(MssParameterID parameterId);
        event ParameterMaxValueChangedEventHandler ParameterMaxValueChanged;
        event ParameterMinValueChangedEventHandler ParameterMinValueChanged;
        event ParameterNameChangedEventHandler ParameterNameChanged;
        event ParameterValueChangedEventHandler ParameterValueChanged;
    }
}

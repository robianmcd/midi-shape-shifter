using System;
using System.Collections.Generic;

namespace MidiShapeShifter.Mss.Parameters
{
    public interface IMssParameterViewer
    {
        List<MssParamInfo> GetVariableParamInfoList();
        List<MssParamInfo> GetPresetParamInfoList();

        int GetParameterMaxValue(MssParameterID parameterId);
        int GetParameterMinValue(MssParameterID parameterId);
        string GetParameterName(MssParameterID parameterId);
        double GetParameterValue(MssParameterID parameterId);
        event ParameterMaxValueChangedEventHandler ParameterMaxValueChanged;
        event ParameterMinValueChangedEventHandler ParameterMinValueChanged;
        event ParameterNameChangedEventHandler ParameterNameChanged;
        event ParameterValueChangedEventHandler ParameterValueChanged;
    }
}

using System;
namespace MidiShapeShifter.Mss
{
    public interface IMssParameterViewer
    {
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

using System;
using MidiShapeShifter.CSharpUtil;

namespace MidiShapeShifter.Mss
{
    public interface IMssEvaluator
    {
        ReturnStatus<double> Evaluate(string expressionString, double input);
        ReturnStatus<double[]> EvaluateMultipleInputValues(string expressionString, double[] inputValues);
    }
}

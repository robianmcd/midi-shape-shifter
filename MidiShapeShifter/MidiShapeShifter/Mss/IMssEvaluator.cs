using System;
using MidiShapeShifter.CSharpUtil;

namespace MidiShapeShifter.Mss
{
    public interface IMssEvaluator
    {
        ReturnStatus<double> Evaluate(string expressionString, MssEvaluatorInput input);
        ReturnStatus<double[]> SampleExpressionWithDefaultInputValues(string expressionString, int numSamplePoints);
    }
}

using System;
using System.Collections.Generic;

using MidiShapeShifter.CSharpUtil;
using MidiShapeShifter.Mss.Mapping;

namespace MidiShapeShifter.Mss.Evaluation
{
    public interface IEvaluator
    {
        ReturnStatus<double> Evaluate(EvaluationCurveInput input);
        bool SampleExpressionWithDefaultInputValues(
                int numSamplePoints, 
                IMssParameterViewer mssParameters,
                IMappingEntry mappingEntry,
                out List<XyPoint<double>> points,
                out double[] curveYValues);
    }
}

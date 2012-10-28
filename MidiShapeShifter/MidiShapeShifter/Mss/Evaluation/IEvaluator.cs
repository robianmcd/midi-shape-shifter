using System;
using System.Collections.Generic;

using MidiShapeShifter.CSharpUtil;
using MidiShapeShifter.Mss.Mapping;
using MidiShapeShifter.Mss.Parameters;

namespace MidiShapeShifter.Mss.Evaluation
{
    public interface IEvaluator
    {
        ReturnStatus<double> Evaluate(EvaluationCurveInput input);
        bool SampleExpressionWithDefaultInputValues(
                double xDistanceBetweenPoints,  
                IMssParameterViewer mssParameters,
                IMappingEntry mappingEntry,
                out List<XyPoint<double>> points,
                out List<List<XyPoint<double>>> curvePointsByCurveList,
                out HashSet<int> erroneousControlPointIndex,
                out HashSet<int> erroneousCurveIndex);
    }
}

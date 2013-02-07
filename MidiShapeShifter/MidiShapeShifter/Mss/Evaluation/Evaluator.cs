using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using NCalc;
using NCalc.Domain;

using MidiShapeShifter.CSharpUtil;
using MidiShapeShifter.Mss.Mapping;
using MidiShapeShifter.Mss.Parameters;
using MidiShapeShifter.Mss.MssMsgInfoTypes;

namespace MidiShapeShifter.Mss.Evaluation
{
    public enum EvalType { Curve, ControlPoint }

    /// <summary>
    /// This class is responsible for evaluating expressions.
    /// </summary>
    public class Evaluator : IEvaluator
    {

        public string LastErrorMsg = "";

        public const int NUM_EXPRESSIONS_IN_CACHE = 1000;
        protected LruCache<string, LogicalExpression> expressionCache;

        public Evaluator() {
            this.expressionCache = new LruCache<string, LogicalExpression>(NUM_EXPRESSIONS_IN_CACHE);
        }

        /// <summary>
        ///  Uses the primary input in evalInput to depermine which curve equation needs to be 
        ///  evaluated. Then evaluates the appropriate curve equation using the given input.
        /// </summary>
        /// <param name="evalInput">Stores info neede to evaluate an expression</param>
        /// <returns>
        /// Returns the output of the evaluated equation. The return status will not be valid if 
        /// the equation could not be evaluated.
        /// </returns>
        public ReturnStatus<double> Evaluate(EvaluationCurveInput evalInput)
        {
            var erroneousPoints = new HashSet<int>();
            var controlPointValuesStatus = CalculateControlPointValues(evalInput.VariableParamInfoList, evalInput.TransformParamInfoList, evalInput.PointEquations, ref erroneousPoints);

            if (controlPointValuesStatus.IsValid) {
                EvaluationCurveJob evalCurveJob = new EvaluationCurveJob();

                string expressionStr = GetCurveExpressionString(evalInput.getPrimaryInputVal(), controlPointValuesStatus.Value, evalInput.CurveEquations);
                var returnedExpression = CreateExpressionFromString(expressionStr, EvalType.Curve);

                if (returnedExpression.IsValid) {
                    evalCurveJob.Configure(evalInput, controlPointValuesStatus.Value, returnedExpression.Value);

                    evalCurveJob.Execute();

                    if (evalCurveJob.OutputIsValid)
                    {
                        return new ReturnStatus<double>(evalCurveJob.OutputVal, true);
                    }
    
                }

            }

            return new ReturnStatus<double>(double.NaN, false);
        }

        /// <summary>
        /// Evaluates the equations for the given mappingEntry. All control point equations will
        /// be evaluated and returned pointList. Curve equations will be evaluated at equally spaced 
        /// sample points and the return values will be returned in curveYValues. This function is 
        /// used by the GUI to create the graph representing the transformation on a mapping.
        /// </summary>
        /// <param name="xDistanceBetweenPoints">
        /// The maximum horizontal distance between any two points.
        /// </param>
        /// <param name="variableParamInfoList">
        /// A list of cloned variable parameter info for each variable parameter.
        /// </param>
        /// <param name="mappingEntry">
        /// The mapping entry to evaluate equations for.
        /// </param>
        /// <param name="controlPointList">
        /// The evaluated control points are returned in this list
        /// </param>
        /// <param name="curvePointsByCurveList">
        /// This will contain the evaluated list of XyPoints for each curve.
        /// </param>
        /// <param name="erroneousControlPointIndexSet">
        /// empty if all points had valid equations. Otherwise the index of at least one point with an invalid equation.
        /// </param>
        /// <param name="erroneousCurveIndexSet">
        /// -1 if there is an invalid control point or if all curve equations are valid. Otherwise 
        /// the index of the first curve with an invalid equation.
        /// </param>
        /// <returns>
        /// true if all equations could be evaluated and false otherwise. The out parameters are only 
        /// garunteed to be valid if the return value is true.
        /// </returns>
        public bool SampleExpressionWithDefaultInputValues(
                double xDistanceBetweenPoints,
                List<MssParamInfo> variableParamInfoList,
                IMappingEntry mappingEntry,
                out List<XyPoint<double>> controlPointList,
                out List<List<XyPoint<double>>> curvePointsByCurveList,
                out HashSet<int> erroneousControlPointIndexSet,
                out HashSet<int> erroneousCurveIndexSet
            )
        {
            erroneousControlPointIndexSet = new HashSet<int>();
            erroneousCurveIndexSet = new HashSet<int>();

            //Initialize this list from an array so that elements can be set directly in parallel.
            curvePointsByCurveList = new List<List<XyPoint<double>>>(new List<XyPoint<double>>[mappingEntry.CurveShapeInfo.CurveEquations.Count]);

            EvaluationCurveJob evalJob = new EvaluationCurveJob();

            var controlPointValuesStatus = CalculateControlPointValues(
                variableParamInfoList, 
                mappingEntry.CurveShapeInfo.ParamInfoList, 
                mappingEntry.CurveShapeInfo.PointEquations, 
                ref erroneousControlPointIndexSet);

            controlPointList = controlPointValuesStatus.Value;

            //Need to make a second version of these variable as out parameters cannot be used in Parallel.For loops.
            List<XyPoint<double>> controlPointList2 = controlPointList;
            List<List<XyPoint<double>>> curvePointsByCurveList2 = curvePointsByCurveList;
            HashSet<int> erroneousCurveIndexSet2 = erroneousCurveIndexSet;

            if (controlPointValuesStatus.IsValid == false)
            {
                return false;
            }

            int numCurves = mappingEntry.CurveShapeInfo.CurveEquations.Count;

            bool noErrorsEncountered = true;

            Parallel.For(0, numCurves, (curveIndex, loopState) =>
            {
                double curveStartXVal = 0;
                double curveEndXVal = 1;

                if (curveIndex > 0)
                {
                    curveStartXVal = controlPointList2[curveIndex - 1].X;
                }

                if (curveIndex < numCurves - 1)
                {
                    curveEndXVal = controlPointList2[curveIndex].X;
                }

                int numPointsInCurve = (int)((curveEndXVal - curveStartXVal) / xDistanceBetweenPoints) + 1;


                List<XyPoint<double>> curCurvePoints = new List<XyPoint<double>>(new XyPoint<double>[numPointsInCurve]);
                curvePointsByCurveList2[curveIndex] = curCurvePoints;

                Parallel.For(0, numPointsInCurve, pointIndex =>
                {
                    //Calling loopState.Stop() does not immedateally stop all threads so that they 
                    //exit without doing any more processing.
                    if (loopState.ShouldExitCurrentIteration)
                    {
                        return;
                    }

                    double curXVal = curveStartXVal + (pointIndex * xDistanceBetweenPoints);

                    //Use the control point for the start of a curve as it is already calcuated
                    if (pointIndex == 0 && curveIndex > 0)
                    {
                        curCurvePoints[0] = controlPointList2[curveIndex - 1];
                    }
                    else
                    {
                        var evalStatus = evaluateCurveAtXVal(curXVal, curveIndex, mappingEntry, variableParamInfoList, controlPointList2);
                        if (evalStatus.IsValid == false)
                        {
                            lock (erroneousCurveIndexSet2)
                            {
                                erroneousCurveIndexSet2.Add(curveIndex);
                                noErrorsEncountered = false;
                                loopState.Stop();
                                return;
                            }
                        }

                        curCurvePoints[pointIndex] = evalStatus.Value;
                    }
                });

            });


            return noErrorsEncountered;
        }

        protected ReturnStatus<XyPoint<double>> evaluateCurveAtXVal(double inputXVal, int curveIndex, IMappingEntry mappingEntry, List<MssParamInfo> variableParamInfoList, List<XyPoint<double>> controlPointList)
        {
            //For each sample point data1 data2 and data3 will be set to the X value of the 
            //sample point.
            double relData1 = inputXVal;
            double relData2 = inputXVal;
            double relData3 = inputXVal;

            IStaticMssMsgInfo inMsgInfo =
                Factory_StaticMssMsgInfo.Create(mappingEntry.InMssMsgRange.MsgType);

            //If curXVal is outside of the relative input range for data 1 then set 
            //relData1 to NaN
            double max = (double)inMsgInfo.MaxData1Value;
            double min = (double)inMsgInfo.MinData1Value;
            double bottom = (double)mappingEntry.InMssMsgRange.Data1RangeBottom;
            double top = (double)mappingEntry.InMssMsgRange.Data1RangeTop;
            if (inputXVal < ((bottom - min) / (max - min + 1)) ||
                inputXVal > ((top - min) / (max - min + 1)))
            {
                relData1 = double.NaN;
            }

            //If curXVal is outside of the relative input range for data 2 then set relData2 
            //to NaN
            max = (double)inMsgInfo.MaxData2Value;
            min = (double)inMsgInfo.MinData2Value;
            bottom = (double)mappingEntry.InMssMsgRange.Data2RangeBottom;
            top = (double)mappingEntry.InMssMsgRange.Data2RangeTop;
            if (inputXVal < ((bottom - min) / (max - min + 1)) ||
                inputXVal > ((top - min) / (max - min + 1)))
            {
                relData2 = double.NaN;
            }

            var evalInput = new EvaluationCurveInput();
            evalInput.Init(
                relData1,
                relData2,
                relData3,
                variableParamInfoList,
                mappingEntry);

            var evalJob = new EvaluationCurveJob();

            var returnedExpression = CreateExpressionFromString(mappingEntry.CurveShapeInfo.CurveEquations[curveIndex], EvalType.Curve);

            if (returnedExpression.IsValid == false)
            {
                return new ReturnStatus<XyPoint<double>>();
            }

            evalJob.Configure(evalInput, controlPointList, returnedExpression.Value);

            evalJob.Execute();

            if (evalJob.OutputIsValid)
            {
                var curPoint = new XyPoint<double>(inputXVal, evalJob.OutputVal);
                return new ReturnStatus<XyPoint<double>>(curPoint);
            }
            else
            {
                return new ReturnStatus<XyPoint<double>>();
            }
        }


        /// <summary>
        /// Calculates the X and Y coordinates of each control point and stores them in this.controlPointValues
        /// </summary>
        /// <param name="erroneousControlPointIndexSet">
        /// empty if all points had valid equations. Otherwise contains the index of at least one point with an invalid equation.
        /// </param>
        /// <returns>True on success, False if the control point equations could not be evaluated.</returns>
        protected ReturnStatus<List<XyPoint<double>>> CalculateControlPointValues(
            List<MssParamInfo> variableParamInfoList,
            List<MssParamInfo> transformParamInfoList,
            List<XyPoint<string>> pointEquations,
            ref HashSet<int> erroneousControlPointIndexSet)
        {
            erroneousControlPointIndexSet.Clear();

            var controlPointList = new List<XyPoint<double>>();

            //Create the input for the control point jobs. The expression string will be 
            //individually set for each equation.
            EvaluationControlPointInput pointEvalInput = new EvaluationControlPointInput();
            pointEvalInput.Init(variableParamInfoList,
                                transformParamInfoList,
                                "");

            //Create jobs to evaluate the x and y coordinates for a control point
            EvaluationControlPointJob pointXEvalJob = new EvaluationControlPointJob();
            EvaluationControlPointJob pointYEvalJob = new EvaluationControlPointJob();


            double previousPointXVal = 0;
            //Itterate through each control point equation and evaluate it's X and Y coordinates.
            for (int i = 0; i < pointEquations.Count; i++)
            {
                XyPoint<string> pointEquation = pointEquations[i];

                var xEquationStatus = CreateExpressionFromString(pointEquation.X, EvalType.ControlPoint);
                var yEquationStatus = CreateExpressionFromString(pointEquation.Y, EvalType.ControlPoint);

                if (!xEquationStatus.IsValid || !yEquationStatus.IsValid) {
                    erroneousControlPointIndexSet.Add(i);
                    return new ReturnStatus<List<XyPoint<double>>>();
                }

                //Evaluate the equation for the current control point's X value
                pointEvalInput.EquationStr = pointEquation.X;
                pointXEvalJob.Configure(pointEvalInput, xEquationStatus.Value);
                pointXEvalJob.Execute();

                //Evaluate the equation for the current control point's Y value
                pointEvalInput.EquationStr = pointEquation.Y;
                pointYEvalJob.Configure(pointEvalInput, yEquationStatus.Value);
                pointYEvalJob.Execute();

                //If one of the equations could not be evaluated return false
                if (pointXEvalJob.OutputIsValid == false || pointYEvalJob.OutputIsValid == false)
                {
                    erroneousControlPointIndexSet.Add(i);
                    return new ReturnStatus<List<XyPoint<double>>>();
                }

                //Store the current control point's X and Y coordinates in this.controlPointValues.
                XyPoint<double> curPoint = new XyPoint<double>(pointXEvalJob.OutputVal, pointYEvalJob.OutputVal);
                controlPointList.Add(curPoint);

                previousPointXVal = pointXEvalJob.OutputVal;
            }

            for (int i = 1; i < controlPointList.Count; i++)
            {
                //If the control points are not in order from smallest to largest then return an 
                //invalid return status.
                if (controlPointList[i - 1].X > controlPointList[i].X)
                {
                    erroneousControlPointIndexSet.Add(i);
                    erroneousControlPointIndexSet.Add(i - 1);
                    return new ReturnStatus<List<XyPoint<double>>>();
                }
            }

            return new ReturnStatus<List<XyPoint<double>>>(controlPointList);
        }


        protected ReturnStatus<Expression> CreateExpressionFromString(string expressionStr, EvalType evalType)
        {
            string formattedExpressionStr;
            switch (evalType)
            {
                case EvalType.ControlPoint:
                    formattedExpressionStr = EvaluationJob.FUNC_NAME_LIMIT + "(" + expressionStr + ")";
                    break;

                case EvalType.Curve:
                    formattedExpressionStr = EvaluationJob.FUNC_NAME_LIMIT + "(" +
                        EvaluationCurveJob.FUNC_NAME_SNAP + "(" + expressionStr + "))";
                    break;

                default:
                    Debug.Assert(false);
                    formattedExpressionStr = expressionStr.ToLower();
                    break;
            }

            //Even though we use EvaluateOptions.IgnoreCase when we construct an expression 
            //it still seems to be necessary to convery everything to lower case. Other 
            //parameters will not be evaluated.
            formattedExpressionStr = formattedExpressionStr.ToLower();

            Expression expression = null;

            if (this.expressionCache.ContainsKey(formattedExpressionStr) == false)
            {
                expression = new Expression(formattedExpressionStr, EvaluateOptions.IgnoreCase);

                //Calling HasErrors() will cause expression.ParsedExpression to be set.
                if (expression.HasErrors())
                {
                    //Cache this expression that causes a syntax error as null so that it doesn't have to be parsed again.
                    this.expressionCache.GetAndAddValue(formattedExpressionStr, () => null);
                }
                else
                {
                    this.expressionCache.GetAndAddValue(formattedExpressionStr, () => expression.ParsedExpression);
                }

            }
            else 
            {
                //We have already ensured that the key is in the cache so the createValue function should not be called.
                LogicalExpression parsedExpression = this.expressionCache.GetAndAddValue(formattedExpressionStr, () => { Debug.Assert(false); return null; });
                if (parsedExpression != null) {
                    expression = new Expression(parsedExpression, EvaluateOptions.IgnoreCase);
                }
            }

            if (expression == null)
            {
                return new ReturnStatus<Expression>();
            }
            else {
                SetExpressionConstants(expression);
                return new ReturnStatus<Expression>(expression);
            }
        }

        /// <summary>
        /// Sets the constant values for an expression.
        /// </summary>
        private void SetExpressionConstants(Expression expression)
        {
            if (expression != null)
            {
                expression.Parameters["semitone"] = 1.0 / 127.0;
                expression.Parameters["octave"] = 12.0 / 127.0;

                expression.Parameters["ignore"] = double.NaN;

                expression.Parameters["pi"] = Math.PI;

            }
            else
            {
                //This function should not be called when the expression is null.
                Debug.Assert(false);
            }
        }

        private string GetCurveExpressionString(double inputXVal, List<XyPoint<double>> controlPoints, List<string> curveEquations)
        {
            int curveIndex = 0;
            foreach (XyPoint<double> point in controlPoints)
            {
                if (point.X > inputXVal)
                {
                    break;
                }
                curveIndex++;
            }

            return curveEquations[curveIndex];
        }

    } //End class
} //End namespace

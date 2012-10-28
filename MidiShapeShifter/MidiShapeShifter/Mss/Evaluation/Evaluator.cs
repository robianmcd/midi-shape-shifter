using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using NCalc;

using MidiShapeShifter.CSharpUtil;
using MidiShapeShifter.Mss.Mapping;
using MidiShapeShifter.Mss.Parameters;
using MidiShapeShifter.Mss.MssMsgInfoTypes;

namespace MidiShapeShifter.Mss.Evaluation
{
    /// <summary>
    /// This class is responsible for evaluating expressions.
    /// </summary>
    public class Evaluator : IEvaluator
    {


        public string LastErrorMsg = "";

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
            EvaluationCurveJob evalCurveJob = new EvaluationCurveJob();
            evalCurveJob.Configure(evalInput);
            evalCurveJob.Execute();

            if (evalCurveJob.OutputIsValid)
            {
                return new ReturnStatus<double>(evalCurveJob.OutputVal, true);
            }
            else
            {
                return new ReturnStatus<double>(double.NaN, false);
            }
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
        /// <param name="mssParameterViewer">
        /// Contains info about the MSS parameters which are needed to evaluate equations.
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
                IMssParameterViewer mssParameterViewer,
                IMappingEntry mappingEntry,
                out List<XyPoint<double>> controlPointList,
                out List<List<XyPoint<double>>> curvePointsByCurveList,
                out HashSet<int> erroneousControlPointIndexSet,
                out HashSet<int> erroneousCurveIndexSet
            )
        {
            erroneousControlPointIndexSet = new HashSet<int>();
            erroneousCurveIndexSet = new HashSet<int>();

            curvePointsByCurveList = new List<List<XyPoint<double>>>();

            //Create the eval input for the curve equations
            EvaluationCurveInput evalInput = new EvaluationCurveInput();
            EvaluationCurveJob evalJob = new EvaluationCurveJob();

            //evalJob just being used to calculate the control point locations. This happens when 
            //evalJob.Configure() gets called so there is no need to call evalJob.Execute(). This 
            //means that we can supply dummy values (-1) for the reldata parameters in evalInput.Init().
            evalInput.Init(
                    -1,
                    -1,
                    -1,
                    mssParameterViewer.GetVariableParamInfoList(),
                    mappingEntry);
            evalJob.Configure(evalInput);
            controlPointList = evalJob.controlPointValues;

            if (evalJob.InputIsValid == false) {
                erroneousControlPointIndexSet = evalJob.ErroneousControlPointIndexSet;
                return false;
            }

            int numCurves = mappingEntry.CurveShapeInfo.CurveEquations.Count;
            for(int curveIndex = 0; curveIndex < numCurves; curveIndex++) {
                double curveStartXVal = 0;
                double curveEndXVal = 1;

                if (curveIndex > 0)
                {
                    curveStartXVal = controlPointList[curveIndex - 1].X;
                }

                if (curveIndex < numCurves - 1)
                {
                    curveEndXVal = controlPointList[curveIndex].X;
                }


                List<XyPoint<double>> curCurvePoints = new List<XyPoint<double>>();
                curvePointsByCurveList.Add(curCurvePoints);

                //Add point at start of curve
                if (curveIndex > 0) 
                {
                    curCurvePoints.Add(controlPointList[curveIndex - 1]);
                }
                else
                {
                    if (!evaluateCurveAtXVal(curveStartXVal, mappingEntry, evalInput, evalJob, mssParameterViewer, out erroneousCurveIndexSet, curCurvePoints))
                    {
                        return false;
                    }
                }               
                

                double curXVal = curveStartXVal + xDistanceBetweenPoints;

                //Add points in the middle of the curve
                while (curXVal < curveEndXVal)
                {
                    if (!evaluateCurveAtXVal(curXVal, mappingEntry, evalInput, evalJob, mssParameterViewer, out erroneousCurveIndexSet, curCurvePoints))
                    {
                        return false;
                    }

                    curXVal += xDistanceBetweenPoints;
                }

                //Add point at end of curve
                if (curveIndex < numCurves - 1)
                {
                    curCurvePoints.Add(controlPointList[curveIndex]);
                }
                else
                {
                    if (!evaluateCurveAtXVal(curveEndXVal, mappingEntry, evalInput, evalJob, mssParameterViewer, out erroneousCurveIndexSet, curCurvePoints))
                    {
                        return false;
                    }
                }


            }

            return true;
        }

        protected bool evaluateCurveAtXVal(double inputXVal, IMappingEntry mappingEntry, EvaluationCurveInput evalInput, EvaluationCurveJob evalJob, IMssParameterViewer mssParameterViewer, out HashSet<int> erroneousCurveIndexSet, List<XyPoint<double>> curvePoints)
        {
            erroneousCurveIndexSet = new HashSet<int>();

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

            evalInput.Init(
                relData1,
                relData2,
                relData3,
                mssParameterViewer.GetVariableParamInfoList(),
                mappingEntry);

            evalJob.Configure(evalInput);
            evalJob.Execute();

            if (evalJob.OutputIsValid)
            {
                curvePoints.Add(new XyPoint<double>(inputXVal, evalJob.OutputVal));
            }
            else
            {
                erroneousCurveIndexSet = evalJob.ErroneousCurveIndexSet;
            }

            return evalJob.OutputIsValid;
        }


    } //End class
} //End namespace

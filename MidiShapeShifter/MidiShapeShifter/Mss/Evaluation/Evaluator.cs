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
        /// <param name="numSamplePoints">
        /// Number of points to evaluate the curve equations at.
        /// </param>
        /// <param name="mssParameterViewer">
        /// Contains info about the MSS parameters which are needed to evaluate equations.
        /// </param>
        /// <param name="mappingEntry">
        /// The mapping entry to evaluate equations for.
        /// </param>
        /// <param name="pointList">
        /// The evaluated control points are returned in this list
        /// </param>
        /// <param name="curveYValues">
        /// The evaluated curve equation values are returned in this list. The X value for the ith 
        /// point is i / (numSamplePoints - 1).
        /// </param>
        /// <returns>
        /// true if all equations could be evaluated and false otherwise. The out parameters are only 
        /// garunteed to be valid if the return value is true.
        /// </returns>
        public bool SampleExpressionWithDefaultInputValues(
                int numSamplePoints, 
                IMssParameterViewer mssParameterViewer,
                IMappingEntry mappingEntry,
                out List<XyPoint<double>> pointList,
                out double[] curveYValues)
        {
            curveYValues = new double[numSamplePoints];
            bool outputIsValid = true;

            //Create the eval input for the curve equations
            EvaluationCurveInput evalInput = new EvaluationCurveInput();
            EvaluationCurveJob evalJob = new EvaluationCurveJob();

            //For each sample point evaluate the appropriate curve equation. Sample points are
            //evenly spaced from X=0 to X=1.
            for (int i = 0; i < numSamplePoints; i++)
            {
                //Calculate the X value of the current sample point.
                double curXVal = (double)i / (double)(numSamplePoints - 1);

                //For each sample point data1 data2 and data3 will be set to the X value of the 
                //sample point.
                double relData1 = curXVal;
                double relData2 = curXVal;
                double relData3 = curXVal;

                IStaticMssMsgInfo inMsgInfo = 
                    Factory_StaticMssMsgInfo.Create(mappingEntry.InMssMsgRange.MsgType);

                //If curXVal is outside of the relative input range for data 1 then set 
                //relData1 to NaN
                double max = (double)inMsgInfo.MaxData1Value;
                double min = (double)inMsgInfo.MinData1Value;
                double bottom = (double)mappingEntry.InMssMsgRange.Data1RangeBottom;
                double top = (double)mappingEntry.InMssMsgRange.Data1RangeTop;
                if (curXVal < ((bottom - min) / (max - min + 1)) ||
                    curXVal > ((top - min) / (max - min + 1)))
                {
                    relData1 = double.NaN;
                }

                //If curXVal is outside of the relative input range for data 2 then set relData2 
                //to NaN
                max = (double)inMsgInfo.MaxData2Value;
                min = (double)inMsgInfo.MinData2Value;
                bottom = (double)mappingEntry.InMssMsgRange.Data2RangeBottom;
                top = (double)mappingEntry.InMssMsgRange.Data2RangeTop;
                if (curXVal < ((bottom - min) / (max - min + 1)) ||
                    curXVal > ((top - min) / (max - min + 1)))
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

                if (evalJob.OutputIsValid == true)
                {
                    curveYValues[i] = evalJob.OutputVal;
                }
                else
                {
                    outputIsValid = false;
                    break;
                }
            } //End for loop

            pointList = evalJob.controlPointValues;
            return outputIsValid;
        }

    } //End class
} //End namespace

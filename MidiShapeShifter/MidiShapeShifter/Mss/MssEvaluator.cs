using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using NCalc;

using MidiShapeShifter.CSharpUtil;
using MidiShapeShifter.Mss.Mapping;

namespace MidiShapeShifter.Mss
{
    public class MssEvaluator : MidiShapeShifter.Mss.IMssEvaluator
    {
        public const string DEFAULT_INPUT_STRING = "input";

        public string LastErrorMsg = "";


        public ReturnStatus<double> Evaluate(MssEvaluatorInput input)
        {
            ReturnStatus<List<XyPoint<double>>> evalPointEquationsRetStatus;
            evalPointEquationsRetStatus = EvaluatePointEquations(input);
            
            if (evalPointEquationsRetStatus.IsValid == false)
            {
                return new ReturnStatus<double>();
            }

            List<XyPoint<double>> pointList = evalPointEquationsRetStatus.Value;
            int curveIndex = GetCurveIndex(pointList, input.getPrimaryInputVal());
            string expressionString = CreateExpStrWithEndPointSnap(input, curveIndex, pointList);
            return EvaluateExpression(expressionString, input);
        }

        public bool SampleExpressionWithDefaultInputValues(
                int numSamplePoints, 
                IMssParameterViewer mssParameterViewer,
                IMappingEntry mappingEntry,
                out List<XyPoint<double>> pointList,
                out double[] curveYValues)
        {
            curveYValues = new double[numSamplePoints];
            pointList = new List<XyPoint<double>>();
            bool outputIsValid = true;

            MssEvaluatorInput evalInput = new MssEvaluatorInput();
            evalInput.InitForCurveEquation(
                    double.NaN,
                    double.NaN,
                    double.NaN,
                    mssParameterViewer.GetParameterValue(MssParameterID.VariableA),
                    mssParameterViewer.GetParameterValue(MssParameterID.VariableB),
                    mssParameterViewer.GetParameterValue(MssParameterID.VariableC),
                    mssParameterViewer.GetParameterValue(MssParameterID.VariableD),
                    mappingEntry);

            ReturnStatus<List<XyPoint<double>>> pointListRetStatus = EvaluatePointEquations(evalInput);
            outputIsValid = pointListRetStatus.IsValid;


            if (outputIsValid == true)
            {
                pointList = pointListRetStatus.Value;

                int CurveIndex = 0;

                string expressionString = "";
                Expression expression = null;

                for (int i = 0; i < numSamplePoints; i++)
                {
                    double relI = (double)i / (double)(numSamplePoints - 1);

                    double relData1 = relI;
                    double relData2 = relI;
                    double relData3 = relI;

                    //If relI is outside of the relative input range for data 1 then set relData1 
                    //to NaN
                    double max = (double)mappingEntry.InMssMsgRange.MsgInfo.MaxData1Value;
                    double min = (double)mappingEntry.InMssMsgRange.MsgInfo.MinData1Value;
                    double bottom = (double)mappingEntry.InMssMsgRange.Data1RangeBottom;
                    double top = (double)mappingEntry.InMssMsgRange.Data1RangeTop;
                    if (relI < ((bottom - min) / (max - min + 1)) ||
                        relI > ((top - min) / (max - min + 1)))
                    {
                        relData1 = double.NaN;
                    }

                    //If relI is outside of the relative input range for data 2 then set relData2 
                    //to NaN
                    max = (double)mappingEntry.InMssMsgRange.MsgInfo.MaxData2Value;
                    min = (double)mappingEntry.InMssMsgRange.MsgInfo.MinData2Value;
                    bottom = (double)mappingEntry.InMssMsgRange.Data2RangeBottom;
                    top = (double)mappingEntry.InMssMsgRange.Data2RangeTop;
                    if (relI < ((bottom - min) / (max - min + 1)) ||
                        relI > ((top - min) / (max - min + 1)))
                    {
                        relData2 = double.NaN;
                    }

                    evalInput.InitForCurveEquation(
                        relData1,
                        relData2,
                        relData3,
                        mssParameterViewer.GetParameterValue(MssParameterID.VariableA),
                        mssParameterViewer.GetParameterValue(MssParameterID.VariableB),
                        mssParameterViewer.GetParameterValue(MssParameterID.VariableC),
                        mssParameterViewer.GetParameterValue(MssParameterID.VariableD),
                        mappingEntry);

                    bool curveIndexChanged = false;
                    while(CurveIndex < pointList.Count && relI > pointList[CurveIndex].X)
                    {
                        CurveIndex++;
                        curveIndexChanged = true;
                    }


                    if (i == 0 || curveIndexChanged)
                    {
                        expressionString = CreateExpStrWithEndPointSnap(evalInput, CurveIndex, pointList);
                        ReturnStatus<Expression> expressionRetStatus = createExpression(expressionString);
                        if (expressionRetStatus.IsValid == false)
                        {
                            return false;
                        }
                        else
                        {
                            expression = expressionRetStatus.Value;
                        }
                    }

                    ReturnStatus<double> evalReturnStatus = EvaluateExpression(expression, evalInput);
                    if (evalReturnStatus.IsValid == true)
                    {
                        curveYValues[i] = evalReturnStatus.Value;
                    }
                    else
                    {
                        outputIsValid = false;
                        break;
                    }
                } //End for loop
            }// End if (outputIsValid == true)

            return outputIsValid;
        }

        private ReturnStatus<double> EvaluateExpression(string expressionString, MssEvaluatorInput input)
        {
            double expAsDouble;
            //This is a simple heuristic for evaluating an expression that is just a number which 
            //is usually the case for point equations.
            if (double.TryParse(expressionString, out expAsDouble))
            {
                return new ReturnStatus<double>(expAsDouble, true);
            }
            else
            {
                ReturnStatus<Expression> expressionRetStatus = createExpression(expressionString);
                if (expressionRetStatus.IsValid == false)
                {
                    return new ReturnStatus<double>();
                }
                else
                {
                    Expression expression = expressionRetStatus.Value;
                    return EvaluateExpression(expression, input);
                }
            }
        }

        private ReturnStatus<double> EvaluateExpression(Expression expression, MssEvaluatorInput input)
        {
            try
            {
                InitializeExpression(expression);

                SetExpressionConstants(expression);
                SetExpressionParameters(expression, input);

                double output = System.Convert.ToDouble(expression.Evaluate());

                return new ReturnStatus<double>(output, true);
            }
            catch 
            {
                //TODO: get actual error message
                this.LastErrorMsg = "error!";
                return new ReturnStatus<double>(-1, false);
            }
        }

        private ReturnStatus<double> EvaluateCurveExpressionAtFixedValue(Expression expression, MssEvaluatorInput evalInput, double fixedValue)
        {
            //This function should only be used for curve equations
            Debug.Assert(evalInput.equationType == EquationType.Curve);

            MssEvaluatorInput fixedValueInput = evalInput.Clone();
            fixedValueInput.RelData1 = fixedValue;
            fixedValueInput.RelData2 = fixedValue;
            fixedValueInput.RelData3 = fixedValue;

            return EvaluateExpression(expression, fixedValueInput);
        }

        private void InitializeExpression(Expression expression)
        {
            expression.EvaluateFunction += FunctionHandler;
        }

        private void SetExpressionParameters(Expression expression, MssEvaluatorInput input)
        { 
            //These variables are only aloud in curve equations
            if (input.equationType == EquationType.Curve)
            {
                expression.Parameters["d1"] = input.RelData1;
                expression.Parameters["chan"] = input.RelData1;
                expression.Parameters["channel"] = input.RelData1;

                expression.Parameters["d2"] = input.RelData2;
                expression.Parameters["notenum"] = input.RelData2;
                expression.Parameters["ccnum"] = input.RelData2;

                expression.Parameters["d3"] = input.RelData3;
                expression.Parameters["vel"] = input.RelData3;
                expression.Parameters["velocity"] = input.RelData3;
                expression.Parameters["ccval"] = input.RelData3;

                double primaryInput = input.getPrimaryInputVal();
                expression.Parameters["x"] = primaryInput;
                expression.Parameters[DEFAULT_INPUT_STRING] = primaryInput;
            }

            expression.Parameters["a"] = input.varA;
            expression.Parameters["b"] = input.varB;
            expression.Parameters["c"] = input.varC;
            expression.Parameters["d"] = input.varD;
        }

        private void SetExpressionConstants(Expression expression)
        {
            expression.Parameters["semitone"] = 1.0 / 128.0;
            expression.Parameters["octave"] = 12.0 / 128.0;

            expression.Parameters["pi"] = Math.PI;
        }


        private void FunctionHandler(string funcName, FunctionArgs args)
        {
            
        }

        public ReturnStatus<List<XyPoint<double>>> EvaluatePointEquations(MssEvaluatorInput curveEvalInput)
        {
            MssEvaluatorInput pointEvalInput = new MssEvaluatorInput();
            pointEvalInput.InitForPointEquation(curveEvalInput.varA,
                                                curveEvalInput.varB,
                                                curveEvalInput.varC,
                                                curveEvalInput.varD);

            List<XyPoint<double>> pointPositions = new List<XyPoint<double>>();

            int previousPointXVal = 0;
            foreach (XyPoint<string> pointEquation in curveEvalInput.PointEquations)
            {
                ReturnStatus<double> xValRetStatus =
                    EvaluateExpression(pointEquation.X, pointEvalInput);
                ReturnStatus<double> yValRetStatus =
                    EvaluateExpression(pointEquation.Y, pointEvalInput);

                if (xValRetStatus.IsValid == false || xValRetStatus.Value < previousPointXVal ||
                    xValRetStatus.IsValid == false)
                {
                    //Return invalid return status.
                    return new ReturnStatus<List<XyPoint<double>>>();
                }

                XyPoint<double> curPoint = new XyPoint<double>(xValRetStatus.Value, yValRetStatus.Value);
                pointPositions.Add(curPoint);
            }

            return new ReturnStatus<List<XyPoint<double>>>(pointPositions, true);
        }

        public ReturnStatus<Expression> createExpression(string expressionString)
        {
            try
            {
                Expression expression = new Expression(expressionString, EvaluateOptions.IgnoreCase);
                return new ReturnStatus<Expression>(expression, true);
            }
            catch
            {
                return new ReturnStatus<Expression>();                
            }
        }

        //Returns "" on an error
        protected string CreateExpStrWithEndPointSnap(MssEvaluatorInput evalInput, 
                                                          int curveEquationIndex,
                                                          List<XyPoint<double>> pointPositions)
        {
            //This function should only be called for curve equations.
            Debug.Assert(evalInput.equationType == EquationType.Curve);

            string expressionStr = evalInput.CurveEquations[curveEquationIndex];

            bool pointBeforeCurveExists = (curveEquationIndex > 0);
            bool pointAfterCurveExists = (curveEquationIndex < pointPositions.Count);

            if (pointBeforeCurveExists || pointAfterCurveExists)
            {
                ReturnStatus<Expression> expressionRetStatus = createExpression(expressionStr);
                if (expressionRetStatus.IsValid == false)
                {
                    return "";
                }

                Expression expression = expressionRetStatus.Value;

                XyPoint<double> pointBeforeCurve = null;
                XyPoint<double> rawCurveStart = null;


                XyPoint<double> pointAfterCurve = null;
                XyPoint<double> rawCurveEnd = null;

                if (pointBeforeCurveExists)
                {
                    pointBeforeCurve = pointPositions[curveEquationIndex - 1];

                    ReturnStatus<double> rawCurveStartYRetStatus =
                        EvaluateCurveExpressionAtFixedValue(expression, evalInput, pointBeforeCurve.X);
                    if (rawCurveStartYRetStatus.IsValid == false)
                    {
                        return "";
                    }
                    rawCurveStart = new XyPoint<double>(pointBeforeCurve.X, rawCurveStartYRetStatus.Value);

                }

                if (pointAfterCurveExists)
                {
                    pointAfterCurve = pointPositions[curveEquationIndex];

                    ReturnStatus<double> rawCurveEndYRetStatus =
                        EvaluateCurveExpressionAtFixedValue(expression, evalInput, pointAfterCurve.X);
                    if (rawCurveEndYRetStatus.IsValid == false)
                    {
                        return "";
                    }
                    rawCurveEnd = new XyPoint<double>(pointAfterCurve.X, rawCurveEndYRetStatus.Value);
                }

                if (pointBeforeCurveExists && pointAfterCurveExists)
                {
                    expressionStr += "+" + GetEndPointSnapString(pointBeforeCurve.Y, rawCurveStart.Y, pointBeforeCurve.X, pointAfterCurve.X);
                    expressionStr += "+" + GetEndPointSnapString(pointAfterCurve.Y, rawCurveEnd.Y, pointAfterCurve.X, pointBeforeCurve.X);
                }
                else if (pointBeforeCurveExists)
                {
                    double yOffset = pointBeforeCurve.Y - rawCurveStart.Y;
                    expressionStr += "+" + yOffset.ToString();
                }
                else //pointAfterCurveExists
                {
                    Debug.Assert(pointAfterCurveExists);
                    double yOffset = pointAfterCurve.Y - rawCurveEnd.Y;
                    expressionStr += "+" + yOffset.ToString();
                }

            }

            return expressionStr;
        }

        private string GetEndPointSnapString(double snapToY, double snapFromY, double snapAtX, double dontAffectAtX)
        {
            return "(" + snapToY.ToString() + "-" + snapFromY.ToString() + ") * " +
                   "(" + DEFAULT_INPUT_STRING + "-" + dontAffectAtX.ToString() + ") / " +
                   "(" + snapAtX.ToString() + "-" + dontAffectAtX.ToString() + ")";
        }

        private int GetCurveIndex(List<XyPoint<double>> pointPositions, double XValInput)
        {
            int curveIndex = 0;

            foreach(XyPoint<double> point in pointPositions)
            {
                if (point.X > XValInput)
                {
                    break;
                }
                curveIndex++;
            }

            return curveIndex;
        }

    }
}

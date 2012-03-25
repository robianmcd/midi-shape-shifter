using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NCalc;

using MidiShapeShifter.CSharpUtil;
using System.Diagnostics;

namespace MidiShapeShifter.Mss.Evaluation
{
    public class EvaluationCurveJob : EvaluationJob
    {
        /// <summary>
        /// Default variable that the user will use for input. This variable could represent 
        /// data1 data2 or data3 depending on what the user selects for input.
        /// </summary>
        public const string DEFAULT_INPUT_STRING = "input";

        public const string FUNC_NAME_SNAP = "snap";


        public List<XyPoint<double>> controlPointValues { get; protected set; }

        protected EvaluationCurveInput evalInput;
        protected EvaluationCurveInput previousEvalInput;

        public EvaluationCurveJob()
        {
            this.controlPointValues = new List<XyPoint<double>>();
        }

        public void Configure(EvaluationCurveInput evalInput)
        {
            this.InputIsValid = true;
            this.OutputIsValid = false;

            this.previousEvalInput = this.evalInput;
            this.evalInput = (EvaluationCurveInput) evalInput.Clone();

            if (CalculateControlPointValues() == false)
            {
                return;
            }

            //Get the index of the curve equation for the given primary input value.
            int curveIndex = GetCurveIndex();

            string tmpExpressionStr = FUNC_NAME_SNAP + "(" + 
                                        this.evalInput.CurveEquations[curveIndex] + ")";
            if (InitializeExpressionMembers(tmpExpressionStr) == false)
            {
                return;
            }

            if (this.expression != null)
            {
                SetExpressionBaseParameters((EvaluationInput)evalInput);
                SetExpressionCurveParameters(this.evalInput, this.expression);
            }
        }

        //return true on success
        protected bool CalculateControlPointValues()
        {
            if (PointEquationChanged() == false &&
                VariableChanged() == false)
            {
                return true;
            }

            this.controlPointValues.Clear();

            EvaluationControlPointInput pointEvalInput = new EvaluationControlPointInput();
            pointEvalInput.Init(this.evalInput.varA,
                                this.evalInput.varB,
                                this.evalInput.varC,
                                this.evalInput.varD,
                                "");

            EvaluationControlPointJob pointXEvalJob = new EvaluationControlPointJob();
            EvaluationControlPointJob pointYEvalJob = new EvaluationControlPointJob();


            double previousPointXVal = 0;
            foreach (XyPoint<string> pointEquation in this.evalInput.PointEquations)
            {
                pointEvalInput.EquationStr = pointEquation.X;
                pointXEvalJob.Configure(pointEvalInput);
                pointXEvalJob.Execute();

                pointEvalInput.EquationStr = pointEquation.Y;
                pointYEvalJob.Configure(pointEvalInput);
                pointYEvalJob.Execute();

                if (pointXEvalJob.OutputIsValid == false || 
                    pointXEvalJob.OutputVal < previousPointXVal ||
                    pointYEvalJob.OutputIsValid == false)
                {
                    this.InputIsValid = false;
                    return false;
                }

                XyPoint<double> curPoint = new XyPoint<double>(pointXEvalJob.OutputVal, pointYEvalJob.OutputVal);
                this.controlPointValues.Add(curPoint);

                previousPointXVal = pointXEvalJob.OutputVal;
            }

            return true;
        }

        protected bool VariableChanged()
        {
            if (this.previousEvalInput == null)
            {
                return true;
            }

            if (this.previousEvalInput.varA == this.evalInput.varA &&
                this.previousEvalInput.varB == this.evalInput.varB &&
                this.previousEvalInput.varC == this.evalInput.varC &&
                this.previousEvalInput.varD == this.evalInput.varD)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        protected bool PointEquationChanged()
        {
            if (this.previousEvalInput == null ||
                this.previousEvalInput.PointEquations.Count != this.evalInput.PointEquations.Count)
            {
                return true;
            }

            for (int i = 0; i <= this.evalInput.PointEquations.Count - 1; i++ )
            {
                if (this.previousEvalInput.PointEquations[i].X != this.evalInput.PointEquations[i].X ||
                    this.previousEvalInput.PointEquations[i].Y != this.evalInput.PointEquations[i].Y)
                {
                    return true;
                }
            }

            return false;
        }

        private int GetCurveIndex()
        {
            int curveIndex = 0;
            double XValInput = this.evalInput.getPrimaryInputVal();
            foreach (XyPoint<double> point in this.controlPointValues)
            {
                if (point.X > XValInput)
                {
                    break;
                }
                curveIndex++;
            }

            return curveIndex;
        }

        protected void SetExpressionCurveParameters(EvaluationCurveInput curveEvalInput, Expression exp)
        {
            if (exp != null)
            {
                exp.Parameters["d1"] = curveEvalInput.RelData1;
                exp.Parameters["chan"] = curveEvalInput.RelData1;
                exp.Parameters["channel"] = curveEvalInput.RelData1;

                exp.Parameters["d2"] = curveEvalInput.RelData2;
                exp.Parameters["notenum"] = curveEvalInput.RelData2;
                exp.Parameters["ccnum"] = curveEvalInput.RelData2;

                exp.Parameters["d3"] = curveEvalInput.RelData3;
                exp.Parameters["vel"] = curveEvalInput.RelData3;
                exp.Parameters["velocity"] = curveEvalInput.RelData3;
                exp.Parameters["ccval"] = curveEvalInput.RelData3;

                double primaryInput = curveEvalInput.getPrimaryInputVal();
                exp.Parameters["x"] = primaryInput;
                exp.Parameters[DEFAULT_INPUT_STRING] = primaryInput;
            }
            else
            {
                //This function should not be called when the expression is null.
                Debug.Assert(false);
            }
        }

        protected override void FunctionHandler(string funcName, FunctionArgs args)
        {
            if (funcName == FUNC_NAME_SNAP && args.Parameters.Count() == 1)
            {
                HandleSnapFunc(args);
            }
            else
            {
                BaseFunctionHandler(funcName, args);
            }
        }

        protected void HandleSnapFunc(FunctionArgs args)
        {
            int curveIndex = GetCurveIndex();

            bool pointBeforeCurveExists = (curveIndex > 0);
            bool pointAfterCurveExists = (curveIndex < this.controlPointValues.Count);

            //TODO: evaluate parameter 0 then set the input to the start point and evaluate again
            //then set the input to the end point and evaluate again. Don't need to create 
            //any new expressions!!

            Expression remainingExpression = args.Parameters[0];

            ReturnStatus<double> evaluateStatus = EvaluateExpression(remainingExpression);
            double snapOutputValue = evaluateStatus.Value;

            if (evaluateStatus.IsValid && 
                (pointBeforeCurveExists || pointAfterCurveExists))
            {
                XyPoint<double> pointBeforeCurve = null;
                XyPoint<double> rawCurveStart = null;


                XyPoint<double> pointAfterCurve = null;
                XyPoint<double> rawCurveEnd = null;

                EvaluationCurveInput endpointInput = (EvaluationCurveInput)this.evalInput.Clone();

                if (pointBeforeCurveExists)
                {
                    pointBeforeCurve = controlPointValues[curveIndex - 1];
                    endpointInput.setPrimaryInputVal(pointBeforeCurve.X);
                    SetExpressionCurveParameters(endpointInput, remainingExpression);

                    evaluateStatus = EvaluateExpression(remainingExpression);

                    rawCurveStart = new XyPoint<double>(pointBeforeCurve.X, evaluateStatus.Value);

                }

                if (pointAfterCurveExists && evaluateStatus.IsValid)
                {
                    pointAfterCurve = controlPointValues[curveIndex];
                    endpointInput.setPrimaryInputVal(pointAfterCurve.X);
                    SetExpressionCurveParameters(endpointInput, remainingExpression);

                    evaluateStatus = EvaluateExpression(remainingExpression);

                    rawCurveEnd = new XyPoint<double>(pointAfterCurve.X, evaluateStatus.Value);
                }

                if (evaluateStatus.IsValid)
                {
                    if (pointBeforeCurveExists && pointAfterCurveExists)
                    {
                        snapOutputValue += GetEndPointSnapOffset(pointBeforeCurve.Y, rawCurveStart.Y, 
                            pointBeforeCurve.X, pointAfterCurve.X, this.evalInput.getPrimaryInputVal());

                        snapOutputValue += GetEndPointSnapOffset(pointAfterCurve.Y, rawCurveEnd.Y, 
                            pointAfterCurve.X, pointBeforeCurve.X, this.evalInput.getPrimaryInputVal());
                    }
                    else if (pointBeforeCurveExists)
                    {
                        double yOffset = pointBeforeCurve.Y - rawCurveStart.Y;
                        snapOutputValue += yOffset;
                    }
                    else //pointAfterCurveExists
                    {
                        Debug.Assert(pointAfterCurveExists);
                        double yOffset = pointAfterCurve.Y - rawCurveEnd.Y;
                        snapOutputValue += yOffset;
                    }
                }
            }

            if (evaluateStatus.IsValid)
            {
                args.Result = snapOutputValue;
            }
        }

        protected double GetEndPointSnapOffset(double snapToY, double snapFromY, 
                                             double snapAtX, double dontAffectAtX,
                                             double XInput)
        {
            return (snapToY - snapFromY) * (XInput - dontAffectAtX) / 
                   (snapAtX - dontAffectAtX);
        }

    } //End class
} //End namespace

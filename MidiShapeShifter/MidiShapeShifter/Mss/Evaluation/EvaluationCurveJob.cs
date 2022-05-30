using MidiShapeShifter.CSharpUtil;
using MidiShapeShifter.Mss.Parameters;
using NCalc;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MidiShapeShifter.Mss.Evaluation
{
    /// <summary>
    /// This class is responsible for evaluating an equation associated with a curve.
    /// </summary>
    public class EvaluationCurveJob : EvaluationJob
    {
        /// <summary>
        /// Default variable that the user will use for input. This variable could represent 
        /// data1 data2 or data3 depending on what the user selects for input.
        /// </summary>
        public const string DEFAULT_INPUT_STRING = "input";

        public const string FUNC_NAME_SNAP = "snap";
        public const string FUNC_NAME_LFO = "lfo";
        public const string FUNC_NAME_WAVEFORM = "waveform";

        public HashSet<int> ErroneousControlPointIndexSet = new HashSet<int>();
        public HashSet<int> ErroneousCurveIndexSet = new HashSet<int>();


        /// <summary>
        /// Stores the x and y coordinates of each control point. This information is 
        /// nessesary for determining which curve equation to use given an X value.
        /// </summary>
        public List<XyPoint<double>> controlPointValues { get; protected set; }

        /// <summary>
        /// The input for this job
        /// </summary>
        protected EvaluationCurveInput evalInput;

        public override bool Execute()
        {
            base.Execute();

            //If the output is invalid and it was not the result of an invalid control point then 
            //blame it on the current curve.
            if (this.OutputIsValid == false && this.ErroneousControlPointIndexSet.Count == 0)
            {
                this.ErroneousCurveIndexSet.Add(GetCurveIndex());
            }
            else
            {
                this.ErroneousCurveIndexSet.Clear();
            }

            return this.OutputIsValid;
        }

        public EvaluationCurveJob()
        {
            this.controlPointValues = new List<XyPoint<double>>();
        }

        /// <summary>
        /// Initializes this class.
        /// </summary>
        public void Configure(EvaluationCurveInput evalInput, List<XyPoint<double>> controlPointValues, Expression expression)
        {
            this.OutputIsValid = false;

            this.evalInput = evalInput;

            this.controlPointValues = controlPointValues;
            this.expression = expression;

            this.expression.EvaluateFunction += FunctionHandler;
            SetExpressionBaseParameters((EvaluationInput)evalInput);
            SetExpressionCurveParameters(this.evalInput, this.expression);
        }

        /// <summary>
        /// Gets the index of the curve equation that will be evaluated when execute is called. The 
        /// values in this.controlPointValues should be initialized before calling this function.
        /// </summary>
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

        /// <summary>
        /// Set the parameters for the expression specified by exp that are associated with a curve equation.
        /// </summary>
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

        //See EvaluationJob.FunctionHandler for documentation
        protected override ReturnStatus<bool> HandleCustomFunctions(string funcName,
                                                      FunctionArgs args,
                                                      List<double> evaluatedArgs)
        {
            ReturnStatus<bool> retStatus = new ReturnStatus<bool>();

            switch (funcName)
            {
                case FUNC_NAME_SNAP:
                    retStatus.IsValid = HandleSnapFunc(args, evaluatedArgs);
                    break;
                case FUNC_NAME_LFO:
                    retStatus.IsValid = HandleLfoFunc(args, evaluatedArgs);
                    break;
                case FUNC_NAME_WAVEFORM:
                    retStatus.IsValid = HandleWaveformFunc(args, evaluatedArgs);
                    break;
                default:
                    return HandleBaseFunctions(funcName, args, evaluatedArgs);
            }

            retStatus.Value = true;
            return retStatus;

        }

        protected bool HandleLfoFunc(FunctionArgs args, List<double> evaluatedArgs)
        {
            if (evaluatedArgs.Count != 6)
            {
                return false;
            }

            double input = evaluatedArgs[0];
            double attack = evaluatedArgs[1];
            WaveformShap shape = (WaveformShap)evaluatedArgs[2];
            double phase = evaluatedArgs[3];
            double cycles = evaluatedArgs[4];
            double amount = evaluatedArgs[5];

            if (attack < 0 || attack > 1)
            {
                return false;
            }

            if (Enum.IsDefined(typeof(WaveformShap), shape) == false)
            {
                return false;
            }

            //input
            double output = input;

            //cycles
            output *= cycles;

            //phase
            output += phase % 360 / 360.0;

            //shape
            HandleWaveformFunc(output, shape, out output);

            output -= 0.5;

            //attack
            double attackAtInput;
            if (attack == 0)
            {
                attackAtInput = 1;
            }
            else
            {
                attackAtInput = Math.Min(1, input / attack);
            }
            output *= attackAtInput;

            //amount
            output *= amount;

            output += 0.5;

            args.Result = output;
            return true;
        }

        protected bool HandleWaveformFunc(FunctionArgs args, List<double> evaluatedArgs)
        {
            if (evaluatedArgs.Count != 2)
            {
                return false;
            }

            double output;
            bool success = HandleWaveformFunc(evaluatedArgs[0],
                                              (WaveformShap)evaluatedArgs[1],
                                              out output);
            args.Result = output;
            return success;
        }

        protected bool HandleWaveformFunc(double input, WaveformShap shape, out double output)
        {
            //simply using "input % 1" does not work for negative values.
            double inAsRamp = (1 + (input % 1)) % 1;

            switch (shape)
            {
                case WaveformShap.Sin:
                    output = Math.Sin(2 * Math.PI * input) / 2 + 0.5;
                    break;
                case WaveformShap.Triangle:
                    //Offset the base ramp wave so that the triangle wave starts at 0.5 instead 
                    //of 0.
                    double inAsOffsetRamp = (1 + ((input + 0.25) % 1)) % 1;
                    output = 1 - Math.Abs(inAsOffsetRamp - 0.5) * 2;
                    break;
                case WaveformShap.Square:
                    output = System.Convert.ToDouble(inAsRamp < 0.5);
                    break;
                case WaveformShap.Ramp:
                    output = inAsRamp;
                    break;
                case WaveformShap.Saw:
                    output = 1 - inAsRamp;
                    break;
                default:
                    output = double.NaN;
                    return false;
            }

            return true;
        }

        /// <summary>
        /// The snap function will transform a curve so that it starts and ends at the control points on either side of it.
        /// </summary>
        /// <param name="args">args should have 1 parameter that stores an equation to apply snap to.</param>
        protected bool HandleSnapFunc(FunctionArgs args, List<double> evaluatedArgs)
        {
            if (evaluatedArgs.Count != 1)
            {
                return false;
            }

            //Get the index of the curve to apply snap to.
            int curveIndex = GetCurveIndex();

            //True if there is a control point before the curve.
            bool pointBeforeCurveExists = (curveIndex > 0);
            //True if there is a control point after the curve.
            bool pointAfterCurveExists = (curveIndex < this.controlPointValues.Count);

            Expression remainingExpression = args.Parameters[0];

            //Store the raw output of the argument.
            double snapOutputValue = evaluatedArgs[0];

            bool errorEncountered = false;
            ReturnStatus<double> evaluateStatus;

            //If there is a control point on either side of the equation then snap will need to 
            //modify the output.
            if (pointBeforeCurveExists || pointAfterCurveExists)
            {
                XyPoint<double> pointBeforeCurve = null;
                XyPoint<double> rawCurveStart = null;

                XyPoint<double> pointAfterCurve = null;
                XyPoint<double> rawCurveEnd = null;

                EvaluationCurveInput endpointInput = (EvaluationCurveInput)this.evalInput.Clone();

                //If there is a control point before the equation then we need to evaluate the 
                //equation at this point.
                if (pointBeforeCurveExists)
                {
                    pointBeforeCurve = controlPointValues[curveIndex - 1];
                    endpointInput.setPrimaryInputVal(pointBeforeCurve.X);
                    SetExpressionCurveParameters(endpointInput, remainingExpression);

                    evaluateStatus = EvaluateExpression(remainingExpression);
                    errorEncountered |= !evaluateStatus.IsValid;
                    rawCurveStart = new XyPoint<double>(pointBeforeCurve.X, evaluateStatus.Value);

                }

                //If there is a control point after the equation then we need to evaluate the 
                //equation at this point.
                if (pointAfterCurveExists && errorEncountered == false)
                {
                    pointAfterCurve = controlPointValues[curveIndex];
                    endpointInput.setPrimaryInputVal(pointAfterCurve.X);
                    SetExpressionCurveParameters(endpointInput, remainingExpression);

                    evaluateStatus = EvaluateExpression(remainingExpression);
                    errorEncountered |= !evaluateStatus.IsValid;

                    rawCurveEnd = new XyPoint<double>(pointAfterCurve.X, evaluateStatus.Value);
                }

                if (errorEncountered == false)
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

            if (errorEncountered)
            {
                return false;
            }
            else
            {
                args.Result = snapOutputValue;
                return true;
            }
        }

        /// <summary>
        /// This method will essentially find a line that can be added to a function which will 
        /// cause the function to pass through the point (snapAtX, snapToY) without affecting the 
        /// functions value where X = dontAffectAtX. Instead of operating on a function this method
        /// operates on a point so the amount of Y offset to apply to the function when X = XInput
        /// is returned.
        /// </summary>
        /// <param name="snapToY">The Y value that the function should pass through when X = snapAtX</param>
        /// <param name="snapFromY">The Y value of the function when X = snapAtX</param>
        /// <param name="snapAtX">See snapToY and snapFromY</param>
        /// <param name="dontAffectAtX">The X value where the function should not be affected after</param>
        /// <param name="XInput">The X value that the function is currently being evaluated at</param>
        /// <returns>Y offset to apply to the function when X = XInput</returns>
        protected double GetEndPointSnapOffset(double snapToY, double snapFromY,
                                             double snapAtX, double dontAffectAtX,
                                             double XInput)
        {
            return (snapToY - snapFromY) * (XInput - dontAffectAtX) /
                   (snapAtX - dontAffectAtX);
        }

    } //End class
} //End namespace

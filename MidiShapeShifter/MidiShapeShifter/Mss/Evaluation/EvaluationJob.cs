using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using NCalc;

using MidiShapeShifter.CSharpUtil;

namespace MidiShapeShifter.Mss.Evaluation
{
    public abstract class EvaluationJob
    {
        public double OutputVal {
            get {
                if (OutputIsValid == false)
                {
                    Debug.Assert(false);
                }
                return _outputVal;
            }

            protected set {
                _outputVal = value;
            }
        }
        private double _outputVal;

        public bool InputIsValid { get; protected set; }
        public bool OutputIsValid { get; protected set; }
        protected bool useConstantOutput = false;


        protected string expressionStr { get; set; }
        protected Expression expression { get; set;}

        public bool Execute()
        {
            if (InputIsValid)
            {
                if (useConstantOutput)
                {
                    //The output value was set in configure()
                    this.OutputIsValid = true;
                }
                else
                {
                    ReturnStatus<double> outputStatus = EvaluateExpression(this.expression);
                    this.OutputIsValid = outputStatus.IsValid;
                    this.OutputVal = outputStatus.Value;
                }
            }
            else //Input is not valid
            {
                this.OutputIsValid = false;
            }
            return this.OutputIsValid;
        }

        protected ReturnStatus<double> EvaluateExpression(Expression exp)
        {
            try
            {
                double output = System.Convert.ToDouble(exp.Evaluate());
                return new ReturnStatus<double>(output, true);
            }
            catch
            {
                return new ReturnStatus<double>(double.NaN, false);
            }
        }

        protected bool InitializeExpressionMembers(string expressionStr)
        {
            //Only regenerate the expression if it has changed
            if (this.expressionStr != expressionStr.ToLower())
            {
                this.expressionStr = expressionStr.ToLower();

                double constantOutput;
                //This is a simple heuristic for evaluating an expression that is just a number which 
                //is usually the case for control point equations.
                if (double.TryParse(expressionStr, out constantOutput))
                {
                    useConstantOutput = true;
                    this.OutputVal = constantOutput;
                    this.OutputIsValid = true;
                    this.expression = null;
                }
                else
                {
                    useConstantOutput = false;

                    ReturnStatus<Expression> expressionRetStatus = CreateExpression(expressionStr);
                    if (expressionRetStatus.IsValid == false)
                    {
                        this.InputIsValid = false;
                    }
                    else
                    {
                        this.expression = expressionRetStatus.Value;

                        this.expression.EvaluateFunction += FunctionHandler;
                        SetExpressionConstants();
                    }
                }
            }

            return InputIsValid;
        }

        protected ReturnStatus<Expression> CreateExpression(string expressionString)
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

        private void SetExpressionConstants()
        {
            if (this.expression != null)
            {
                this.expression.Parameters["semitone"] = 1.0 / 127.0;
                this.expression.Parameters["octave"] = 12.0 / 127.0;

                this.expression.Parameters["ignore"] = double.NaN;

                this.expression.Parameters["pi"] = Math.PI;

            }
            else
            {
                //This function should not be called when the expression is null.
                Debug.Assert(false);
            }
            
        }

        protected void SetExpressionBaseParameters(EvaluationInput input)
        {
            if (this.expression != null)
            {
                this.expression.Parameters["a"] = input.varA;
                this.expression.Parameters["b"] = input.varB;
                this.expression.Parameters["c"] = input.varC;
                this.expression.Parameters["d"] = input.varD;
            }
            else
            {
                //This function should not be called when the expression is null.
                Debug.Assert(false);
            }

        }

        protected virtual void FunctionHandler(string funcName, FunctionArgs args)
        {
            BaseFunctionHandler(funcName, args);
        }

        //Return true if the function was handled
        protected bool BaseFunctionHandler(string funcName, FunctionArgs args)
        {
            return false;
            //TODO: handel functions here
        }

    }
}

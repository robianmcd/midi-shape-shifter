using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using NCalc;

using MidiShapeShifter.CSharpUtil;

namespace MidiShapeShifter.Mss.Evaluation
{
    /// <summary>
    /// This is the base class for an evaluation job. An evaluation job is 
    /// responsible for evaluation an equation. Classes inheriting from this
    /// class should create a configure(...) method that prepares the class 
    /// for a call to Execute. After calling Configure() and Execute(), the 
    /// output value can be retrieved from OutputVal if OutputIsValid is
    /// set to true.
    /// </summary>
    public abstract class EvaluationJob
    {
        public const string FUNC_NAME_LIMIT = "limit";

        /// <summary>
        /// Stores the value of the evaluated equation if OutputIsValid is 
        /// true.
        /// </summary>
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

        /// <summary>
        /// This should be set to false if there is an error during 
        /// configuration due to invalid input.
        /// </summary>
        protected bool inputIsValid { get; protected set; }

        /// <summary>
        /// Specifies whether OutputVal is valid.
        /// </summary>
        public bool OutputIsValid { get; protected set; }

        /// <summary>
        /// During configureation if it is clear that the input equation will always return the same 
        /// value then OutputVal can be set to that value and this can be set to true so that we 
        /// don't have to create an execute an Expression everytime Evecute() is called. This is
        /// useful when an equation is just a number and setting this flag will improve 
        /// preformance.
        /// </summary>
        protected bool useConstantOutput = false;

        /// <summary>
        /// The equation as a string.
        /// </summary>
        protected string expressionStr { get; set; }

        /// <summary>
        /// The equation as an Expression object.
        /// </summary>
        protected Expression expression { get; set;}

        /// <summary>
        /// Evaluates the equation and stores the output in OutputVal.
        /// </summary>
        /// <returns>
        /// True if the output is valid and false if the equation could not be evaluated.
        /// </returns>
        public bool Execute()
        {
            if (inputIsValid)
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

        /// <summary>
        /// Evaluate exp and return the output
        /// </summary>
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

        /// <summary>
        /// Initialize this.expressionStr and this.expression. This should be called during 
        /// configuration.
        /// </summary>
        /// <returns>True on success, false if the input expressionStr was not valid</returns>
        protected bool InitializeExpressionMembers(string rawExpressionStr)
        {
            //Add the limit function to the equation.
            string limitedExpressionStr = FUNC_NAME_LIMIT + "(" + rawExpressionStr + ")";

            //Only regenerate the expression if it has changed
            if (this.expressionStr != limitedExpressionStr.ToLower())
            {
                this.expressionStr = limitedExpressionStr.ToLower();

                double constantOutput;
                //This is a simple heuristic for evaluating an expression that is just a number which 
                //is usually the case for control point equations.
                if (double.TryParse(rawExpressionStr, out constantOutput))
                {
                    //TODO: limit should be applied to the output value

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
                        this.inputIsValid = false;
                    }
                    else
                    {
                        this.expression = expressionRetStatus.Value;

                        this.expression.EvaluateFunction += FunctionHandler;
                        SetExpressionConstants();
                    }
                }
            }

            return inputIsValid;
        }

        /// <summary>
        /// Create an expression object from a string.
        /// </summary>
        protected ReturnStatus<Expression> CreateExpression(string expressionString)
        {
            try
            {
                Expression expression = new Expression(expressionString, EvaluateOptions.IgnoreCase);
                return new ReturnStatus<Expression>(expression, true);
            }
            catch
            {
                //Return invalid return status
                return new ReturnStatus<Expression>();
            }
        }

        /// <summary>
        /// Sets the constant values for an expression.
        /// </summary>
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

        /// <summary>
        /// Sets the parameters that are common to all EvaluationJobs. This should be called by the 
        /// class inheriting from this class during configuration.
        /// </summary>
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

        /// <summary>
        /// Called by NCalc when attempting to evaluate custom functions.
        /// </summary>
        /// <param name="funcName">Name of the function being evaluated</param>
        /// <param name="args">Arguments for the function specified by funcName</param>
        protected virtual void FunctionHandler(string funcName, FunctionArgs args)
        {
            BaseFunctionHandler(funcName, args);
        }

        //Return true if the function was handled
        protected bool BaseFunctionHandler(string funcName, FunctionArgs args)
        {
            int numParams = args.Parameters.Count();

            if (funcName == FUNC_NAME_LIMIT && numParams == 1)
            {
                ReturnStatus<double> evaluateStatus = EvaluateExpression(args.Parameters[0]);
                if (evaluateStatus.IsValid)
                {
                    if (evaluateStatus.Value > 1)
                    {
                        args.Result = 1;
                    }
                    else if (evaluateStatus.Value < 0)
                    {
                        args.Result = 0;
                    }
                    else
                    {
                        args.Result = evaluateStatus.Value;
                    }
                }
            }
            return false;
            //TODO: handel functions here
        }

    }
}

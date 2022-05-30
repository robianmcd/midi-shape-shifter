using MidiShapeShifter.CSharpUtil;
using MidiShapeShifter.Mss.Parameters;
using NCalc;
using System.Collections.Generic;
using System.Diagnostics;

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

        protected bool functionHandlingErrorEncountered;

        /// <summary>
        /// Stores the value of the evaluated equation if OutputIsValid is 
        /// true.
        /// </summary>
        public double OutputVal
        {
            get
            {
                if (OutputIsValid == false)
                {
                    Debug.Assert(false);
                }
                return _outputVal;
            }

            protected set
            {
                _outputVal = value;
            }
        }
        private double _outputVal;

        /// <summary>
        /// Specifies whether OutputVal is valid.
        /// </summary>
        public bool OutputIsValid { get; protected set; }


        /// <summary>
        /// The equation as a string.
        /// </summary>
        protected string expressionStr { get; set; }

        /// <summary>
        /// The equation as an Expression object.
        /// </summary>
        protected Expression expression { get; set; }

        /// <summary>
        /// Evaluates the equation and stores the output in OutputVal.
        /// </summary>
        /// <returns>
        /// True if the output is valid and false if the equation could not be evaluated.
        /// </returns>
        public virtual bool Execute()
        {
            ReturnStatus<double> outputStatus = EvaluateExpression(this.expression);
            this.OutputIsValid = outputStatus.IsValid;
            this.OutputVal = outputStatus.Value;

            return this.OutputIsValid;
        }

        /// <summary>
        /// Evaluate exp and return the output
        /// </summary>
        protected ReturnStatus<double> EvaluateExpression(Expression exp)
        {
            try
            {
                functionHandlingErrorEncountered = false;
                double output = System.Convert.ToDouble(exp.Evaluate());

                if (functionHandlingErrorEncountered == true)
                {
                    functionHandlingErrorEncountered = false;
                    return new ReturnStatus<double>(double.NaN, false);
                }
                else
                {
                    return new ReturnStatus<double>(output, true);
                }
            }
            catch
            {
                return new ReturnStatus<double>(double.NaN, false);
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
                char charVarName = 'a';
                foreach (MssParamInfo varInfo in input.VariableParamInfoList)
                {
                    double relVal = CustomMathUtils.AbsToRelVal(varInfo.MinValue, varInfo.MaxValue, varInfo.GetValue());

                    this.expression.Parameters[varInfo.Name.ToLower()] = varInfo.GetValue();
                    this.expression.Parameters[varInfo.Name.ToLower() + "_rel"] = relVal;

                    if (charVarName <= 'z')
                    {
                        this.expression.Parameters[charVarName.ToString()] = varInfo.GetValue();
                        this.expression.Parameters[charVarName.ToString() + "_rel"] = relVal;
                        charVarName++;
                    }
                }

                int paramNum = 1;
                foreach (MssParamInfo varInfo in input.TransformParamInfoList)
                {
                    double relVal = CustomMathUtils.AbsToRelVal(varInfo.MinValue, varInfo.MaxValue, varInfo.GetValue());

                    this.expression.Parameters[varInfo.Name.ToLower()] = varInfo.GetValue();
                    this.expression.Parameters[varInfo.Name.ToLower() + "_rel"] = relVal;

                    this.expression.Parameters["p" + paramNum.ToString()] = varInfo.GetValue();
                    this.expression.Parameters["p" + paramNum.ToString() + "_rel"] = relVal;
                    paramNum++;
                }
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
        protected void FunctionHandler(string funcName, FunctionArgs args)
        {
            List<double> evaluatedParams = new List<double>(args.Parameters.Length);

            foreach (Expression curParam in args.Parameters)
            {
                ReturnStatus<double> evaluateStatus = EvaluateExpression(curParam);
                if (evaluateStatus.IsValid)
                {
                    evaluatedParams.Add(evaluateStatus.Value);
                }
                else
                {
                    args.Result = double.NaN;
                    this.functionHandlingErrorEncountered = true;
                    return;
                }
            }

            ReturnStatus<bool> retStatus = HandleCustomFunctions(funcName, args, evaluatedParams);
            if (retStatus.IsValid == false)
            {
                args.Result = double.NaN;
                this.functionHandlingErrorEncountered = true;
            }
        }

        protected virtual ReturnStatus<bool> HandleCustomFunctions(string funcName,
                                                     FunctionArgs args,
                                                     List<double> evaluatedArgs)
        {
            return HandleBaseFunctions(funcName, args, evaluatedArgs);
        }

        //Return true if the function was handled
        protected ReturnStatus<bool> HandleBaseFunctions(string funcName,
                                           FunctionArgs args,
                                           List<double> evaluatedArgs)
        {
            ReturnStatus<bool> retStatus = new ReturnStatus<bool>();

            switch (funcName)
            {
                case FUNC_NAME_LIMIT:
                    retStatus.IsValid = HandleLimitFunc(args, evaluatedArgs);
                    break;
                default:
                    retStatus.IsValid = true;
                    retStatus.Value = false;
                    return retStatus;
            }

            retStatus.Value = true;
            return retStatus;
        }

        protected bool HandleLimitFunc(FunctionArgs args, List<double> evaluatedArgs)
        {
            if (evaluatedArgs.Count != 1)
            {
                return false;
            }

            if (evaluatedArgs[0] > 1)
            {
                args.Result = 1;
            }
            else if (evaluatedArgs[0] < 0)
            {
                args.Result = 0;
            }
            else
            {
                args.Result = evaluatedArgs[0];
            }

            return true;
        }

    }
}

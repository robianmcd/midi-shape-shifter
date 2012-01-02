using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NCalc;

using MidiShapeShifter.CSharpUtil;

namespace MidiShapeShifter.Mss
{
    public class MssEvaluator : MidiShapeShifter.Mss.IMssEvaluator
    {
        public string LastErrorMsg = "";

        //precondition: expressionString is valid
        public ReturnStatus<double> Evaluate(string expressionString, double input)
        {
            Expression expression = new Expression(expressionString, EvaluateOptions.IgnoreCase);
            InitializeExpression(expression);

            return EvaluateExpression(expression, input);
        }

        public ReturnStatus<double[]> EvaluateMultipleInputValues(string expressionString, double[] inputValues)
        {
            double[] outputValues = new double[inputValues.Length];
            bool outputIsValid;

            //The Exporession constructor throws an exception if the expression string is empty
            if (expressionString == "")
            {
                outputIsValid = false;
            }
            else
            {

                Expression expression = new Expression(expressionString, EvaluateOptions.IgnoreCase);
                InitializeExpression(expression);

                outputIsValid = true;
                for (int i = 0; i < inputValues.Length; i++)
                {
                    ReturnStatus<double> evalReturnStatus = EvaluateExpression(expression, inputValues[i]);
                    if (evalReturnStatus.IsValid == true)
                    {
                        outputValues[i] = evalReturnStatus.ReturnVal;
                    }
                    else
                    {
                        outputIsValid = false;
                        break;
                    }
                }
            }

            return new ReturnStatus<double[]>(outputValues, outputIsValid);
        }

        private ReturnStatus<double> EvaluateExpression(Expression expression, double input)
        {
            SetExpressionInput(expression, input);

            try
            {
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

        private void InitializeExpression(Expression expression)
        {
            expression.EvaluateFunction += FunctionHandler;
        }

        private void SetExpressionInput(Expression expression, double input)
        { 
            expression.Parameters["x"] = input;
            expression.Parameters["input"] = input;       
        }


        private void FunctionHandler(string funcName, FunctionArgs args)
        {
            
        }
    }
}

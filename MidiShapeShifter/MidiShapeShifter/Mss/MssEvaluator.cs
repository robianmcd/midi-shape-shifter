using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NCalc;

namespace MidiShapeShifter.Mss
{
    public class MssEvaluator
    {
        public string LastErrorMsg = "";

        //precondition: expressionString is valid
        public bool Evaluate(string expressionString, double input, out double output)
        {
            Expression expression = new Expression(expressionString, EvaluateOptions.IgnoreCase);
            InitializeExpression(expression);

            return EvaluateExpression(expression, input, out output);
        }

        public bool EvaluateMultipleInputValues(string expressionString, double[] inputValues, out double[] outputValues)
        {
            Expression expression = new Expression(expressionString, EvaluateOptions.IgnoreCase);
            InitializeExpression(expression);

            outputValues = new double[inputValues.Length];
            for(int i = 0; i < inputValues.Length; i++)
            {
                if (EvaluateExpression(expression, inputValues[i], out outputValues[i]) == false)
                {
                    return false;
                }
            }

            return true;
        }

        private bool EvaluateExpression(Expression expression, double input, out double output)
        {
            SetExpressionInput(expression, input);

            try
            {
                output = (double)expression.Evaluate();
                return true;
            }
            catch (Exception exception)
            {
                output = -1;
                //TODO: get actual error message
                this.LastErrorMsg = "error!";
                return false;
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

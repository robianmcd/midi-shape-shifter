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
        public ReturnStatus<double> Evaluate(string expressionString, MssEvaluatorInput input)
        {
            Expression expression = new Expression(expressionString, EvaluateOptions.IgnoreCase);
            InitializeExpression(expression);

            return EvaluateExpression(expression, input);
        }

        public ReturnStatus<double[]> SampleExpressionWithDefaultInputValues(
                string expressionString, int numSamplePoints, IMssParameterViewer mssParameterViewer)
        {
            double[] outputValues = new double[numSamplePoints];
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
                MssEvaluatorInput defaultInput = new MssEvaluatorInput();
                for (int i = 0; i < numSamplePoints; i++)
                {
                    defaultInput.Reinit(
                        (double)i / (double) numSamplePoints,
                        (double)i / (double) numSamplePoints,
                        (double)i / (double) numSamplePoints,
                        mssParameterViewer.GetParameterValue(MssParameterID.VariableA),
                        mssParameterViewer.GetParameterValue(MssParameterID.VariableB),
                        mssParameterViewer.GetParameterValue(MssParameterID.VariableC),
                        mssParameterViewer.GetParameterValue(MssParameterID.VariableD));
                    ReturnStatus<double> evalReturnStatus = EvaluateExpression(expression, defaultInput);
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

        private ReturnStatus<double> EvaluateExpression(Expression expression, MssEvaluatorInput input)
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

        private void SetExpressionInput(Expression expression, MssEvaluatorInput input)
        { 
            expression.Parameters["z"] = input.RelData1;
            expression.Parameters["d1"] = input.RelData1;
            expression.Parameters["chan"] = input.RelData1;
            expression.Parameters["channel"] = input.RelData1;

            expression.Parameters["y"] = input.RelData2;
            expression.Parameters["d2"] = input.RelData2;
            expression.Parameters["notenum"] = input.RelData2;
            expression.Parameters["ccnum"] = input.RelData2;

            expression.Parameters["x"] = input.RelData3;
            expression.Parameters["d3"] = input.RelData3;
            expression.Parameters["vel"] = input.RelData3;
            expression.Parameters["velocity"] = input.RelData3;
            expression.Parameters["ccval"] = input.RelData3;

            expression.Parameters["a"] = input.varA;
            expression.Parameters["b"] = input.varB;
            expression.Parameters["c"] = input.varC;
            expression.Parameters["d"] = input.varD;
        }


        private void FunctionHandler(string funcName, FunctionArgs args)
        {
            
        }
    }
}

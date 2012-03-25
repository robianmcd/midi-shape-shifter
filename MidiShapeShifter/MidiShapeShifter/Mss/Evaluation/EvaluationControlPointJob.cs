using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MidiShapeShifter.CSharpUtil;

namespace MidiShapeShifter.Mss.Evaluation
{
    public class EvaluationControlPointJob : EvaluationJob
    {
        protected EvaluationControlPointInput evalInput;

        public void Configure(EvaluationControlPointInput evalInput)
        {
            this.InputIsValid = true;
            this.OutputIsValid = false;

            this.evalInput = evalInput;

            if (InitializeExpressionMembers(evalInput.EquationStr) == false)
            {
                return;
            }

            if (this.expression != null)
            {
                SetExpressionBaseParameters((EvaluationInput) evalInput);
            }
        }


    }
}

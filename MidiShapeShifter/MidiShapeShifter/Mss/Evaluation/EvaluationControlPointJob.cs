using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MidiShapeShifter.CSharpUtil;

namespace MidiShapeShifter.Mss.Evaluation
{
    /// <summary>
    /// This class is responsible for evaluating an equation associated with a control point.
    /// Most of the functionality for this class is implimented in EvaluationJob.
    /// </summary>
    public class EvaluationControlPointJob : EvaluationJob
    {
        protected EvaluationControlPointInput evalInput;

        /// <summary>
        /// Initializes this class. This must be called before calling Execute().
        /// </summary>
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

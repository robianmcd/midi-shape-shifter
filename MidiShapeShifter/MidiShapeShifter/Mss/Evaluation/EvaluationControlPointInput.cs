using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.Evaluation
{
    public class EvaluationControlPointInput : EvaluationInput
    {
        public string EquationStr { get; set; }

        public void Init(double varA, double varB, double varC, double varD, string equationStr)
        {
            this.equationType = EquationType.Point;

            this.varA = varA;
            this.varB = varB;
            this.varC = varC;
            this.varD = varD;

            this.EquationStr = equationStr;
        }
    }
}

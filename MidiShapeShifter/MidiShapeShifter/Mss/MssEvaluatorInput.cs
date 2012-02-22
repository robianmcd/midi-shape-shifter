using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss
{
    public class MssEvaluatorInput
    {
        public double RelData1 { get;  private set; }
        public double RelData2 { get; private set; }
        public double RelData3 { get; private set; }

        public double varA { get; private set; }
        public double varB { get; private set; }
        public double varC { get; private set; }
        public double varD { get; private set; }

        //Must be called before this object is used. Can be called multiple times.
        public void Reinit(double relData1, double relData2, double relData3,
                           double varA, double varB, double varC, double varD)
        {
            this.RelData1 = relData1;
            this.RelData2 = relData2;
            this.RelData3 = relData3;

            this.varA = varA;
            this.varB = varB;
            this.varC = varC;
            this.varD = varD;
        }
    }
}

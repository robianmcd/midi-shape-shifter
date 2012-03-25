using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using MidiShapeShifter.CSharpUtil;
using MidiShapeShifter.Mss.Mapping;
using MidiShapeShifter.Mss.MssMsgInfoTypes;

namespace MidiShapeShifter.Mss.Evaluation
{
    public abstract class EvaluationInput : ICloneable
    {
        public EquationType equationType { get; protected set; }

        public double varA { get; set; }
        public double varB { get; set; }
        public double varC { get; set; }
        public double varD { get; set; }


        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}

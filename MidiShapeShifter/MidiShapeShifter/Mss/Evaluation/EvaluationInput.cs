using MidiShapeShifter.Mss.Parameters;
using System;
using System.Collections.Generic;

namespace MidiShapeShifter.Mss.Evaluation
{
    /// <summary>
    /// This class wraps up all of the input information that is sent to an 
    /// EvaluationJob.
    /// </summary>
    public abstract class EvaluationInput : ICloneable
    {
        public abstract EquationType equationType { get; }

        public List<MssParamInfo> VariableParamInfoList;
        public List<MssParamInfo> TransformParamInfoList;


        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}

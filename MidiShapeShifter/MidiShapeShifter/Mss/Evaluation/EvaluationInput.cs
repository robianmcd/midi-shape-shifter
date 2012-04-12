using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using MidiShapeShifter.CSharpUtil;
using MidiShapeShifter.Mss.Mapping;
using MidiShapeShifter.Mss.MssMsgInfoTypes;
using MidiShapeShifter.Mss.Parameters;

namespace MidiShapeShifter.Mss.Evaluation
{
    /// <summary>
    /// This class wraps up all of the input information that is sent to an 
    /// EvaluationJob.
    /// </summary>
    public abstract class EvaluationInput : ICloneable
    {
        public abstract EquationType equationType { get; }

        public List<MssParameterInfo> VariableParamInfoList;
        public List<MssParameterInfo> TransformParamInfoList;


        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}

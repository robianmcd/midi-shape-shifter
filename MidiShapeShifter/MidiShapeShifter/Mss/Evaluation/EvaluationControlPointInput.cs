using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MidiShapeShifter.Mss.Parameters;

namespace MidiShapeShifter.Mss.Evaluation
{
    /// <summary>
    /// Stores the input information that is needed by an EvaluationControlPointJob
    /// </summary>
    public class EvaluationControlPointInput : EvaluationInput
    {
        public string EquationStr { get; set; }

        public void Init(List<MssParamInfo> variableParamInfoList, 
                         List<MssParamInfo> transformParamInfoList,
                        string equationStr)
        {
            this.VariableParamInfoList = variableParamInfoList;
            this.TransformParamInfoList = transformParamInfoList;

            this.EquationStr = equationStr;
        }

        public override EquationType equationType
        {
            get { return EquationType.Point; }
        }
    }
}

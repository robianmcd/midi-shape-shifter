using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.Evaluation
{
    /// <summary>
    /// Stores the input information that is needed by an EvaluationControlPointJob
    /// </summary>
    public class EvaluationControlPointInput : EvaluationInput
    {
        public string EquationStr { get; set; }

        public void Init(List<MssParameterInfo> variableParamInfoList, 
                         List<MssParameterInfo> transformParamInfoList,
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

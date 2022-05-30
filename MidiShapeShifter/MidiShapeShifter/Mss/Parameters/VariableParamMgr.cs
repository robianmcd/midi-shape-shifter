using MidiShapeShifter.CSharpUtil;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace MidiShapeShifter.Mss.Parameters
{

    [DataContract]
    public class VariableParamMgr
    {
        /// <summary>
        ///     Stores all of the information about the variable parameters.
        /// </summary>
        [DataMember]
        protected Dictionary<MssParameterID, MssParamInfo> variableParamDict;

        protected object memberLock = new object();

        public VariableParamMgr()
        {
            variableParamDict = new Dictionary<MssParameterID, MssParamInfo>();
        }


        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            this.memberLock = new object();
        }

        /// <summary>
        ///     Populates paramDict with default info about each parameter
        /// </summary>
        public void Init()
        {
            MssParamInfo defaultParameterInfo = new MssNumberParamInfo();
            defaultParameterInfo.Init("");

            MssParamInfo tempParameterInfo;

            //Populate paramDict with default values for each parameter
            tempParameterInfo = defaultParameterInfo.Clone();
            tempParameterInfo.Name = "A";
            this.variableParamDict.Add(MssParameterID.VariableA, tempParameterInfo);

            tempParameterInfo = defaultParameterInfo.Clone();
            tempParameterInfo.Name = "B";
            this.variableParamDict.Add(MssParameterID.VariableB, tempParameterInfo);

            tempParameterInfo = defaultParameterInfo.Clone();
            tempParameterInfo.Name = "C";
            this.variableParamDict.Add(MssParameterID.VariableC, tempParameterInfo);

            tempParameterInfo = defaultParameterInfo.Clone();
            tempParameterInfo.Name = "D";
            this.variableParamDict.Add(MssParameterID.VariableD, tempParameterInfo);

            tempParameterInfo = defaultParameterInfo.Clone();
            tempParameterInfo.Name = "E";
            this.variableParamDict.Add(MssParameterID.VariableE, tempParameterInfo);
        }


        public bool RunFuncOnParamInfo(MssParameterID variableParamId, ParamInfoAccessor variableParamAccessor)
        {
            lock (this.memberLock)
            {
                //TODO: impliment This.
                bool variableParamIdIsValid = this.variableParamDict.ContainsKey(variableParamId);
                if (variableParamIdIsValid)
                {
                    variableParamAccessor(this.variableParamDict[variableParamId]);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public IReturnStatus<MssParamInfo> GetCopyOfVariableParamInfo(MssParameterID variableParamId)
        {
            lock (this.memberLock)
            {
                bool variableParamIdIsValid = this.variableParamDict.ContainsKey(variableParamId);
                if (variableParamIdIsValid)
                {
                    return new ReturnStatus<MssParamInfo>(this.variableParamDict[variableParamId].Clone());
                }
                else
                {
                    return new ReturnStatus<MssParamInfo>();
                }
            }
        }

        public void SetVariableParamInfo(MssParamInfo varParamInfo, MssParameterID variableParamId)
        {
            lock (this.memberLock)
            {
                bool variableParamIdIsValid = this.variableParamDict.ContainsKey(variableParamId);
                if (variableParamIdIsValid)
                {
                    variableParamDict[variableParamId] = varParamInfo.Clone();
                }
                else
                {
                    //variableParamId does not refer to a valid variable parameter id.
                    Debug.Assert(false);
                    return;
                }
            }
        }

    }
}

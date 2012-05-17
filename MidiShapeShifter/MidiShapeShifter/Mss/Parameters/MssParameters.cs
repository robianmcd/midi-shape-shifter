using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace MidiShapeShifter.Mss.Parameters
{
    /// <summary>
    ///     There is a unique MssParameterID for each parameter which is used to distinguish between parameters.
    /// </summary>
    /// <remarks>
    /// The number associated with each ID should not be changed to ensure backward compatibility as
    /// it is sometimes saved as an integer.
    /// </remarks>
    public enum MssParameterID { VariableA = 0, VariableB = 1, VariableC = 2, VariableD = 3, 
                                 VariableE = 4, Preset1 = 5, Preset2 = 6, Preset3 = 7, 
                                 Preset4 = 8, Preset5 = 9 };

    public delegate void ParameterNameChangedEventHandler(MssParameterID paramId, string name);
    public delegate void ParameterValueChangedEventHandler(MssParameterID paramId, double value);
    public delegate void ParameterMinValueChangedEventHandler(MssParameterID paramId, double minValue);
    public delegate void ParameterMaxValueChangedEventHandler(MssParameterID paramId, double maxValue);

    /// <summary>
    ///     MssParameters stores information about all of the parameters in Midi Shape Shifter. An instance of 
    ///     MssParameterInfo is mapped to a unique id for each parameter. MssParameters provides events that other 
    ///     classes can subscribe to if they want to be notified of changes to parameter information.
    /// </summary>
    [DataContract]
    public class MssParameters : IMssParameterViewer
    {
        //These events will be thrown whenever a parameter is modified
        public event ParameterNameChangedEventHandler ParameterNameChanged;
        public event ParameterValueChangedEventHandler ParameterValueChanged;
        public event ParameterMinValueChangedEventHandler ParameterMinValueChanged;
        public event ParameterMaxValueChangedEventHandler ParameterMaxValueChanged;

        public const int NUM_VARIABLE_PARAMS = 5;
        public const int NUM_PRESET_PARAMS = 5;
        public static readonly List<MssParameterID> VARIABLE_PARAM_ID_LIST;
        public static readonly List<MssParameterID> PRESET_PARAM_ID_LIST;
        public static readonly List<MssParameterID> ALL_PARAMS_ID_LIST;

        /// <summary>
        ///     Stores all of the information about each parameter.
        /// </summary>
        [DataMember]
        protected Dictionary<MssParameterID, MssParamInfo> paramDict;

        static MssParameters()
        {
            VARIABLE_PARAM_ID_LIST = new List<MssParameterID>(NUM_VARIABLE_PARAMS);
            VARIABLE_PARAM_ID_LIST.Add(MssParameterID.VariableA);
            VARIABLE_PARAM_ID_LIST.Add(MssParameterID.VariableB);
            VARIABLE_PARAM_ID_LIST.Add(MssParameterID.VariableC);
            VARIABLE_PARAM_ID_LIST.Add(MssParameterID.VariableD);
            VARIABLE_PARAM_ID_LIST.Add(MssParameterID.VariableE);

            PRESET_PARAM_ID_LIST = new List<MssParameterID>(NUM_PRESET_PARAMS);
            PRESET_PARAM_ID_LIST.Add(MssParameterID.Preset1);
            PRESET_PARAM_ID_LIST.Add(MssParameterID.Preset2);
            PRESET_PARAM_ID_LIST.Add(MssParameterID.Preset3);
            PRESET_PARAM_ID_LIST.Add(MssParameterID.Preset4);
            PRESET_PARAM_ID_LIST.Add(MssParameterID.Preset5);

            ALL_PARAMS_ID_LIST = new List<MssParameterID>(NUM_VARIABLE_PARAMS + NUM_PRESET_PARAMS);
            ALL_PARAMS_ID_LIST.AddRange(VARIABLE_PARAM_ID_LIST);
            ALL_PARAMS_ID_LIST.AddRange(PRESET_PARAM_ID_LIST);
        }

        public MssParameters()
        {
            paramDict = new Dictionary<MssParameterID, MssParamInfo>();
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
            this.paramDict.Add(MssParameterID.VariableA, tempParameterInfo);

            tempParameterInfo = defaultParameterInfo.Clone();
            tempParameterInfo.Name = "B";
            this.paramDict.Add(MssParameterID.VariableB, tempParameterInfo);

            tempParameterInfo = defaultParameterInfo.Clone();
            tempParameterInfo.Name = "C";
            this.paramDict.Add(MssParameterID.VariableC, tempParameterInfo);

            tempParameterInfo = defaultParameterInfo.Clone();
            tempParameterInfo.Name = "D";
            this.paramDict.Add(MssParameterID.VariableD, tempParameterInfo);

            tempParameterInfo = defaultParameterInfo.Clone();
            tempParameterInfo.Name = "E";
            this.paramDict.Add(MssParameterID.VariableE, tempParameterInfo);


            List<MssParamInfo> defaultPresetParamList = CreateDefaultPresetParamInfoList();
            for (int i = 0; i < defaultPresetParamList.Count; i++ )
            {
                this.paramDict.Add(PRESET_PARAM_ID_LIST[i], defaultPresetParamList[i]);
            }
        }

        public static List<MssParamInfo> CreateDefaultPresetParamInfoList()
        {
            List<MssParamInfo> presetParamInfoList = new List<MssParamInfo>();

            MssParamInfo defaultParameterInfo = new MssNumberParamInfo();
            defaultParameterInfo.Init("");

            MssParamInfo tempParameterInfo;

            tempParameterInfo = defaultParameterInfo.Clone();
            tempParameterInfo.Name = "P1";
            presetParamInfoList.Add(tempParameterInfo);

            tempParameterInfo = defaultParameterInfo.Clone();
            tempParameterInfo.Name = "P2";
            presetParamInfoList.Add(tempParameterInfo);

            tempParameterInfo = defaultParameterInfo.Clone();
            tempParameterInfo.Name = "P3";
            presetParamInfoList.Add(tempParameterInfo);

            tempParameterInfo = defaultParameterInfo.Clone();
            tempParameterInfo.Name = "P4";
            presetParamInfoList.Add(tempParameterInfo);

            tempParameterInfo = defaultParameterInfo.Clone();
            tempParameterInfo.Name = "P5";
            presetParamInfoList.Add(tempParameterInfo);

            return presetParamInfoList;
        }

        public List<MssParamInfo> GetVariableParamInfoList()
        {
            List<MssParamInfo> varParamInfoList = new List<MssParamInfo>();
            foreach (MssParameterID paramId in VARIABLE_PARAM_ID_LIST)
            {
                varParamInfoList.Add(paramDict[paramId]);
            }

            return varParamInfoList;
        }

        public List<MssParamInfo> GetPresetParamInfoList()
        {
            List<MssParamInfo> presetParamInfoList = new List<MssParamInfo>();
            foreach (MssParameterID paramId in PRESET_PARAM_ID_LIST)
            {
                presetParamInfoList.Add(paramDict[paramId]);
            }

            return presetParamInfoList;
        }

        public MssParamInfo GetParameterInfo(MssParameterID parameterId)
        {
            if (this.paramDict.ContainsKey(parameterId) == false)
            {
                //paramDict should always contain every possible MssParameterID
                Debug.Assert(false);
                return null;
            }

            return this.paramDict[parameterId];
        }

        public double GetRelativeParamValue(MssParameterID parameterId)
        {
            MssParamInfo paramInfo = GetParameterInfo(parameterId);
            return (paramInfo.RawValue - paramInfo.MinValue) / (paramInfo.MaxValue - paramInfo.MinValue);
        }

        public void SetPresetParamInfoList(List<MssParamInfo> parameterInfoList)
        {
            Debug.Assert(parameterInfoList.Count == NUM_VARIABLE_PARAMS);
            for (int i = 0; i < parameterInfoList.Count; i++)
            {
                MssParameterID curId = PRESET_PARAM_ID_LIST[i];
                MssParamInfo curParamInfo = parameterInfoList[i];

                SetParamInfo(curId, curParamInfo);
            }
        }

        public void SetVariableParamInfo(MssParameterID paramId, MssParamInfo paramInfo)
        {
            int varIndex = VARIABLE_PARAM_ID_LIST.FindIndex(curId => curId == paramId);
            //ensure that paramId referes to a varialbe and not a transform preset.
            Debug.Assert(varIndex != -1);

            SetParamInfo(paramId, paramInfo);
        }

        protected void SetParamInfo(MssParameterID paramId, MssParamInfo paramInfo)
        {
            MssParamInfo prevParamInfo = this.paramDict[paramId];
            this.paramDict[paramId] = paramInfo;

            bool paramTypeChanged = (prevParamInfo.paramType != paramInfo.paramType);

            if (prevParamInfo.Name != paramInfo.Name && ParameterNameChanged != null)
            {
                ParameterNameChanged(paramId, paramInfo.Name);
            }

            //We need to check of the param type has changed here because the same raw value
            //can be displayed differently for different param types.
            if ((paramTypeChanged || prevParamInfo.RawValue != paramInfo.RawValue) && 
                ParameterValueChanged != null)
            {
                ParameterValueChanged(paramId, paramInfo.RawValue);
            }

            if (prevParamInfo.MaxValue != paramInfo.MaxValue && ParameterMaxValueChanged != null)
            {
                ParameterMaxValueChanged(paramId, paramInfo.MaxValue);
            }

            if (prevParamInfo.MinValue != paramInfo.MinValue && ParameterMinValueChanged != null)
            {
                ParameterMinValueChanged(paramId, paramInfo.MinValue);
            }
        }

        public void SetParameterName(MssParameterID parameterId, string name)
        {
            if (this.paramDict.ContainsKey(parameterId) == false) 
            {
                //paramDict should always contain every possible MssParameterID
                Debug.Assert(false);
                return;
            }

            if (paramDict[parameterId].Name != name)
            {
                paramDict[parameterId].Name = name;
                if (ParameterNameChanged != null)
                {
                    ParameterNameChanged(parameterId, name);
                }
            }
        }

        public void SetParameterRawValue(MssParameterID parameterId, double rawValue)
        {
            if (this.paramDict.ContainsKey(parameterId) == false)
            {
                //paramDict should always contain every possible MssParameterID
                Debug.Assert(false);
                return;
            }

            double previousValue = paramDict[parameterId].GetValue();
            paramDict[parameterId].RawValue = rawValue;
            double newValue = paramDict[parameterId].GetValue();

            if (previousValue != newValue)
            {
                if (ParameterValueChanged != null)
                {
                    ParameterValueChanged(parameterId, newValue);
                }
            }
        }

        public void SetParameterRelativeValue(MssParameterID parameterId, double relValue)
        {
            if (this.paramDict.ContainsKey(parameterId) == false)
            {
                //paramDict should always contain every possible MssParameterID
                Debug.Assert(false);
                return;
            }

            if (relValue < 0 || relValue > 1)
            {
                //relValue should be in the range [0, 1]
                Debug.Assert(false);
                return;
            }

            MssParamInfo paramInfo = GetParameterInfo(parameterId);
            double rawValue = paramInfo.MinValue + (relValue * (paramInfo.MaxValue - paramInfo.MinValue));
            SetParameterRawValue(parameterId, rawValue);
        }

        public void SetParameterMinValue(MssParameterID parameterId, int minValue)
        {
            
            if (this.paramDict.ContainsKey(parameterId) == false)
            {
                //paramDict should always contain every possible MssParameterID
                Debug.Assert(false);
                return;
            }

            if (paramDict[parameterId].MinValue != minValue)
            {
                paramDict[parameterId].MinValue = minValue;
                if (ParameterMinValueChanged != null)
                {
                    ParameterMinValueChanged(parameterId, minValue);
                }
            }
        }

        public void SetParameterMaxValue(MssParameterID parameterId, int maxValue)
        {
            
            if (this.paramDict.ContainsKey(parameterId) == false)
            {
                //paramDict should always contain every possible MssParameterID
                Debug.Assert(false);
                return;
            }

            if (paramDict[parameterId].MaxValue != maxValue)
            {
                paramDict[parameterId].MaxValue = maxValue;
                if (ParameterMaxValueChanged != null)
                {
                    ParameterMaxValueChanged(parameterId, maxValue);
                }
            }
        }
    
    }
}

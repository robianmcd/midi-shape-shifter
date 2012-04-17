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
    public enum MssParameterID { VariableA, VariableB, VariableC, VariableD, Preset1, Preset2, Preset3, Preset4 };

    public delegate void ParameterNameChangedEventHandler(MssParameterID paramId, string name);
    public delegate void ParameterValueChangedEventHandler(MssParameterID paramId, double value);
    public delegate void ParameterMinValueChangedEventHandler(MssParameterID paramId, int minValue);
    public delegate void ParameterMaxValueChangedEventHandler(MssParameterID paramId, int maxValue);

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

        public const int NUM_VARIABLE_PARAMS = 4;
        public const int NUM_PRESET_PARAMS = 4;
        public static readonly List<MssParameterID> VARIABLE_PARAM_ID_LIST;
        public static readonly List<MssParameterID> PRESET_PARAM_ID_LIST;

        /// <summary>
        ///     Stores all of the information about each parameter.
        /// </summary>
        [DataMember]
        protected Dictionary<MssParameterID, MssParamInfo> paramDict;

        //Used to prevent parameter change events from being recursively triggered.
        protected bool acceptParameterChanges;

        static MssParameters()
        {
            VARIABLE_PARAM_ID_LIST = new List<MssParameterID>();
            VARIABLE_PARAM_ID_LIST.Add(MssParameterID.VariableA);
            VARIABLE_PARAM_ID_LIST.Add(MssParameterID.VariableB);
            VARIABLE_PARAM_ID_LIST.Add(MssParameterID.VariableC);
            VARIABLE_PARAM_ID_LIST.Add(MssParameterID.VariableD);

            PRESET_PARAM_ID_LIST = new List<MssParameterID>();
            PRESET_PARAM_ID_LIST.Add(MssParameterID.Preset1);
            PRESET_PARAM_ID_LIST.Add(MssParameterID.Preset2);
            PRESET_PARAM_ID_LIST.Add(MssParameterID.Preset3);
            PRESET_PARAM_ID_LIST.Add(MssParameterID.Preset4);
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
            InitNonSerializableFields();

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


            List<MssParamInfo> defaultPresetParamList = CreateDefaultPresetParamInfoList();
            for (int i = 0; i < defaultPresetParamList.Count; i++ )
            {
                this.paramDict.Add(PRESET_PARAM_ID_LIST[i], defaultPresetParamList[i]);
            }
        }

        [OnDeserializing]
        private void BeforeDeserializing(StreamingContext sc)
        {
            InitNonSerializableFields();
        }

        protected void InitNonSerializableFields()
        {
            acceptParameterChanges = true;
        }

        public static List<MssParamInfo> CreateDefaultPresetParamInfoList()
        {
            List<MssParamInfo> presetParamInfoList = new List<MssParamInfo>();

            MssParamInfo defaultParameterInfo = new MssNumberParamInfo();
            defaultParameterInfo.Init("");

            MssParamInfo tempParameterInfo;

            tempParameterInfo = defaultParameterInfo.Clone();
            tempParameterInfo.Name = "Preset1";
            presetParamInfoList.Add(tempParameterInfo);

            tempParameterInfo = defaultParameterInfo.Clone();
            tempParameterInfo.Name = "Preset2";
            presetParamInfoList.Add(tempParameterInfo);

            tempParameterInfo = defaultParameterInfo.Clone();
            tempParameterInfo.Name = "Preset3";
            presetParamInfoList.Add(tempParameterInfo);

            tempParameterInfo = defaultParameterInfo.Clone();
            tempParameterInfo.Name = "Preset4";
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

        public void SetPresetParamInfoList(List<MssParamInfo> parameterInfoList)
        {
            Debug.Assert(parameterInfoList.Count == NUM_VARIABLE_PARAMS);
            for (int i = 0; i < parameterInfoList.Count; i++)
            {
                MssParameterID curId = PRESET_PARAM_ID_LIST[i];

                MssParamInfo curParamInfo = parameterInfoList[i];
                MssParamInfo prevParamInfo = this.paramDict[curId];

                this.paramDict[curId] = curParamInfo;

                if (prevParamInfo.Name != curParamInfo.Name && ParameterNameChanged != null)
                {
                    ParameterNameChanged(curId, curParamInfo.Name);
                }

                if (prevParamInfo.RawValue != curParamInfo.RawValue && ParameterValueChanged != null)
                {
                    ParameterValueChanged(curId, curParamInfo.RawValue);
                }

                if (prevParamInfo.MaxValue != curParamInfo.MaxValue && ParameterMaxValueChanged != null)
                {
                    ParameterMaxValueChanged(curId, curParamInfo.MaxValue);
                }

                if (prevParamInfo.MinValue != curParamInfo.MinValue && ParameterMinValueChanged != null)
                {
                    ParameterMinValueChanged(curId, curParamInfo.MinValue);
                }
            }
        }

        public string GetParameterName(MssParameterID parameterId)
        {
            if (this.paramDict.ContainsKey(parameterId) == false)
            {
                //paramDict should always contain every possible MssParameterID
                Debug.Assert(false);
                return "";
            }

            return this.paramDict[parameterId].Name;
        }

        public void SetParameterName(MssParameterID parameterId, string name)
        {
            if (this.paramDict.ContainsKey(parameterId) == false) 
            {
                //paramDict should always contain every possible MssParameterID
                Debug.Assert(false);
                return;
            }

            if (this.acceptParameterChanges && paramDict[parameterId].Name != name)
            {
                paramDict[parameterId].Name = name;
                if (ParameterNameChanged != null)
                {
                    this.acceptParameterChanges = false;
                    ParameterNameChanged(parameterId, name);
                    this.acceptParameterChanges = true;
                }
            }
        }



        public double GetParameterValue(MssParameterID parameterId)
        {
            if (this.paramDict.ContainsKey(parameterId) == false)
            {
                //paramDict should always contain every possible MssParameterID
                Debug.Assert(false);
                return 0;
            }

            return this.paramDict[parameterId].RawValue;
        }

        public void SetParameterValue(MssParameterID parameterId, double value)
        {
            if (this.paramDict.ContainsKey(parameterId) == false)
            {
                //paramDict should always contain every possible MssParameterID
                Debug.Assert(false);
                return;
            }

            if (this.acceptParameterChanges && paramDict[parameterId].RawValue != value)
            {
                paramDict[parameterId].RawValue = value;
                if (ParameterValueChanged != null)
                {
                    this.acceptParameterChanges = false;
                    ParameterValueChanged(parameterId, value);
                    this.acceptParameterChanges = true;
                }
            }
        }



        public int GetParameterMinValue(MssParameterID parameterId)
        {
            if (this.paramDict.ContainsKey(parameterId) == false)
            {
                //paramDict should always contain every possible MssParameterID
                Debug.Assert(false);
                return 0;
            }

            return this.paramDict[parameterId].MinValue;
        }

        public void SetParameterMinValue(MssParameterID parameterId, int minValue)
        {
            
            if (this.paramDict.ContainsKey(parameterId) == false)
            {
                //paramDict should always contain every possible MssParameterID
                Debug.Assert(false);
                return;
            }

            if (this.acceptParameterChanges && paramDict[parameterId].MinValue != minValue)
            {
                paramDict[parameterId].MinValue = minValue;
                if (ParameterMinValueChanged != null)
                {
                    this.acceptParameterChanges = false;
                    ParameterMinValueChanged(parameterId, minValue);
                    this.acceptParameterChanges = true;
                }
            }
        }



        public int GetParameterMaxValue(MssParameterID parameterId)
        {
            if (this.paramDict.ContainsKey(parameterId) == false)
            {
                //paramDict should always contain every possible MssParameterID
                Debug.Assert(false);
                return 0;
            }

            return this.paramDict[parameterId].MaxValue;
        }

        public void SetParameterMaxValue(MssParameterID parameterId, int maxValue)
        {
            
            if (this.paramDict.ContainsKey(parameterId) == false)
            {
                //paramDict should always contain every possible MssParameterID
                Debug.Assert(false);
                return;
            }

            if (this.acceptParameterChanges && paramDict[parameterId].MaxValue != maxValue)
            {
                paramDict[parameterId].MaxValue = maxValue;
                if (ParameterMaxValueChanged != null)
                {
                    this.acceptParameterChanges = false;
                    ParameterMaxValueChanged(parameterId, maxValue);
                    this.acceptParameterChanges = true;
                }
            }
        }
    
    }
}

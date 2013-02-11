using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.Serialization;

using MidiShapeShifter.Mss.Mapping;
using MidiShapeShifter.CSharpUtil;

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

    public delegate void ParamInfoAccessor(MssParamInfo paramInfo);

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

        protected SendRateLimittedEvent<Tuple<MssParameterID, double>> ParameterValueChangedLimitter;

        public const int NUM_VARIABLE_PARAMS = 5;
        public const int NUM_PRESET_PARAMS = 5;
        public static readonly List<MssParameterID> VARIABLE_PARAM_ID_LIST;
        public static readonly List<MssParameterID> PRESET_PARAM_ID_LIST;
        public static readonly List<MssParameterID> ALL_PARAMS_ID_LIST;

        protected ActiveMappingInfo activeMappingInfo;
        protected VariableParamMgr variableParamMgr;

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
            ParameterValueChangedLimitter = new SendRateLimittedEvent<Tuple<MssParameterID, double>>(50,
                eventParams => {
                    if (ParameterValueChanged != null) {
                        ParameterValueChanged(eventParams.Item1, eventParams.Item2);
                    }
                });
        }

        /// <summary>
        ///     Populates paramDict with default info about each parameter
        /// </summary>
        public void Init(ActiveMappingInfo activeMappingInfo, VariableParamMgr variableParamMgr)
        {
            this.activeMappingInfo = activeMappingInfo;
            this.variableParamMgr = variableParamMgr;
        }

        public static string GetDefaultPresetName(MssParameterID presetId)
        {
            switch (presetId) { 
                case MssParameterID.Preset1:
                    return "P1";
                case MssParameterID.Preset2:
                    return "P2";
                case MssParameterID.Preset3:
                    return "P3";
                case MssParameterID.Preset4:
                    return "P4";
                case MssParameterID.Preset5:
                    return "P5";
                default:
                    Debug.Assert(false);
                    return "";
            }
        }

        public List<MssParamInfo> GetVariableParamInfoList()
        {
            List<MssParamInfo> varParamInfoList = new List<MssParamInfo>();
            foreach (MssParameterID paramId in VARIABLE_PARAM_ID_LIST)
            {
                varParamInfoList.Add(GetParameterInfoCopy(paramId));
            }

            return varParamInfoList;
        }

        public List<MssParamInfo> GetPresetParamInfoList()
        {
            List<MssParamInfo> presetParamInfoList = new List<MssParamInfo>();
            foreach (MssParameterID paramId in PRESET_PARAM_ID_LIST)
            {
                presetParamInfoList.Add(GetParameterInfoCopy(paramId));
            }

            return presetParamInfoList;
        }

        public MssParamInfo GetParameterInfoCopy(MssParameterID parameterId)
        {
            MssParamInfo paramInfoClone = null;
            RunFuncOnParamInfo(parameterId, paramInfo => paramInfoClone = paramInfo.Clone());

            return paramInfoClone;
        }

        public double GetRelativeParamValue(MssParameterID paramId)
        {
            MssParamInfo paramInfo = GetParameterInfoCopy(paramId);
            return (paramInfo.RawValue - paramInfo.MinValue) / (paramInfo.MaxValue - paramInfo.MinValue);
        }

        public void SetParamInfo(MssParameterID paramId, MssParamInfo paramInfo)
        {
            MssParamInfo prevParamInfo = GetParameterInfoCopy(paramId);

            if (VARIABLE_PARAM_ID_LIST.Contains(paramId))
            {
                this.variableParamMgr.SetVariableParamInfo(paramInfo, paramId);
            }
            else if (PRESET_PARAM_ID_LIST.Contains(paramId))
            {
                this.activeMappingInfo.GetActiveGraphableEntryManager().RunFuncOnMappingEntry(this.activeMappingInfo.ActiveGraphableEntryId,
                    (mappingEntry) => mappingEntry.CurveShapeInfo.ParamInfoList[PRESET_PARAM_ID_LIST.IndexOf(paramId)] = paramInfo.Clone());
            }
            else
            {
                //every possible MssParameterID should either be contained in VARIABLE_PARAM_ID_LIST or PRESET_PARAM_ID_LIST.
                Debug.Assert(false);
                return;
            }

            bool paramTypeChanged = (prevParamInfo.paramType != paramInfo.paramType);

            if (prevParamInfo.Name != paramInfo.Name && ParameterNameChanged != null)
            {
                ParameterNameChanged(paramId, paramInfo.Name);
            }

            //We need to check of the param type has changed here because the same raw value
            //can be displayed differently for different param types.
            if (paramTypeChanged || prevParamInfo.RawValue != paramInfo.RawValue)
            {
                this.ParameterValueChangedLimitter.SendEvent(Tuple.Create(paramId, paramInfo.RawValue));
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

        protected bool RunFuncOnParamInfo(MssParameterID paramId, ParamInfoAccessor paramAccessor)
        {
            if (VARIABLE_PARAM_ID_LIST.Contains(paramId))
            {
                return this.variableParamMgr.RunFuncOnParamInfo(paramId, paramInfo => paramAccessor(paramInfo));
            }
            else if (PRESET_PARAM_ID_LIST.Contains(paramId))
            {
                return this.activeMappingInfo.GetActiveGraphableEntryManager().RunFuncOnMappingEntry(this.activeMappingInfo.ActiveGraphableEntryId,
                    (mappingEntry) => {
                        int paramIndex = PRESET_PARAM_ID_LIST.IndexOf(paramId);
                        MssParamInfo paramInfo = mappingEntry.CurveShapeInfo.ParamInfoList[paramIndex];
                        paramAccessor(paramInfo);
                    });
            }
            else
            {
                //every possible MssParameterID should either be contained in VARIABLE_PARAM_ID_LIST or PRESET_PARAM_ID_LIST.
                Debug.Assert(false);
                return false;
            }
        }

        public bool GetActiveMappingExists() {
            return this.activeMappingInfo.GetActiveMappingExists();
        }

        public void SetParameterName(MssParameterID paramId, string name)
        {
            if (ALL_PARAMS_ID_LIST.Contains(paramId) == false) 
            {
                //paramDict should always contain every possible MssParameterID
                Debug.Assert(false);
                return;
            }

            bool triggerNameChangedEvent = false;

            RunFuncOnParamInfo(paramId, 
                paramInfo => 
                {
                    if (paramInfo.Name != name)
                    {
                        paramInfo.Name = name;
                        triggerNameChangedEvent = true;
                    }
                });

            if (triggerNameChangedEvent && ParameterNameChanged != null) {
                ParameterNameChanged(paramId, name);
            }
        }

        public void SetParameterRawValue(MssParameterID paramId, double rawValue)
        {
            if (ALL_PARAMS_ID_LIST.Contains(paramId) == false)
            {
                //paramDict should always contain every possible MssParameterID
                Debug.Assert(false);
                return;
            }

            bool triggerValueChangedEvent = false;
            double newValue = -1;

            RunFuncOnParamInfo(paramId, 
                paramInfo => 
                {
                    double previousValue = paramInfo.GetValue();
                    paramInfo.RawValue = rawValue;
                    newValue = paramInfo.GetValue();

                    if (previousValue != newValue)
                    {
                        triggerValueChangedEvent = true;
                    }
                });

            if (triggerValueChangedEvent)
            {
                this.ParameterValueChangedLimitter.SendEvent(Tuple.Create(paramId, newValue));
            }


        }

        public void SetParameterRelativeValue(MssParameterID paramId, double relValue)
        {
            if (ALL_PARAMS_ID_LIST.Contains(paramId) == false)
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

            MssParamInfo paramInfo = GetParameterInfoCopy(paramId);
            //ParamInfo could be null when there is no active mapping and a preset param is 
            //modified from the host.
            if (paramInfo != null) {
                double rawValue = paramInfo.MinValue + (relValue * (paramInfo.MaxValue - paramInfo.MinValue));
                SetParameterRawValue(paramId, rawValue);
            }
        }

        public void SetParameterMinValue(MssParameterID paramId, int minValue)
        {
            if (ALL_PARAMS_ID_LIST.Contains(paramId) == false)
            {
                //paramDict should always contain every possible MssParameterID
                Debug.Assert(false);
                return;
            }

            bool triggerMinValueChangedEvent = false;

             RunFuncOnParamInfo(paramId, 
                paramInfo => 
                {
                    if (paramInfo.MinValue != minValue)
                    {
                        paramInfo.MinValue = minValue;
                        triggerMinValueChangedEvent = true;
                    }
            });

             if (triggerMinValueChangedEvent && ParameterMinValueChanged != null)
             {
                 ParameterMinValueChanged(paramId, minValue);
             }


        }

        public void SetParameterMaxValue(MssParameterID paramId, int maxValue)
        {

            if (ALL_PARAMS_ID_LIST.Contains(paramId) == false)
            {
                //paramDict should always contain every possible MssParameterID
                Debug.Assert(false);
                return;
            }

            bool triggerMaxValueChangedEvent = false;

            RunFuncOnParamInfo(paramId,
               paramInfo =>
               {
                   if (paramInfo.MaxValue != maxValue)
                   {
                       paramInfo.MaxValue = maxValue;
                       triggerMaxValueChangedEvent = true;
                   }
               });

            if (triggerMaxValueChangedEvent && ParameterMaxValueChanged != null)
            {
                ParameterMaxValueChanged(paramId, maxValue);
            }
        }
    
    }
}

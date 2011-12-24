using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace MidiShapeShifter.Mss
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
    [Serializable]
    public class MssParameters
    {
        //These events will be thrown whenever a parameter is modified
        [field: NonSerialized]
        public event ParameterNameChangedEventHandler ParameterNameChanged;
        [field: NonSerialized]
        public event ParameterValueChangedEventHandler ParameterValueChanged;
        [field: NonSerialized]
        public event ParameterMinValueChangedEventHandler ParameterMinValueChanged;
        [field: NonSerialized]
        public event ParameterMaxValueChangedEventHandler ParameterMaxValueChanged;

        /// <summary>
        ///     Stores all of the information about each parameter.
        /// </summary>
        protected Dictionary<MssParameterID, MssParameterInfo> paramDict;

        //Used to prevent parameter change events from being recursively triggered.
        protected bool acceptParameterChanges = true;

        public MssParameters()
        {
            paramDict = new Dictionary<MssParameterID, MssParameterInfo>();
        }

        /// <summary>
        ///     Populates paramDict with default info about each parameter
        /// </summary>
        public void Init()
        {
            const double DEFAULT_PRAM_VALUE = 0;
            const int DEFAULT_PRAM_MIN_VALUE = 0;
            const int DEFAULT_PRAM_MAX_VALUE = 1;

            MssParameterInfo defaultParameterInfo = new MssParameterInfo();
            defaultParameterInfo.Value = DEFAULT_PRAM_VALUE;
            defaultParameterInfo.MinValue = DEFAULT_PRAM_MIN_VALUE;
            defaultParameterInfo.MaxValue = DEFAULT_PRAM_MAX_VALUE;

            MssParameterInfo tempParameterInfo;

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
            tempParameterInfo.Name = "Preset1";
            this.paramDict.Add(MssParameterID.Preset1, tempParameterInfo);

            tempParameterInfo = defaultParameterInfo.Clone();
            tempParameterInfo.Name = "Preset2";
            this.paramDict.Add(MssParameterID.Preset2, tempParameterInfo);

            tempParameterInfo = defaultParameterInfo.Clone();
            tempParameterInfo.Name = "Preset3";
            this.paramDict.Add(MssParameterID.Preset3, tempParameterInfo);

            tempParameterInfo = defaultParameterInfo.Clone();
            tempParameterInfo.Name = "Preset4";
            this.paramDict.Add(MssParameterID.Preset4, tempParameterInfo);
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

            return this.paramDict[parameterId].Value;
        }

        public void SetParameterValue(MssParameterID parameterId, double value)
        {
            if (this.paramDict.ContainsKey(parameterId) == false)
            {
                //paramDict should always contain every possible MssParameterID
                Debug.Assert(false);
                return;
            }

            if (this.acceptParameterChanges && paramDict[parameterId].Value != value)
            {
                paramDict[parameterId].Value = value;
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

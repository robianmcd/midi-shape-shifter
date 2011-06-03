using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace MidiShapeShifter.Mss
{
    public enum MssParameterID { VariableA, VariableB, VariableC, VariableD, Preset1, Preset2, Preset3, Preset4 };

    public delegate void ParameterNameChangedEventHandler(MssParameterID paramId, string name);
    public delegate void ParameterValueChangedEventHandler(MssParameterID paramId, double value);
    public delegate void ParameterMinValueChangedEventHandler(MssParameterID paramId, int minValue);
    public delegate void ParameterMaxValueChangedEventHandler(MssParameterID paramId, int maxValue);

    public class MssParameters
    {
        public event ParameterNameChangedEventHandler ParameterNameChanged;
        public event ParameterValueChangedEventHandler ParameterValueChanged;
        public event ParameterMinValueChangedEventHandler ParameterMinValueChanged;
        public event ParameterMaxValueChangedEventHandler ParameterMaxValueChanged;

        protected Dictionary<MssParameterID, MssParameterInfo> paramDict = new Dictionary<MssParameterID, MssParameterInfo>();

        public MssParameters()
        {
            const double DEFAULT_PRAM_VALUE = 0;
            const int DEFAULT_PRAM_MIN_VALUE = 0;
            const int DEFAULT_PRAM_MAX_VALUE = 1;

            MssParameterInfo defaultParameterInfo = new MssParameterInfo();
            defaultParameterInfo.Value = DEFAULT_PRAM_VALUE;
            defaultParameterInfo.MinValue = DEFAULT_PRAM_MIN_VALUE;
            defaultParameterInfo.MaxValue = DEFAULT_PRAM_MAX_VALUE;

            MssParameterInfo tempParameterInfo;

            //Populate paramDict
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

            paramDict[parameterId].Name = name;
            ParameterNameChanged(parameterId, name);
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

            paramDict[parameterId].Value = value;
            ParameterValueChanged(parameterId, value);
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

            paramDict[parameterId].MinValue = minValue;
            ParameterMinValueChanged(parameterId, minValue);
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

            paramDict[parameterId].MaxValue = maxValue;
            ParameterMaxValueChanged(parameterId, maxValue);
        }
    }
}

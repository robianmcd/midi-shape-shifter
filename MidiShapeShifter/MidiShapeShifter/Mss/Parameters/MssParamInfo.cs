using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MidiShapeShifter.Mss.Parameters
{
    public enum MssParamType { Number, Integer, Waveform }
    public enum ValueInputType { Number, Integer, Selection }

    /// <summary>
    ///     A representation of a parameter in Midi Shape Shifter.
    /// </summary>
    [DataContract]
    public abstract class MssParamInfo : ICloneable
    {
        public static readonly IList<string> MssParamTypeNameList;

        public abstract MssParamType paramType { get; }
        public abstract bool allowUserToEditMaxMin { get; }
        public abstract ValueInputType methodOfValueInput { get; }
        //It is only nessesary to impliment this if methodOfValueInput is set to Selection.
        public virtual IList<string> ValueNameList { get { return null; } }

        [DataMember]
        public string Name;
        [DataMember]
        public double RawValue;
        [DataMember]
        public double MaxValue;
        [DataMember]
        public double MinValue;

        static MssParamInfo()
        {
            int numParamTypes = Enum.GetValues(typeof(MssParamType)).Length;
            List<string> _mssParamTypeList = new List<string>();

            _mssParamTypeList.Insert((int)MssParamType.Number, "Number");
            _mssParamTypeList.Insert((int)MssParamType.Integer, "Integer");
            _mssParamTypeList.Insert((int)MssParamType.Waveform, "Waveform");

            MssParamTypeNameList = _mssParamTypeList.AsReadOnly();
        }

        public void Init(string name)
        {
            this.Name = name;

            SetMembersWithDefaultValues();
        }

        protected abstract void SetMembersWithDefaultValues();

        public abstract double GetValue();
        public abstract string GetValueAsString();

        object ICloneable.Clone()
        {
            return Clone();
        }

        public MssParamInfo Clone()
        {
            //This will return a deep copy as long as the deriving types do not create any reference type fields.
            return (MssParamInfo)this.MemberwiseClone();
        }

        override public string ToString()
        {
            return string.Format("MssParamInfo - name: {0}, raw: {1}, max: {2}, min: {3}",
                this.Name, this.RawValue, this.MaxValue, this.MinValue);
        }

    }
}

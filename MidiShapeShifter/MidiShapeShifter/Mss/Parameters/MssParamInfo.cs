using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MidiShapeShifter.Mss.Parameters
{
    public enum MssParamType { Number, Integer, Waveform }

    /// <summary>
    ///     A representation of a parameter in Midi Shape Shifter.
    /// </summary>
    [DataContract]
    public abstract class MssParamInfo
    {
        public abstract MssParamType paramType { get; }
        public abstract bool allowUserToEditMaxMin { get; }
        public abstract ValueInputType methodOfValueInput { get; }

        [DataMember]
        public string Name;
        [DataMember]
        public double RawValue;
        [DataMember]
        public int MaxValue;
        [DataMember]
        public int MinValue;

        public MssParamInfo()
        { 
            
        }

        public void Init(string name)
        {
            this.Name = name;

            SetMembersWithDefaultValues();
        }

        protected abstract void SetMembersWithDefaultValues();

        public abstract double GetValue();
        public abstract string GetValueAsString();

        public MssParamInfo Clone()
        {
            return (MssParamInfo)this.MemberwiseClone();
        }
    }
}

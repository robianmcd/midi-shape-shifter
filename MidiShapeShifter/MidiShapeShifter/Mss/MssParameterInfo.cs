using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss
{
    /// <summary>
    ///     A representation of a parameter in Midi Shape Shifter.
    /// </summary>
    [Serializable]
    public class MssParameterInfo
    {
        public string Name;
        public double Value;
        public int MaxValue;
        public int MinValue;

        public MssParameterInfo()
        { 
            
        }

        public MssParameterInfo(string name, double value, int maxValue, int minValue)
        {
            this.Name = name;
            this.Value = value;
            this.MaxValue = maxValue;
            this.MinValue = minValue;
        }

        public MssParameterInfo Clone()
        {
            return (MssParameterInfo)this.MemberwiseClone();
        }
    }
}

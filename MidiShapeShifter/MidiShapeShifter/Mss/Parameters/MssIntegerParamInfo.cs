using System;
using System.Runtime.Serialization;

namespace MidiShapeShifter.Mss.Parameters
{
    [DataContract]
    internal class MssIntegerParamInfo : MssParamInfo
    {
        protected const double DEFAULT_INTEGER_PRAM_VALUE = 0;
        protected const int DEFAULT_INTEGER_PRAM_MIN_VALUE = 0;
        protected const int DEFAULT_INTEGER_PRAM_MAX_VALUE = 1;

        public override MssParamType paramType => MssParamType.Integer;

        public override bool allowUserToEditMaxMin => true;

        public override ValueInputType methodOfValueInput => ValueInputType.Integer;

        protected override void SetMembersWithDefaultValues()
        {
            this.MinValue = DEFAULT_INTEGER_PRAM_MIN_VALUE;
            this.MaxValue = DEFAULT_INTEGER_PRAM_MAX_VALUE;
            this.RawValue = DEFAULT_INTEGER_PRAM_VALUE;
        }

        public override double GetValue()
        {
            return Math.Round(this.RawValue);
        }

        public override string GetValueAsString()
        {
            return GetValue().ToString();
        }
    }
}

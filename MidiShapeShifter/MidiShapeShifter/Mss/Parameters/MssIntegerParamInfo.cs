using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MidiShapeShifter.Mss.Parameters
{
    [DataContract]
    class MssIntegerParamInfo : MssParamInfo
    {
        protected const double DEFAULT_INTEGER_PRAM_VALUE = 0;
        protected const int DEFAULT_INTEGER_PRAM_MIN_VALUE = 0;
        protected const int DEFAULT_INTEGER_PRAM_MAX_VALUE = 1;

        public override MssParamType paramType
        {
            get { return MssParamType.Integer; }
        }

        public override bool allowUserToEditMaxMin
        {
            get { return true; }
        }

        public override ValueInputType methodOfValueInput
        {
            get { return ValueInputType.Integer; }
        }

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

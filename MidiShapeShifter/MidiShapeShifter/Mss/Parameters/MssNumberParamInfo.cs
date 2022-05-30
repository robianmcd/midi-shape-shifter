using System.Runtime.Serialization;

namespace MidiShapeShifter.Mss.Parameters
{
    [DataContract]
    public class MssNumberParamInfo : MssParamInfo
    {
        protected const double DEFAULT_NUMBER_PRAM_VALUE = 0;
        protected const int DEFAULT_NUMBER_PRAM_MIN_VALUE = 0;
        protected const int DEFAULT_NUMBER_PRAM_MAX_VALUE = 1;

        public override MssParamType paramType => MssParamType.Number;

        public override bool allowUserToEditMaxMin => true;

        public override ValueInputType methodOfValueInput => ValueInputType.Number;

        protected override void SetMembersWithDefaultValues()
        {
            this.MinValue = DEFAULT_NUMBER_PRAM_MIN_VALUE;
            this.MaxValue = DEFAULT_NUMBER_PRAM_MAX_VALUE;
            this.RawValue = DEFAULT_NUMBER_PRAM_VALUE;
        }

        public override double GetValue()
        {
            return this.RawValue;
        }

        public override string GetValueAsString()
        {

            return System.Math.Round(this.RawValue, 2).ToString();
        }
    }
}

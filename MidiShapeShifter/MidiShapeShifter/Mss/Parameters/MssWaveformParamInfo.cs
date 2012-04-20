using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace MidiShapeShifter.Mss.Parameters
{
    public enum WaveformShap { Sin, Square, Triangle, Ramp, Saw }

    [DataContract]
    class MssWaveformParamInfo : MssParamInfo
    {
        private static readonly IList<string> _valueNameList;

        static MssWaveformParamInfo()
        {
            int numWaveformShapes = Enum.GetValues(typeof(WaveformShap)).Length;
            List<String> valueNames = new List<string>(numWaveformShapes);
            valueNames.Insert((int)WaveformShap.Sin, "Sin");
            valueNames.Insert((int)WaveformShap.Square, "Square");
            valueNames.Insert((int)WaveformShap.Triangle, "Triangle");
            valueNames.Insert((int)WaveformShap.Ramp, "Ramp");
            valueNames.Insert((int)WaveformShap.Saw, "Saw");

            //If this asserts false then WaveformShap must have been changed. Make sure valueNames
            //contains an entry for each WaveformShap and then update this assert statment.
            Debug.Assert((int)WaveformShap.Saw == numWaveformShapes-1);

            _valueNameList = valueNames.AsReadOnly();
        }


        public override MssParamType paramType
        {
            get { return MssParamType.Waveform; }
        }

        public override bool allowUserToEditMaxMin
        {
            get { return false; }
        }

        public override ValueInputType methodOfValueInput
        {
            get { return ValueInputType.Selection; }
        }

        public override IList<string> ValueNameList { get { return _valueNameList; } }

        protected override void SetMembersWithDefaultValues()
        {
            this.MinValue = 0;
            this.MaxValue = Enum.GetValues(typeof(WaveformShap)).Length-1;
            this.RawValue = (int)WaveformShap.Sin;
        }

        public override double GetValue()
        {
            return Math.Round(this.RawValue);
        }

        public override string GetValueAsString()
        {

            return this.ValueNameList[(int)GetValue()];
        }
    }
}

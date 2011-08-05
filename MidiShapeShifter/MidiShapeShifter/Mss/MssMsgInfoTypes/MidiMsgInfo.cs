using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MidiShapeShifter.Midi;

namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    public abstract class MidiMsgInfo : MssMsgInfo
    {
        public override double MaxData1Value
        {
            get { return MidiUtil.MAX_MIDI_CHAN_VAL; }
        }

        public override double MinData1Value
        {
            get { return MidiUtil.MIN_MIDI_CHAN_VAL; }
        }

        public override double MaxData2Value
        {
            get { return MidiUtil.MAX_MIDI_PARAM_VAL; }
        }

        public override double MinData2Value
        {
            get { return MidiUtil.MIN_MIDI_PARAM_VAL; }
        }

        public override double MaxData3Value
        {
            get { return MidiUtil.MAX_MIDI_PARAM_VAL; }
        }

        public override double MinData3Value
        {
            get { return MidiUtil.MIN_MIDI_PARAM_VAL; }
        }

        public override string ConvertData1ToString(double Data1)
        {
            throw new NotImplementedException();
        }

        public override string ConvertData2ToString(double Data2)
        {
            throw new NotImplementedException();
        }

        public override string ConvertData3ToString(double Data3)
        {
            throw new NotImplementedException();
        }
    }
}

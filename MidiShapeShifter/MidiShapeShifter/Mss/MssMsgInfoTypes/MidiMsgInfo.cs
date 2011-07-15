using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MidiShapeShifter.Midi;

namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    public abstract class MidiMsgInfo : MssMsgInfo
    {
        public override int MaxData1Value
        {
            get { return MidiUtil.MAX_MIDI_CHAN_VAL; }
        }

        public override int MinData1Value
        {
            get { return MidiUtil.MIN_MIDI_CHAN_VAL; }
        }

        public override int MaxData2Value
        {
            get { return MidiUtil.MAX_MIDI_PARAM_VAL; }
        }

        public override int MinData2Value
        {
            get { return MidiUtil.MIN_MIDI_PARAM_VAL; }
        }

        public override int MaxData3Value
        {
            get { return MidiUtil.MAX_MIDI_PARAM_VAL; }
        }

        public override int MinData3Value
        {
            get { return MidiUtil.MIN_MIDI_PARAM_VAL; }
        }

        public override string ConvertData1ToString(int Data1)
        {
            throw new NotImplementedException();
        }

        public override string ConvertData2ToString(int Data2)
        {
            throw new NotImplementedException();
        }

        public override string ConvertData3ToString(int Data3)
        {
            throw new NotImplementedException();
        }
    }
}

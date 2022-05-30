
using MidiShapeShifter.Midi;

namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    public abstract class StaticMidiMsgInfo : StaticMssMsgInfo
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

        public override string Data1Name
        {
            get { return DATA1_NAME_CHANNEL; }
        }
    }
}

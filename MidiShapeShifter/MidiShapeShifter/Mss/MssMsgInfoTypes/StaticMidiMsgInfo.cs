
using MidiShapeShifter.Midi;

namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    public abstract class StaticMidiMsgInfo : StaticMssMsgInfo
    {
        public override double MaxData1Value => MidiUtil.MAX_MIDI_CHAN_VAL;

        public override double MinData1Value => MidiUtil.MIN_MIDI_CHAN_VAL;

        public override double MaxData2Value => MidiUtil.MAX_MIDI_PARAM_VAL;

        public override double MinData2Value => MidiUtil.MIN_MIDI_PARAM_VAL;

        public override double MaxData3Value => MidiUtil.MAX_MIDI_PARAM_VAL;

        public override double MinData3Value => MidiUtil.MIN_MIDI_PARAM_VAL;

        public override string Data1Name => DATA1_NAME_CHANNEL;
    }
}

using MidiShapeShifter.Midi;

namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    public class StaticPitchBendMsgInfo : StaticMidiMsgInfo
    {
        public override MssMsgType MsgType => MssMsgType.PitchBend;

        public override double MinData2Value => MssMsgUtil.UNUSED_MSS_MSG_DATA;

        public override double MaxData2Value => MssMsgUtil.UNUSED_MSS_MSG_DATA;

        public override double MaxData3Value => MidiUtil.MAX_LARGE_MIDI_PARAM_VAL;

        public override string Data2Name => StaticMssMsgInfo.DATA_NAME_UNUSED;

        public override string Data3Name => "Pitch Bend";
    }
}

using MidiShapeShifter.Midi;

namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    public class StaticPitchBendMsgInfo : StaticMidiMsgInfo
    {
        public override MssMsgType MsgType
        {
            get { return MssMsgType.PitchBend; }
        }

        public override double MinData2Value
        {
            get { return MssMsgUtil.UNUSED_MSS_MSG_DATA; }
        }

        public override double MaxData2Value
        {
            get { return MssMsgUtil.UNUSED_MSS_MSG_DATA; }
        }

        public override double MaxData3Value
        {
            get { return MidiUtil.MAX_LARGE_MIDI_PARAM_VAL; }
        }

        public override string Data2Name
        {
            get { return StaticMssMsgInfo.DATA_NAME_UNUSED; }
        }

        public override string Data3Name
        {
            get { return "Pitch Bend"; }
        }
    }
}

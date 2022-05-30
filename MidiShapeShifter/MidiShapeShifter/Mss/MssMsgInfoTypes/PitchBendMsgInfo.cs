namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    public class PitchBendMsgInfo : MidiMsgInfo
    {
        public override MssMsgType MsgType
        {
            get { return MssMsgType.PitchBend; }
        }

        public override string ConvertData2ToString(double Data2)
        {
            return MssMsgUtil.UNUSED_MSS_MSG_STRING;
        }
    }
}

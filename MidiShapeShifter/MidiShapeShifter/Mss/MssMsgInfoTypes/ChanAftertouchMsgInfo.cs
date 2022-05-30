namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    internal class ChanAftertouchMsgInfo : MidiMsgInfo
    {
        public override MssMsgType MsgType => MssMsgType.ChanAftertouch;

        public override string ConvertData2ToString(double Data2)
        {
            return MssMsgUtil.UNUSED_MSS_MSG_STRING;
        }
    }
}

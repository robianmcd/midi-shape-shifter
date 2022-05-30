namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    class ChanAftertouchMsgInfo : MidiMsgInfo
    {
        public override MssMsgType MsgType
        {
            get { return MssMsgType.ChanAftertouch; }
        }

        public override string ConvertData2ToString(double Data2)
        {
            return MssMsgUtil.UNUSED_MSS_MSG_STRING;
        }
    }
}

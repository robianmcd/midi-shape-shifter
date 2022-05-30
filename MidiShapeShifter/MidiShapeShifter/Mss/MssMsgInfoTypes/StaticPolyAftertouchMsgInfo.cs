namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    public class StaticPolyAftertouchMsgInfo : StaticMidiMsgInfo
    {
        public override MssMsgType MsgType
        {
            get { return MssMsgType.PolyAftertouch; }
        }

        public override string Data2Name
        {
            get { return StaticMssMsgInfo.DATA2_NAME_NOTE; }
        }

        public override string Data3Name
        {
            get { return StaticMssMsgInfo.DATA3_NAME_PRESSURE; }
        }
    }
}

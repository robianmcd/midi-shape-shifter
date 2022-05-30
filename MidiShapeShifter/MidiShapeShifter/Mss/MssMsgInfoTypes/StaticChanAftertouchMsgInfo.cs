namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    public class StaticChanAftertouchMsgInfo : StaticMidiMsgInfo
    {
        public override MssMsgType MsgType
        {
            get { return MssMsgType.ChanAftertouch; }
        }

        public override string Data2Name
        {
            get { return DATA_NAME_UNUSED; }
        }

        public override string Data3Name
        {
            get { return DATA3_NAME_PRESSURE; }
        }
    }
}

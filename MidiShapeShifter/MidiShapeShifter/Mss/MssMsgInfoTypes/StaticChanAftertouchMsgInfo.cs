namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    public class StaticChanAftertouchMsgInfo : StaticMidiMsgInfo
    {
        public override MssMsgType MsgType => MssMsgType.ChanAftertouch;

        public override string Data2Name => DATA_NAME_UNUSED;

        public override string Data3Name => DATA3_NAME_PRESSURE;
    }
}

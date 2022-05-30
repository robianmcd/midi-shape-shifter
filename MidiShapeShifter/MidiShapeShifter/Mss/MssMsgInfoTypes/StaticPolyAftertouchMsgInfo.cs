namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    public class StaticPolyAftertouchMsgInfo : StaticMidiMsgInfo
    {
        public override MssMsgType MsgType => MssMsgType.PolyAftertouch;

        public override string Data2Name => StaticMssMsgInfo.DATA2_NAME_NOTE;

        public override string Data3Name => StaticMssMsgInfo.DATA3_NAME_PRESSURE;
    }
}

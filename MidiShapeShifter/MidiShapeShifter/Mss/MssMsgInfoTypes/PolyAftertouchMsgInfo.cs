namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    public class PolyAftertouchMsgInfo : MidiMsgInfo
    {
        public override MssMsgType MsgType
        {
            get { return MssMsgType.PolyAftertouch; }
        }
    }
}

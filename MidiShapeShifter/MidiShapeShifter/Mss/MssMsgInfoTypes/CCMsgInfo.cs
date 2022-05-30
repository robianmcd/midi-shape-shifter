namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    public class CCMsgInfo : MidiMsgInfo
    {
        public override MssMsgType MsgType
        {
            get { return MssMsgType.CC; }
        }
    }
}

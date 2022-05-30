namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    class NoteMsgInfo : MidiMsgInfo
    {
        public override MssMsgType MsgType
        {
            get { return MssMsgType.Note; }
        }
    }
}

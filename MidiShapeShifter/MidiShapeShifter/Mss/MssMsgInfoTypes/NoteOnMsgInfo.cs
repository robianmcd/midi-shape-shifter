namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    class NoteOnMsgInfo : MidiMsgInfo
    {
        public override MssMsgType MsgType
        {
            get { return MssMsgType.NoteOn; }
        }
    }
}

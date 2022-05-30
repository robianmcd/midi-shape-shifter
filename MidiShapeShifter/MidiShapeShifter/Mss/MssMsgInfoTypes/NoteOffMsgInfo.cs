namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    class NoteOffMsgInfo : MidiMsgInfo
    {
        public override MssMsgType MsgType
        {
            get { return MssMsgType.NoteOff; }
        }
    }
}

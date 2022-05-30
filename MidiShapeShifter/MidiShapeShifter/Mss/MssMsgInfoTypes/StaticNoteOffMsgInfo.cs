namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    public class StaticNoteOffMsgInfo : StaticMidiMsgInfo
    {
        public override MssMsgType MsgType => MssMsgType.NoteOff;

        public override string Data2Name => StaticMssMsgInfo.DATA2_NAME_NOTE;

        public override string Data3Name => StaticMssMsgInfo.DATA3_NAME_VELOCITY;
    }
}

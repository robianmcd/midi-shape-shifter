namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    public class StaticNoteOnMsgInfo : StaticMidiMsgInfo
    {
        public override MssMsgType MsgType
        {
            get { return MssMsgType.NoteOn; }
        }

        public override string Data2Name
        {
            get { return StaticMssMsgInfo.DATA2_NAME_NOTE; }
        }

        public override string Data3Name
        {
            get { return StaticMssMsgInfo.DATA3_NAME_VELOCITY; }
        }

        public override void ApplyPreMappingQueryProcessing(MssMsg msgToProcess)
        {
            if (msgToProcess.Data3 == 0)
            {
                msgToProcess.Type = MssMsgType.NoteOff;
            }
        }
    }
}

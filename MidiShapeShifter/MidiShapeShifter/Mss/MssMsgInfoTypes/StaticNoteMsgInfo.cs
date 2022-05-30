namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    public class StaticNoteMsgInfo : StaticMidiMsgInfo
    {
        public override MssMsgType MsgType
        {
            get { return MssMsgType.Note; }
        }

        public override string Data2Name
        {
            get { return StaticMssMsgInfo.DATA2_NAME_NOTE; }
        }

        public override string Data3Name
        {
            get { return StaticMssMsgInfo.DATA3_NAME_VELOCITY; }
        }

        public override void ApplyPostProcessing(MssMsg preProcessedMsg, MssMsg msgToProcess)
        {
            if (preProcessedMsg.Type == MssMsgType.NoteOff || msgToProcess.Data3 == 0)
            {
                msgToProcess.Type = MssMsgType.NoteOff;
            }
            else
            {
                msgToProcess.Type = MssMsgType.NoteOn;
            }
        }

        public override bool TypeIsInRange(MssMsgType msgType)
        {
            return (msgType == MssMsgType.NoteOn || msgType == MssMsgType.NoteOff);
        }
    }
}

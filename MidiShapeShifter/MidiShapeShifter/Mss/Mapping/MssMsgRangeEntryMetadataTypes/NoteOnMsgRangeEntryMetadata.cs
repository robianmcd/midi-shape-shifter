namespace MidiShapeShifter.Mss.Mapping.MssMsgRangeEntryMetadataTypes
{
    public class NoteOnMsgRangeEntryMetadata : MidiMsgRangeEntryMetadata
    {
        public override MssMsgType MsgType
        {
            get { return MssMsgType.NoteOn; }
        }
    }
}

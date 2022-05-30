namespace MidiShapeShifter.Mss.Mapping.MssMsgRangeEntryMetadataTypes
{
    public class CCMsgRangeEntryMetadata : MidiMsgRangeEntryMetadata
    {
        public override MssMsgType MsgType
        {
            get { return MssMsgType.CC; }
        }
    }
}

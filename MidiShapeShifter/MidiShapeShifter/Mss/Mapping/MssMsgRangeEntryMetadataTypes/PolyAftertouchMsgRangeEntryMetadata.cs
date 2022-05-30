namespace MidiShapeShifter.Mss.Mapping.MssMsgRangeEntryMetadataTypes
{
    public class PolyAftertouchMsgRangeEntryMetadata : MidiMsgRangeEntryMetadata
    {
        public override MssMsgType MsgType
        {
            get { return MssMsgType.PolyAftertouch; }
        }
    }
}

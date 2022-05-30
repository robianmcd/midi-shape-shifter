using System;
using System.Windows.Forms;

namespace MidiShapeShifter.Mss.Mapping.MssMsgRangeEntryMetadataTypes
{
    //TODO: comment this class
    public class GeneratorMsgRangeEntryMetadata : MsgRangeEntryMetadataWithGenSelect
    {
        public override MssMsgType MsgType
        {
            get { return MssMsgType.Generator; }
        }

        public override Control EntryField2
        {
            get
            {
                return null;
            }
        }

        protected override void SetEntryField2FromRange(IMssMsgRange msgRange)
        {
            throw new NotImplementedException();
        }

        protected override bool canSelectSameAsInput
        {
            get { return false; }
        }
    }
}

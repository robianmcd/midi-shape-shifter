using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

namespace MidiShapeShifter.Mss.Mapping.MssMsgRangeEntryMetadataTypes
{
    public class GeneratorToggleMsgRangeEntryMetadata : MsgRangeEntryMetadataWithGenSelect
    {
        public override MssMsgType MsgType
        {
            get { return MssMsgType.GeneratorToggle; }
        }

        protected override Control EntryField2
        {
            get
            {
                return null;
            }
        }

        protected override void SetEntryField2FromRange(MssMsgRange msgRange)
        {
            throw new NotImplementedException();
        }

        protected override void InitSameAsInputCompatibleTypes()
        {

        }
    }
}

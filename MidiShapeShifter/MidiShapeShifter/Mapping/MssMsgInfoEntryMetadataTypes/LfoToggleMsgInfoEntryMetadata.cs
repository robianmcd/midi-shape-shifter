using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MidiShapeShifter.Mapping.MssMsgInfoTypes;

namespace MidiShapeShifter.Mapping.MssMsgInfoEntryMetadataTypes
{
    class LfoToggleMsgInfoEntryMetadata : MssMsgInfoEntryMetadata
    {
        protected override void SetMappingDlgEntryFieldCustomProperties()
        {
            this.entryFields.EntryField1Lbl.Visible = true;
            this.entryFields.EntryField1Lbl.Text = "LFO Name:";
            this.entryFields.EntryField1Combo.Visible = true;
        }

        public override MssMsgInfo CreateMsgInfo()
        {
            return null;
        }
    }
}

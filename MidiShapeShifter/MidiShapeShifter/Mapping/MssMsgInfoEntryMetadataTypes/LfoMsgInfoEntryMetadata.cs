using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using MidiShapeShifter.Mapping.MssMsgInfoTypes;

namespace MidiShapeShifter.Mapping.MssMsgInfoEntryMetadataTypes
{
    class LfoMsgInfoEntryMetadata : MssMsgInfoEntryMetadata
    {
        protected override void SetMappingDlgEntryFieldCustomProperties()
        {
            this.entryFields.EntryField1Lbl.Visible = true;
            this.entryFields.EntryField1Lbl.Text = "LFO Name:";

            if (this.ioCatagory == MappingEntry.IO.Input)
            {
                this.entryFields.EntryField1TextBox.Visible = true;
            }
            else if (this.ioCatagory == MappingEntry.IO.Output)
            {
                this.entryFields.EntryField1Combo.Visible = true;
            }
            else 
            {
                Debug.Assert(false);
            }
        }

        public override MssMsgInfo CreateMsgInfo()
        {
            return null;
        }
    }
}

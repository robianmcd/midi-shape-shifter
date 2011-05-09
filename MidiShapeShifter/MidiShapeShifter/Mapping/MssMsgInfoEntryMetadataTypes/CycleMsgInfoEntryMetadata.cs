using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using MidiShapeShifter.Mapping.MssMsgInfoTypes;

namespace MidiShapeShifter.Mapping.MssMsgInfoEntryMetadataTypes
{
    public class CycleMsgInfoEntryMetadata : MssMsgInfoEntryMetadata
    {

        protected override Control EntryField1
        {
            get
            {
                if (this.ioCatagory == MappingEntry.IO.Input)
                {
                    return this.mappingDlg.inEntryField1Combo;
                }
                else
                {
                    //cycle messages cannot be selected for output
                    Debug.Assert(false);
                    return null;
                }
            }
        }

        protected override Control EntryField2
        {
            get
            {
                if (this.ioCatagory == MappingEntry.IO.Input)
                {
                    return this.mappingDlg.inEntryField2TextBox;
                }
                else
                {
                    //cycle messages cannot be selected for output
                    Debug.Assert(false);
                    return null;
                }
            }
        }

        protected override void SetMappingDlgEntryFieldCustomProperties()
        {
            this.EntryField1Lbl.Visible = true;
            this.EntryField1Lbl.Text = "Cycle Time (ms):";

            if (this.ioCatagory == MappingEntry.IO.Input)
            {
                this.EntryField1.Visible = true;
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using MidiShapeShifter.Mss.Mapping.MssMsgInfoTypes;

namespace MidiShapeShifter.Mss.Mapping.MssMsgInfoEntryMetadataTypes
{
    public class GeneratorToggleMsgInfoEntryMetadata : MssMsgInfoEntryMetadata
    {

        protected override Control EntryField1
        {
            get
            {
                if (this.ioCatagory == MappingEntry.IO.Output)
                {
                    return this.mappingDlg.outEntryField1Combo;
                }
                else
                {
                    //Generator Toggle messages cannot be selected for input
                    Debug.Assert(false);
                    return null;
                }
            }
        }

        protected override Control EntryField2
        {
            get
            {
                return null;
            }
        }

        protected override void SetMappingDlgEntryFieldCustomProperties()
        {
            this.EntryField1Lbl.Visible = true;
            this.EntryField1Lbl.Text = "Generator Name:";
            this.EntryField1.Visible = true;
        }

        protected override MssMsgInfo CreateMsgInfoFromValidatedFields()
        {
            GeneratorToggleMsgInfo genToggleMsgInfo = new GeneratorToggleMsgInfo();
            //TODO: Initialize genToggleMsgInfo
            return genToggleMsgInfo;
        }
    }
}

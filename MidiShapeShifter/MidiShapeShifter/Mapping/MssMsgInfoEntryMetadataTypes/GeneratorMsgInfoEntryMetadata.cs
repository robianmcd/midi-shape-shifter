using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using MidiShapeShifter.Mapping.MssMsgInfoTypes;

namespace MidiShapeShifter.Mapping.MssMsgInfoEntryMetadataTypes
{
    public class GeneratorMsgInfoEntryMetadata : MssMsgInfoEntryMetadata
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
                    //Cannot output to generator
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

        public override MssMsgInfo CreateMsgInfo()
        {
            GeneratorMsgInfo generatorMsgInfo = new GeneratorMsgInfo();
            //TODO: Initialize generatorMsgInfo
            return generatorMsgInfo;
        }
    }
}

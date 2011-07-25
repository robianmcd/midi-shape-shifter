using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

namespace MidiShapeShifter.Mss.Mapping.MssMsgRangeEntryMetadataTypes
{
    public class GeneratorToggleMsgRangeEntryMetadata : MssMsgRangeEntryMetadata
    {
        public override MssMsgType MsgType
        {
            get { return MssMsgType.GeneratorToggle; }
        }

        protected override Control EntryField1
        {
            get
            {
                if (this.ioCatagory == IoType.Output)
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

        protected override void SetEntryField1FromRange(MssMsgRange msgRange)
        {
            throw new NotImplementedException();
        }

        protected override void SetEntryField2FromRange(MssMsgRange msgRange)
        {
            throw new NotImplementedException();
        }

        protected override void SetMappingDlgEntryFieldCustomProperties()
        {
            this.EntryField1Lbl.Visible = true;
            this.EntryField1Lbl.Text = "Generator Name:";
            this.EntryField1.Visible = true;
        }

        protected override void InitSameAsInputCompatibleTypes()
        {

        }
    }
}

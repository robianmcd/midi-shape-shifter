using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

using MidiShapeShifter.Mss.MssMsgInfoTypes;

namespace MidiShapeShifter.Mss.Mapping.MssMsgRangeEntryMetadataTypes
{
    public class GeneratorModifyMsgRangeEntryMetadata : MsgRangeEntryMetadataWithGenSelect
    {
        public override MssMsgType MsgType
        {
            get { return MssMsgType.GeneratorModify; }
        }

        public override Control EntryField2
        {
            get
            {
                if (this.ioCatagory == IoType.Input)
                {
                    return this.mappingDlg.inEntryField2Combo;
                }
                else if (this.ioCatagory == IoType.Output)
                {
                    return this.mappingDlg.outEntryField2Combo;
                }
                else
                {
                    //unexpected ioCatagory
                    Debug.Assert(false);
                    return null;
                }
            }
        }

        public override bool SetData2RangeFromField(out string errorMsg)
        {
            int SelectedIndex = ((ComboBox)EntryField2).SelectedIndex;

            this.msgRange.Data2RangeBottom = SelectedIndex;
            this.msgRange.Data2RangeTop = SelectedIndex;

            errorMsg = "";
            return true;
        }

        protected override void SetEntryField2FromRange(IMssMsgRange msgRange)
        {
            ((ComboBox)EntryField2).SelectedIndex = (int)this.msgRange.Data2RangeBottom;
        }

        protected override void SetMappingDlgEntryFieldCustomProperties()
        {
            //Sets the properties for the first entry field
            base.SetMappingDlgEntryFieldCustomProperties();
            
            this.EntryField2Lbl.Visible = true;
            this.EntryField2Lbl.Text = "Operation:";
            this.EntryField2.Visible = true;

            for (int i = 0; i < StaticGeneratorModifyMsgInfo.NUM_GEN_OPERATIONS; i++)
            {
                ((ComboBox)EntryField2).Items.Add(StaticGeneratorModifyMsgInfo.GenOperationNames[i]);
            }

            ((ComboBox)EntryField2).SelectedIndex = 0;
        }

        protected override bool canSelectSameAsInput
        {
            get { return false; }
        }
    }
}

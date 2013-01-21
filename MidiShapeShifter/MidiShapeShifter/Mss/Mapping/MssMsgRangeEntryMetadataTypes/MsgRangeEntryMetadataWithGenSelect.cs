using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using MidiShapeShifter.Mss.Generator;

namespace MidiShapeShifter.Mss.Mapping.MssMsgRangeEntryMetadataTypes
{
    //TODO: comment this class
    public abstract class MsgRangeEntryMetadataWithGenSelect : MssMsgRangeEntryMetadata
    {
        protected IGeneratorMappingManager genMappingMgr;

        public void Init(IGeneratorMappingManager genMappingMgr)
        {
            this.genMappingMgr = genMappingMgr;
        }

        public override Control EntryField1
        {
            get
            {
                if (this.ioCatagory == IoType.Input)
                {
                    return this.mappingDlg.inEntryField1Combo;
                }
                else if (this.ioCatagory == IoType.Output)
                {
                    return this.mappingDlg.outEntryField1Combo;
                }
                else
                {
                    //unexpected ioCatagory
                    Debug.Assert(false);
                    return null;
                }
            }
        }

        public override bool SetData1RangeFromField(out string errorMsg)
        {
            IGeneratorMappingEntry selectedGenEntry = (IGeneratorMappingEntry)((ComboBox)EntryField1).SelectedItem;
            if (selectedGenEntry == null)
            {
                errorMsg = "You must create a generator before mapping it to something";
                return false;
            }
            else
            {
                int selectedGenId = selectedGenEntry.Id;
                this.msgRange.Data1RangeBottom = selectedGenId;
                this.msgRange.Data1RangeTop = selectedGenId;

                errorMsg = "";
                return true;
            }
        }

        protected override void SetEntryField1FromRange(IMssMsgRange msgRange)
        {
            ((ComboBox)EntryField1).SelectedIndex = this.genMappingMgr.GetMappingEntryIndexById((int)msgRange.Data1RangeBottom);
        }

        protected override void SetMappingDlgEntryFieldCustomProperties()
        {
            this.EntryField1Lbl.Visible = true;
            this.EntryField1Lbl.Text = "Generator Name:";
            this.EntryField1.Visible = true;

            List<IGeneratorMappingEntry>  genEntrieList = this.genMappingMgr.GetCopyOfMappingEntryList();

            foreach (IGeneratorMappingEntry genEntry in genEntrieList) {
                ((ComboBox)this.EntryField1).Items.Add(genEntry);
            }

            if (genEntrieList.Count > 0)
            {
                ((ComboBox)EntryField1).SelectedIndex = 0;
            }
        }

    }
}

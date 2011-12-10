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

        protected override Control EntryField1
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
            int SelectedIndex = ((ComboBox)EntryField1).SelectedIndex;
            if (SelectedIndex < 0)
            {
                errorMsg = "You must create a generator before mapping it to something";
                return false;
            }
            else 
            { 
                IGeneratorMappingEntry SelectedGenEntry = 
                        this.genMappingMgr.GetGenMappingEntryByIndex(SelectedIndex);
                int SelectedGenId = SelectedGenEntry.GenConfigInfo.Id;
                this.msgRange.Data1RangeBottom = SelectedGenId;
                this.msgRange.Data1RangeTop = SelectedGenId;

                errorMsg = "";
                return true;
            }
        }

        protected override void SetEntryField1FromRange(IMssMsgRange msgRange)
        {
            ((ComboBox)EntryField1).SelectedIndex = msgRange.Data1RangeBottom;
        }

        protected override void SetMappingDlgEntryFieldCustomProperties()
        {
            this.EntryField1Lbl.Visible = true;
            this.EntryField1Lbl.Text = "Generator Name:";
            this.EntryField1.Visible = true;

            for (int i = 0; i < this.genMappingMgr.GetNumEntries(); i++ )
            {
                IGeneratorMappingEntry curEntry = this.genMappingMgr.GetGenMappingEntryByIndex(i);
                ((ComboBox)EntryField1).Items.Add(curEntry.GenConfigInfo.Name);
            }

            if (this.genMappingMgr.GetNumEntries() > 0)
            {
                ((ComboBox)EntryField1).SelectedIndex = 0;
            }
        }

    }
}

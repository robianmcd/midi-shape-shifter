﻿using System.Windows.Forms;

namespace MidiShapeShifter.Mss.Mapping.MssMsgRangeEntryMetadataTypes
{
    public class PitchBendMsgRangeEntryMetadata : MidiMsgRangeEntryMetadata
    {
        public override MssMsgType MsgType => MssMsgType.PitchBend;

        public override Control EntryField2 => null;

        protected override void SetMappingDlgEntryFieldCustomProperties()
        {
            this.EntryField1Lbl.Visible = true;
            this.EntryField1Lbl.Text = CHAN_RANGE_STR;

            this.EntryField1.Visible = true;
        }

        public override bool SetData2RangeFromField(out string errorMessage)
        {
            errorMessage = "";
            this.msgRange.Data2RangeBottom = MssMsgUtil.UNUSED_MSS_MSG_DATA;
            this.msgRange.Data2RangeTop = MssMsgUtil.UNUSED_MSS_MSG_DATA;
            return true;
        }
    }
}

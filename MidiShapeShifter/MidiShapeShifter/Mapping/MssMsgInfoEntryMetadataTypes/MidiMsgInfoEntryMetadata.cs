using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MidiShapeShifter.Mapping.MssMsgInfoTypes;

namespace MidiShapeShifter.Mapping.MssMsgInfoEntryMetadataTypes
{
    abstract class MidiMsgInfoEntryMetadata : MssMsgInfoEntryMetadata
    {
        protected int paramRangeBottom;
        protected int paramRangeTop;
        protected int chanRangeBottom;
        protected int chanRangeTop;

        //set the properties of all the the controls in the entryFields member variable whose properties should differ 
        //from the default properties set in SetMappingDlgEntryFieldsDefaultProperties().
        protected override void SetMappingDlgEntryFieldCustomProperties()
        {
            this.entryFields.EntryField1Lbl.Visible = true;
            this.entryFields.EntryField1Lbl.Text = "Channel Range:";

            this.entryFields.EntryField1TextBox.Visible = true;

            this.entryFields.EntryField2Lbl.Visible = true;
            this.entryFields.EntryField2Lbl.Text = "Param Range:";

            this.entryFields.EntryField2TextBox.Visible = true;
        }

        public override bool ValidateEntryField1()
        {
            string userInput = this.entryFields.EntryField1Lbl.Text;
            return ValidateRangeField(userInput, MssMsgUtil.MIN_CHANNEL, MssMsgUtil.MAX_CHANNEL, 
                               ref this.chanRangeBottom, ref this.chanRangeTop);
            
        }

        public override bool ValidateEntryField2()
        {
            string userInput = this.entryFields.EntryField2Lbl.Text;
            return ValidateRangeField(userInput, MssMsgUtil.MIN_PARAM, MssMsgUtil.MAX_PARAM,
                               ref this.paramRangeBottom, ref this.paramRangeTop);

        }

        public virtual bool ValidateRangeField(string userInput, int minValue, int maxValue,
                                               ref int rangeBottom, ref int rangeTop)
        {
            int singleRangeValue;
            int tempRangeTop;
            int tempRangeBottom;

            if (EntryFieldInterpretingUtils.InterpretAsInt(userInput, out singleRangeValue))
            {
                rangeTop = singleRangeValue;
                rangeBottom = singleRangeValue;
            }
            else if (EntryFieldInterpretingUtils.InterpretAsRangeAllStr(userInput))
            {
                rangeTop = MssMsgUtil.MIN_CHANNEL;
                rangeBottom = MssMsgUtil.MAX_CHANNEL;
            }
            else if (EntryFieldInterpretingUtils.InterpretAsRange(userInput, out tempRangeTop, out tempRangeBottom))
            {
                rangeTop = tempRangeTop; 
                rangeBottom = tempRangeBottom;
            }
            else
            {
                return false;
            }
            return true;
        }

        protected void InitializeMidiMsgInfo(MidiMsgInfo midiMsgInfo)
        {
            midiMsgInfo.Initialize(this.chanRangeBottom, this.chanRangeTop, this.paramRangeBottom, this.paramRangeTop);
        }
    }
}

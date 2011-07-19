using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace MidiShapeShifter.Mss.Mapping.MssMsgRangeEntryMetadataTypes
{
    public abstract class MidiMsgRangeEntryMetadata : MssMsgRangeEntryMetadata
    {
        protected int paramRangeBottom;
        protected int paramRangeTop;
        protected int chanRangeBottom;
        protected int chanRangeTop;



        protected override Control EntryField1
        {
            get
            {
                if (this.ioCatagory == IoType.Input)
                {
                    return this.mappingDlg.inEntryField1TextBox;
                }
                else if (this.ioCatagory == IoType.Output)
                {
                    return this.mappingDlg.outEntryField1TextBox;
                }
                else
                {
                    Debug.Assert(false);
                    return null;
                }
            }
        }

        protected override Control EntryField2
        {
            get
            {
                if (this.ioCatagory == IoType.Input)
                {
                    return this.mappingDlg.inEntryField2TextBox;
                }
                else if (this.ioCatagory == IoType.Output)
                {
                    return this.mappingDlg.outEntryField2TextBox;
                }
                else
                {
                    Debug.Assert(false);
                    return null;
                }
            }
        }

        //set the properties of all the the controls in the entryFields member variable whose properties should differ 
        //from the default properties set in SetMappingDlgEntryFieldsDefaultProperties().
        protected override void SetMappingDlgEntryFieldCustomProperties()
        {
            this.EntryField1Lbl.Visible = true;
            this.EntryField1Lbl.Text = "Channel Range:";

            this.EntryField1.Visible = true;

            this.EntryField2Lbl.Visible = true;
            this.EntryField2Lbl.Text = "Param Range:";

            this.EntryField2.Visible = true;
        }

        public override bool SetData1RangeFromField(out string errorMsg)
        {
            string userInput = this.EntryField1.Text;
            int rangeBottom;
            int rangeTop;
            bool userInputIsValid = GetRangeFromUserInput(userInput, 
                this.msgRange.MsgInfo.MinData1Value, this.msgRange.MsgInfo.MaxData1Value,
                out rangeBottom, out rangeTop, 
                out errorMsg);

            this.msgRange.Data1RangeBottom = rangeBottom;
            this.msgRange.Data1RangeTop = rangeTop;

            return userInputIsValid;
        }

        public override bool SetData2RangeFromField(out string errorMsg)
        {
            string userInput = this.EntryField2.Text;
            int rangeBottom;
            int rangeTop;
            bool userInputIsValid = GetRangeFromUserInput(userInput,
                this.msgRange.MsgInfo.MinData2Value, this.msgRange.MsgInfo.MaxData2Value,
                out rangeBottom, out rangeTop,
                out errorMsg);

            this.msgRange.Data2RangeBottom = rangeBottom;
            this.msgRange.Data2RangeTop = rangeTop;

            return userInputIsValid;

        }

        public virtual bool GetRangeFromUserInput(string userInput, int minValue, int maxValue,
                                               out int rangeBottom, out int rangeTop, out string errorMsg)
        {
            int singleRangeValue;
            int tempRangeTop;
            int tempRangeBottom;

            errorMsg = "";
            bool validFormat;
            bool validRange;

            if (EntryFieldInterpretingUtils.InterpretAsInt(userInput, out singleRangeValue))
            {
                rangeTop = singleRangeValue;
                rangeBottom = singleRangeValue;
                validFormat = true;
            }
            else if (EntryFieldInterpretingUtils.InterpretAsRangeAllStr(userInput))
            {
                rangeTop = maxValue;
                rangeBottom = minValue;
                validFormat = true;
            }
            else if (EntryFieldInterpretingUtils.InterpretAsRange(userInput, out tempRangeTop, out tempRangeBottom))
            {
                rangeTop = tempRangeTop; 
                rangeBottom = tempRangeBottom;
                validFormat = true;
            }
            else
            {
                errorMsg = "Invalid range format";
                rangeTop = MssMsgUtil.UNUSED_MSS_MSG_DATA;
                rangeBottom = MssMsgUtil.UNUSED_MSS_MSG_DATA;
                validFormat = false;
            }

            if (validFormat == true)
            {
                if (rangeBottom >= minValue && rangeTop <= maxValue)
                {
                    validRange = true;
                }
                else
                {
                    errorMsg = "Out of range";
                    validRange = false;
                }
            }
            else 
            {
                validRange = false;
            }

            return validRange;
        }

        protected override void InitSameAsInputCompatibleTypes()
        {
            this.sameAsInputCompatibleTypes.Add(MssMsgType.NoteOn);
            this.sameAsInputCompatibleTypes.Add(MssMsgType.NoteOff);
            this.sameAsInputCompatibleTypes.Add(MssMsgType.CC);
            this.sameAsInputCompatibleTypes.Add(MssMsgType.PitchBend);
            this.sameAsInputCompatibleTypes.Add(MssMsgType.PolyAftertouch);
        }
    }
}

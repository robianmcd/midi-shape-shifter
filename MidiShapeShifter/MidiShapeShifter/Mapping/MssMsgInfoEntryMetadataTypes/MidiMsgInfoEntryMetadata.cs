using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using MidiShapeShifter.Mapping.MssMsgInfoTypes;

namespace MidiShapeShifter.Mapping.MssMsgInfoEntryMetadataTypes
{
    public abstract class MidiMsgInfoEntryMetadata : MssMsgInfoEntryMetadata
    {
        protected int paramRangeBottom;
        protected int paramRangeTop;
        protected int chanRangeBottom;
        protected int chanRangeTop;


        protected override Control EntryField1
        {
            get
            {
                if (this.ioCatagory == MappingEntry.IO.Input)
                {
                    return this.mappingDlg.inEntryField1TextBox;
                }
                else if (this.ioCatagory == MappingEntry.IO.Output)
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
                if (this.ioCatagory == MappingEntry.IO.Input)
                {
                    return this.mappingDlg.inEntryField2TextBox;
                }
                else if (this.ioCatagory == MappingEntry.IO.Output)
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

        public override bool IsEntryField1Valid(out string errorMsg)
        {
            string userInput = this.EntryField1.Text;
            return ValidateRangeField(userInput, MssMsgUtil.MIN_CHANNEL, MssMsgUtil.MAX_CHANNEL, 
                                      ref this.chanRangeBottom, ref this.chanRangeTop, out errorMsg);
            
        }

        public override bool IsEntryField2Valid(out string errorMsg)
        {
            string userInput = this.EntryField2.Text;
            return ValidateRangeField(userInput, MssMsgUtil.MIN_PARAM, MssMsgUtil.MAX_PARAM,
                                      ref this.paramRangeBottom, ref this.paramRangeTop, out errorMsg);

        }

        public virtual bool ValidateRangeField(string userInput, int minValue, int maxValue,
                                               ref int rangeBottom, ref int rangeTop, out string errorMsg)
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
                rangeBottom = minValue;
                rangeTop = maxValue;
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

        protected void InitializeMidiMsgInfo(MidiMsgInfo midiMsgInfo)
        {
            midiMsgInfo.Initialize(this.chanRangeBottom, this.chanRangeTop, this.paramRangeBottom, this.paramRangeTop);
        }

        protected override void InitSameAsInputCompatibleTypes()
        {
            this.sameAsInputCompatibleTypes.Add(MssMsgUtil.MssMsgType.NoteOn);
            this.sameAsInputCompatibleTypes.Add(MssMsgUtil.MssMsgType.NoteOff);
            this.sameAsInputCompatibleTypes.Add(MssMsgUtil.MssMsgType.CC);
            this.sameAsInputCompatibleTypes.Add(MssMsgUtil.MssMsgType.PitchBend);
            this.sameAsInputCompatibleTypes.Add(MssMsgUtil.MssMsgType.Aftertouch);
        }
    }
}

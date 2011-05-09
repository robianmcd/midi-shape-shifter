using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using MidiShapeShifter.Mapping.MssMsgInfoEntryMetadataTypes;

namespace MidiShapeShifter.Mapping
{
    public partial class MappingDlg : Form
    {
        public enum FieldType { ChannelField, ParamField };
        public readonly string[] FieldTypeStr = new string[] { "channel", "parameter"};
        protected const string RANGE_EXAMPLE = "\"1\", \"1-5\", or \"All\"";

        protected MssMsgInfoEntryMetadata inMsgMetadata;
        protected MssMsgInfoEntryMetadata outMsgMetadata;

        //This enum is intended to be used to diagnose issues with user input in range fields.
        public enum RangeValidity {ValidRange,              //range is valid
                                   EmptyRange,              //range has not been entered
                                   OutOfChannelRange,       //range contains a value less then 1 or greater then 16
                                   OutOfParamRange,         //range contains a value less then 0 or greater then 127
                                   InvalidRangeFormat,      //range is not in a format like "1", "1-5" or "All"
                                   InvalidInOutRangeRatio,  //ratio between in/out range size is not 1 to n, n to n, or n to 1
                                   UnexpectedError };

        public MappingEntry mappingEntry = new MappingEntry();
        public bool useMappingEntryForDefaultValues = false;

        public MappingDlg()
        {
            InitializeComponent();

            inTypeCombo.Items.Add(MssMsgUtil.MssMsgTypeNames[(int)MssMsgUtil.MssMsgType.NoteOn]);
            inTypeCombo.Items.Add(MssMsgUtil.MssMsgTypeNames[(int)MssMsgUtil.MssMsgType.NoteOff]);
            inTypeCombo.Items.Add(MssMsgUtil.MssMsgTypeNames[(int)MssMsgUtil.MssMsgType.CC]);
            inTypeCombo.Items.Add(MssMsgUtil.MssMsgTypeNames[(int)MssMsgUtil.MssMsgType.PitchBend]);
            inTypeCombo.Items.Add(MssMsgUtil.MssMsgTypeNames[(int)MssMsgUtil.MssMsgType.Aftertouch]);
            inTypeCombo.Items.Add(MssMsgUtil.MssMsgTypeNames[(int)MssMsgUtil.MssMsgType.LFO]);
            inTypeCombo.Items.Add(MssMsgUtil.MssMsgTypeNames[(int)MssMsgUtil.MssMsgType.Cycle]);
            inTypeCombo.SelectedIndex = 0;
        }

        public MssMsgUtil.MssMsgType GetMessageTypeFromCombo(ComboBox combo) 
        {
            return (MssMsgUtil.MssMsgType)MssMsgUtil.MssMsgTypeNames.FindIndex(item => item.Equals(combo.Text));
        }

        protected void validateRangeTextBox(TextBox sender, CancelEventArgs e, FieldType fieldType)
        {
            errorProvider.SetError((Control)sender, "");

            int rangeBottom;
            int rangeTop;
            RangeValidity rangeValidity =
                GetRangeFromString(sender.Text, fieldType, out rangeBottom, out rangeTop);
            if (rangeValidity != RangeValidity.ValidRange)
            {
                errorProvider.SetError((Control)sender, GetRangeValidityMessage(rangeValidity, fieldType));
            }
        }

        protected string GetRangeValidityMessage(RangeValidity rangeValidity, FieldType fieldType) 
        {
            String msg = "";
            switch (rangeValidity) 
            {
                case RangeValidity.EmptyRange:
                    {
                        msg = "You must enter a " + FieldTypeStr[(int)fieldType] + " range. Eg. " + RANGE_EXAMPLE + ".";
                        break;
                    }
                case RangeValidity.InvalidInOutRangeRatio:
                    {
                        msg = "The size of the output range must be 1 or the same as the size of the input range.";
                        break;
                    }
                case RangeValidity.InvalidRangeFormat:
                    {
                        msg = "The range must be formatted in a way similar to one of the following examples: " + 
                            RANGE_EXAMPLE + ".";
                        break;
                    }
                case RangeValidity.OutOfChannelRange:
                    {
                        msg = "Channel values must be between 1 and 16.";
                        break;
                    }
                case RangeValidity.OutOfParamRange:
                    {
                        msg = "Parameter values must be between 0 and 127.";
                        break;
                    }
                default:
                    {
                        Debug.Assert(false);
                        break;
                    }
            }
            return msg;
        }

        protected RangeValidity GetRangeFromTextBox(TextBox rangeTextBox, FieldType fieldType,
                                          out int rangeBottom, out int rangeTop)
        {
            RangeValidity rangeValidity = GetRangeFromString(rangeTextBox.Text, fieldType, out rangeBottom, out rangeTop);
            
            if (rangeValidity != RangeValidity.ValidRange) {
                errorProvider.SetError((Control)rangeTextBox, GetRangeValidityMessage(rangeValidity, fieldType));                
            }

            return rangeValidity;
        }

        protected RangeValidity GetRangeFromString(string rangeStr, FieldType fieldType, 
                                          out int rangeBottom, out int rangeTop)
        {
            //TODO needs to handle note ranges like C1-D5 or A-1-D-1

            bool validRangeStructure;
            RangeValidity rangeValidity = RangeValidity.ValidRange;
            int singleChannelRange;
            rangeStr = rangeStr.Trim();

            //Parse rangeStr to pull out the top and bottom of the range and set validRangeStructure.
            //This if statement doesn't need to deal with ensuring the the bottom and top of the range are actually valid
            //param or channel values
            if (rangeStr.Equals(""))
            {
                rangeBottom = MssMsgUtil.RANGE_INVALID;
                rangeTop = MssMsgUtil.RANGE_INVALID;
                rangeValidity = RangeValidity.EmptyRange;
                validRangeStructure = false;
            }
            else if (rangeStr.Equals(MssMsgUtil.RANGE_ALL_STR, StringComparison.OrdinalIgnoreCase))
            {
                if (fieldType == FieldType.ChannelField)
                {
                    rangeBottom = MssMsgUtil.MIN_CHANNEL;
                    rangeTop = MssMsgUtil.MAX_CHANNEL;
                }
                else if (fieldType == FieldType.ParamField)
                {
                    rangeBottom = MssMsgUtil.MIN_PARAM;
                    rangeTop = MssMsgUtil.MAX_PARAM;
                }
                else
                {
                    //Unknown field type
                    Debug.Assert(false);
                    rangeBottom = MssMsgUtil.RANGE_INVALID;
                    rangeTop = MssMsgUtil.RANGE_INVALID;
                    return RangeValidity.UnexpectedError;
                }
                validRangeStructure = true;
            }
            else if (int.TryParse(rangeStr.Trim(), out singleChannelRange))
            {
                rangeBottom = singleChannelRange;
                rangeTop = singleChannelRange;
                validRangeStructure = true;
            }
            else
            {

                string[] rangeArr = rangeStr.Split('-');
                if (rangeArr.Length != 2)
                {
                    rangeBottom = MssMsgUtil.RANGE_INVALID;
                    rangeTop = MssMsgUtil.RANGE_INVALID;
                    validRangeStructure = false;
                    rangeValidity = RangeValidity.InvalidRangeFormat;
                }
                else 
                {
                    
                    bool rangeIsInts = int.TryParse(rangeArr[0].Trim(), out rangeBottom);
                    rangeIsInts &= int.TryParse(rangeArr[1].Trim(), out rangeTop);
                    if (rangeIsInts)
                    {
                        validRangeStructure = true;
                    }
                    else
                    {
                        rangeBottom = MssMsgUtil.RANGE_INVALID;
                        rangeTop = MssMsgUtil.RANGE_INVALID;
                        validRangeStructure = false;
                        rangeValidity = RangeValidity.InvalidRangeFormat;                     
                    }
                }
            }

            //swap the top and bottom of the range if the bottom is greater then the top
            if (rangeBottom > rangeTop) {
                int rangeBottomBackup = rangeBottom;
                rangeBottom = rangeTop;
                rangeTop = rangeBottomBackup;
            }

            //check if rangeBottom and rangeTop make a valid range and set validRange
            if (validRangeStructure == true)
            {
                if (fieldType == FieldType.ChannelField)
                {
                    if (MssMsgUtil.isValidChannel(rangeBottom) && MssMsgUtil.isValidChannel(rangeTop))
                    {
                        rangeValidity = RangeValidity.ValidRange;
                    }
                    else
                    {
                        rangeValidity = RangeValidity.OutOfChannelRange;    
                    }
                }
                else if (fieldType == FieldType.ParamField)
                {
                    if (MssMsgUtil.isValidParamValue(rangeBottom) && MssMsgUtil.isValidParamValue(rangeTop))
                    {
                        rangeValidity = RangeValidity.ValidRange;
                    }
                    else
                    {
                        rangeValidity = RangeValidity.OutOfParamRange;
                    }
                }
                else
                {
                    //Unknown field type
                    Debug.Assert(false);
                    return RangeValidity.UnexpectedError;
                }
            }

            return rangeValidity;
        }

        private void outSameAsInCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bool enabledStatus = !((CheckBox)sender).Checked;

            outTypeCombo.Enabled = enabledStatus;
            outEntryField2TextBox.Enabled = enabledStatus;
            outEntryField1TextBox.Enabled = enabledStatus;
            outLearnBtn.Enabled = enabledStatus;
        }

        private void inEntryField1TextBox_Validating(object sender, CancelEventArgs e)
        {
            inMsgMetadata.ValidateEntryField1();
        }

        private void inEntryField1Combo_Validating(object sender, CancelEventArgs e)
        {
            inMsgMetadata.ValidateEntryField1();
        }

        private void inEntryField2TextBox_Validating(object sender, CancelEventArgs e)
        {
            inMsgMetadata.ValidateEntryField2();
        }

        private void inEntryField2Combo_Validating(object sender, CancelEventArgs e)
        {
            inMsgMetadata.ValidateEntryField2();
        }

        private void outEntryField1TextBox_Validating(object sender, CancelEventArgs e)
        {
            outMsgMetadata.ValidateEntryField1();
        }

        private void outEntryField1Combo_Validating(object sender, CancelEventArgs e)
        {
            outMsgMetadata.ValidateEntryField1();
        }

        private void outEntryField2TextBox_Validating(object sender, CancelEventArgs e)
        {
            outMsgMetadata.ValidateEntryField2();
        }

        private void outEntryField2Combo_Validating(object sender, CancelEventArgs e)
        {
            outMsgMetadata.ValidateEntryField2();
        }

        private void inTypeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //PopulateOutTypeCombo();

            MsgTypeComboChanged((ComboBox)sender, MappingEntry.IO.Input, ref inMsgMetadata);
        }

        private void outTypeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            MsgTypeComboChanged((ComboBox)sender, MappingEntry.IO.Output, ref outMsgMetadata);
        }

        protected void MsgTypeComboChanged(ComboBox msgTypeCombo, 
                                           MappingEntry.IO ioCategory, 
                                           ref MssMsgInfoEntryMetadata msgMetadata)
        {
            MssMsgUtil.MssMsgType msgType = GetMessageTypeFromCombo(msgTypeCombo);

            switch (msgType)
            {
                case MssMsgUtil.MssMsgType.NoteOn:
                    {
                        msgMetadata = new NoteOnMsgInfoEntryMetadata();
                        break;
                    }
                case MssMsgUtil.MssMsgType.NoteOff:
                    {
                        msgMetadata = new NoteOffMsgInfoEntryMetadata();
                        break;
                    }
                case MssMsgUtil.MssMsgType.CC:
                    {
                        msgMetadata = new CCMsgInfoEntryMetadata();
                        break;
                    }
                case MssMsgUtil.MssMsgType.PitchBend:
                    {
                        msgMetadata = new PitchBendMsgInfoEntryMetadata();
                        break;
                    }
                case MssMsgUtil.MssMsgType.Aftertouch:
                    {
                        msgMetadata = new AftertouchMsgInfoEntryMetadata();
                        break;
                    }
                case MssMsgUtil.MssMsgType.Cycle:
                    {
                        msgMetadata = new CycleMsgInfoEntryMetadata();
                        break;
                    }
                case MssMsgUtil.MssMsgType.LFO:
                    {
                        msgMetadata = new LfoMsgInfoEntryMetadata();
                        break;
                    }
                case MssMsgUtil.MssMsgType.LFOToggle:
                    {
                        msgMetadata = new LfoToggleMsgInfoEntryMetadata();
                        break;
                    }
            }

            msgMetadata.Init(this, ioCategory);
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            //TODO if same as input is on then don't validate out entry fields.

            bool allFieldsAreValid = inMsgMetadata.ValidateEntryField1();
            allFieldsAreValid &= inMsgMetadata.ValidateEntryField2();
            allFieldsAreValid &= outMsgMetadata.ValidateEntryField1();
            allFieldsAreValid &= outMsgMetadata.ValidateEntryField2();

            if (allFieldsAreValid)
            {
                OkBtn.DialogResult = DialogResult.OK;
                this.Close();
            }
        }   
    }
}

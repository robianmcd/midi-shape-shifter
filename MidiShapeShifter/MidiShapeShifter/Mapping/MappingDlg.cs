using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace MidiShapeShifter.Mapping
{
    public partial class MappingDlg : Form
    {
        public enum FieldType { ChannelField, ParamField };
        public readonly string[] FieldTypeStr = new string[] { "channel", "parameter"};
        protected const string RANGE_EXAMPLE = "\"1\", \"1-5\", or \"All\"";

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

            inTypeCombo.Items.AddRange(MidiHelper.MssMsgTypeNames.ToArray());
            inTypeCombo.SelectedIndex = 0;

            PopulateOutTypeCombo();
        }

        protected MidiHelper.MssMsgType GetMessageTypeFromCombo(ComboBox combo) 
        {
            return (MidiHelper.MssMsgType)MidiHelper.MssMsgTypeNames.FindIndex(item => item.Equals(combo.Text));
        }

        protected void PopulateOutTypeCombo()
        {
            outTypeCombo.Items.Clear();

            MidiHelper.MssMsgType inMsgType = GetMessageTypeFromCombo(inTypeCombo);

            switch (inMsgType)
            {
                case MidiHelper.MssMsgType.Cycle:
                {
                    outTypeCombo.Items.Add(MidiHelper.MssMsgTypeNames[(int) MidiHelper.MssMsgType.LFO]);
                    outSameAsInCheckBox.Checked = false;
                    outSameAsInCheckBox.Enabled = false;
                    break;
                }
                default:
                {
                    outTypeCombo.Items.Add(MidiHelper.MssMsgTypeNames[(int)MidiHelper.MssMsgType.NoteOn]);
                    outTypeCombo.Items.Add(MidiHelper.MssMsgTypeNames[(int)MidiHelper.MssMsgType.NoteOff]);
                    outTypeCombo.Items.Add(MidiHelper.MssMsgTypeNames[(int)MidiHelper.MssMsgType.CC]);
                    outTypeCombo.Items.Add(MidiHelper.MssMsgTypeNames[(int)MidiHelper.MssMsgType.PitchBend]);
                    outTypeCombo.Items.Add(MidiHelper.MssMsgTypeNames[(int)MidiHelper.MssMsgType.Aftertouch]);
                    outSameAsInCheckBox.Enabled = true;
                    break;
                }
            }

            outTypeCombo.SelectedIndex = 0;

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
                rangeBottom = MidiHelper.RANGE_INVALID;
                rangeTop = MidiHelper.RANGE_INVALID;
                rangeValidity = RangeValidity.EmptyRange;
                validRangeStructure = false;
            }
            else if (rangeStr.Equals(MidiHelper.RANGE_ALL_STR, StringComparison.OrdinalIgnoreCase))
            {
                if (fieldType == FieldType.ChannelField)
                {
                    rangeBottom = MidiHelper.MIN_CHANNEL;
                    rangeTop = MidiHelper.MAX_CHANNEL;
                }
                else if (fieldType == FieldType.ParamField)
                {
                    rangeBottom = MidiHelper.MIN_PARAM;
                    rangeTop = MidiHelper.MAX_PARAM;
                }
                else
                {
                    //Unknown field type
                    Debug.Assert(false);
                    rangeBottom = MidiHelper.RANGE_INVALID;
                    rangeTop = MidiHelper.RANGE_INVALID;
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
                    rangeBottom = MidiHelper.RANGE_INVALID;
                    rangeTop = MidiHelper.RANGE_INVALID;
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
                        rangeBottom = MidiHelper.RANGE_INVALID;
                        rangeTop = MidiHelper.RANGE_INVALID;
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
                    if (MidiHelper.isValidChannel(rangeBottom) && MidiHelper.isValidChannel(rangeTop))
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
                    if (MidiHelper.isValidParamValue(rangeBottom) && MidiHelper.isValidParamValue(rangeTop))
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
            outParamRangeTextBox.Enabled = enabledStatus;
            outChannelRangeTextBox.Enabled = enabledStatus;
            outLearnBtn.Enabled = enabledStatus;
        }


        private void inChannelRangeTextBox_Validating(object sender, CancelEventArgs e)
        {
            validateRangeTextBox((TextBox)sender, e, FieldType.ChannelField);
        }

        private void outChannelRangeTextBox_Validating(object sender, CancelEventArgs e)
        {
            validateRangeTextBox((TextBox)sender, e, FieldType.ChannelField);

        }

        private void inParamRangeTextBox_Validating(object sender, CancelEventArgs e)
        {
            validateRangeTextBox((TextBox)sender, e, FieldType.ParamField);

        }

        private void outParamRangeTextBox_Validating(object sender, CancelEventArgs e)
        {
            validateRangeTextBox((TextBox)sender, e, FieldType.ParamField);

        }

        private void OkBtn_Validating(object sender, CancelEventArgs e)
        {
            //TODO: make sure all fields are valid and set values in mappingEntry

            e.Cancel = true;
        }

        private void inTypeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateOutTypeCombo();
        }
    }
}

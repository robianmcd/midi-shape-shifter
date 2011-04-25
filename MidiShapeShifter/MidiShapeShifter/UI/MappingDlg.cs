using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace MidiShapeShifter
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
        }


        private void inChannelRangeTextBox_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.SetError((Control)sender, "");
            int rangeBottom;
            int rangeTop;
            RangeValidity rangeValidity = 
                GetRangeFromString(((TextBox)sender).Text, FieldType.ChannelField, out rangeBottom, out rangeTop);
            if (rangeValidity != RangeValidity.ValidRange)
            {
                e.Cancel = true;
                errorProvider.SetError((Control)sender, "You must enter a valid channel range. Eg. \"1\", \"1-5\", or \"All\".");
            }
        }

        protected string GetRangeValidityMessage(RangeValidity rangeValidity, FieldType fieldType) 
        {
            String msg = "";
            switch (rangeValidity) 
            {
                case RangeValidity.EmptyRange:
                    {
                        msg = "You must enter a ";
                        break;
                    }
                case RangeValidity.InvalidInOutRangeRatio:
                    {
                        break;
                    }
                case RangeValidity.InvalidRangeFormat:
                    {
                        break;
                    }
                case RangeValidity.OutOfChannelRange:
                    {
                        break;
                    }
                case RangeValidity.OutOfParamRange:
                    {
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

        //returns false if the string does not contain a valid range
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
    }
}

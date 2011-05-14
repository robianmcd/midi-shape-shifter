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

        public MappingEntry mappingEntry;
        public bool useMappingEntryForDefaultValues = false;

        public MappingDlg(MappingEntry mappingEntry)
        {
            InitializeComponent();
            this.mappingEntry = mappingEntry;

            this.inTypeCombo.Items.Add(MssMsgUtil.MssMsgTypeNames[(int)MssMsgUtil.MssMsgType.NoteOn]);
            this.inTypeCombo.Items.Add(MssMsgUtil.MssMsgTypeNames[(int)MssMsgUtil.MssMsgType.NoteOff]);
            this.inTypeCombo.Items.Add(MssMsgUtil.MssMsgTypeNames[(int)MssMsgUtil.MssMsgType.CC]);
            this.inTypeCombo.Items.Add(MssMsgUtil.MssMsgTypeNames[(int)MssMsgUtil.MssMsgType.PitchBend]);
            this.inTypeCombo.Items.Add(MssMsgUtil.MssMsgTypeNames[(int)MssMsgUtil.MssMsgType.PolyAftertouch]);
            this.inTypeCombo.Items.Add(MssMsgUtil.MssMsgTypeNames[(int)MssMsgUtil.MssMsgType.ChanAftertouch]);
            this.inTypeCombo.Items.Add(MssMsgUtil.MssMsgTypeNames[(int)MssMsgUtil.MssMsgType.Generator]);
            this.inTypeCombo.SelectedIndex = 0;

            this.outSameAsInCheckBox.Checked = true;
        }

        public MssMsgUtil.MssMsgType GetMessageTypeFromCombo(ComboBox combo) 
        {
            return (MssMsgUtil.MssMsgType)MssMsgUtil.MssMsgTypeNames.FindIndex(item => item.Equals(combo.Text));
        }

        /*msg = "You must enter a " + FieldTypeStr[(int)fieldType] + " range. Eg. " + RANGE_EXAMPLE + ".";

        msg = "The size of the output range must be 1 or the same as the size of the input range.";

        msg = "The range must be formatted in a way similar to one of the following examples: " + RANGE_EXAMPLE + ".";

        msg = "Channel values must be between 1 and 16.";

        msg = "Parameter values must be between 0 and 127.";*/

        private void outSameAsInCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bool enabledStatus = !((CheckBox)sender).Checked;

            outTypeCombo.Enabled = enabledStatus;
            outEntryField2TextBox.Enabled = enabledStatus;
            outEntryField1TextBox.Enabled = enabledStatus;
            outLearnBtn.Enabled = enabledStatus;

            if (enabledStatus == false)
            {
                //we cannot just set the outTypeCombo's selected index to the same as in inTypeCombo's selected index
                //because they may will not contain all of the same items.
                MssMsgUtil.MssMsgType inType = GetMessageTypeFromCombo(this.inTypeCombo);
                string inTypeName = MssMsgUtil.MssMsgTypeNames[(int)inType];
                this.outTypeCombo.SelectedIndex = this.outTypeCombo.FindStringExact(inTypeName);

                this.outEntryField1TextBox.Text = this.inEntryField1TextBox.Text;
                this.outEntryField1Combo.SelectedIndex = this.inEntryField1Combo.SelectedIndex;
                this.outEntryField2TextBox.Text = this.inEntryField2TextBox.Text;
                this.outEntryField2Combo.SelectedIndex = this.inEntryField2Combo.SelectedIndex;
                
            }
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
            MsgTypeComboChanged((ComboBox)sender, MappingEntry.IO.Input, ref inMsgMetadata);

            if (this.outSameAsInCheckBox.Checked == true)
            {
                //we cannot just set the outTypeCombo's selected index to the same as in inTypeCombo's selected index
                //because they may will not contain all of the same items.
                MssMsgUtil.MssMsgType inType = GetMessageTypeFromCombo((ComboBox)sender);
                string inTypeName = MssMsgUtil.MssMsgTypeNames[(int)inType];
                this.outTypeCombo.SelectedIndex = this.outTypeCombo.FindStringExact(inTypeName);
            }
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
                case MssMsgUtil.MssMsgType.PolyAftertouch:
                    {
                        msgMetadata = new PolyAftertouchMsgInfoEntryMetadata();
                        break;
                    }
                case MssMsgUtil.MssMsgType.Generator:
                    {
                        msgMetadata = new GeneratorMsgInfoEntryMetadata();
                        break;
                    }
                case MssMsgUtil.MssMsgType.GeneratorToggle:
                    {
                        msgMetadata = new GeneratorToggleMsgInfoEntryMetadata();
                        break;
                    }
            }

            msgMetadata.Init(this, ioCategory);
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            bool allFieldsAreValid = inMsgMetadata.ValidateEntryField1();
            allFieldsAreValid &= inMsgMetadata.ValidateEntryField2();

            if (this.outSameAsInCheckBox.Checked == false)
            {
                allFieldsAreValid &= outMsgMetadata.ValidateEntryField1();
                allFieldsAreValid &= outMsgMetadata.ValidateEntryField2();
            }

            if (allFieldsAreValid)
            {
                //doesn't work if same as output is on
                mappingEntry.inMssMsgInfo = this.inMsgMetadata.CreateMsgInfo();
                mappingEntry.outMssMsgInfo = this.outMsgMetadata.CreateMsgInfo();
                mappingEntry.overrideDuplicates = this.inOverrideDupsCheckBox.Checked;

                OkBtn.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void inEntryField1TextBox_TextChanged(object sender, EventArgs e)
        {
            if (this.outSameAsInCheckBox.Checked == true)
            {
                this.outEntryField1TextBox.Text = ((TextBox)sender).Text;
            }
        }

        private void inEntryField1Combo_SelectedValueChanged(object sender, EventArgs e)
        {
            if (this.outSameAsInCheckBox.Checked == true)
            {
                this.outEntryField1Combo.SelectedIndex = ((ComboBox)sender).SelectedIndex;
            }
        }

        private void inEntryField2TextBox_TextChanged(object sender, EventArgs e)
        {
            if (this.outSameAsInCheckBox.Checked == true)
            {
                this.outEntryField2TextBox.Text = ((TextBox)sender).Text;
            }
        }

        private void inEntryField2Combo_TextChanged(object sender, EventArgs e)
        {
            if (this.outSameAsInCheckBox.Checked == true)
            {
                this.outEntryField2Combo.SelectedIndex = ((ComboBox)sender).SelectedIndex;
            }
        }   
    }
}

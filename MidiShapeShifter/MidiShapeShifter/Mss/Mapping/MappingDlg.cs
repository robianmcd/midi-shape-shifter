using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using MidiShapeShifter.Mss.Mapping.MssMsgInfoEntryMetadataTypes;

namespace MidiShapeShifter.Mss.Mapping
{
    /// <summary>
    /// MappingDlg is a dialog used to create and edit MssMsg mappings.
    /// </summary>
    public partial class MappingDlg : Form
    {

        protected MssMsgInfoEntryMetadata inMsgMetadata;
        protected MssMsgInfoEntryMetadata outMsgMetadata;

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

            msgMetadata = Factory_MssMsgInfoEntryMetadata.Create(msgType);

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
                mappingEntry.InMssMsgInfo = this.inMsgMetadata.CreateMsgInfo();
                mappingEntry.OutMssMsgInfo = this.outMsgMetadata.CreateMsgInfo();
                mappingEntry.OverrideDuplicates = this.inOverrideDupsCheckBox.Checked;

                this.DialogResult = DialogResult.OK;
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

        private void MappingDlg_Load(object sender, EventArgs e)
        {

        }   
    }
}

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
    ///     MappingDlg is a dialog used to create and edit MssMsg mappings.
    /// </summary>
    public partial class MappingDlg : Form
    {

        protected MssMsgInfoEntryMetadata inMsgMetadata;
        protected MssMsgInfoEntryMetadata outMsgMetadata;

        /// <summary>
        ///     mappingEntry is passed into this dialog through the Init() method. It can be used to determine the 
        ///     default values for all of the entry fields. If the dialog exits with DialogResult.OK then mappingEntry 
        ///     is populated based on data from the entry fields.
        /// </summary>
        public MappingEntry mappingEntry {get; private set;}

        public MappingDlg()
        {
            InitializeComponent();

            this.inTypeCombo.Items.Add(MssMsg.MssMsgTypeNames[(int)MssMsgType.NoteOn]);
            this.inTypeCombo.Items.Add(MssMsg.MssMsgTypeNames[(int)MssMsgType.NoteOff]);
            this.inTypeCombo.Items.Add(MssMsg.MssMsgTypeNames[(int)MssMsgType.CC]);
            this.inTypeCombo.Items.Add(MssMsg.MssMsgTypeNames[(int)MssMsgType.PitchBend]);
            this.inTypeCombo.Items.Add(MssMsg.MssMsgTypeNames[(int)MssMsgType.PolyAftertouch]);
            this.inTypeCombo.Items.Add(MssMsg.MssMsgTypeNames[(int)MssMsgType.ChanAftertouch]);
            this.inTypeCombo.Items.Add(MssMsg.MssMsgTypeNames[(int)MssMsgType.Generator]);
            this.inTypeCombo.SelectedIndex = 0;

            this.outSameAsInCheckBox.Checked = true;
        }

        /// <summary>
        ///     Initializes this MappingDlg. Init() must be called for this MappingDlg to work correctly.
        /// </summary>
        /// <param name="mappingEntry"> MappingEntry instance to use for the mappingEntry member variable.</param>
        /// <param name="useMappingEntryForDefaultValues"> 
        ///     If true, use the data in <paramref name="mappingEntry"/> to populate the entry fields.
        /// </param>
        public void Init(MappingEntry mappingEntry, bool useMappingEntryForDefaultValues)
        {
            this.mappingEntry = mappingEntry;
            
            if (useMappingEntryForDefaultValues == true)
            {
                //TODO: initialize entry fields
            }

        }

        /// <summary>
        ///     Gets the MssMsgType associated with the current selection in the ComboBox specified by 
        ///     <paramref name="combo"/>.
        /// </summary>
        public MssMsgType GetMessageTypeFromCombo(ComboBox combo) 
        {
            return (MssMsgType)MssMsg.MssMsgTypeNames.FindIndex(item => item.Equals(combo.Text));
        }

        private void outSameAsInCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bool enabledStatus = !((CheckBox)sender).Checked;

            outTypeCombo.Enabled = enabledStatus;
            outEntryField2TextBox.Enabled = enabledStatus;
            outEntryField1TextBox.Enabled = enabledStatus;
            outLearnBtn.Enabled = enabledStatus;

            if (((CheckBox)sender).Checked == true)
            {
                //we cannot just set the outTypeCombo's selected index to the same as in inTypeCombo's selected index
                //because they may will not contain all of the same items.
                MssMsgType inType = GetMessageTypeFromCombo(this.inTypeCombo);
                string inTypeName = MssMsg.MssMsgTypeNames[(int)inType];
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
            MsgTypeComboChanged((ComboBox)sender, IoType.Input, ref inMsgMetadata);

            if (this.outSameAsInCheckBox.Checked == true)
            {
                //we cannot just set the outTypeCombo's selected index to the same as in inTypeCombo's selected index
                //because they may will not contain all of the same items.
                MssMsgType inType = GetMessageTypeFromCombo((ComboBox)sender);
                string inTypeName = MssMsg.MssMsgTypeNames[(int)inType];
                this.outTypeCombo.SelectedIndex = this.outTypeCombo.FindStringExact(inTypeName);
            }
        }

        private void outTypeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            MsgTypeComboChanged((ComboBox)sender, IoType.Output, ref outMsgMetadata);
        }

        protected void MsgTypeComboChanged(ComboBox msgTypeCombo, 
                                           IoType ioCategory, 
                                           ref MssMsgInfoEntryMetadata msgMetadata)
        {
            MssMsgType msgType = GetMessageTypeFromCombo(msgTypeCombo);

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

                mappingEntry.CurveShapeInfo = new CurveShapeInfo();

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

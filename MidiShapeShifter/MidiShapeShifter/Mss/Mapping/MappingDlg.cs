using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using MidiShapeShifter.Mss.Mapping.MssMsgRangeEntryMetadataTypes;
using MidiShapeShifter.Mss.MssMsgInfoTypes;
using MidiShapeShifter.Mss.Generator;
using MidiShapeShifter.Mss.Relays;

namespace MidiShapeShifter.Mss.Mapping
{
    /// <summary>
    ///     MappingDlg is a dialog used to create and edit MssMsg mappings.
    /// </summary>
    public partial class MappingDlg : Form
    {
        protected enum LearnMode {In, Out, Off}
        protected LearnMode learnMode;

        public Factory_MssMsgRangeEntryMetadata MsgMetadataFactory;
        public IFactory_MssMsgInfo MsgInfoFactory;

        protected MssMsgRangeEntryMetadata inMsgMetadata;
        protected MssMsgRangeEntryMetadata outMsgMetadata;

        protected IDryMssEventOutputPort dryEventOut;

        protected MssMsg lastMsgReceived;

        /// <summary>
        ///     mappingEntry is passed into this dialog through the Init() method. It can be used to determine the 
        ///     default values for all of the entry fields. If the dialog exits with DialogResult.OK then mappingEntry 
        ///     is populated based on data from the entry fields.
        /// </summary>
        public IMappingEntry mappingEntry {get; private set;}

        public MappingDlg()
        {
            InitializeComponent();

            //Add the types that are valid for input to the input combo.
            foreach(MssMsgType msgType in MssMsgRangeEntryMetadata.VALID_INPUT_TYPES)
            {
                this.inTypeCombo.Items.Add(MssMsg.MssMsgTypeNames[(int)msgType]);
            }

            this.learnMode = LearnMode.Off;
            this.lastMsgReceived = null;
        }

        /// <summary>
        ///     Initializes this MappingDlg. Init() must be called for this MappingDlg to work correctly.
        /// </summary>
        /// <param name="mappingEntry"> MappingEntry instance to use for the mappingEntry member variable.</param>
        /// <param name="useMappingEntryForDefaultValues"> 
        ///     If true, use the data in <paramref name="mappingEntry"/> to populate the entry fields.
        /// </param>
        public void Init(IMappingEntry mappingEntry, 
                         bool useMappingEntryForDefaultValues, 
                         Factory_MssMsgRangeEntryMetadata msgMetadataFactory,
                         IFactory_MssMsgInfo msgInfoFactory,
                         IDryMssEventOutputPort dryEventOut)
        {
            this.mappingEntry = mappingEntry;
            this.MsgMetadataFactory = msgMetadataFactory;
            this.MsgInfoFactory = msgInfoFactory;
            this.dryEventOut = dryEventOut;

            this.dryEventOut.DryMssEventRecieved += 
                new DryMssEventRecievedEventHandler(dryMssEventOutputPort_DryMssEventRecieved);

            if (useMappingEntryForDefaultValues == true)
            {
                //Initializes inMsgMetadata and outMsgMetadata
                this.inTypeCombo.Text = MssMsg.MssMsgTypeNames[(int)this.mappingEntry.InMssMsgRange.MsgType];
                this.outTypeCombo.Text = MssMsg.MssMsgTypeNames[(int)this.mappingEntry.OutMssMsgRange.MsgType];

                this.inMsgMetadata.UseExistingMsgRange(mappingEntry.InMssMsgRange);
                this.outMsgMetadata.UseExistingMsgRange(mappingEntry.OutMssMsgRange);

                this.outSameAsInCheckBox.Checked = 
                    (this.mappingEntry.InMssMsgRange.Equals(this.mappingEntry.OutMssMsgRange));
            }
            else
            {
                //Initializes inMsgMetadata and outMsgMetadata
                this.inTypeCombo.SelectedIndex = 0;

                this.mappingEntry.CurveShapeInfo = new CurveShapeInfo();
                this.mappingEntry.CurveShapeInfo.InitWithDefaultValues();

                this.outSameAsInCheckBox.Checked = true;
            }
        }

        /// <summary>
        ///     Event handler for MssEvents coming
        /// </summary>
        /// <param name="dryMssEvent"></param>
        protected void dryMssEventOutputPort_DryMssEventRecieved(MssEvent dryMssEvent)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<MssEvent>(dryMssEventOutputPort_DryMssEventRecieved), dryMssEvent);
                return;
            }

            MssMsg curMsg = dryMssEvent.mssMsg;

            if (this.learnMode != LearnMode.Off && 
                curMsg.Type != MssMsgType.NoteOff && 
                curMsg.Type != MssMsgType.Generator)
            {
                if (curMsg.Type == MssMsgType.NoteOn)
                {
                    curMsg.Type = MssMsgType.Note;
                }

                ComboBox curTypeCombo = null;

                if (this.learnMode == LearnMode.In)
                {
                    curTypeCombo = this.inTypeCombo;
                }
                else if (this.learnMode == LearnMode.Out)
                {
                    curTypeCombo = this.outTypeCombo;
                }
                else
                {
                    //Unknown learn mode
                    Debug.Assert(false);
                    return;
                }

                string newTypeName = MssMsg.MssMsgTypeNames[(int)curMsg.Type];
                //Ensure that the type of the message recieved can be assigned to the current type combo box.
                if (curTypeCombo.FindStringExact(newTypeName) != -1)
                {
                    //This will trigger the corresponding MsgMetadata to be regenerated.
                    curTypeCombo.Text = newTypeName;
                }
                else
                {
                    return;
                }
                

                MssMsgRangeEntryMetadata curEntryMetadata;
                if(this.learnMode == LearnMode.In)
                {
                    curEntryMetadata = this.inMsgMetadata;
                }
                else //(this.learnMode == LearnMode.Out)
                {
                    curEntryMetadata = this.outMsgMetadata;
                }

                IMssMsgRange learnedRange = new MssMsgRange();
                learnedRange.MsgType = curMsg.Type;

                //If the type is the same as the last message and either data1 or data2 has changed 
                //then treat this as a range of notes.
                if (this.lastMsgReceived != null &&
                    this.lastMsgReceived.Type == curMsg.Type &&
                    (this.lastMsgReceived.Data1 != curMsg.Data1 ||
                    this.lastMsgReceived.Data2 != curMsg.Data2))
                {
                    learnedRange.Data1RangeBottom = Math.Min(this.lastMsgReceived.Data1, curMsg.Data1);
                    learnedRange.Data1RangeTop = Math.Max(this.lastMsgReceived.Data1, curMsg.Data1);
                    learnedRange.Data2RangeBottom = Math.Min(this.lastMsgReceived.Data2, curMsg.Data2);
                    learnedRange.Data2RangeTop = Math.Max(this.lastMsgReceived.Data2, curMsg.Data2);
                }
                else
                {
                    learnedRange.Data1RangeBottom = curMsg.Data1;
                    learnedRange.Data1RangeTop = curMsg.Data1;
                    learnedRange.Data2RangeBottom = curMsg.Data2;
                    learnedRange.Data2RangeTop = curMsg.Data2;
                }

                curEntryMetadata.UseExistingMsgRange(learnedRange);

                this.lastMsgReceived = curMsg;
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
            if (this.outMsgMetadata.EntryField1 != null)
            {
                this.outMsgMetadata.EntryField1.Enabled = enabledStatus;
            }
            if (this.outMsgMetadata.EntryField2 != null)
            {
                this.outMsgMetadata.EntryField2.Enabled = enabledStatus;
            }
            outLearnBtn.Enabled = enabledStatus;

            if (((CheckBox)sender).Checked == true)
            {
                //we cannot just set the outTypeCombo's selected index to the same as in inTypeCombo's selected index
                //because they may not contain all of the same items.
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
            MsgTypeComboChanged((ComboBox)sender, IoType.Input, ref this.inMsgMetadata);

            if (this.outSameAsInCheckBox.Checked == true)
            {
                //we cannot just set the outTypeCombo's selected index to the same as in inTypeCombo's selected index
                //because they may not contain all of the same items.
                MssMsgType inType = GetMessageTypeFromCombo((ComboBox)sender);
                string inTypeName = MssMsg.MssMsgTypeNames[(int)inType];
                this.outTypeCombo.SelectedIndex = this.outTypeCombo.FindStringExact(inTypeName);
            }
        }

        private void outTypeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            MsgTypeComboChanged((ComboBox)sender, IoType.Output, ref this.outMsgMetadata);
        }

        protected void MsgTypeComboChanged(ComboBox msgTypeCombo, 
                                           IoType ioCategory, 
                                           ref MssMsgRangeEntryMetadata msgMetadata)
        {
            MssMsgType msgType = GetMessageTypeFromCombo(msgTypeCombo);

            msgMetadata = MsgMetadataFactory.Create(msgType);

            msgMetadata.AttachToDlg(this, ioCategory);
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
                mappingEntry.InMssMsgRange = this.inMsgMetadata.CreateValidMsgRange();
                mappingEntry.OutMssMsgRange = this.outMsgMetadata.CreateValidMsgRange();
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

        private void inLearnBtn_Click(object sender, EventArgs e)
        {
            if (this.learnMode == LearnMode.In)
            {
                this.learnMode = LearnMode.Off;
                this.lastMsgReceived = null;
                this.inLearnBtn.Text = "Learn";
            }
            else 
            {
                this.learnMode = LearnMode.In;
                this.inLearnBtn.Text = "Stop Learn";
            }

            this.outLearnBtn.Text = "Learn";
        }

        private void outLearnBtn_Click(object sender, EventArgs e)
        {
            if (this.learnMode == LearnMode.Out)
            {
                this.learnMode = LearnMode.Off;
                this.lastMsgReceived = null;
                this.outLearnBtn.Text = "Learn";
            }
            else
            {
                this.learnMode = LearnMode.Out;
                this.outLearnBtn.Text = "Stop Learn";
            }

            this.inLearnBtn.Text = "Learn";
        }

        private void MappingDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.dryEventOut.DryMssEventRecieved -=
                new DryMssEventRecievedEventHandler(dryMssEventOutputPort_DryMssEventRecieved);
        }
   

    }
}

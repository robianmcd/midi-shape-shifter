using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using MidiShapeShifter.Mapping.MssMsgInfoTypes;

namespace MidiShapeShifter.Mapping.MssMsgInfoEntryMetadataTypes
{
    public abstract class MssMsgInfoEntryMetadata
    {
        protected MappingEntry.IO ioCatagory;
        protected MappingDlg mappingDlg;

        protected bool entryField1IsValid = false;
        protected bool entryField2IsValid = false;

        //Contains a list of valid output message types when this class is the input type
        protected List<string> outMssMsgTypeNames = new List<string>();

        //The "Same as Input" checkbox will be enabled iff this class's msg type is selected as input/output and a type
        //in this list is selected as output/input.
        protected List<MssMsgUtil.MssMsgType> sameAsInputCompatibleTypes = new List<MssMsgUtil.MssMsgType>();

        protected Label EntryField1Lbl
        {
            get
            { 
                if (this.ioCatagory == MappingEntry.IO.Input)
                {
                    return this.mappingDlg.inEntryField1Lbl;
                }
                else if (this.ioCatagory == MappingEntry.IO.Output)
                {
                    return this.mappingDlg.outEntryField1Lbl;
                }
                else
                {
                    Debug.Assert(false);
                    return null;
                }
            }
        }

        protected Label EntryField2Lbl
        {
            get
            {
                if (this.ioCatagory == MappingEntry.IO.Input)
                {
                    return this.mappingDlg.inEntryField2Lbl;
                }
                else if (this.ioCatagory == MappingEntry.IO.Output)
                {
                    return this.mappingDlg.outEntryField2Lbl;
                }
                else
                {
                    Debug.Assert(false);
                    return null;
                }
            }
        }

        protected abstract Control EntryField1
        {
            get;
        }

        protected abstract Control EntryField2
        {
            get;
        }

        //initializes the visable controls on the mapping dialog and the member variables entryFields and ioCategory
        public void Init(MappingDlg mappingDlg, MappingEntry.IO io)
        {
            this.ioCatagory = io;
            this.mappingDlg = mappingDlg;

            SetMappingDlgEntryFieldsDefaultProperties();
            SetMappingDlgEntryFieldCustomProperties();

            InitSameAsInputCompatibleTypes();

            //This variable will contail the type combo box that did not trigger the creation of this class
            ComboBox otherTypeCombo;

            if (io == MappingEntry.IO.Input)
            {
                InitOutMssMsgTypeNames();
                mappingDlg.outTypeCombo.Items.Clear();
                mappingDlg.outTypeCombo.Items.AddRange(outMssMsgTypeNames.ToArray());
                mappingDlg.outTypeCombo.SelectedIndex = 0;

                otherTypeCombo = mappingDlg.outTypeCombo;
            }
            else if (io == MappingEntry.IO.Output)
            {
                otherTypeCombo = mappingDlg.inTypeCombo;
            } 
            else
            {
                //Unknown IO type
                Debug.Assert(false);
                return;
            }

            MssMsgUtil.MssMsgType otherMsgType = mappingDlg.GetMessageTypeFromCombo(otherTypeCombo);
            if (sameAsInputCompatibleTypes.Contains(otherMsgType))
            {
                mappingDlg.outSameAsInCheckBox.Enabled = true;
            }
            else
            {
                mappingDlg.outSameAsInCheckBox.Checked = false;
                mappingDlg.outSameAsInCheckBox.Enabled = false;
                
            }
        }

        protected void SetMappingDlgEntryFieldsDefaultProperties()
        {
            Label EntryField1Lbl;
            Label EntryField2Lbl;
            TextBox EntryField1TextBox;
            TextBox EntryField2TextBox;
            ComboBox EntryField1Combo;
            ComboBox EntryField2Combo;

            if (this.ioCatagory == MappingEntry.IO.Input)
            {
                EntryField1Lbl = this.mappingDlg.inEntryField1Lbl;
                EntryField2Lbl = this.mappingDlg.inEntryField2Lbl;
                EntryField1TextBox = this.mappingDlg.inEntryField1TextBox;
                EntryField2TextBox = this.mappingDlg.inEntryField2TextBox;
                EntryField1Combo = this.mappingDlg.inEntryField1Combo;
                EntryField2Combo = this.mappingDlg.inEntryField2Combo;
            }
            else if (this.ioCatagory == MappingEntry.IO.Output)
            {
                EntryField1Lbl = this.mappingDlg.outEntryField1Lbl;
                EntryField2Lbl = this.mappingDlg.outEntryField2Lbl;
                EntryField1TextBox = this.mappingDlg.outEntryField1TextBox;
                EntryField2TextBox = this.mappingDlg.outEntryField2TextBox;
                EntryField1Combo = this.mappingDlg.outEntryField1Combo;
                EntryField2Combo = this.mappingDlg.outEntryField2Combo;
            }
            else
            {
                Debug.Assert(false);
                return;
            }

            EntryField1Lbl.Text = "";
            EntryField1Lbl.Visible = false;

            EntryField2Lbl.Text = "";
            EntryField2Lbl.Visible = false;

            EntryField1TextBox.Text = "";
            //EntryField1TextBox.Enabled = true;
            EntryField1TextBox.Visible = false;

            EntryField2TextBox.Text = "";
            //EntryField2TextBox.Enabled = true;
            EntryField2TextBox.Visible = false;

            //EntryField1Combo.Enabled = true;
            EntryField1Combo.Items.Clear();
            EntryField1Combo.Visible = false;

            //EntryField2Combo.Enabled = true;
            EntryField2Combo.Items.Clear();
            EntryField2Combo.Visible = false;
        }

        protected virtual void InitOutMssMsgTypeNames()
        {
            this.outMssMsgTypeNames.Add(MssMsgUtil.MssMsgTypeNames[(int)MssMsgUtil.MssMsgType.NoteOn]);
            this.outMssMsgTypeNames.Add(MssMsgUtil.MssMsgTypeNames[(int)MssMsgUtil.MssMsgType.NoteOff]);
            this.outMssMsgTypeNames.Add(MssMsgUtil.MssMsgTypeNames[(int)MssMsgUtil.MssMsgType.CC]);
            this.outMssMsgTypeNames.Add(MssMsgUtil.MssMsgTypeNames[(int)MssMsgUtil.MssMsgType.PitchBend]);
            this.outMssMsgTypeNames.Add(MssMsgUtil.MssMsgTypeNames[(int)MssMsgUtil.MssMsgType.PolyAftertouch]);
            this.outMssMsgTypeNames.Add(MssMsgUtil.MssMsgTypeNames[(int)MssMsgUtil.MssMsgType.ChanAftertouch]);
            this.outMssMsgTypeNames.Add(MssMsgUtil.MssMsgTypeNames[(int)MssMsgUtil.MssMsgType.GeneratorToggle]);
        }

        protected virtual void InitSameAsInputCompatibleTypes()
        { 

        }

        //set the properties of all the the controls in the mapping dialog whose properties should differ 
        //from the default properties set in SetMappingDlgEntryFieldsDefaultProperties().
        protected abstract void SetMappingDlgEntryFieldCustomProperties();

        //this can be left not overridden if entry 1 is a combo box and needs no validation.
        public bool ValidateEntryField1() 
        {
            string errorMsg;
            this.entryField1IsValid = IsEntryField1Valid(out errorMsg);
        
            mappingDlg.errorProvider.SetError(EntryField1, errorMsg);

            return this.entryField1IsValid;
        }

        public virtual bool IsEntryField1Valid(out string errorMessage)
        {
            errorMessage = "";
            return true;
        }

        //this can be left not overridden if entry 2 is a combo box and needs no validation.
        public bool ValidateEntryField2()
        {
            string errorMsg;
            this.entryField2IsValid = IsEntryField2Valid(out errorMsg);

            mappingDlg.errorProvider.SetError(EntryField2, errorMsg);

            return this.entryField2IsValid;
        }

        public virtual bool IsEntryField2Valid(out string errorMessage)
        {
            errorMessage = "";
            return true;
        }

        //Precondition: The entry fields must have valid content (but ValidateEntryField#() does not need to have been 
        //called)
        public MssMsgInfo CreateMsgInfo()
        {
            if (this.entryField1IsValid == false)
            {
                ValidateEntryField1();
            }

            if (this.entryField2IsValid == false)
            {
                ValidateEntryField2();
            }

            if (this.entryField1IsValid == true && this.entryField2IsValid == true)
            {
                return CreateMsgInfoFromValidatedFields();
            }
            else
            {
                //The precondition was not met
                Debug.Assert(false);
                return null;
            }
        }

        //Precondition: ValidateEntryField#() must have been called and returned true for all fields
        protected abstract MssMsgInfo CreateMsgInfoFromValidatedFields();
    }
}

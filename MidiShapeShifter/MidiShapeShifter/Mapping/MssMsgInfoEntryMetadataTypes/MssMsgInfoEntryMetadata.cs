using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using MidiShapeShifter.Mapping.MssMsgInfoTypes;

namespace MidiShapeShifter.Mapping.MssMsgInfoEntryMetadataTypes
{
    abstract class MssMsgInfoEntryMetadata
    {
        protected MappingEntry.IO ioCatagory;
        protected EntryFields entryFields;

        //initializes the visable controls on the mapping dialog and the member variables entryFields and ioCategory
        public void Init(MappingDlg mappingDlg, MappingEntry.IO io)
        {
            this.ioCatagory = io;
            InitEntryFields(mappingDlg);

            SetMappingDlgEntryFieldCustomProperties();
        }

        //Initializes the member variable entryFields
        protected void InitEntryFields(MappingDlg mappingDlg)
        {
            this.entryFields = new EntryFields();
            if (this.ioCatagory == MappingEntry.IO.Input)
            {
                this.entryFields.EntryField1Lbl = mappingDlg.inEntryField1Lbl;
                this.entryFields.EntryField2Lbl = mappingDlg.inEntryField2Lbl;
                this.entryFields.EntryField1TextBox = mappingDlg.inEntryField1TextBox;
                this.entryFields.EntryField2TextBox = mappingDlg.inEntryField2TextBox;
                this.entryFields.EntryField1Combo = mappingDlg.inEntryField1Combo;
                this.entryFields.EntryField2Combo = mappingDlg.inEntryField2Combo;
            }
            else if (this.ioCatagory == MappingEntry.IO.Output)
            {
                this.entryFields.EntryField1Lbl = mappingDlg.outEntryField1Lbl;
                this.entryFields.EntryField2Lbl = mappingDlg.outEntryField2Lbl;
                this.entryFields.EntryField1TextBox = mappingDlg.outEntryField1TextBox;
                this.entryFields.EntryField2TextBox = mappingDlg.outEntryField2TextBox;
                this.entryFields.EntryField1Combo = mappingDlg.outEntryField1Combo;
                this.entryFields.EntryField2Combo = mappingDlg.outEntryField2Combo;
            }
            else
            {
                Debug.Assert(false);
            }
        }

        protected void SetMappingDlgEntryFieldsDefaultProperties()
        { 
            this.entryFields.EntryField1Lbl.Text = "";
            this.entryFields.EntryField1Lbl.Visible = false;

            this.entryFields.EntryField2Lbl.Text = "";
            this.entryFields.EntryField2Lbl.Visible = false;

            this.entryFields.EntryField1TextBox.Text = "";
            this.entryFields.EntryField1TextBox.Enabled = true;
            this.entryFields.EntryField1TextBox.Visible = false;

            this.entryFields.EntryField2TextBox.Text = "";
            this.entryFields.EntryField2TextBox.Enabled = true;
            this.entryFields.EntryField2TextBox.Visible = false;

            this.entryFields.EntryField1Combo.Enabled = true;
            this.entryFields.EntryField1Combo.Items.Clear();
            this.entryFields.EntryField1Combo.Visible = false;

            this.entryFields.EntryField2Combo.Enabled = true;
            this.entryFields.EntryField2Combo.Items.Clear();
            this.entryFields.EntryField2Combo.Visible = false;
        }

        //set the properties of all the the controls in the entryFields member variable whose properties should differ 
        //from the default properties set in SetMappingDlgEntryFieldsDefaultProperties().
        protected abstract void SetMappingDlgEntryFieldCustomProperties();

        //this can be left not overridden if entry 1 is a combo box and needs no validation.
        public virtual bool ValidateEntryField1() 
        {
            return true;
        }

        //this can be left not overridden if entry 2 is a combo box and needs no validation.
        public virtual bool ValidateEntryField2()
        {
            return true;
        }

        public abstract MssMsgInfo CreateMsgInfo();
        
        public class EntryFields
        {
            public Label EntryField1Lbl;
            public Label EntryField2Lbl;
            public TextBox EntryField1TextBox;
            public TextBox EntryField2TextBox;
            public ComboBox EntryField1Combo;
            public ComboBox EntryField2Combo;
        }
    }
}

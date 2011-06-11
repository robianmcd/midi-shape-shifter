using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using MidiShapeShifter.Mss.Mapping.MssMsgInfoTypes;

namespace MidiShapeShifter.Mss.Mapping.MssMsgInfoEntryMetadataTypes
{
    /// <summary>
    ///     This class is tightly coupled with the class MappingDlg. In the mapping dialog the user can select an MSS 
    ///     message type for input and output. The users selection for these will greatly affect how the rest of the 
    ///     information is entered and stored. This class and the ones implimenting it are responsible for implimenting
    ///     the logic that is specific to the selection of input and output type. Since the user can make a seperate 
    ///     selection for input and output type, an instance of this calss is only associated with either input or 
    ///     output.
    /// </summary>
    public abstract class MssMsgInfoEntryMetadata
    {
        /// <summary>
        ///     Specifies wheather this is associated with the input or output entry fields.
        /// </summary>
        protected IoType ioCatagory;

        /// <summary>
        ///     The MappingDlg that this is associated with.
        /// </summary>
        protected MappingDlg mappingDlg;


        protected bool entryField1IsValid = false;
        protected bool entryField2IsValid = false;

        //Contains a list of valid output message types when this class is the input type
        protected List<string> outMssMsgTypeNames = new List<string>();

        //The "Same as Input" checkbox will be enabled iff this class's msg type is selected as input/output and a type
        //in this list is selected as output/input.
        protected List<MssMsgUtil.MssMsgType> sameAsInputCompatibleTypes = new List<MssMsgUtil.MssMsgType>();

        /// <summary>
        ///     The label associated with the first entry field.
        /// </summary>
        protected Label EntryField1Lbl
        {
            get
            { 
                if (this.ioCatagory == IoType.Input)
                {
                    return this.mappingDlg.inEntryField1Lbl;
                }
                else if (this.ioCatagory == IoType.Output)
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

        /// <summary>
        ///     The label associated with the second entry field.
        /// </summary>
        protected Label EntryField2Lbl
        {
            get
            {
                if (this.ioCatagory == IoType.Input)
                {
                    return this.mappingDlg.inEntryField2Lbl;
                }
                else if (this.ioCatagory == IoType.Output)
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

        /// <summary>
        ///     The control that is the first entry field.
        /// </summary>
        protected abstract Control EntryField1
        {
            get;
        }

        /// <summary>
        ///     The control that is the second entry field.
        /// </summary>
        protected abstract Control EntryField2
        {
            get;
        }

        /// <summary>
        ///     Initializes this MssMsgInfoEntryMetadata and sets the default properties for controls on the mapping 
        ///     dialog
        /// </summary>
        /// <param name="mappingDlg">The mapping dialog this is associated with</param>
        /// <param name="io">Specifies wheather this associated with the input or output entry fields.</param>
        public void Init(MappingDlg mappingDlg, IoType io)
        {
            this.ioCatagory = io;
            this.mappingDlg = mappingDlg;

            SetMappingDlgEntryFieldsDefaultProperties();
            SetMappingDlgEntryFieldCustomProperties();

            InitSameAsInputCompatibleTypes();
            InitOutMssMsgTypeNames();


            //Contains the type combo box that did not trigger the creation of this class
            ComboBox otherTypeCombo;

            //For each MSS message type selected for input there are only a subset of MSS message types that are 
            //considered valid output. When the input type changes then the output combo box must be repopulated
            if (io == IoType.Input)
            {
                otherTypeCombo = mappingDlg.outTypeCombo;

                otherTypeCombo.Items.Clear();
                otherTypeCombo.Items.AddRange(outMssMsgTypeNames.ToArray());
                //This will cause the output MssMsgInfoEntryMetadata to be created.
                otherTypeCombo.SelectedIndex = 0;
            }
            else if (io == IoType.Output)
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
            //Checks if it makes sence to use the values from the input entry fields in the output entry fields
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

        /// <summary>
        ///     Sets the entry fields so that they are all blank and invisable.
        /// </summary>
        protected void SetMappingDlgEntryFieldsDefaultProperties()
        {
            Label EntryField1Lbl;
            Label EntryField2Lbl;
            TextBox EntryField1TextBox;
            TextBox EntryField2TextBox;
            ComboBox EntryField1Combo;
            ComboBox EntryField2Combo;

            if (this.ioCatagory == IoType.Input)
            {
                EntryField1Lbl = this.mappingDlg.inEntryField1Lbl;
                EntryField2Lbl = this.mappingDlg.inEntryField2Lbl;
                EntryField1TextBox = this.mappingDlg.inEntryField1TextBox;
                EntryField2TextBox = this.mappingDlg.inEntryField2TextBox;
                EntryField1Combo = this.mappingDlg.inEntryField1Combo;
                EntryField2Combo = this.mappingDlg.inEntryField2Combo;
            }
            else if (this.ioCatagory == IoType.Output)
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
            EntryField1TextBox.Visible = false;

            EntryField2TextBox.Text = "";
            EntryField2TextBox.Visible = false;

            EntryField1Combo.Items.Clear();
            EntryField1Combo.Visible = false;

            EntryField2Combo.Items.Clear();
            EntryField2Combo.Visible = false;
        }

        /// <summary>
        ///     Initializes outMssMsgTypeNames so that it contains all MSS message types that are valid for output.
        /// </summary>
        protected virtual void InitOutMssMsgTypeNames()
        {
            //Default set of valid output types.
            this.outMssMsgTypeNames.Add(MssMsgUtil.MssMsgTypeNames[(int)MssMsgUtil.MssMsgType.NoteOn]);
            this.outMssMsgTypeNames.Add(MssMsgUtil.MssMsgTypeNames[(int)MssMsgUtil.MssMsgType.NoteOff]);
            this.outMssMsgTypeNames.Add(MssMsgUtil.MssMsgTypeNames[(int)MssMsgUtil.MssMsgType.CC]);
            this.outMssMsgTypeNames.Add(MssMsgUtil.MssMsgTypeNames[(int)MssMsgUtil.MssMsgType.PitchBend]);
            this.outMssMsgTypeNames.Add(MssMsgUtil.MssMsgTypeNames[(int)MssMsgUtil.MssMsgType.PolyAftertouch]);
            this.outMssMsgTypeNames.Add(MssMsgUtil.MssMsgTypeNames[(int)MssMsgUtil.MssMsgType.ChanAftertouch]);
            this.outMssMsgTypeNames.Add(MssMsgUtil.MssMsgTypeNames[(int)MssMsgUtil.MssMsgType.GeneratorToggle]);
        }

        /// <summary>
        ///     Initializes sameAsInputCompatibleTypes so that it contains all MSS message types that use the same 
        ///     entry fields as the MSS message type associated with the class that impliments this.
        /// </summary>
        protected abstract void InitSameAsInputCompatibleTypes();

        /// <summary>
        ///     Sets the properties of all the the controls in the mapping dialog whose properties should differ
        ///     from the default properties set in SetMappingDlgEntryFieldsDefaultProperties().
        /// </summary>
        protected abstract void SetMappingDlgEntryFieldCustomProperties();

        /// <summary>
        ///     Determines wheather entry field 1 contains valid user input. If it does not then the mapping dialog's 
        ///     error provider will be alerted.
        /// </summary>
        /// <returns>True if entry field 1 contains valid user input.</returns>
        public bool ValidateEntryField1() 
        {
            string errorMsg;
            this.entryField1IsValid = StoreContentIfEntryField1IsValid(out errorMsg);
        
            mappingDlg.errorProvider.SetError(EntryField1, errorMsg);

            return this.entryField1IsValid;
        }

        /// <summary>
        ///     Stores the relavent content from entry field 1 if it contains valid user input. 
        /// </summary>
        /// <remarks>
        ///     This method should be overridden in a child class unless the child class does not use entry field 1.
        /// </remarks>
        /// <param name="errorMessage"> 
        ///     Contains the empty string if entry field 1 contains valid user input. Othewise errorMessage contains a 
        ///     message describing why the user input was not valid.
        /// </param>
        /// <returns>True if entry field 1 contains valid user input</returns>
        public virtual bool StoreContentIfEntryField1IsValid(out string errorMessage)
        {
            errorMessage = "";
            return true;
        }

        /// <summary>
        ///     Determines wheather entry field 2 contains valid user input. If it does not then the mapping dialog's 
        ///     error provider will be alerted.
        /// </summary>
        /// <returns>True if entry field 2 contains valid user input.</returns>
        public bool ValidateEntryField2()
        {
            string errorMsg;
            this.entryField2IsValid = StoreContentIfEntryField2IsValid(out errorMsg);

            mappingDlg.errorProvider.SetError(EntryField2, errorMsg);

            return this.entryField2IsValid;
        }

        /// <summary>
        ///     Stores the relavent content from entry field 2 if it contains valid user input. 
        /// </summary>
        /// <remarks>
        ///     This method should be overridden in a child class unless the child class does not use entry field 2.
        /// </remarks>
        /// <param name="errorMessage"> 
        ///     Contains the empty string if entry field 2 contains valid user input. Othewise errorMessage contains a 
        ///     message describing why the user input was not valid.
        /// </param>
        /// <returns>True if entry field 1 contains valid user input</returns>
        public virtual bool StoreContentIfEntryField2IsValid(out string errorMessage)
        {
            errorMessage = "";
            return true;
        }

        /// <summary>
        ///     Creates an instance of MssMsgInfo, populating it with information from the mapping dialog.
        /// </summary>
        /// <remarks>Precondition: The entry fields must contain valid user input.</remarks>
        /// <returns>The newly created MssMsgInfo instance.</returns>
        public MssMsgInfo CreateMsgInfo()
        {
            if (this.entryField1IsValid == false)
            {
                string dummyErrMsg;
                this.entryField1IsValid = StoreContentIfEntryField1IsValid(out dummyErrMsg);

                //The precondition has been violated
                Debug.Assert(this.entryField1IsValid);
            }

            if (this.entryField2IsValid == false)
            {
                string dummyErrMsg;
                this.entryField2IsValid = StoreContentIfEntryField2IsValid(out dummyErrMsg);

                //The precondition has been violated
                Debug.Assert(this.entryField2IsValid);
            }

            if (this.entryField1IsValid == true && this.entryField2IsValid == true)
            {
                return CreateMsgInfoFromStoredContent();
            }
            else
            {
                //The precondition was not met. We don't need to assert false here though because one of the previous
                //Asserts will have already been hit.
                return null;
            }
        }

        /// <summary>
        ///     Creates an instance of MssMsgInfo, populating it with information that was stored when 
        ///     StoreContentIfEntryField#IsValid() was called.
        /// </summary>
        /// <remarks>
        ///     Precondition: StoreContentIfEntryField#IsValid() must have been called and returned true for all fields.
        /// </remarks>
        /// <returns>The newly created MssMsgInfo instance.</returns>
        protected abstract MssMsgInfo CreateMsgInfoFromStoredContent();
    }
}

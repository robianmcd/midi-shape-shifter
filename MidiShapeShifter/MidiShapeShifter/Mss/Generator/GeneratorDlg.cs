using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace MidiShapeShifter.Mss.Generator
{
    /// <summary>
    /// Dialog for creating or editing a GeneratorMappingEntry. This dialog does not actually 
    /// create/edit a GeneratorMappingEntry but instead creates/edits a GenEntryConfigInfo that 
    /// will be used to create/edit a GeneratorMappingEntry.
    /// </summary>
    public partial class GeneratorDlg : Form
    {
        /// <summary>
        /// The GenEntryConfigInfo resulting form the users input in this dialog. This should 
        /// be retrieved after this dialog exits with DialogResult.OK. 
        /// </summary>
        public GenEntryConfigInfo GenInfoResult { get; private set; }

        public GeneratorDlg()
        {
            InitializeComponent();

            GenInfoResult = new GenEntryConfigInfo();

            //Populate the dropdown meunes on the dialog

            foreach (string PeriodTypeName in GenEntryConfigInfo.GenPeriodTypeNames)
            {
                this.periodTypeCombo.Items.Add(PeriodTypeName);
            }

            foreach (string BarsPeriodName in GenEntryConfigInfo.GenBarsPeriodNames)
            {
                this.periodCombo.Items.Add(BarsPeriodName);
            }
        }

        //Initialized the entry fields on the dialog. This method must be called.
        public void Init(GenEntryConfigInfo genInfo)
        {
            this.genNameTextBox.Text = genInfo.Name;
            this.periodTypeCombo.Text = GenEntryConfigInfo.GenPeriodTypeNames[(int)genInfo.PeriodType];
            this.periodTextBox.Text = genInfo.TimePeriodInMs.ToString();
            this.periodCombo.Text = GenEntryConfigInfo.GenBarsPeriodNames[(int)genInfo.BarsPeriod];
            this.loopCheckBox.Checked = genInfo.Loop;
            this.enabledCheckBox.Checked = genInfo.Enabled;
        }

        /// <summary>
        /// Retrieves the GenPeriodType represented by the selection in the periodTypeCombo
        /// </summary>
        protected GenPeriodType GetSelectedPeriodType()
        {
            return (GenPeriodType)GenEntryConfigInfo.GenPeriodTypeNames.FindIndex(
                periodTypeName => periodTypeName.Equals(this.periodTypeCombo.Text));
        }

        /// <summary>
        /// Retrieves the GenBarsPeriod represented by the selection in the periodCombo
        /// </summary>
        protected GenBarsPeriod GetSelectedBarsPeriod()
        {
            return (GenBarsPeriod)GenEntryConfigInfo.GenBarsPeriodNames.FindIndex(
                BarsPeriodName => BarsPeriodName.Equals(this.periodCombo.Text));
        }

        /// <summary>
        /// Checks if the content of the periodTextBox is a valid period size and 
        /// displays/clears the error message in the errorProvider
        /// </summary>
        /// <returns>True if the content is valid. False otherwise.</returns>
        protected bool ValidatePeriodTextBox()
        {
            int periodSize;
            if (int.TryParse(this.periodTextBox.Text.Trim(), out periodSize) && periodSize > 0)
            {
                this.errorProvider.SetError(this.periodTextBox, "");
                return true;
            }
            else
            {
                this.errorProvider.SetError(this.periodTextBox, "Invalid period size.");
                return false;
            }
        }

        /// <summary>
        /// Different controls will be used to enter the period size depending on the selection
        /// in the periodTypeCombo. This method catches the SelectedIndexChanged event for the
        /// periodTypeCombo and enables the appropriate controls for entering the period size.
        /// </summary>
        private void periodTypeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            GenPeriodType SelectedPeriodType = GetSelectedPeriodType();
            if (SelectedPeriodType == GenPeriodType.BeatSynced) 
            {
                this.periodCombo.Visible = true;
                this.periodTextBox.Visible = false;
                this.periodLbl.Text = "Period";
            }
            else if (SelectedPeriodType == GenPeriodType.Time)
            {
                this.periodCombo.Visible = false;
                this.periodTextBox.Visible = true;
                this.periodLbl.Text = "Period(ms)";
            }
            else
            {
                //Unknown period type
                Debug.Assert(false);
            }
        }

        private void periodTextBox_Validating(object sender, CancelEventArgs e)
        {
            ValidatePeriodTextBox();
        }

        /// <summary>
        /// Ensures that the user input is valid and if it is, initializes GenInfoResult and 
        /// closes the dialog with DialogResult.OK.
        /// </summary>
        private void OkBtn_Click(object sender, EventArgs e)
        {
            GenPeriodType SelectedPeriodType = GetSelectedPeriodType();

            if (ValidatePeriodTextBox() == false)
            {
                if (SelectedPeriodType == GenPeriodType.Time)
                {
                    return;
                }
                else
                {
                    this.GenInfoResult.TimePeriodInMs = GenEntryConfigInfo.DEFAULT_TIME_PERIOD;
                }
            }
            else
            {
                int periodSize;
                //this should always be able to be parsed as an int because ValidatePeriodTextBox() returned true.
                int.TryParse(this.periodTextBox.Text.Trim(), out periodSize);
                this.GenInfoResult.TimePeriodInMs = periodSize;
            }

            this.GenInfoResult.Name = this.genNameTextBox.Text;
            this.GenInfoResult.PeriodType = SelectedPeriodType;
            this.GenInfoResult.BarsPeriod = GetSelectedBarsPeriod();
            this.GenInfoResult.Loop = this.loopCheckBox.Checked;
            this.GenInfoResult.Enabled = this.enabledCheckBox.Checked;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}

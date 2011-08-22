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
    // TODO: comment this calss
    public partial class GeneratorDlg : Form
    {
        public GeneratorMappingEntryInfo GenInfoResult { get; private set; }

        public GeneratorDlg()
        {
            InitializeComponent();

            GenInfoResult = new GeneratorMappingEntryInfo();

            foreach (string PeriodTypeName in GeneratorMappingEntryInfo.GenPeriodTypeNames)
            {
                this.periodTypeCombo.Items.Add(PeriodTypeName);
            }

            foreach (string BarsPeriodName in GeneratorMappingEntryInfo.GenBarsPeriodNames)
            {
                this.periodCombo.Items.Add(BarsPeriodName);
            }
        }

        public void Init(GeneratorMappingEntryInfo genInfo)
        {
            this.GenInfoResult.Id = genInfo.Id;
            this.genNameTextBox.Text = genInfo.Name;
            this.periodTypeCombo.Text = GeneratorMappingEntryInfo.GenPeriodTypeNames[(int)genInfo.PeriodType];
            this.periodTextBox.Text = genInfo.TimePeriod.ToString();
            this.periodCombo.Text = GeneratorMappingEntryInfo.GenBarsPeriodNames[(int)genInfo.BarsPeriod];
            this.loopCheckBox.Checked = genInfo.Loop;
            this.generatingCheckBox.Checked = genInfo.IsGenerating;
        }

        protected GenPeriodType GetSelectedPeriodType()
        {
            return (GenPeriodType)GeneratorMappingEntryInfo.GenPeriodTypeNames.FindIndex(
                periodTypeName => periodTypeName.Equals(this.periodTypeCombo.Text));
        }

        protected GenBarsPeriod GetSelectedBarsPeriod()
        {
            return (GenBarsPeriod)GeneratorMappingEntryInfo.GenBarsPeriodNames.FindIndex(
                BarsPeriodName => BarsPeriodName.Equals(this.periodCombo.Text));
        }

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

        private void periodTypeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            GenPeriodType SelectedPeriodType = GetSelectedPeriodType();
            if (SelectedPeriodType == GenPeriodType.Beats) 
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
                    this.GenInfoResult.TimePeriod = GeneratorMappingEntryInfo.DEFAULT_TIME_PERIOD;
                }
            }
            else
            {
                int periodSize;
                //this should always be able to be parsed as an int because ValidatePeriodTextBox() returned true.
                int.TryParse(this.periodTextBox.Text.Trim(), out periodSize);
                this.GenInfoResult.TimePeriod = periodSize;
            }

            this.GenInfoResult.Name = this.genNameTextBox.Text;
            this.GenInfoResult.PeriodType = SelectedPeriodType;
            this.GenInfoResult.BarsPeriod = GetSelectedBarsPeriod();
            this.GenInfoResult.Loop = this.loopCheckBox.Checked;
            this.GenInfoResult.IsGenerating = this.generatingCheckBox.Checked;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}

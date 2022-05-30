using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace MidiShapeShifter.Mss.Parameters
{

    public partial class ParameterEditor : Form
    {
        protected const string ERR_NOT_INT = "All values must be integers for this parameter type.";
        protected const string ERR_MIN_GREATER_THAN_MAX = "The minimum value must be smaller then the maximum value.";
        protected const string ERR_VALUE_OUTSIDE_RANGE = "The parameter value must be between the minimum and maximum values.";
        protected const string ERR_NOT_NUMBER = "This value must be a valid number.";

        protected MssParamInfo inputParamInfo;
        public MssParamInfo resultParamInfo;

        public ParameterEditor()
        {
            InitializeComponent();
        }

        public void Init(MssParamInfo inputParamInfo)
        {
            this.inputParamInfo = inputParamInfo.Clone();

            this.paramTypeCombo.Items.Clear();
            foreach (string parameterType in MssParamInfo.MssParamTypeNameList)
            {
                this.paramTypeCombo.Items.Add(parameterType);
            }

            ConfigureFieldsFromInputParamInfo();
        }

        protected void ConfigureFieldsFromInputParamInfo()
        {
            //Configure name text box
            this.paramNameTextBox.Text = this.inputParamInfo.Name;

            //Configure Parameter Type Combo
            this.paramTypeCombo.SelectedIndex = (int)this.inputParamInfo.paramType;

            //Configure min/max fields
            this.minParamValueLabel.Visible = this.inputParamInfo.allowUserToEditMaxMin;
            this.minParamValueTextBox.Visible = this.inputParamInfo.allowUserToEditMaxMin;
            this.maxParamValueLabel.Visible = this.inputParamInfo.allowUserToEditMaxMin;
            this.maxParamValueTextBox.Visible = this.inputParamInfo.allowUserToEditMaxMin;

            if (this.inputParamInfo.allowUserToEditMaxMin)
            {
                this.minParamValueTextBox.Text = this.inputParamInfo.MinValue.ToString();
                this.maxParamValueTextBox.Text = this.inputParamInfo.MaxValue.ToString();
            }

            //Configure value fields.
            if (this.inputParamInfo.methodOfValueInput == ValueInputType.Number ||
                this.inputParamInfo.methodOfValueInput == ValueInputType.Integer)
            {
                this.paramValueCombo.Visible = false;
                this.paramValueTextBox.Visible = true;
                this.paramValueTextBox.Text = this.inputParamInfo.GetValueAsString();
            }
            else if (this.inputParamInfo.methodOfValueInput == ValueInputType.Selection)
            {
                this.paramValueTextBox.Visible = false;
                this.paramValueCombo.Visible = true;
                this.paramValueCombo.Items.Clear();
                foreach (string valueName in this.inputParamInfo.ValueNameList)
                {
                    this.paramValueCombo.Items.Add(valueName);
                }

                this.paramValueCombo.SelectedIndex = (int)this.inputParamInfo.GetValue();
            }
            else
            {
                //Unknown ValueInputType
                Debug.Assert(false);
            }
        }

        protected bool AttemptToSetResultParamInfo()
        {
            bool resultParamInfoSetSuccessfully = true;

            MssParamType paramType = (MssParamType)this.paramTypeCombo.SelectedIndex;
            string paramName = this.paramNameTextBox.Text;
            this.resultParamInfo = Factory_MssParamInfo.Create(paramType, paramName);


            //Ensure that the min field is valid.
            double minValue = -1;
            bool minIsValidNumber = true;
            if (this.inputParamInfo.methodOfValueInput == ValueInputType.Number)
            {
                if (double.TryParse(this.minParamValueTextBox.Text, out minValue) == false)
                {
                    this.errorProvider.SetError(this.minParamValueTextBox, ERR_NOT_NUMBER);
                    resultParamInfoSetSuccessfully = false;
                    minIsValidNumber = false;
                }
                else
                {
                    this.errorProvider.SetError(this.minParamValueTextBox, "");
                }
            }
            else if (this.inputParamInfo.methodOfValueInput == ValueInputType.Integer)
            {
                int minValueAsInt;
                if (int.TryParse(this.minParamValueTextBox.Text, out minValueAsInt) == false)
                {
                    this.errorProvider.SetError(this.minParamValueTextBox, ERR_NOT_INT);
                    resultParamInfoSetSuccessfully = false;
                    minIsValidNumber = false;
                }
                else
                {
                    this.errorProvider.SetError(this.minParamValueTextBox, "");
                    minValue = minValueAsInt;
                }
            }
            else if (this.inputParamInfo.methodOfValueInput == ValueInputType.Selection)
            {
                minValue = this.inputParamInfo.MinValue;
            }
            else
            {
                //Unknown value input type.
                Debug.Assert(false);
            }



            //Ensure that the max field is valid.
            double maxValue = -1;
            bool maxIsValidNumber = true;
            if (this.inputParamInfo.methodOfValueInput == ValueInputType.Number)
            {
                if (double.TryParse(this.maxParamValueTextBox.Text, out maxValue) == false)
                {
                    this.errorProvider.SetError(this.maxParamValueTextBox, ERR_NOT_NUMBER);
                    resultParamInfoSetSuccessfully = false;
                    maxIsValidNumber = false;
                }
                else
                {
                    this.errorProvider.SetError(this.maxParamValueTextBox, "");
                }
            }
            else if (this.inputParamInfo.methodOfValueInput == ValueInputType.Integer)
            {
                int maxValueAsInt;
                if (int.TryParse(this.maxParamValueTextBox.Text, out maxValueAsInt) == false)
                {
                    this.errorProvider.SetError(this.maxParamValueTextBox, ERR_NOT_INT);
                    resultParamInfoSetSuccessfully = false;
                    maxIsValidNumber = false;
                }
                else
                {
                    this.errorProvider.SetError(this.maxParamValueTextBox, "");
                    maxValue = maxValueAsInt;
                }
            }
            else if (this.inputParamInfo.methodOfValueInput == ValueInputType.Selection)
            {
                maxValue = this.inputParamInfo.MaxValue;
            }
            else
            {
                //Unknown value input type.
                Debug.Assert(false);
            }


            //Ensure that the min value is not greater then the max value.
            bool maxMinMakeValidRange = false;
            if (maxIsValidNumber && minIsValidNumber)
            {
                if (minValue >= maxValue)
                {
                    this.errorProvider.SetError(this.minParamValueTextBox, ERR_MIN_GREATER_THAN_MAX);
                    this.errorProvider.SetError(this.maxParamValueTextBox, ERR_MIN_GREATER_THAN_MAX);
                    resultParamInfoSetSuccessfully = false;
                }
                else
                {
                    this.errorProvider.SetError(this.minParamValueTextBox, "");
                    this.errorProvider.SetError(this.maxParamValueTextBox, "");
                    maxMinMakeValidRange = true;
                }
            }

            //Check the value field.
            double value = -1;
            bool valueIsValidNumber = true;
            if (this.inputParamInfo.methodOfValueInput == ValueInputType.Number)
            {
                if (double.TryParse(this.paramValueTextBox.Text, out value) == false)
                {
                    this.errorProvider.SetError(this.paramValueTextBox, ERR_NOT_NUMBER);
                    resultParamInfoSetSuccessfully = false;
                    valueIsValidNumber = false;
                }
                else
                {
                    this.errorProvider.SetError(this.paramValueTextBox, "");
                }
            }
            else if (this.inputParamInfo.methodOfValueInput == ValueInputType.Integer)
            {
                int valueAsInt;
                if (int.TryParse(this.paramValueTextBox.Text, out valueAsInt) == false)
                {
                    this.errorProvider.SetError(this.paramValueTextBox, ERR_NOT_INT);
                    resultParamInfoSetSuccessfully = false;
                    valueIsValidNumber = false;
                }
                else
                {
                    this.errorProvider.SetError(this.paramValueTextBox, "");
                    value = valueAsInt;
                }
            }
            else if (this.inputParamInfo.methodOfValueInput == ValueInputType.Selection)
            {
                value = this.paramValueCombo.SelectedIndex;
            }
            else
            {
                //Unknown value input type.
                Debug.Assert(false);
            }

            if (valueIsValidNumber && maxMinMakeValidRange)
            {
                if (value < minValue || value > maxValue)
                {
                    this.errorProvider.SetError(this.paramValueTextBox, ERR_VALUE_OUTSIDE_RANGE);
                    resultParamInfoSetSuccessfully = false;
                }
                else
                {
                    this.errorProvider.SetError(this.paramValueTextBox, "");
                }
            }

            //Set the resulting param info if there were no errors.
            if (resultParamInfoSetSuccessfully)
            {
                this.resultParamInfo.MinValue = minValue;
                this.resultParamInfo.MaxValue = maxValue;
                this.resultParamInfo.RawValue = value;
            }

            return resultParamInfoSetSuccessfully;
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            if (AttemptToSetResultParamInfo() == true)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void paramTypeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            MssParamType paramType = (MssParamType)this.paramTypeCombo.SelectedIndex;
            if (paramType != this.inputParamInfo.paramType)
            {
                this.inputParamInfo = Factory_MssParamInfo.Create(paramType, this.paramNameTextBox.Text);

                ConfigureFieldsFromInputParamInfo();
            }
        }

        private void minParamValueTextBox_Validating(object sender, CancelEventArgs e)
        {
            AttemptToSetResultParamInfo();
        }

        private void maxParamValueTextBox_Validating(object sender, CancelEventArgs e)
        {
            AttemptToSetResultParamInfo();
        }

        private void paramValueTextBox_Validating(object sender, CancelEventArgs e)
        {
            AttemptToSetResultParamInfo();
        }
    }
}

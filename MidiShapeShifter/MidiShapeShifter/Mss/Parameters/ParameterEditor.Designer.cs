namespace MidiShapeShifter.Mss.Parameters
{
    partial class ParameterEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.OkBtn = new System.Windows.Forms.Button();
            this.paramValueTextBox = new System.Windows.Forms.TextBox();
            this.paramValueCombo = new System.Windows.Forms.ComboBox();
            this.parameterValue = new System.Windows.Forms.Label();
            this.paramTypeCombo = new System.Windows.Forms.ComboBox();
            this.paramTypeLbl = new System.Windows.Forms.Label();
            this.paramNameTextBox = new System.Windows.Forms.TextBox();
            this.paramNameLbl = new System.Windows.Forms.Label();
            this.maxParamValueTextBox = new System.Windows.Forms.TextBox();
            this.maxParamValueLabel = new System.Windows.Forms.Label();
            this.minParamValueTextBox = new System.Windows.Forms.TextBox();
            this.minParamValueLabel = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // cancelBtn
            // 
            this.cancelBtn.CausesValidation = false;
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new System.Drawing.Point(109, 145);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 12;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            // 
            // OkBtn
            // 
            this.OkBtn.Location = new System.Drawing.Point(28, 145);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(75, 23);
            this.OkBtn.TabIndex = 11;
            this.OkBtn.Text = "Ok";
            this.OkBtn.UseVisualStyleBackColor = true;
            this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // paramValueTextBox
            // 
            this.paramValueTextBox.Location = new System.Drawing.Point(101, 116);
            this.paramValueTextBox.Name = "paramValueTextBox";
            this.paramValueTextBox.Size = new System.Drawing.Size(105, 20);
            this.paramValueTextBox.TabIndex = 10;
            this.paramValueTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.paramValueTextBox_Validating);
            // 
            // paramValueCombo
            // 
            this.paramValueCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.paramValueCombo.FormattingEnabled = true;
            this.paramValueCombo.Location = new System.Drawing.Point(101, 116);
            this.paramValueCombo.Name = "paramValueCombo";
            this.paramValueCombo.Size = new System.Drawing.Size(105, 21);
            this.paramValueCombo.TabIndex = 16;
            // 
            // parameterValue
            // 
            this.parameterValue.AutoSize = true;
            this.parameterValue.Location = new System.Drawing.Point(7, 118);
            this.parameterValue.Name = "parameterValue";
            this.parameterValue.Size = new System.Drawing.Size(37, 13);
            this.parameterValue.TabIndex = 8;
            this.parameterValue.Text = "Value:";
            // 
            // paramTypeCombo
            // 
            this.paramTypeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.paramTypeCombo.FormattingEnabled = true;
            this.paramTypeCombo.Location = new System.Drawing.Point(101, 37);
            this.paramTypeCombo.Name = "paramTypeCombo";
            this.paramTypeCombo.Size = new System.Drawing.Size(105, 21);
            this.paramTypeCombo.TabIndex = 3;
            this.paramTypeCombo.SelectedIndexChanged += new System.EventHandler(this.paramTypeCombo_SelectedIndexChanged);
            // 
            // paramTypeLbl
            // 
            this.paramTypeLbl.AutoSize = true;
            this.paramTypeLbl.Location = new System.Drawing.Point(7, 40);
            this.paramTypeLbl.Name = "paramTypeLbl";
            this.paramTypeLbl.Size = new System.Drawing.Size(85, 13);
            this.paramTypeLbl.TabIndex = 2;
            this.paramTypeLbl.Text = "Parameter Type:";
            // 
            // paramNameTextBox
            // 
            this.paramNameTextBox.Location = new System.Drawing.Point(101, 10);
            this.paramNameTextBox.Name = "paramNameTextBox";
            this.paramNameTextBox.Size = new System.Drawing.Size(105, 20);
            this.paramNameTextBox.TabIndex = 1;
            // 
            // paramNameLbl
            // 
            this.paramNameLbl.AutoSize = true;
            this.paramNameLbl.Location = new System.Drawing.Point(7, 13);
            this.paramNameLbl.Name = "paramNameLbl";
            this.paramNameLbl.Size = new System.Drawing.Size(89, 13);
            this.paramNameLbl.TabIndex = 0;
            this.paramNameLbl.Text = "Parameter Name:";
            // 
            // maxParamValueTextBox
            // 
            this.maxParamValueTextBox.Location = new System.Drawing.Point(101, 90);
            this.maxParamValueTextBox.Name = "maxParamValueTextBox";
            this.maxParamValueTextBox.Size = new System.Drawing.Size(105, 20);
            this.maxParamValueTextBox.TabIndex = 7;
            this.maxParamValueTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.maxParamValueTextBox_Validating);
            // 
            // maxParamValueLabel
            // 
            this.maxParamValueLabel.AutoSize = true;
            this.maxParamValueLabel.Location = new System.Drawing.Point(7, 93);
            this.maxParamValueLabel.Name = "maxParamValueLabel";
            this.maxParamValueLabel.Size = new System.Drawing.Size(60, 13);
            this.maxParamValueLabel.TabIndex = 6;
            this.maxParamValueLabel.Text = "Max Value:";
            // 
            // minParamValueTextBox
            // 
            this.minParamValueTextBox.Location = new System.Drawing.Point(101, 64);
            this.minParamValueTextBox.Name = "minParamValueTextBox";
            this.minParamValueTextBox.Size = new System.Drawing.Size(105, 20);
            this.minParamValueTextBox.TabIndex = 5;
            this.minParamValueTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.minParamValueTextBox_Validating);
            // 
            // minParamValueLabel
            // 
            this.minParamValueLabel.AutoSize = true;
            this.minParamValueLabel.Location = new System.Drawing.Point(7, 67);
            this.minParamValueLabel.Name = "minParamValueLabel";
            this.minParamValueLabel.Size = new System.Drawing.Size(57, 13);
            this.minParamValueLabel.TabIndex = 4;
            this.minParamValueLabel.Text = "Min Value:";
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider.ContainerControl = this;
            // 
            // ParameterEditor
            // 
            this.AcceptButton = this.OkBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(227, 178);
            this.Controls.Add(this.minParamValueTextBox);
            this.Controls.Add(this.minParamValueLabel);
            this.Controls.Add(this.maxParamValueTextBox);
            this.Controls.Add(this.maxParamValueLabel);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.OkBtn);
            this.Controls.Add(this.paramValueTextBox);
            this.Controls.Add(this.paramValueCombo);
            this.Controls.Add(this.parameterValue);
            this.Controls.Add(this.paramTypeCombo);
            this.Controls.Add(this.paramTypeLbl);
            this.Controls.Add(this.paramNameTextBox);
            this.Controls.Add(this.paramNameLbl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ParameterEditor";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Parameter Editor";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Button OkBtn;
        internal System.Windows.Forms.TextBox paramValueTextBox;
        internal System.Windows.Forms.ComboBox paramValueCombo;
        private System.Windows.Forms.Label parameterValue;
        internal System.Windows.Forms.ComboBox paramTypeCombo;
        private System.Windows.Forms.Label paramTypeLbl;
        internal System.Windows.Forms.TextBox paramNameTextBox;
        private System.Windows.Forms.Label paramNameLbl;
        internal System.Windows.Forms.TextBox maxParamValueTextBox;
        private System.Windows.Forms.Label maxParamValueLabel;
        internal System.Windows.Forms.TextBox minParamValueTextBox;
        private System.Windows.Forms.Label minParamValueLabel;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}
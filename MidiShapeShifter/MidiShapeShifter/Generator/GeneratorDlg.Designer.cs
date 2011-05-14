namespace MidiShapeShifter.Generator
{
    partial class GeneratorDlg
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
            this.genNameLbl = new System.Windows.Forms.Label();
            this.genNameTextBox = new System.Windows.Forms.TextBox();
            this.playModeLbl = new System.Windows.Forms.Label();
            this.playModeCombo = new System.Windows.Forms.ComboBox();
            this.loopCheckBox = new System.Windows.Forms.CheckBox();
            this.periodTypeCombo = new System.Windows.Forms.ComboBox();
            this.periodTypeLbl = new System.Windows.Forms.Label();
            this.periodCombo = new System.Windows.Forms.ComboBox();
            this.periodLbl = new System.Windows.Forms.Label();
            this.periodTextBox = new System.Windows.Forms.TextBox();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.OkBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // genNameLbl
            // 
            this.genNameLbl.AutoSize = true;
            this.genNameLbl.Location = new System.Drawing.Point(12, 9);
            this.genNameLbl.Name = "genNameLbl";
            this.genNameLbl.Size = new System.Drawing.Size(88, 13);
            this.genNameLbl.TabIndex = 2;
            this.genNameLbl.Text = "Generator Name:";
            // 
            // genNameTextBox
            // 
            this.genNameTextBox.Location = new System.Drawing.Point(106, 6);
            this.genNameTextBox.Name = "genNameTextBox";
            this.genNameTextBox.Size = new System.Drawing.Size(105, 20);
            this.genNameTextBox.TabIndex = 4;
            // 
            // playModeLbl
            // 
            this.playModeLbl.AutoSize = true;
            this.playModeLbl.Location = new System.Drawing.Point(12, 35);
            this.playModeLbl.Name = "playModeLbl";
            this.playModeLbl.Size = new System.Drawing.Size(60, 13);
            this.playModeLbl.TabIndex = 5;
            this.playModeLbl.Text = "Play Mode:";
            // 
            // playModeCombo
            // 
            this.playModeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.playModeCombo.FormattingEnabled = true;
            this.playModeCombo.Items.AddRange(new object[] {
            "Always",
            "Trigger"});
            this.playModeCombo.Location = new System.Drawing.Point(106, 32);
            this.playModeCombo.Name = "playModeCombo";
            this.playModeCombo.Size = new System.Drawing.Size(105, 21);
            this.playModeCombo.TabIndex = 10;
            // 
            // loopCheckBox
            // 
            this.loopCheckBox.AutoSize = true;
            this.loopCheckBox.Location = new System.Drawing.Point(15, 113);
            this.loopCheckBox.Name = "loopCheckBox";
            this.loopCheckBox.Size = new System.Drawing.Size(50, 17);
            this.loopCheckBox.TabIndex = 11;
            this.loopCheckBox.Text = "Loop";
            this.loopCheckBox.UseVisualStyleBackColor = true;
            // 
            // periodTypeCombo
            // 
            this.periodTypeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.periodTypeCombo.FormattingEnabled = true;
            this.periodTypeCombo.Location = new System.Drawing.Point(106, 59);
            this.periodTypeCombo.Name = "periodTypeCombo";
            this.periodTypeCombo.Size = new System.Drawing.Size(105, 21);
            this.periodTypeCombo.TabIndex = 13;
            // 
            // periodTypeLbl
            // 
            this.periodTypeLbl.AutoSize = true;
            this.periodTypeLbl.Location = new System.Drawing.Point(12, 62);
            this.periodTypeLbl.Name = "periodTypeLbl";
            this.periodTypeLbl.Size = new System.Drawing.Size(67, 13);
            this.periodTypeLbl.TabIndex = 12;
            this.periodTypeLbl.Text = "Period Type:";
            // 
            // periodCombo
            // 
            this.periodCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.periodCombo.FormattingEnabled = true;
            this.periodCombo.Location = new System.Drawing.Point(106, 86);
            this.periodCombo.Name = "periodCombo";
            this.periodCombo.Size = new System.Drawing.Size(105, 21);
            this.periodCombo.TabIndex = 15;
            // 
            // periodLbl
            // 
            this.periodLbl.AutoSize = true;
            this.periodLbl.Location = new System.Drawing.Point(12, 89);
            this.periodLbl.Name = "periodLbl";
            this.periodLbl.Size = new System.Drawing.Size(40, 13);
            this.periodLbl.TabIndex = 14;
            this.periodLbl.Text = "Period:";
            // 
            // periodTextBox
            // 
            this.periodTextBox.Location = new System.Drawing.Point(106, 86);
            this.periodTextBox.Name = "periodTextBox";
            this.periodTextBox.Size = new System.Drawing.Size(105, 20);
            this.periodTextBox.TabIndex = 16;
            // 
            // cancelBtn
            // 
            this.cancelBtn.CausesValidation = false;
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new System.Drawing.Point(106, 136);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 18;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            // 
            // OkBtn
            // 
            this.OkBtn.Location = new System.Drawing.Point(25, 136);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(75, 23);
            this.OkBtn.TabIndex = 17;
            this.OkBtn.Text = "Ok";
            this.OkBtn.UseVisualStyleBackColor = true;
            // 
            // GeneratorDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(223, 172);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.OkBtn);
            this.Controls.Add(this.periodTextBox);
            this.Controls.Add(this.periodCombo);
            this.Controls.Add(this.periodLbl);
            this.Controls.Add(this.periodTypeCombo);
            this.Controls.Add(this.periodTypeLbl);
            this.Controls.Add(this.loopCheckBox);
            this.Controls.Add(this.playModeCombo);
            this.Controls.Add(this.playModeLbl);
            this.Controls.Add(this.genNameTextBox);
            this.Controls.Add(this.genNameLbl);
            this.MaximizeBox = false;
            this.Name = "GeneratorDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Generator Editor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label genNameLbl;
        internal System.Windows.Forms.TextBox genNameTextBox;
        private System.Windows.Forms.Label playModeLbl;
        internal System.Windows.Forms.ComboBox playModeCombo;
        private System.Windows.Forms.CheckBox loopCheckBox;
        internal System.Windows.Forms.ComboBox periodTypeCombo;
        private System.Windows.Forms.Label periodTypeLbl;
        internal System.Windows.Forms.ComboBox periodCombo;
        private System.Windows.Forms.Label periodLbl;
        internal System.Windows.Forms.TextBox periodTextBox;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Button OkBtn;

    }
}
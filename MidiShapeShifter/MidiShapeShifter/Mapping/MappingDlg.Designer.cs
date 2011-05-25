namespace MidiShapeShifter.Mapping
{
    partial class MappingDlg
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
            this.inGroup = new System.Windows.Forms.GroupBox();
            this.inEntryField2Combo = new System.Windows.Forms.ComboBox();
            this.inEntryField1Combo = new System.Windows.Forms.ComboBox();
            this.inLearnBtn = new System.Windows.Forms.Button();
            this.inOverrideDupsCheckBox = new System.Windows.Forms.CheckBox();
            this.inEntryField2TextBox = new System.Windows.Forms.TextBox();
            this.inEntryField2Lbl = new System.Windows.Forms.Label();
            this.inEntryField1TextBox = new System.Windows.Forms.TextBox();
            this.inEntryField1Lbl = new System.Windows.Forms.Label();
            this.inTypeCombo = new System.Windows.Forms.ComboBox();
            this.inTypeLbl = new System.Windows.Forms.Label();
            this.outGroup = new System.Windows.Forms.GroupBox();
            this.outEntryField2Combo = new System.Windows.Forms.ComboBox();
            this.outEntryField1Combo = new System.Windows.Forms.ComboBox();
            this.outSameAsInCheckBox = new System.Windows.Forms.CheckBox();
            this.outLearnBtn = new System.Windows.Forms.Button();
            this.outEntryField2TextBox = new System.Windows.Forms.TextBox();
            this.outEntryField2Lbl = new System.Windows.Forms.Label();
            this.outEntryField1TextBox = new System.Windows.Forms.TextBox();
            this.outEntryField1Lbl = new System.Windows.Forms.Label();
            this.outTypeCombo = new System.Windows.Forms.ComboBox();
            this.outTypeLbl = new System.Windows.Forms.Label();
            this.OkBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.inGroup.SuspendLayout();
            this.outGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // inGroup
            // 
            this.inGroup.Controls.Add(this.inEntryField2Combo);
            this.inGroup.Controls.Add(this.inEntryField1Combo);
            this.inGroup.Controls.Add(this.inLearnBtn);
            this.inGroup.Controls.Add(this.inOverrideDupsCheckBox);
            this.inGroup.Controls.Add(this.inEntryField2TextBox);
            this.inGroup.Controls.Add(this.inEntryField2Lbl);
            this.inGroup.Controls.Add(this.inEntryField1TextBox);
            this.inGroup.Controls.Add(this.inEntryField1Lbl);
            this.inGroup.Controls.Add(this.inTypeCombo);
            this.inGroup.Controls.Add(this.inTypeLbl);
            this.inGroup.Location = new System.Drawing.Point(12, 12);
            this.inGroup.Name = "inGroup";
            this.inGroup.Size = new System.Drawing.Size(135, 218);
            this.inGroup.TabIndex = 0;
            this.inGroup.TabStop = false;
            this.inGroup.Text = "In";
            // 
            // inEntryField2Combo
            // 
            this.inEntryField2Combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.inEntryField2Combo.FormattingEnabled = true;
            this.inEntryField2Combo.Location = new System.Drawing.Point(9, 125);
            this.inEntryField2Combo.Name = "inEntryField2Combo";
            this.inEntryField2Combo.Size = new System.Drawing.Size(105, 21);
            this.inEntryField2Combo.TabIndex = 10;
            this.inEntryField2Combo.Validating += new System.ComponentModel.CancelEventHandler(this.inEntryField2Combo_Validating);
            this.inEntryField2Combo.TextChanged += new System.EventHandler(this.inEntryField2Combo_TextChanged);
            // 
            // inEntryField1Combo
            // 
            this.inEntryField1Combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.inEntryField1Combo.FormattingEnabled = true;
            this.inEntryField1Combo.Location = new System.Drawing.Point(9, 81);
            this.inEntryField1Combo.Name = "inEntryField1Combo";
            this.inEntryField1Combo.Size = new System.Drawing.Size(105, 21);
            this.inEntryField1Combo.TabIndex = 9;
            this.inEntryField1Combo.Validating += new System.ComponentModel.CancelEventHandler(this.inEntryField1Combo_Validating);
            this.inEntryField1Combo.SelectedValueChanged += new System.EventHandler(this.inEntryField1Combo_SelectedValueChanged);
            // 
            // inLearnBtn
            // 
            this.inLearnBtn.CausesValidation = false;
            this.inLearnBtn.Location = new System.Drawing.Point(29, 185);
            this.inLearnBtn.Name = "inLearnBtn";
            this.inLearnBtn.Size = new System.Drawing.Size(75, 23);
            this.inLearnBtn.TabIndex = 8;
            this.inLearnBtn.Text = "Learn";
            this.inLearnBtn.UseVisualStyleBackColor = true;
            // 
            // inOverrideDupsCheckBox
            // 
            this.inOverrideDupsCheckBox.AutoSize = true;
            this.inOverrideDupsCheckBox.Location = new System.Drawing.Point(9, 157);
            this.inOverrideDupsCheckBox.Name = "inOverrideDupsCheckBox";
            this.inOverrideDupsCheckBox.Size = new System.Drawing.Size(119, 17);
            this.inOverrideDupsCheckBox.TabIndex = 7;
            this.inOverrideDupsCheckBox.Text = "Override Duplicates";
            this.inOverrideDupsCheckBox.UseVisualStyleBackColor = true;
            // 
            // inEntryField2TextBox
            // 
            this.inEntryField2TextBox.Location = new System.Drawing.Point(9, 126);
            this.inEntryField2TextBox.Name = "inEntryField2TextBox";
            this.inEntryField2TextBox.Size = new System.Drawing.Size(105, 20);
            this.inEntryField2TextBox.TabIndex = 5;
            this.inEntryField2TextBox.TextChanged += new System.EventHandler(this.inEntryField2TextBox_TextChanged);
            this.inEntryField2TextBox.Validating += new System.ComponentModel.CancelEventHandler(this.inEntryField2TextBox_Validating);
            // 
            // inEntryField2Lbl
            // 
            this.inEntryField2Lbl.AutoSize = true;
            this.inEntryField2Lbl.Location = new System.Drawing.Point(6, 110);
            this.inEntryField2Lbl.Name = "inEntryField2Lbl";
            this.inEntryField2Lbl.Size = new System.Drawing.Size(75, 13);
            this.inEntryField2Lbl.TabIndex = 4;
            this.inEntryField2Lbl.Text = "Param Range:";
            // 
            // inEntryField1TextBox
            // 
            this.inEntryField1TextBox.Location = new System.Drawing.Point(9, 82);
            this.inEntryField1TextBox.Name = "inEntryField1TextBox";
            this.inEntryField1TextBox.Size = new System.Drawing.Size(105, 20);
            this.inEntryField1TextBox.TabIndex = 3;
            this.inEntryField1TextBox.TextChanged += new System.EventHandler(this.inEntryField1TextBox_TextChanged);
            this.inEntryField1TextBox.Validating += new System.ComponentModel.CancelEventHandler(this.inEntryField1TextBox_Validating);
            // 
            // inEntryField1Lbl
            // 
            this.inEntryField1Lbl.AutoSize = true;
            this.inEntryField1Lbl.Location = new System.Drawing.Point(6, 66);
            this.inEntryField1Lbl.Name = "inEntryField1Lbl";
            this.inEntryField1Lbl.Size = new System.Drawing.Size(84, 13);
            this.inEntryField1Lbl.TabIndex = 1;
            this.inEntryField1Lbl.Text = "Channel Range:";
            // 
            // inTypeCombo
            // 
            this.inTypeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.inTypeCombo.FormattingEnabled = true;
            this.inTypeCombo.Location = new System.Drawing.Point(6, 37);
            this.inTypeCombo.Name = "inTypeCombo";
            this.inTypeCombo.Size = new System.Drawing.Size(108, 21);
            this.inTypeCombo.TabIndex = 2;
            this.inTypeCombo.SelectedIndexChanged += new System.EventHandler(this.inTypeCombo_SelectedIndexChanged);
            // 
            // inTypeLbl
            // 
            this.inTypeLbl.AutoSize = true;
            this.inTypeLbl.Location = new System.Drawing.Point(6, 21);
            this.inTypeLbl.Name = "inTypeLbl";
            this.inTypeLbl.Size = new System.Drawing.Size(80, 13);
            this.inTypeLbl.TabIndex = 1;
            this.inTypeLbl.Text = "Message Type:";
            // 
            // outGroup
            // 
            this.outGroup.Controls.Add(this.outEntryField2Combo);
            this.outGroup.Controls.Add(this.outEntryField1Combo);
            this.outGroup.Controls.Add(this.outSameAsInCheckBox);
            this.outGroup.Controls.Add(this.outLearnBtn);
            this.outGroup.Controls.Add(this.outEntryField2TextBox);
            this.outGroup.Controls.Add(this.outEntryField2Lbl);
            this.outGroup.Controls.Add(this.outEntryField1TextBox);
            this.outGroup.Controls.Add(this.outEntryField1Lbl);
            this.outGroup.Controls.Add(this.outTypeCombo);
            this.outGroup.Controls.Add(this.outTypeLbl);
            this.outGroup.Location = new System.Drawing.Point(153, 12);
            this.outGroup.Name = "outGroup";
            this.outGroup.Size = new System.Drawing.Size(135, 218);
            this.outGroup.TabIndex = 9;
            this.outGroup.TabStop = false;
            this.outGroup.Text = "Out";
            // 
            // outEntryField2Combo
            // 
            this.outEntryField2Combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.outEntryField2Combo.FormattingEnabled = true;
            this.outEntryField2Combo.Location = new System.Drawing.Point(6, 125);
            this.outEntryField2Combo.Name = "outEntryField2Combo";
            this.outEntryField2Combo.Size = new System.Drawing.Size(108, 21);
            this.outEntryField2Combo.TabIndex = 12;
            this.outEntryField2Combo.Validating += new System.ComponentModel.CancelEventHandler(this.outEntryField2Combo_Validating);
            // 
            // outEntryField1Combo
            // 
            this.outEntryField1Combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.outEntryField1Combo.FormattingEnabled = true;
            this.outEntryField1Combo.Location = new System.Drawing.Point(6, 81);
            this.outEntryField1Combo.Name = "outEntryField1Combo";
            this.outEntryField1Combo.Size = new System.Drawing.Size(108, 21);
            this.outEntryField1Combo.TabIndex = 11;
            this.outEntryField1Combo.Validating += new System.ComponentModel.CancelEventHandler(this.outEntryField1Combo_Validating);
            // 
            // outSameAsInCheckBox
            // 
            this.outSameAsInCheckBox.AutoSize = true;
            this.outSameAsInCheckBox.Location = new System.Drawing.Point(9, 157);
            this.outSameAsInCheckBox.Name = "outSameAsInCheckBox";
            this.outSameAsInCheckBox.Size = new System.Drawing.Size(94, 17);
            this.outSameAsInCheckBox.TabIndex = 8;
            this.outSameAsInCheckBox.Text = "Same as Input";
            this.outSameAsInCheckBox.UseVisualStyleBackColor = true;
            this.outSameAsInCheckBox.CheckedChanged += new System.EventHandler(this.outSameAsInCheckBox_CheckedChanged);
            // 
            // outLearnBtn
            // 
            this.outLearnBtn.CausesValidation = false;
            this.outLearnBtn.Enabled = false;
            this.outLearnBtn.Location = new System.Drawing.Point(29, 185);
            this.outLearnBtn.Name = "outLearnBtn";
            this.outLearnBtn.Size = new System.Drawing.Size(75, 23);
            this.outLearnBtn.TabIndex = 9;
            this.outLearnBtn.Text = "Learn";
            this.outLearnBtn.UseVisualStyleBackColor = true;
            // 
            // outEntryField2TextBox
            // 
            this.outEntryField2TextBox.Location = new System.Drawing.Point(9, 126);
            this.outEntryField2TextBox.Name = "outEntryField2TextBox";
            this.outEntryField2TextBox.Size = new System.Drawing.Size(105, 20);
            this.outEntryField2TextBox.TabIndex = 5;
            this.outEntryField2TextBox.Validating += new System.ComponentModel.CancelEventHandler(this.outEntryField2TextBox_Validating);
            // 
            // outEntryField2Lbl
            // 
            this.outEntryField2Lbl.AutoSize = true;
            this.outEntryField2Lbl.Location = new System.Drawing.Point(6, 110);
            this.outEntryField2Lbl.Name = "outEntryField2Lbl";
            this.outEntryField2Lbl.Size = new System.Drawing.Size(75, 13);
            this.outEntryField2Lbl.TabIndex = 4;
            this.outEntryField2Lbl.Text = "Param Range:";
            // 
            // outEntryField1TextBox
            // 
            this.outEntryField1TextBox.Location = new System.Drawing.Point(9, 82);
            this.outEntryField1TextBox.Name = "outEntryField1TextBox";
            this.outEntryField1TextBox.Size = new System.Drawing.Size(105, 20);
            this.outEntryField1TextBox.TabIndex = 3;
            this.outEntryField1TextBox.Validating += new System.ComponentModel.CancelEventHandler(this.outEntryField1TextBox_Validating);
            // 
            // outEntryField1Lbl
            // 
            this.outEntryField1Lbl.AutoSize = true;
            this.outEntryField1Lbl.Location = new System.Drawing.Point(6, 66);
            this.outEntryField1Lbl.Name = "outEntryField1Lbl";
            this.outEntryField1Lbl.Size = new System.Drawing.Size(84, 13);
            this.outEntryField1Lbl.TabIndex = 1;
            this.outEntryField1Lbl.Text = "Channel Range:";
            // 
            // outTypeCombo
            // 
            this.outTypeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.outTypeCombo.FormattingEnabled = true;
            this.outTypeCombo.Location = new System.Drawing.Point(6, 37);
            this.outTypeCombo.Name = "outTypeCombo";
            this.outTypeCombo.Size = new System.Drawing.Size(108, 21);
            this.outTypeCombo.TabIndex = 2;
            this.outTypeCombo.SelectedIndexChanged += new System.EventHandler(this.outTypeCombo_SelectedIndexChanged);
            // 
            // outTypeLbl
            // 
            this.outTypeLbl.AutoSize = true;
            this.outTypeLbl.Location = new System.Drawing.Point(6, 21);
            this.outTypeLbl.Name = "outTypeLbl";
            this.outTypeLbl.Size = new System.Drawing.Size(80, 13);
            this.outTypeLbl.TabIndex = 1;
            this.outTypeLbl.Text = "Message Type:";
            // 
            // OkBtn
            // 
            this.OkBtn.Location = new System.Drawing.Point(72, 241);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(75, 23);
            this.OkBtn.TabIndex = 10;
            this.OkBtn.Text = "Ok";
            this.OkBtn.UseVisualStyleBackColor = true;
            this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.CausesValidation = false;
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new System.Drawing.Point(153, 241);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 11;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider.ContainerControl = this;
            // 
            // MappingDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(295, 271);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.OkBtn);
            this.Controls.Add(this.outGroup);
            this.Controls.Add(this.inGroup);
            this.MaximizeBox = false;
            this.Name = "MappingDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Mapping Editor";
            this.Load += new System.EventHandler(this.MappingDlg_Load);
            this.inGroup.ResumeLayout(false);
            this.inGroup.PerformLayout();
            this.outGroup.ResumeLayout(false);
            this.outGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox inGroup;
        private System.Windows.Forms.Label inTypeLbl;
        private System.Windows.Forms.CheckBox inOverrideDupsCheckBox;
        private System.Windows.Forms.Button inLearnBtn;
        private System.Windows.Forms.GroupBox outGroup;
        private System.Windows.Forms.Button outLearnBtn;
        private System.Windows.Forms.Label outTypeLbl;
        private System.Windows.Forms.Button OkBtn;
        private System.Windows.Forms.Button cancelBtn;
        internal System.Windows.Forms.ErrorProvider errorProvider;
        internal System.Windows.Forms.ComboBox inEntryField2Combo;
        internal System.Windows.Forms.ComboBox inEntryField1Combo;
        internal System.Windows.Forms.ComboBox outEntryField2Combo;
        internal System.Windows.Forms.ComboBox outEntryField1Combo;
        internal System.Windows.Forms.TextBox inEntryField1TextBox;
        internal System.Windows.Forms.Label inEntryField1Lbl;
        internal System.Windows.Forms.TextBox inEntryField2TextBox;
        internal System.Windows.Forms.Label inEntryField2Lbl;
        internal System.Windows.Forms.TextBox outEntryField2TextBox;
        internal System.Windows.Forms.Label outEntryField2Lbl;
        internal System.Windows.Forms.TextBox outEntryField1TextBox;
        internal System.Windows.Forms.Label outEntryField1Lbl;
        internal System.Windows.Forms.ComboBox outTypeCombo;
        internal System.Windows.Forms.CheckBox outSameAsInCheckBox;
        internal System.Windows.Forms.ComboBox inTypeCombo;
    }
}
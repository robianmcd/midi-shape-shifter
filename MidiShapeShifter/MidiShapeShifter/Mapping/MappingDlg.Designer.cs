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
            this.inLearnBtn = new System.Windows.Forms.Button();
            this.inOverrideDupsCheckBox = new System.Windows.Forms.CheckBox();
            this.inParamRangeTextBox = new System.Windows.Forms.TextBox();
            this.inParamRangeLbl = new System.Windows.Forms.Label();
            this.inChannelRangeTextBox = new System.Windows.Forms.TextBox();
            this.inChanelRangeLbl = new System.Windows.Forms.Label();
            this.inTypeCombo = new System.Windows.Forms.ComboBox();
            this.inTypeLbl = new System.Windows.Forms.Label();
            this.outGroup = new System.Windows.Forms.GroupBox();
            this.outSameAsInCheckBox = new System.Windows.Forms.CheckBox();
            this.outLearnBtn = new System.Windows.Forms.Button();
            this.outParamRangeTextBox = new System.Windows.Forms.TextBox();
            this.outParamRangeLbl = new System.Windows.Forms.Label();
            this.outChannelRangeTextBox = new System.Windows.Forms.TextBox();
            this.outChannelRangeLbl = new System.Windows.Forms.Label();
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
            this.inGroup.Controls.Add(this.inLearnBtn);
            this.inGroup.Controls.Add(this.inOverrideDupsCheckBox);
            this.inGroup.Controls.Add(this.inParamRangeTextBox);
            this.inGroup.Controls.Add(this.inParamRangeLbl);
            this.inGroup.Controls.Add(this.inChannelRangeTextBox);
            this.inGroup.Controls.Add(this.inChanelRangeLbl);
            this.inGroup.Controls.Add(this.inTypeCombo);
            this.inGroup.Controls.Add(this.inTypeLbl);
            this.inGroup.Location = new System.Drawing.Point(12, 12);
            this.inGroup.Name = "inGroup";
            this.inGroup.Size = new System.Drawing.Size(135, 218);
            this.inGroup.TabIndex = 0;
            this.inGroup.TabStop = false;
            this.inGroup.Text = "In";
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
            // inParamRangeTextBox
            // 
            this.inParamRangeTextBox.Location = new System.Drawing.Point(9, 126);
            this.inParamRangeTextBox.Name = "inParamRangeTextBox";
            this.inParamRangeTextBox.Size = new System.Drawing.Size(105, 20);
            this.inParamRangeTextBox.TabIndex = 5;
            this.inParamRangeTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.inParamRangeTextBox_Validating);
            // 
            // inParamRangeLbl
            // 
            this.inParamRangeLbl.AutoSize = true;
            this.inParamRangeLbl.Location = new System.Drawing.Point(6, 110);
            this.inParamRangeLbl.Name = "inParamRangeLbl";
            this.inParamRangeLbl.Size = new System.Drawing.Size(75, 13);
            this.inParamRangeLbl.TabIndex = 4;
            this.inParamRangeLbl.Text = "Param Range:";
            // 
            // inChannelRangeTextBox
            // 
            this.inChannelRangeTextBox.Location = new System.Drawing.Point(9, 82);
            this.inChannelRangeTextBox.Name = "inChannelRangeTextBox";
            this.inChannelRangeTextBox.Size = new System.Drawing.Size(105, 20);
            this.inChannelRangeTextBox.TabIndex = 3;
            this.inChannelRangeTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.inChannelRangeTextBox_Validating);
            // 
            // inChanelRangeLbl
            // 
            this.inChanelRangeLbl.AutoSize = true;
            this.inChanelRangeLbl.Location = new System.Drawing.Point(6, 66);
            this.inChanelRangeLbl.Name = "inChanelRangeLbl";
            this.inChanelRangeLbl.Size = new System.Drawing.Size(84, 13);
            this.inChanelRangeLbl.TabIndex = 1;
            this.inChanelRangeLbl.Text = "Channel Range:";
            // 
            // inTypeCombo
            // 
            this.inTypeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.inTypeCombo.FormattingEnabled = true;
            this.inTypeCombo.Location = new System.Drawing.Point(6, 37);
            this.inTypeCombo.Name = "inTypeCombo";
            this.inTypeCombo.Size = new System.Drawing.Size(105, 21);
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
            this.outGroup.Controls.Add(this.outSameAsInCheckBox);
            this.outGroup.Controls.Add(this.outLearnBtn);
            this.outGroup.Controls.Add(this.outParamRangeTextBox);
            this.outGroup.Controls.Add(this.outParamRangeLbl);
            this.outGroup.Controls.Add(this.outChannelRangeTextBox);
            this.outGroup.Controls.Add(this.outChannelRangeLbl);
            this.outGroup.Controls.Add(this.outTypeCombo);
            this.outGroup.Controls.Add(this.outTypeLbl);
            this.outGroup.Location = new System.Drawing.Point(153, 12);
            this.outGroup.Name = "outGroup";
            this.outGroup.Size = new System.Drawing.Size(135, 218);
            this.outGroup.TabIndex = 9;
            this.outGroup.TabStop = false;
            this.outGroup.Text = "Out";
            // 
            // outSameAsInCheckBox
            // 
            this.outSameAsInCheckBox.AutoSize = true;
            this.outSameAsInCheckBox.Checked = true;
            this.outSameAsInCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.outSameAsInCheckBox.Location = new System.Drawing.Point(9, 157);
            this.outSameAsInCheckBox.Name = "outSameAsInCheckBox";
            this.outSameAsInCheckBox.Size = new System.Drawing.Size(94, 17);
            this.outSameAsInCheckBox.TabIndex = 9;
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
            this.outLearnBtn.TabIndex = 8;
            this.outLearnBtn.Text = "Learn";
            this.outLearnBtn.UseVisualStyleBackColor = true;
            // 
            // outParamRangeTextBox
            // 
            this.outParamRangeTextBox.Enabled = false;
            this.outParamRangeTextBox.Location = new System.Drawing.Point(9, 126);
            this.outParamRangeTextBox.Name = "outParamRangeTextBox";
            this.outParamRangeTextBox.Size = new System.Drawing.Size(105, 20);
            this.outParamRangeTextBox.TabIndex = 5;
            this.outParamRangeTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.outParamRangeTextBox_Validating);
            // 
            // outParamRangeLbl
            // 
            this.outParamRangeLbl.AutoSize = true;
            this.outParamRangeLbl.Location = new System.Drawing.Point(6, 110);
            this.outParamRangeLbl.Name = "outParamRangeLbl";
            this.outParamRangeLbl.Size = new System.Drawing.Size(75, 13);
            this.outParamRangeLbl.TabIndex = 4;
            this.outParamRangeLbl.Text = "Param Range:";
            // 
            // outChannelRangeTextBox
            // 
            this.outChannelRangeTextBox.Enabled = false;
            this.outChannelRangeTextBox.Location = new System.Drawing.Point(9, 82);
            this.outChannelRangeTextBox.Name = "outChannelRangeTextBox";
            this.outChannelRangeTextBox.Size = new System.Drawing.Size(105, 20);
            this.outChannelRangeTextBox.TabIndex = 3;
            this.outChannelRangeTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.outChannelRangeTextBox_Validating);
            // 
            // outChannelRangeLbl
            // 
            this.outChannelRangeLbl.AutoSize = true;
            this.outChannelRangeLbl.Location = new System.Drawing.Point(6, 66);
            this.outChannelRangeLbl.Name = "outChannelRangeLbl";
            this.outChannelRangeLbl.Size = new System.Drawing.Size(84, 13);
            this.outChannelRangeLbl.TabIndex = 1;
            this.outChannelRangeLbl.Text = "Channel Range:";
            // 
            // outTypeCombo
            // 
            this.outTypeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.outTypeCombo.Enabled = false;
            this.outTypeCombo.FormattingEnabled = true;
            this.outTypeCombo.Location = new System.Drawing.Point(6, 37);
            this.outTypeCombo.Name = "outTypeCombo";
            this.outTypeCombo.Size = new System.Drawing.Size(105, 21);
            this.outTypeCombo.TabIndex = 2;
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
            this.OkBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkBtn.Location = new System.Drawing.Point(72, 241);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(75, 23);
            this.OkBtn.TabIndex = 10;
            this.OkBtn.Text = "Ok";
            this.OkBtn.UseVisualStyleBackColor = true;
            this.OkBtn.Validating += new System.ComponentModel.CancelEventHandler(this.OkBtn_Validating);
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
            this.Text = "MappingDlg";
            this.inGroup.ResumeLayout(false);
            this.inGroup.PerformLayout();
            this.outGroup.ResumeLayout(false);
            this.outGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox inGroup;
        private System.Windows.Forms.ComboBox inTypeCombo;
        private System.Windows.Forms.Label inTypeLbl;
        private System.Windows.Forms.TextBox inChannelRangeTextBox;
        private System.Windows.Forms.Label inChanelRangeLbl;
        private System.Windows.Forms.TextBox inParamRangeTextBox;
        private System.Windows.Forms.Label inParamRangeLbl;
        private System.Windows.Forms.CheckBox inOverrideDupsCheckBox;
        private System.Windows.Forms.Button inLearnBtn;
        private System.Windows.Forms.GroupBox outGroup;
        private System.Windows.Forms.Button outLearnBtn;
        private System.Windows.Forms.TextBox outParamRangeTextBox;
        private System.Windows.Forms.Label outParamRangeLbl;
        private System.Windows.Forms.TextBox outChannelRangeTextBox;
        private System.Windows.Forms.Label outChannelRangeLbl;
        private System.Windows.Forms.ComboBox outTypeCombo;
        private System.Windows.Forms.Label outTypeLbl;
        private System.Windows.Forms.Button OkBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.CheckBox outSameAsInCheckBox;
    }
}
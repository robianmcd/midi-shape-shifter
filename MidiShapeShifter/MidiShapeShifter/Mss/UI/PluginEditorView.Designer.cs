namespace MidiShapeShifter.Mss.UI
{
    partial class PluginEditorView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PluginEditorView));
            this.mainGraphControl = new ZedGraph.ZedGraphControl();
            this.presetParam1Knob = new LBSoft.IndustrialCtrls.Knobs.LBKnob();
            this.presetParam1Value = new System.Windows.Forms.Label();
            this.curveGroup = new System.Windows.Forms.GroupBox();
            this.equationBookBtn = new System.Windows.Forms.Button();
            this.presetParam4Title = new System.Windows.Forms.Label();
            this.presetParam4Value = new System.Windows.Forms.Label();
            this.presetParam4Knob = new LBSoft.IndustrialCtrls.Knobs.LBKnob();
            this.presetParam3Title = new System.Windows.Forms.Label();
            this.presetParam3Value = new System.Windows.Forms.Label();
            this.presetParam3Knob = new LBSoft.IndustrialCtrls.Knobs.LBKnob();
            this.presetParam2Title = new System.Windows.Forms.Label();
            this.presetParam2Value = new System.Windows.Forms.Label();
            this.presetParam2Knob = new LBSoft.IndustrialCtrls.Knobs.LBKnob();
            this.presetParam1Title = new System.Windows.Forms.Label();
            this.curvePresetCombo = new System.Windows.Forms.ComboBox();
            this.curveEquationTextBox = new System.Windows.Forms.TextBox();
            this.curveShapePresetRadio = new System.Windows.Forms.RadioButton();
            this.curveShapeEquationRadio = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.variableDMinLbl = new System.Windows.Forms.Label();
            this.variableDMin = new System.Windows.Forms.TextBox();
            this.variableDMax = new System.Windows.Forms.TextBox();
            this.variableDMaxLbl = new System.Windows.Forms.Label();
            this.variableDValue = new System.Windows.Forms.Label();
            this.variableDTitle = new System.Windows.Forms.Label();
            this.variableDKnob = new LBSoft.IndustrialCtrls.Knobs.LBKnob();
            this.variableCMinLbl = new System.Windows.Forms.Label();
            this.variableCMin = new System.Windows.Forms.TextBox();
            this.variableCMax = new System.Windows.Forms.TextBox();
            this.variableCMaxLbl = new System.Windows.Forms.Label();
            this.variableCValue = new System.Windows.Forms.Label();
            this.variableCTitle = new System.Windows.Forms.Label();
            this.variableCKnob = new LBSoft.IndustrialCtrls.Knobs.LBKnob();
            this.variableBMinLbl = new System.Windows.Forms.Label();
            this.variableBMin = new System.Windows.Forms.TextBox();
            this.variableBMax = new System.Windows.Forms.TextBox();
            this.variableBMaxLbl = new System.Windows.Forms.Label();
            this.variableBValue = new System.Windows.Forms.Label();
            this.variableBTitle = new System.Windows.Forms.Label();
            this.variableBKnob = new LBSoft.IndustrialCtrls.Knobs.LBKnob();
            this.variableAMinLbl = new System.Windows.Forms.Label();
            this.variableAMin = new System.Windows.Forms.TextBox();
            this.variableAMax = new System.Windows.Forms.TextBox();
            this.variableAMaxLbl = new System.Windows.Forms.Label();
            this.variableAValue = new System.Windows.Forms.Label();
            this.variableATitle = new System.Windows.Forms.Label();
            this.variableAKnob = new LBSoft.IndustrialCtrls.Knobs.LBKnob();
            this.mappingListView = new System.Windows.Forms.ListView();
            this.inTypeColHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.inChannelsColHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.inParamsColHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.outTypeColHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.outChannelsColHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.outParamsColHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.overrideColHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.mappingGroupBox = new System.Windows.Forms.GroupBox();
            this.moveMappingDownBtn = new System.Windows.Forms.Button();
            this.moveMappingUpBtn = new System.Windows.Forms.Button();
            this.addMappingBtn = new System.Windows.Forms.Button();
            this.editMappingBtn = new System.Windows.Forms.Button();
            this.deleteMappingBtn = new System.Windows.Forms.Button();
            this.generatorGroupBox = new System.Windows.Forms.GroupBox();
            this.generatorListView = new System.Windows.Forms.ListView();
            this.generaotrNameColHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.generatorPeriodColHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.generatorLoopColHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.generatorEnabledColHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.addGeneratorBtn = new System.Windows.Forms.Button();
            this.editGeneratorBtn = new System.Windows.Forms.Button();
            this.deleteGeneratorBtn = new System.Windows.Forms.Button();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.openProgram = new System.Windows.Forms.ToolStripButton();
            this.saveSplitButton = new System.Windows.Forms.ToolStripSplitButton();
            this.saveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.programList = new System.Windows.Forms.ToolStripDropDownButton();
            this.curveGroup.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.mappingGroupBox.SuspendLayout();
            this.generatorGroupBox.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainGraphControl
            // 
            this.mainGraphControl.Location = new System.Drawing.Point(362, 35);
            this.mainGraphControl.Name = "mainGraphControl";
            this.mainGraphControl.ScrollGrace = 0D;
            this.mainGraphControl.ScrollMaxX = 0D;
            this.mainGraphControl.ScrollMaxY = 0D;
            this.mainGraphControl.ScrollMaxY2 = 0D;
            this.mainGraphControl.ScrollMinX = 0D;
            this.mainGraphControl.ScrollMinY = 0D;
            this.mainGraphControl.ScrollMinY2 = 0D;
            this.mainGraphControl.Size = new System.Drawing.Size(275, 275);
            this.mainGraphControl.TabIndex = 1;
            // 
            // presetParam1Knob
            // 
            this.presetParam1Knob.BackColor = System.Drawing.Color.Transparent;
            this.presetParam1Knob.DrawRatio = 0.2F;
            this.presetParam1Knob.Enabled = false;
            this.presetParam1Knob.IndicatorColor = System.Drawing.Color.Black;
            this.presetParam1Knob.IndicatorOffset = 7F;
            this.presetParam1Knob.KnobCenter = ((System.Drawing.PointF)(resources.GetObject("presetParam1Knob.KnobCenter")));
            this.presetParam1Knob.KnobColor = System.Drawing.Color.Silver;
            this.presetParam1Knob.KnobRect = ((System.Drawing.RectangleF)(resources.GetObject("presetParam1Knob.KnobRect")));
            this.presetParam1Knob.Location = new System.Drawing.Point(30, 98);
            this.presetParam1Knob.MaxValue = 1F;
            this.presetParam1Knob.MinValue = 0F;
            this.presetParam1Knob.Name = "presetParam1Knob";
            this.presetParam1Knob.Renderer = null;
            this.presetParam1Knob.ScaleColor = System.Drawing.SystemColors.ControlDarkDark;
            this.presetParam1Knob.Size = new System.Drawing.Size(40, 40);
            this.presetParam1Knob.StepValue = 0.1F;
            this.presetParam1Knob.Style = LBSoft.IndustrialCtrls.Knobs.LBKnob.KnobStyle.Circular;
            this.presetParam1Knob.TabIndex = 2;
            this.presetParam1Knob.Value = 0F;
            this.presetParam1Knob.KnobChangeValue += new LBSoft.IndustrialCtrls.Knobs.KnobChangeValue(this.lbKnob_KnobChangeValue);
            // 
            // presetParam1Value
            // 
            this.presetParam1Value.BackColor = System.Drawing.Color.White;
            this.presetParam1Value.Location = new System.Drawing.Point(30, 141);
            this.presetParam1Value.Name = "presetParam1Value";
            this.presetParam1Value.Size = new System.Drawing.Size(40, 16);
            this.presetParam1Value.TabIndex = 4;
            this.presetParam1Value.Text = "0";
            // 
            // curveGroup
            // 
            this.curveGroup.Controls.Add(this.equationBookBtn);
            this.curveGroup.Controls.Add(this.presetParam4Title);
            this.curveGroup.Controls.Add(this.presetParam4Value);
            this.curveGroup.Controls.Add(this.presetParam4Knob);
            this.curveGroup.Controls.Add(this.presetParam3Title);
            this.curveGroup.Controls.Add(this.presetParam3Value);
            this.curveGroup.Controls.Add(this.presetParam3Knob);
            this.curveGroup.Controls.Add(this.presetParam2Title);
            this.curveGroup.Controls.Add(this.presetParam2Value);
            this.curveGroup.Controls.Add(this.presetParam2Knob);
            this.curveGroup.Controls.Add(this.presetParam1Title);
            this.curveGroup.Controls.Add(this.curvePresetCombo);
            this.curveGroup.Controls.Add(this.presetParam1Value);
            this.curveGroup.Controls.Add(this.curveEquationTextBox);
            this.curveGroup.Controls.Add(this.presetParam1Knob);
            this.curveGroup.Controls.Add(this.curveShapePresetRadio);
            this.curveGroup.Controls.Add(this.curveShapeEquationRadio);
            this.curveGroup.Location = new System.Drawing.Point(362, 313);
            this.curveGroup.Name = "curveGroup";
            this.curveGroup.Size = new System.Drawing.Size(273, 171);
            this.curveGroup.TabIndex = 6;
            this.curveGroup.TabStop = false;
            this.curveGroup.Text = "Transformation";
            // 
            // equationBookBtn
            // 
            this.equationBookBtn.BackgroundImage = global::MidiShapeShifter.Properties.Resources.imgNotebookBlue;
            this.equationBookBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.equationBookBtn.Location = new System.Drawing.Point(242, 21);
            this.equationBookBtn.Margin = new System.Windows.Forms.Padding(0);
            this.equationBookBtn.Name = "equationBookBtn";
            this.equationBookBtn.Size = new System.Drawing.Size(24, 24);
            this.equationBookBtn.TabIndex = 9;
            this.equationBookBtn.UseVisualStyleBackColor = true;
            // 
            // presetParam4Title
            // 
            this.presetParam4Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.presetParam4Title.Location = new System.Drawing.Point(200, 82);
            this.presetParam4Title.Name = "presetParam4Title";
            this.presetParam4Title.Size = new System.Drawing.Size(60, 13);
            this.presetParam4Title.TabIndex = 16;
            this.presetParam4Title.Text = "Param4";
            this.presetParam4Title.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // presetParam4Value
            // 
            this.presetParam4Value.BackColor = System.Drawing.Color.White;
            this.presetParam4Value.Location = new System.Drawing.Point(210, 141);
            this.presetParam4Value.Name = "presetParam4Value";
            this.presetParam4Value.Size = new System.Drawing.Size(40, 16);
            this.presetParam4Value.TabIndex = 15;
            this.presetParam4Value.Text = "0";
            // 
            // presetParam4Knob
            // 
            this.presetParam4Knob.BackColor = System.Drawing.Color.Transparent;
            this.presetParam4Knob.DrawRatio = 0.2F;
            this.presetParam4Knob.Enabled = false;
            this.presetParam4Knob.IndicatorColor = System.Drawing.Color.Black;
            this.presetParam4Knob.IndicatorOffset = 7F;
            this.presetParam4Knob.KnobCenter = ((System.Drawing.PointF)(resources.GetObject("presetParam4Knob.KnobCenter")));
            this.presetParam4Knob.KnobColor = System.Drawing.Color.Silver;
            this.presetParam4Knob.KnobRect = ((System.Drawing.RectangleF)(resources.GetObject("presetParam4Knob.KnobRect")));
            this.presetParam4Knob.Location = new System.Drawing.Point(210, 98);
            this.presetParam4Knob.MaxValue = 1F;
            this.presetParam4Knob.MinValue = 0F;
            this.presetParam4Knob.Name = "presetParam4Knob";
            this.presetParam4Knob.Renderer = null;
            this.presetParam4Knob.ScaleColor = System.Drawing.SystemColors.ControlDarkDark;
            this.presetParam4Knob.Size = new System.Drawing.Size(40, 40);
            this.presetParam4Knob.StepValue = 0.1F;
            this.presetParam4Knob.Style = LBSoft.IndustrialCtrls.Knobs.LBKnob.KnobStyle.Circular;
            this.presetParam4Knob.TabIndex = 14;
            this.presetParam4Knob.Value = 0F;
            this.presetParam4Knob.KnobChangeValue += new LBSoft.IndustrialCtrls.Knobs.KnobChangeValue(this.lbKnob_KnobChangeValue);
            // 
            // presetParam3Title
            // 
            this.presetParam3Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.presetParam3Title.Location = new System.Drawing.Point(140, 82);
            this.presetParam3Title.Name = "presetParam3Title";
            this.presetParam3Title.Size = new System.Drawing.Size(60, 13);
            this.presetParam3Title.TabIndex = 13;
            this.presetParam3Title.Text = "Param3";
            this.presetParam3Title.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // presetParam3Value
            // 
            this.presetParam3Value.BackColor = System.Drawing.Color.White;
            this.presetParam3Value.Location = new System.Drawing.Point(150, 141);
            this.presetParam3Value.Name = "presetParam3Value";
            this.presetParam3Value.Size = new System.Drawing.Size(40, 16);
            this.presetParam3Value.TabIndex = 12;
            this.presetParam3Value.Text = "0";
            // 
            // presetParam3Knob
            // 
            this.presetParam3Knob.BackColor = System.Drawing.Color.Transparent;
            this.presetParam3Knob.DrawRatio = 0.2F;
            this.presetParam3Knob.Enabled = false;
            this.presetParam3Knob.IndicatorColor = System.Drawing.Color.Black;
            this.presetParam3Knob.IndicatorOffset = 7F;
            this.presetParam3Knob.KnobCenter = ((System.Drawing.PointF)(resources.GetObject("presetParam3Knob.KnobCenter")));
            this.presetParam3Knob.KnobColor = System.Drawing.Color.Silver;
            this.presetParam3Knob.KnobRect = ((System.Drawing.RectangleF)(resources.GetObject("presetParam3Knob.KnobRect")));
            this.presetParam3Knob.Location = new System.Drawing.Point(150, 98);
            this.presetParam3Knob.MaxValue = 1F;
            this.presetParam3Knob.MinValue = 0F;
            this.presetParam3Knob.Name = "presetParam3Knob";
            this.presetParam3Knob.Renderer = null;
            this.presetParam3Knob.ScaleColor = System.Drawing.SystemColors.ControlDarkDark;
            this.presetParam3Knob.Size = new System.Drawing.Size(40, 40);
            this.presetParam3Knob.StepValue = 0.1F;
            this.presetParam3Knob.Style = LBSoft.IndustrialCtrls.Knobs.LBKnob.KnobStyle.Circular;
            this.presetParam3Knob.TabIndex = 11;
            this.presetParam3Knob.Value = 0F;
            this.presetParam3Knob.KnobChangeValue += new LBSoft.IndustrialCtrls.Knobs.KnobChangeValue(this.lbKnob_KnobChangeValue);
            // 
            // presetParam2Title
            // 
            this.presetParam2Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.presetParam2Title.Location = new System.Drawing.Point(80, 82);
            this.presetParam2Title.Name = "presetParam2Title";
            this.presetParam2Title.Size = new System.Drawing.Size(60, 13);
            this.presetParam2Title.TabIndex = 10;
            this.presetParam2Title.Text = "Param2";
            this.presetParam2Title.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // presetParam2Value
            // 
            this.presetParam2Value.BackColor = System.Drawing.Color.White;
            this.presetParam2Value.Location = new System.Drawing.Point(90, 141);
            this.presetParam2Value.Name = "presetParam2Value";
            this.presetParam2Value.Size = new System.Drawing.Size(40, 16);
            this.presetParam2Value.TabIndex = 9;
            this.presetParam2Value.Text = "0";
            // 
            // presetParam2Knob
            // 
            this.presetParam2Knob.BackColor = System.Drawing.Color.Transparent;
            this.presetParam2Knob.DrawRatio = 0.2F;
            this.presetParam2Knob.Enabled = false;
            this.presetParam2Knob.IndicatorColor = System.Drawing.Color.Black;
            this.presetParam2Knob.IndicatorOffset = 7F;
            this.presetParam2Knob.KnobCenter = ((System.Drawing.PointF)(resources.GetObject("presetParam2Knob.KnobCenter")));
            this.presetParam2Knob.KnobColor = System.Drawing.Color.Silver;
            this.presetParam2Knob.KnobRect = ((System.Drawing.RectangleF)(resources.GetObject("presetParam2Knob.KnobRect")));
            this.presetParam2Knob.Location = new System.Drawing.Point(90, 98);
            this.presetParam2Knob.MaxValue = 1F;
            this.presetParam2Knob.MinValue = 0F;
            this.presetParam2Knob.Name = "presetParam2Knob";
            this.presetParam2Knob.Renderer = null;
            this.presetParam2Knob.ScaleColor = System.Drawing.SystemColors.ControlDarkDark;
            this.presetParam2Knob.Size = new System.Drawing.Size(40, 40);
            this.presetParam2Knob.StepValue = 0.1F;
            this.presetParam2Knob.Style = LBSoft.IndustrialCtrls.Knobs.LBKnob.KnobStyle.Circular;
            this.presetParam2Knob.TabIndex = 8;
            this.presetParam2Knob.Value = 0F;
            this.presetParam2Knob.KnobChangeValue += new LBSoft.IndustrialCtrls.Knobs.KnobChangeValue(this.lbKnob_KnobChangeValue);
            // 
            // presetParam1Title
            // 
            this.presetParam1Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.presetParam1Title.Location = new System.Drawing.Point(20, 82);
            this.presetParam1Title.Name = "presetParam1Title";
            this.presetParam1Title.Size = new System.Drawing.Size(60, 13);
            this.presetParam1Title.TabIndex = 7;
            this.presetParam1Title.Text = "Param1";
            this.presetParam1Title.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // curvePresetCombo
            // 
            this.curvePresetCombo.Enabled = false;
            this.curvePresetCombo.FormattingEnabled = true;
            this.curvePresetCombo.Location = new System.Drawing.Point(88, 56);
            this.curvePresetCombo.Name = "curvePresetCombo";
            this.curvePresetCombo.Size = new System.Drawing.Size(178, 21);
            this.curvePresetCombo.TabIndex = 3;
            // 
            // curveEquationTextBox
            // 
            this.curveEquationTextBox.Enabled = false;
            this.curveEquationTextBox.Location = new System.Drawing.Point(88, 25);
            this.curveEquationTextBox.Name = "curveEquationTextBox";
            this.curveEquationTextBox.Size = new System.Drawing.Size(151, 20);
            this.curveEquationTextBox.TabIndex = 2;
            this.curveEquationTextBox.TextChanged += new System.EventHandler(this.curveEquationTextBox_TextChanged);
            // 
            // curveShapePresetRadio
            // 
            this.curveShapePresetRadio.AutoSize = true;
            this.curveShapePresetRadio.Enabled = false;
            this.curveShapePresetRadio.Location = new System.Drawing.Point(15, 60);
            this.curveShapePresetRadio.Name = "curveShapePresetRadio";
            this.curveShapePresetRadio.Size = new System.Drawing.Size(58, 17);
            this.curveShapePresetRadio.TabIndex = 1;
            this.curveShapePresetRadio.TabStop = true;
            this.curveShapePresetRadio.Text = "Preset:";
            this.curveShapePresetRadio.UseVisualStyleBackColor = true;
            this.curveShapePresetRadio.CheckedChanged += new System.EventHandler(this.CurveShapeRadio_CheckedChanged);
            // 
            // curveShapeEquationRadio
            // 
            this.curveShapeEquationRadio.AutoSize = true;
            this.curveShapeEquationRadio.Enabled = false;
            this.curveShapeEquationRadio.Location = new System.Drawing.Point(15, 28);
            this.curveShapeEquationRadio.Name = "curveShapeEquationRadio";
            this.curveShapeEquationRadio.Size = new System.Drawing.Size(70, 17);
            this.curveShapeEquationRadio.TabIndex = 0;
            this.curveShapeEquationRadio.TabStop = true;
            this.curveShapeEquationRadio.Text = "Equation:";
            this.curveShapeEquationRadio.UseVisualStyleBackColor = true;
            this.curveShapeEquationRadio.CheckedChanged += new System.EventHandler(this.CurveShapeRadio_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.variableDMinLbl);
            this.groupBox1.Controls.Add(this.variableDMin);
            this.groupBox1.Controls.Add(this.variableDMax);
            this.groupBox1.Controls.Add(this.variableDMaxLbl);
            this.groupBox1.Controls.Add(this.variableDValue);
            this.groupBox1.Controls.Add(this.variableDTitle);
            this.groupBox1.Controls.Add(this.variableDKnob);
            this.groupBox1.Controls.Add(this.variableCMinLbl);
            this.groupBox1.Controls.Add(this.variableCMin);
            this.groupBox1.Controls.Add(this.variableCMax);
            this.groupBox1.Controls.Add(this.variableCMaxLbl);
            this.groupBox1.Controls.Add(this.variableCValue);
            this.groupBox1.Controls.Add(this.variableCTitle);
            this.groupBox1.Controls.Add(this.variableCKnob);
            this.groupBox1.Controls.Add(this.variableBMinLbl);
            this.groupBox1.Controls.Add(this.variableBMin);
            this.groupBox1.Controls.Add(this.variableBMax);
            this.groupBox1.Controls.Add(this.variableBMaxLbl);
            this.groupBox1.Controls.Add(this.variableBValue);
            this.groupBox1.Controls.Add(this.variableBTitle);
            this.groupBox1.Controls.Add(this.variableBKnob);
            this.groupBox1.Controls.Add(this.variableAMinLbl);
            this.groupBox1.Controls.Add(this.variableAMin);
            this.groupBox1.Controls.Add(this.variableAMax);
            this.groupBox1.Controls.Add(this.variableAMaxLbl);
            this.groupBox1.Controls.Add(this.variableAValue);
            this.groupBox1.Controls.Add(this.variableATitle);
            this.groupBox1.Controls.Add(this.variableAKnob);
            this.groupBox1.Location = new System.Drawing.Point(5, 344);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(351, 140);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Variables";
            // 
            // variableDMinLbl
            // 
            this.variableDMinLbl.AutoSize = true;
            this.variableDMinLbl.Location = new System.Drawing.Point(266, 113);
            this.variableDMinLbl.Name = "variableDMinLbl";
            this.variableDMinLbl.Size = new System.Drawing.Size(27, 13);
            this.variableDMinLbl.TabIndex = 43;
            this.variableDMinLbl.Text = "Min:";
            // 
            // variableDMin
            // 
            this.variableDMin.Location = new System.Drawing.Point(296, 110);
            this.variableDMin.Name = "variableDMin";
            this.variableDMin.Size = new System.Drawing.Size(40, 20);
            this.variableDMin.TabIndex = 42;
            this.variableDMin.Text = "0";
            // 
            // variableDMax
            // 
            this.variableDMax.Location = new System.Drawing.Point(296, 84);
            this.variableDMax.Name = "variableDMax";
            this.variableDMax.Size = new System.Drawing.Size(40, 20);
            this.variableDMax.TabIndex = 41;
            this.variableDMax.Text = "1";
            // 
            // variableDMaxLbl
            // 
            this.variableDMaxLbl.AutoSize = true;
            this.variableDMaxLbl.Location = new System.Drawing.Point(266, 87);
            this.variableDMaxLbl.Name = "variableDMaxLbl";
            this.variableDMaxLbl.Size = new System.Drawing.Size(30, 13);
            this.variableDMaxLbl.TabIndex = 40;
            this.variableDMaxLbl.Text = "Max:";
            // 
            // variableDValue
            // 
            this.variableDValue.BackColor = System.Drawing.Color.White;
            this.variableDValue.Location = new System.Drawing.Point(296, 63);
            this.variableDValue.Name = "variableDValue";
            this.variableDValue.Size = new System.Drawing.Size(40, 16);
            this.variableDValue.TabIndex = 39;
            this.variableDValue.Text = "0";
            // 
            // variableDTitle
            // 
            this.variableDTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.variableDTitle.Location = new System.Drawing.Point(286, 14);
            this.variableDTitle.Name = "variableDTitle";
            this.variableDTitle.Size = new System.Drawing.Size(10, 13);
            this.variableDTitle.TabIndex = 37;
            this.variableDTitle.Text = "D";
            this.variableDTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // variableDKnob
            // 
            this.variableDKnob.BackColor = System.Drawing.Color.Transparent;
            this.variableDKnob.DrawRatio = 0.2F;
            this.variableDKnob.IndicatorColor = System.Drawing.Color.Black;
            this.variableDKnob.IndicatorOffset = 7F;
            this.variableDKnob.KnobCenter = ((System.Drawing.PointF)(resources.GetObject("variableDKnob.KnobCenter")));
            this.variableDKnob.KnobColor = System.Drawing.Color.Silver;
            this.variableDKnob.KnobRect = ((System.Drawing.RectangleF)(resources.GetObject("variableDKnob.KnobRect")));
            this.variableDKnob.Location = new System.Drawing.Point(296, 20);
            this.variableDKnob.MaxValue = 1F;
            this.variableDKnob.MinValue = 0F;
            this.variableDKnob.Name = "variableDKnob";
            this.variableDKnob.Renderer = null;
            this.variableDKnob.ScaleColor = System.Drawing.SystemColors.ControlDarkDark;
            this.variableDKnob.Size = new System.Drawing.Size(40, 40);
            this.variableDKnob.StepValue = 0.1F;
            this.variableDKnob.Style = LBSoft.IndustrialCtrls.Knobs.LBKnob.KnobStyle.Circular;
            this.variableDKnob.TabIndex = 38;
            this.variableDKnob.Value = 0F;
            this.variableDKnob.KnobChangeValue += new LBSoft.IndustrialCtrls.Knobs.KnobChangeValue(this.lbKnob_KnobChangeValue);
            // 
            // variableCMinLbl
            // 
            this.variableCMinLbl.AutoSize = true;
            this.variableCMinLbl.Location = new System.Drawing.Point(183, 113);
            this.variableCMinLbl.Name = "variableCMinLbl";
            this.variableCMinLbl.Size = new System.Drawing.Size(27, 13);
            this.variableCMinLbl.TabIndex = 36;
            this.variableCMinLbl.Text = "Min:";
            // 
            // variableCMin
            // 
            this.variableCMin.Location = new System.Drawing.Point(213, 110);
            this.variableCMin.Name = "variableCMin";
            this.variableCMin.Size = new System.Drawing.Size(40, 20);
            this.variableCMin.TabIndex = 35;
            this.variableCMin.Text = "0";
            // 
            // variableCMax
            // 
            this.variableCMax.Location = new System.Drawing.Point(213, 84);
            this.variableCMax.Name = "variableCMax";
            this.variableCMax.Size = new System.Drawing.Size(40, 20);
            this.variableCMax.TabIndex = 34;
            this.variableCMax.Text = "1";
            // 
            // variableCMaxLbl
            // 
            this.variableCMaxLbl.AutoSize = true;
            this.variableCMaxLbl.Location = new System.Drawing.Point(183, 87);
            this.variableCMaxLbl.Name = "variableCMaxLbl";
            this.variableCMaxLbl.Size = new System.Drawing.Size(30, 13);
            this.variableCMaxLbl.TabIndex = 33;
            this.variableCMaxLbl.Text = "Max:";
            // 
            // variableCValue
            // 
            this.variableCValue.BackColor = System.Drawing.Color.White;
            this.variableCValue.Location = new System.Drawing.Point(213, 63);
            this.variableCValue.Name = "variableCValue";
            this.variableCValue.Size = new System.Drawing.Size(40, 16);
            this.variableCValue.TabIndex = 32;
            this.variableCValue.Text = "0";
            // 
            // variableCTitle
            // 
            this.variableCTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.variableCTitle.Location = new System.Drawing.Point(203, 14);
            this.variableCTitle.Name = "variableCTitle";
            this.variableCTitle.Size = new System.Drawing.Size(10, 13);
            this.variableCTitle.TabIndex = 30;
            this.variableCTitle.Text = "C";
            this.variableCTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // variableCKnob
            // 
            this.variableCKnob.BackColor = System.Drawing.Color.Transparent;
            this.variableCKnob.DrawRatio = 0.2F;
            this.variableCKnob.IndicatorColor = System.Drawing.Color.Black;
            this.variableCKnob.IndicatorOffset = 7F;
            this.variableCKnob.KnobCenter = ((System.Drawing.PointF)(resources.GetObject("variableCKnob.KnobCenter")));
            this.variableCKnob.KnobColor = System.Drawing.Color.Silver;
            this.variableCKnob.KnobRect = ((System.Drawing.RectangleF)(resources.GetObject("variableCKnob.KnobRect")));
            this.variableCKnob.Location = new System.Drawing.Point(213, 20);
            this.variableCKnob.MaxValue = 1F;
            this.variableCKnob.MinValue = 0F;
            this.variableCKnob.Name = "variableCKnob";
            this.variableCKnob.Renderer = null;
            this.variableCKnob.ScaleColor = System.Drawing.SystemColors.ControlDarkDark;
            this.variableCKnob.Size = new System.Drawing.Size(40, 40);
            this.variableCKnob.StepValue = 0.1F;
            this.variableCKnob.Style = LBSoft.IndustrialCtrls.Knobs.LBKnob.KnobStyle.Circular;
            this.variableCKnob.TabIndex = 31;
            this.variableCKnob.Value = 0F;
            this.variableCKnob.KnobChangeValue += new LBSoft.IndustrialCtrls.Knobs.KnobChangeValue(this.lbKnob_KnobChangeValue);
            // 
            // variableBMinLbl
            // 
            this.variableBMinLbl.AutoSize = true;
            this.variableBMinLbl.Location = new System.Drawing.Point(100, 113);
            this.variableBMinLbl.Name = "variableBMinLbl";
            this.variableBMinLbl.Size = new System.Drawing.Size(27, 13);
            this.variableBMinLbl.TabIndex = 29;
            this.variableBMinLbl.Text = "Min:";
            // 
            // variableBMin
            // 
            this.variableBMin.Location = new System.Drawing.Point(130, 110);
            this.variableBMin.Name = "variableBMin";
            this.variableBMin.Size = new System.Drawing.Size(40, 20);
            this.variableBMin.TabIndex = 28;
            this.variableBMin.Text = "0";
            // 
            // variableBMax
            // 
            this.variableBMax.Location = new System.Drawing.Point(130, 84);
            this.variableBMax.Name = "variableBMax";
            this.variableBMax.Size = new System.Drawing.Size(40, 20);
            this.variableBMax.TabIndex = 27;
            this.variableBMax.Text = "1";
            // 
            // variableBMaxLbl
            // 
            this.variableBMaxLbl.AutoSize = true;
            this.variableBMaxLbl.Location = new System.Drawing.Point(100, 87);
            this.variableBMaxLbl.Name = "variableBMaxLbl";
            this.variableBMaxLbl.Size = new System.Drawing.Size(30, 13);
            this.variableBMaxLbl.TabIndex = 26;
            this.variableBMaxLbl.Text = "Max:";
            // 
            // variableBValue
            // 
            this.variableBValue.BackColor = System.Drawing.Color.White;
            this.variableBValue.Location = new System.Drawing.Point(130, 63);
            this.variableBValue.Name = "variableBValue";
            this.variableBValue.Size = new System.Drawing.Size(40, 16);
            this.variableBValue.TabIndex = 25;
            this.variableBValue.Text = "0";
            // 
            // variableBTitle
            // 
            this.variableBTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.variableBTitle.Location = new System.Drawing.Point(120, 14);
            this.variableBTitle.Name = "variableBTitle";
            this.variableBTitle.Size = new System.Drawing.Size(10, 13);
            this.variableBTitle.TabIndex = 23;
            this.variableBTitle.Text = "B";
            this.variableBTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // variableBKnob
            // 
            this.variableBKnob.BackColor = System.Drawing.Color.Transparent;
            this.variableBKnob.DrawRatio = 0.2F;
            this.variableBKnob.IndicatorColor = System.Drawing.Color.Black;
            this.variableBKnob.IndicatorOffset = 7F;
            this.variableBKnob.KnobCenter = ((System.Drawing.PointF)(resources.GetObject("variableBKnob.KnobCenter")));
            this.variableBKnob.KnobColor = System.Drawing.Color.Silver;
            this.variableBKnob.KnobRect = ((System.Drawing.RectangleF)(resources.GetObject("variableBKnob.KnobRect")));
            this.variableBKnob.Location = new System.Drawing.Point(130, 20);
            this.variableBKnob.MaxValue = 1F;
            this.variableBKnob.MinValue = 0F;
            this.variableBKnob.Name = "variableBKnob";
            this.variableBKnob.Renderer = null;
            this.variableBKnob.ScaleColor = System.Drawing.SystemColors.ControlDarkDark;
            this.variableBKnob.Size = new System.Drawing.Size(40, 40);
            this.variableBKnob.StepValue = 0.1F;
            this.variableBKnob.Style = LBSoft.IndustrialCtrls.Knobs.LBKnob.KnobStyle.Circular;
            this.variableBKnob.TabIndex = 24;
            this.variableBKnob.Value = 0F;
            this.variableBKnob.KnobChangeValue += new LBSoft.IndustrialCtrls.Knobs.KnobChangeValue(this.lbKnob_KnobChangeValue);
            // 
            // variableAMinLbl
            // 
            this.variableAMinLbl.AutoSize = true;
            this.variableAMinLbl.Location = new System.Drawing.Point(17, 114);
            this.variableAMinLbl.Name = "variableAMinLbl";
            this.variableAMinLbl.Size = new System.Drawing.Size(27, 13);
            this.variableAMinLbl.TabIndex = 22;
            this.variableAMinLbl.Text = "Min:";
            // 
            // variableAMin
            // 
            this.variableAMin.Location = new System.Drawing.Point(47, 111);
            this.variableAMin.Name = "variableAMin";
            this.variableAMin.Size = new System.Drawing.Size(40, 20);
            this.variableAMin.TabIndex = 21;
            this.variableAMin.Text = "0";
            // 
            // variableAMax
            // 
            this.variableAMax.Location = new System.Drawing.Point(47, 85);
            this.variableAMax.Name = "variableAMax";
            this.variableAMax.Size = new System.Drawing.Size(40, 20);
            this.variableAMax.TabIndex = 20;
            this.variableAMax.Text = "1";
            // 
            // variableAMaxLbl
            // 
            this.variableAMaxLbl.AutoSize = true;
            this.variableAMaxLbl.Location = new System.Drawing.Point(17, 88);
            this.variableAMaxLbl.Name = "variableAMaxLbl";
            this.variableAMaxLbl.Size = new System.Drawing.Size(30, 13);
            this.variableAMaxLbl.TabIndex = 19;
            this.variableAMaxLbl.Text = "Max:";
            // 
            // variableAValue
            // 
            this.variableAValue.BackColor = System.Drawing.Color.White;
            this.variableAValue.Location = new System.Drawing.Point(47, 64);
            this.variableAValue.Name = "variableAValue";
            this.variableAValue.Size = new System.Drawing.Size(40, 16);
            this.variableAValue.TabIndex = 18;
            this.variableAValue.Text = "0";
            // 
            // variableATitle
            // 
            this.variableATitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.variableATitle.Location = new System.Drawing.Point(37, 15);
            this.variableATitle.Name = "variableATitle";
            this.variableATitle.Size = new System.Drawing.Size(10, 13);
            this.variableATitle.TabIndex = 0;
            this.variableATitle.Text = "A";
            this.variableATitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // variableAKnob
            // 
            this.variableAKnob.BackColor = System.Drawing.Color.Transparent;
            this.variableAKnob.DrawRatio = 0.2F;
            this.variableAKnob.IndicatorColor = System.Drawing.Color.Black;
            this.variableAKnob.IndicatorOffset = 7F;
            this.variableAKnob.KnobCenter = ((System.Drawing.PointF)(resources.GetObject("variableAKnob.KnobCenter")));
            this.variableAKnob.KnobColor = System.Drawing.Color.Silver;
            this.variableAKnob.KnobRect = ((System.Drawing.RectangleF)(resources.GetObject("variableAKnob.KnobRect")));
            this.variableAKnob.Location = new System.Drawing.Point(47, 21);
            this.variableAKnob.MaxValue = 1F;
            this.variableAKnob.MinValue = 0F;
            this.variableAKnob.Name = "variableAKnob";
            this.variableAKnob.Renderer = null;
            this.variableAKnob.ScaleColor = System.Drawing.SystemColors.ControlDarkDark;
            this.variableAKnob.Size = new System.Drawing.Size(40, 40);
            this.variableAKnob.StepValue = 0.1F;
            this.variableAKnob.Style = LBSoft.IndustrialCtrls.Knobs.LBKnob.KnobStyle.Circular;
            this.variableAKnob.TabIndex = 17;
            this.variableAKnob.Value = 0F;
            this.variableAKnob.KnobChangeValue += new LBSoft.IndustrialCtrls.Knobs.KnobChangeValue(this.lbKnob_KnobChangeValue);
            // 
            // mappingListView
            // 
            this.mappingListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.inTypeColHeader,
            this.inChannelsColHeader,
            this.inParamsColHeader,
            this.outTypeColHeader,
            this.outChannelsColHeader,
            this.outParamsColHeader,
            this.overrideColHeader});
            this.mappingListView.Font = new System.Drawing.Font("Arial", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mappingListView.FullRowSelect = true;
            this.mappingListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.mappingListView.HideSelection = false;
            this.mappingListView.Location = new System.Drawing.Point(8, 16);
            this.mappingListView.MultiSelect = false;
            this.mappingListView.Name = "mappingListView";
            this.mappingListView.Size = new System.Drawing.Size(337, 118);
            this.mappingListView.TabIndex = 13;
            this.mappingListView.UseCompatibleStateImageBehavior = false;
            this.mappingListView.View = System.Windows.Forms.View.Details;
            this.mappingListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.mappingListView_ItemSelectionChanged);
            // 
            // inTypeColHeader
            // 
            this.inTypeColHeader.Text = "In Type";
            this.inTypeColHeader.Width = 57;
            // 
            // inChannelsColHeader
            // 
            this.inChannelsColHeader.Text = "Chans";
            this.inChannelsColHeader.Width = 40;
            // 
            // inParamsColHeader
            // 
            this.inParamsColHeader.Text = "Params";
            this.inParamsColHeader.Width = 45;
            // 
            // outTypeColHeader
            // 
            this.outTypeColHeader.Text = "Out Type";
            this.outTypeColHeader.Width = 57;
            // 
            // outChannelsColHeader
            // 
            this.outChannelsColHeader.Text = "Chans";
            this.outChannelsColHeader.Width = 40;
            // 
            // outParamsColHeader
            // 
            this.outParamsColHeader.Text = "Params";
            this.outParamsColHeader.Width = 45;
            // 
            // overrideColHeader
            // 
            this.overrideColHeader.Text = "Override";
            this.overrideColHeader.Width = 49;
            // 
            // mappingGroupBox
            // 
            this.mappingGroupBox.Controls.Add(this.moveMappingDownBtn);
            this.mappingGroupBox.Controls.Add(this.mappingListView);
            this.mappingGroupBox.Controls.Add(this.moveMappingUpBtn);
            this.mappingGroupBox.Controls.Add(this.addMappingBtn);
            this.mappingGroupBox.Controls.Add(this.editMappingBtn);
            this.mappingGroupBox.Controls.Add(this.deleteMappingBtn);
            this.mappingGroupBox.Location = new System.Drawing.Point(5, 35);
            this.mappingGroupBox.Name = "mappingGroupBox";
            this.mappingGroupBox.Size = new System.Drawing.Size(351, 164);
            this.mappingGroupBox.TabIndex = 14;
            this.mappingGroupBox.TabStop = false;
            this.mappingGroupBox.Text = "Mapings";
            // 
            // moveMappingDownBtn
            // 
            this.moveMappingDownBtn.BackgroundImage = global::MidiShapeShifter.Properties.Resources.imgDownBlue;
            this.moveMappingDownBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.moveMappingDownBtn.Location = new System.Drawing.Point(105, 137);
            this.moveMappingDownBtn.Margin = new System.Windows.Forms.Padding(0);
            this.moveMappingDownBtn.Name = "moveMappingDownBtn";
            this.moveMappingDownBtn.Size = new System.Drawing.Size(24, 24);
            this.moveMappingDownBtn.TabIndex = 12;
            this.moveMappingDownBtn.UseVisualStyleBackColor = true;
            this.moveMappingDownBtn.EnabledChanged += new System.EventHandler(this.moveDownBtn_EnabledChanged);
            this.moveMappingDownBtn.Click += new System.EventHandler(this.moveMappingDownBtn_Click);
            // 
            // moveMappingUpBtn
            // 
            this.moveMappingUpBtn.BackgroundImage = global::MidiShapeShifter.Properties.Resources.imgUpBlue;
            this.moveMappingUpBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.moveMappingUpBtn.Location = new System.Drawing.Point(81, 137);
            this.moveMappingUpBtn.Margin = new System.Windows.Forms.Padding(0);
            this.moveMappingUpBtn.Name = "moveMappingUpBtn";
            this.moveMappingUpBtn.Size = new System.Drawing.Size(24, 24);
            this.moveMappingUpBtn.TabIndex = 11;
            this.moveMappingUpBtn.UseVisualStyleBackColor = true;
            this.moveMappingUpBtn.EnabledChanged += new System.EventHandler(this.moveUpBtn_EnabledChanged);
            this.moveMappingUpBtn.Click += new System.EventHandler(this.moveMappingUpBtn_Click);
            // 
            // addMappingBtn
            // 
            this.addMappingBtn.BackgroundImage = global::MidiShapeShifter.Properties.Resources.imgAddBlue;
            this.addMappingBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.addMappingBtn.Location = new System.Drawing.Point(8, 137);
            this.addMappingBtn.Margin = new System.Windows.Forms.Padding(0);
            this.addMappingBtn.Name = "addMappingBtn";
            this.addMappingBtn.Size = new System.Drawing.Size(24, 24);
            this.addMappingBtn.TabIndex = 8;
            this.addMappingBtn.UseVisualStyleBackColor = true;
            this.addMappingBtn.Click += new System.EventHandler(this.addMappingBtn_Click);
            // 
            // editMappingBtn
            // 
            this.editMappingBtn.BackgroundImage = global::MidiShapeShifter.Properties.Resources.imgEditBlue;
            this.editMappingBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.editMappingBtn.Location = new System.Drawing.Point(57, 137);
            this.editMappingBtn.Margin = new System.Windows.Forms.Padding(0);
            this.editMappingBtn.Name = "editMappingBtn";
            this.editMappingBtn.Size = new System.Drawing.Size(24, 24);
            this.editMappingBtn.TabIndex = 10;
            this.editMappingBtn.UseVisualStyleBackColor = true;
            this.editMappingBtn.EnabledChanged += new System.EventHandler(this.editBtn_EnabledChanged);
            this.editMappingBtn.Click += new System.EventHandler(this.editMappingBtn_Click);
            // 
            // deleteMappingBtn
            // 
            this.deleteMappingBtn.BackgroundImage = global::MidiShapeShifter.Properties.Resources.imgDeleteBlue;
            this.deleteMappingBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.deleteMappingBtn.Location = new System.Drawing.Point(32, 137);
            this.deleteMappingBtn.Margin = new System.Windows.Forms.Padding(0);
            this.deleteMappingBtn.Name = "deleteMappingBtn";
            this.deleteMappingBtn.Size = new System.Drawing.Size(24, 24);
            this.deleteMappingBtn.TabIndex = 9;
            this.deleteMappingBtn.UseVisualStyleBackColor = true;
            this.deleteMappingBtn.EnabledChanged += new System.EventHandler(this.deleteBtn_EnabledChanged);
            this.deleteMappingBtn.Click += new System.EventHandler(this.deleteMappingBtn_Click);
            // 
            // generatorGroupBox
            // 
            this.generatorGroupBox.BackColor = System.Drawing.SystemColors.Control;
            this.generatorGroupBox.Controls.Add(this.generatorListView);
            this.generatorGroupBox.Controls.Add(this.addGeneratorBtn);
            this.generatorGroupBox.Controls.Add(this.editGeneratorBtn);
            this.generatorGroupBox.Controls.Add(this.deleteGeneratorBtn);
            this.generatorGroupBox.Location = new System.Drawing.Point(5, 205);
            this.generatorGroupBox.Name = "generatorGroupBox";
            this.generatorGroupBox.Size = new System.Drawing.Size(351, 133);
            this.generatorGroupBox.TabIndex = 15;
            this.generatorGroupBox.TabStop = false;
            this.generatorGroupBox.Text = "Generators";
            // 
            // generatorListView
            // 
            this.generatorListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.generaotrNameColHeader,
            this.generatorPeriodColHeader,
            this.generatorLoopColHeader,
            this.generatorEnabledColHeader});
            this.generatorListView.Font = new System.Drawing.Font("Arial", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.generatorListView.FullRowSelect = true;
            this.generatorListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.generatorListView.HideSelection = false;
            this.generatorListView.Location = new System.Drawing.Point(8, 19);
            this.generatorListView.MultiSelect = false;
            this.generatorListView.Name = "generatorListView";
            this.generatorListView.Size = new System.Drawing.Size(337, 84);
            this.generatorListView.TabIndex = 13;
            this.generatorListView.UseCompatibleStateImageBehavior = false;
            this.generatorListView.View = System.Windows.Forms.View.Details;
            this.generatorListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.generatorListView_ItemSelectionChanged);
            // 
            // generaotrNameColHeader
            // 
            this.generaotrNameColHeader.Text = "Name";
            this.generaotrNameColHeader.Width = 133;
            // 
            // generatorPeriodColHeader
            // 
            this.generatorPeriodColHeader.Text = "Period";
            this.generatorPeriodColHeader.Width = 92;
            // 
            // generatorLoopColHeader
            // 
            this.generatorLoopColHeader.Text = "Loop";
            this.generatorLoopColHeader.Width = 50;
            // 
            // generatorEnabledColHeader
            // 
            this.generatorEnabledColHeader.Text = "Enabled";
            this.generatorEnabledColHeader.Width = 58;
            // 
            // addGeneratorBtn
            // 
            this.addGeneratorBtn.BackgroundImage = global::MidiShapeShifter.Properties.Resources.imgAddBlue;
            this.addGeneratorBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.addGeneratorBtn.Location = new System.Drawing.Point(8, 103);
            this.addGeneratorBtn.Margin = new System.Windows.Forms.Padding(0);
            this.addGeneratorBtn.Name = "addGeneratorBtn";
            this.addGeneratorBtn.Size = new System.Drawing.Size(24, 24);
            this.addGeneratorBtn.TabIndex = 8;
            this.addGeneratorBtn.UseVisualStyleBackColor = true;
            this.addGeneratorBtn.Click += new System.EventHandler(this.addGeneratorBtn_Click);
            // 
            // editGeneratorBtn
            // 
            this.editGeneratorBtn.BackgroundImage = global::MidiShapeShifter.Properties.Resources.imgEditBlue;
            this.editGeneratorBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.editGeneratorBtn.Location = new System.Drawing.Point(56, 103);
            this.editGeneratorBtn.Margin = new System.Windows.Forms.Padding(0);
            this.editGeneratorBtn.Name = "editGeneratorBtn";
            this.editGeneratorBtn.Size = new System.Drawing.Size(24, 24);
            this.editGeneratorBtn.TabIndex = 10;
            this.editGeneratorBtn.UseVisualStyleBackColor = true;
            this.editGeneratorBtn.EnabledChanged += new System.EventHandler(this.editBtn_EnabledChanged);
            this.editGeneratorBtn.Click += new System.EventHandler(this.editGeneratorBtn_Click);
            // 
            // deleteGeneratorBtn
            // 
            this.deleteGeneratorBtn.BackgroundImage = global::MidiShapeShifter.Properties.Resources.imgDeleteBlue;
            this.deleteGeneratorBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.deleteGeneratorBtn.Location = new System.Drawing.Point(32, 103);
            this.deleteGeneratorBtn.Margin = new System.Windows.Forms.Padding(0);
            this.deleteGeneratorBtn.Name = "deleteGeneratorBtn";
            this.deleteGeneratorBtn.Size = new System.Drawing.Size(24, 24);
            this.deleteGeneratorBtn.TabIndex = 9;
            this.deleteGeneratorBtn.UseVisualStyleBackColor = true;
            this.deleteGeneratorBtn.EnabledChanged += new System.EventHandler(this.deleteBtn_EnabledChanged);
            this.deleteGeneratorBtn.Click += new System.EventHandler(this.deleteGeneratorBtn_Click);
            // 
            // toolStrip
            // 
            this.toolStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(236)))));
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openProgram,
            this.saveSplitButton,
            this.programList});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(641, 32);
            this.toolStrip.TabIndex = 16;
            this.toolStrip.Text = "Tool Strip";
            // 
            // openProgram
            // 
            this.openProgram.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openProgram.Image = global::MidiShapeShifter.Properties.Resources.imgOpenBlue;
            this.openProgram.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openProgram.Name = "openProgram";
            this.openProgram.Size = new System.Drawing.Size(23, 29);
            this.openProgram.Text = "Open Program";
            this.openProgram.Click += new System.EventHandler(this.openProgram_Click);
            // 
            // saveSplitButton
            // 
            this.saveSplitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveSplitButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveMenuItem,
            this.saveAsMenuItem});
            this.saveSplitButton.Image = global::MidiShapeShifter.Properties.Resources.imgSaveBlue;
            this.saveSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveSplitButton.Name = "saveSplitButton";
            this.saveSplitButton.Size = new System.Drawing.Size(32, 29);
            this.saveSplitButton.Text = "Save Program";
            this.saveSplitButton.ButtonClick += new System.EventHandler(this.saveProgram_Click);
            // 
            // saveMenuItem
            // 
            this.saveMenuItem.Image = global::MidiShapeShifter.Properties.Resources.imgSaveBlue;
            this.saveMenuItem.Name = "saveMenuItem";
            this.saveMenuItem.Size = new System.Drawing.Size(172, 22);
            this.saveMenuItem.Text = "Save Program";
            this.saveMenuItem.Click += new System.EventHandler(this.saveProgram_Click);
            // 
            // saveAsMenuItem
            // 
            this.saveAsMenuItem.Name = "saveAsMenuItem";
            this.saveAsMenuItem.Size = new System.Drawing.Size(172, 22);
            this.saveAsMenuItem.Text = "Save Program As...";
            this.saveAsMenuItem.Click += new System.EventHandler(this.saveProgramAs_Click);
            // 
            // programList
            // 
            this.programList.AutoSize = false;
            this.programList.BackColor = System.Drawing.Color.White;
            this.programList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.programList.Image = ((System.Drawing.Image)(resources.GetObject("programList.Image")));
            this.programList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.programList.Margin = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.programList.Name = "programList";
            this.programList.Size = new System.Drawing.Size(160, 22);
            this.programList.Text = "Blank";
            this.programList.ToolTipText = "Select a Program";
            // 
            // PluginEditorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.generatorGroupBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.curveGroup);
            this.Controls.Add(this.mainGraphControl);
            this.Controls.Add(this.mappingGroupBox);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "PluginEditorView";
            this.Size = new System.Drawing.Size(641, 488);
            this.curveGroup.ResumeLayout(false);
            this.curveGroup.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.mappingGroupBox.ResumeLayout(false);
            this.generatorGroupBox.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ZedGraph.ZedGraphControl mainGraphControl;
        private LBSoft.IndustrialCtrls.Knobs.LBKnob presetParam1Knob;
        private System.Windows.Forms.Label presetParam1Value;
        private System.Windows.Forms.GroupBox curveGroup;
        private System.Windows.Forms.RadioButton curveShapePresetRadio;
        private System.Windows.Forms.RadioButton curveShapeEquationRadio;
        private System.Windows.Forms.Label presetParam1Title;
        private System.Windows.Forms.ComboBox curvePresetCombo;
        private System.Windows.Forms.TextBox curveEquationTextBox;
        private System.Windows.Forms.Label presetParam4Title;
        private System.Windows.Forms.Label presetParam4Value;
        private LBSoft.IndustrialCtrls.Knobs.LBKnob presetParam4Knob;
        private System.Windows.Forms.Label presetParam3Title;
        private System.Windows.Forms.Label presetParam3Value;
        private LBSoft.IndustrialCtrls.Knobs.LBKnob presetParam3Knob;
        private System.Windows.Forms.Label presetParam2Title;
        private System.Windows.Forms.Label presetParam2Value;
        private LBSoft.IndustrialCtrls.Knobs.LBKnob presetParam2Knob;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label variableATitle;
        private System.Windows.Forms.Label variableAValue;
        private LBSoft.IndustrialCtrls.Knobs.LBKnob variableAKnob;
        private System.Windows.Forms.Label variableAMaxLbl;
        private System.Windows.Forms.Label variableAMinLbl;
        private System.Windows.Forms.TextBox variableAMin;
        private System.Windows.Forms.TextBox variableAMax;
        private System.Windows.Forms.Label variableDMinLbl;
        private System.Windows.Forms.TextBox variableDMin;
        private System.Windows.Forms.TextBox variableDMax;
        private System.Windows.Forms.Label variableDMaxLbl;
        private System.Windows.Forms.Label variableDValue;
        private System.Windows.Forms.Label variableDTitle;
        private LBSoft.IndustrialCtrls.Knobs.LBKnob variableDKnob;
        private System.Windows.Forms.Label variableCMinLbl;
        private System.Windows.Forms.TextBox variableCMin;
        private System.Windows.Forms.TextBox variableCMax;
        private System.Windows.Forms.Label variableCMaxLbl;
        private System.Windows.Forms.Label variableCValue;
        private System.Windows.Forms.Label variableCTitle;
        private LBSoft.IndustrialCtrls.Knobs.LBKnob variableCKnob;
        private System.Windows.Forms.Label variableBMinLbl;
        private System.Windows.Forms.TextBox variableBMin;
        private System.Windows.Forms.TextBox variableBMax;
        private System.Windows.Forms.Label variableBMaxLbl;
        private System.Windows.Forms.Label variableBValue;
        private System.Windows.Forms.Label variableBTitle;
        private LBSoft.IndustrialCtrls.Knobs.LBKnob variableBKnob;
        private System.Windows.Forms.Button equationBookBtn;
        private System.Windows.Forms.Button addMappingBtn;
        private System.Windows.Forms.Button deleteMappingBtn;
        private System.Windows.Forms.Button editMappingBtn;
        private System.Windows.Forms.Button moveMappingUpBtn;
        private System.Windows.Forms.Button moveMappingDownBtn;
        private System.Windows.Forms.ListView mappingListView;
        private System.Windows.Forms.ColumnHeader inTypeColHeader;
        private System.Windows.Forms.ColumnHeader inChannelsColHeader;
        private System.Windows.Forms.ColumnHeader inParamsColHeader;
        private System.Windows.Forms.ColumnHeader outTypeColHeader;
        private System.Windows.Forms.ColumnHeader outChannelsColHeader;
        private System.Windows.Forms.ColumnHeader outParamsColHeader;
        private System.Windows.Forms.ColumnHeader overrideColHeader;
        private System.Windows.Forms.GroupBox mappingGroupBox;
        private System.Windows.Forms.GroupBox generatorGroupBox;
        private System.Windows.Forms.ListView generatorListView;
        private System.Windows.Forms.ColumnHeader generaotrNameColHeader;
        private System.Windows.Forms.Button addGeneratorBtn;
        private System.Windows.Forms.Button editGeneratorBtn;
        private System.Windows.Forms.Button deleteGeneratorBtn;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton openProgram;
        private System.Windows.Forms.ColumnHeader generatorPeriodColHeader;
        private System.Windows.Forms.ColumnHeader generatorLoopColHeader;
        private System.Windows.Forms.ColumnHeader generatorEnabledColHeader;
        private System.Windows.Forms.ToolStripDropDownButton programList;
        private System.Windows.Forms.ToolStripSplitButton saveSplitButton;
        private System.Windows.Forms.ToolStripMenuItem saveMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsMenuItem;

    }
}

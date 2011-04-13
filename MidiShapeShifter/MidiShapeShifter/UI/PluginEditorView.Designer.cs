namespace MidiShapeShifter.UI
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PluginEditorView));
            this.zedGraphControl1 = new ZedGraph.ZedGraphControl();
            this.presetParam1Knob = new LBSoft.IndustrialCtrls.Knobs.LBKnob();
            this.presetParam1Value = new System.Windows.Forms.Label();
            this.mappingList = new System.Windows.Forms.ListBox();
            this.curveGroup = new System.Windows.Forms.GroupBox();
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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.radioCurvePreset = new System.Windows.Forms.RadioButton();
            this.radioCurveEquation = new System.Windows.Forms.RadioButton();
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
            this.curveGroup.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // zedGraphControl1
            // 
            this.zedGraphControl1.Location = new System.Drawing.Point(360, 14);
            this.zedGraphControl1.Name = "zedGraphControl1";
            this.zedGraphControl1.ScrollGrace = 0;
            this.zedGraphControl1.ScrollMaxX = 0;
            this.zedGraphControl1.ScrollMaxY = 0;
            this.zedGraphControl1.ScrollMaxY2 = 0;
            this.zedGraphControl1.ScrollMinX = 0;
            this.zedGraphControl1.ScrollMinY = 0;
            this.zedGraphControl1.ScrollMinY2 = 0;
            this.zedGraphControl1.Size = new System.Drawing.Size(275, 275);
            this.zedGraphControl1.TabIndex = 1;
            // 
            // presetParam1Knob
            // 
            this.presetParam1Knob.BackColor = System.Drawing.Color.Transparent;
            this.presetParam1Knob.DrawRatio = 0.2F;
            this.presetParam1Knob.IndicatorColor = System.Drawing.Color.Black;
            this.presetParam1Knob.IndicatorOffset = 7F;
            this.presetParam1Knob.KnobCenter = ((System.Drawing.PointF)(resources.GetObject("presetParam1Knob.KnobCenter")));
            this.presetParam1Knob.KnobColor = System.Drawing.Color.Silver;
            this.presetParam1Knob.KnobRect = ((System.Drawing.RectangleF)(resources.GetObject("presetParam1Knob.KnobRect")));
            this.presetParam1Knob.Location = new System.Drawing.Point(28, 101);
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
            this.presetParam1Knob.KnobChangeValue += new LBSoft.IndustrialCtrls.Knobs.KnobChangeValue(this.testKnob_KnobChangeValue);
            // 
            // presetParam1Value
            // 
            this.presetParam1Value.BackColor = System.Drawing.Color.White;
            this.presetParam1Value.Location = new System.Drawing.Point(28, 144);
            this.presetParam1Value.Name = "presetParam1Value";
            this.presetParam1Value.Size = new System.Drawing.Size(40, 16);
            this.presetParam1Value.TabIndex = 4;
            this.presetParam1Value.Text = "0";
            // 
            // mappingList
            // 
            this.mappingList.FormattingEnabled = true;
            this.mappingList.Location = new System.Drawing.Point(3, 14);
            this.mappingList.Name = "mappingList";
            this.mappingList.Size = new System.Drawing.Size(337, 277);
            this.mappingList.TabIndex = 5;
            // 
            // curveGroup
            // 
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
            this.curveGroup.Controls.Add(this.comboBox1);
            this.curveGroup.Controls.Add(this.presetParam1Value);
            this.curveGroup.Controls.Add(this.textBox1);
            this.curveGroup.Controls.Add(this.presetParam1Knob);
            this.curveGroup.Controls.Add(this.radioCurvePreset);
            this.curveGroup.Controls.Add(this.radioCurveEquation);
            this.curveGroup.Location = new System.Drawing.Point(360, 318);
            this.curveGroup.Name = "curveGroup";
            this.curveGroup.Size = new System.Drawing.Size(273, 166);
            this.curveGroup.TabIndex = 6;
            this.curveGroup.TabStop = false;
            this.curveGroup.Text = "Curve Shape";
            // 
            // presetParam4Title
            // 
            this.presetParam4Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.presetParam4Title.Location = new System.Drawing.Point(198, 85);
            this.presetParam4Title.Name = "presetParam4Title";
            this.presetParam4Title.Size = new System.Drawing.Size(60, 13);
            this.presetParam4Title.TabIndex = 16;
            this.presetParam4Title.Text = "Param4";
            this.presetParam4Title.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // presetParam4Value
            // 
            this.presetParam4Value.BackColor = System.Drawing.Color.White;
            this.presetParam4Value.Location = new System.Drawing.Point(208, 144);
            this.presetParam4Value.Name = "presetParam4Value";
            this.presetParam4Value.Size = new System.Drawing.Size(40, 16);
            this.presetParam4Value.TabIndex = 15;
            this.presetParam4Value.Text = "0";
            // 
            // presetParam4Knob
            // 
            this.presetParam4Knob.BackColor = System.Drawing.Color.Transparent;
            this.presetParam4Knob.DrawRatio = 0.2F;
            this.presetParam4Knob.IndicatorColor = System.Drawing.Color.Black;
            this.presetParam4Knob.IndicatorOffset = 7F;
            this.presetParam4Knob.KnobCenter = ((System.Drawing.PointF)(resources.GetObject("presetParam4Knob.KnobCenter")));
            this.presetParam4Knob.KnobColor = System.Drawing.Color.Silver;
            this.presetParam4Knob.KnobRect = ((System.Drawing.RectangleF)(resources.GetObject("presetParam4Knob.KnobRect")));
            this.presetParam4Knob.Location = new System.Drawing.Point(208, 101);
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
            // 
            // presetParam3Title
            // 
            this.presetParam3Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.presetParam3Title.Location = new System.Drawing.Point(138, 85);
            this.presetParam3Title.Name = "presetParam3Title";
            this.presetParam3Title.Size = new System.Drawing.Size(60, 13);
            this.presetParam3Title.TabIndex = 13;
            this.presetParam3Title.Text = "Param3";
            this.presetParam3Title.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // presetParam3Value
            // 
            this.presetParam3Value.BackColor = System.Drawing.Color.White;
            this.presetParam3Value.Location = new System.Drawing.Point(148, 144);
            this.presetParam3Value.Name = "presetParam3Value";
            this.presetParam3Value.Size = new System.Drawing.Size(40, 16);
            this.presetParam3Value.TabIndex = 12;
            this.presetParam3Value.Text = "0";
            // 
            // presetParam3Knob
            // 
            this.presetParam3Knob.BackColor = System.Drawing.Color.Transparent;
            this.presetParam3Knob.DrawRatio = 0.2F;
            this.presetParam3Knob.IndicatorColor = System.Drawing.Color.Black;
            this.presetParam3Knob.IndicatorOffset = 7F;
            this.presetParam3Knob.KnobCenter = ((System.Drawing.PointF)(resources.GetObject("presetParam3Knob.KnobCenter")));
            this.presetParam3Knob.KnobColor = System.Drawing.Color.Silver;
            this.presetParam3Knob.KnobRect = ((System.Drawing.RectangleF)(resources.GetObject("presetParam3Knob.KnobRect")));
            this.presetParam3Knob.Location = new System.Drawing.Point(148, 101);
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
            // 
            // presetParam2Title
            // 
            this.presetParam2Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.presetParam2Title.Location = new System.Drawing.Point(78, 85);
            this.presetParam2Title.Name = "presetParam2Title";
            this.presetParam2Title.Size = new System.Drawing.Size(60, 13);
            this.presetParam2Title.TabIndex = 10;
            this.presetParam2Title.Text = "Param2";
            this.presetParam2Title.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // presetParam2Value
            // 
            this.presetParam2Value.BackColor = System.Drawing.Color.White;
            this.presetParam2Value.Location = new System.Drawing.Point(88, 144);
            this.presetParam2Value.Name = "presetParam2Value";
            this.presetParam2Value.Size = new System.Drawing.Size(40, 16);
            this.presetParam2Value.TabIndex = 9;
            this.presetParam2Value.Text = "0";
            // 
            // presetParam2Knob
            // 
            this.presetParam2Knob.BackColor = System.Drawing.Color.Transparent;
            this.presetParam2Knob.DrawRatio = 0.2F;
            this.presetParam2Knob.IndicatorColor = System.Drawing.Color.Black;
            this.presetParam2Knob.IndicatorOffset = 7F;
            this.presetParam2Knob.KnobCenter = ((System.Drawing.PointF)(resources.GetObject("presetParam2Knob.KnobCenter")));
            this.presetParam2Knob.KnobColor = System.Drawing.Color.Silver;
            this.presetParam2Knob.KnobRect = ((System.Drawing.RectangleF)(resources.GetObject("presetParam2Knob.KnobRect")));
            this.presetParam2Knob.Location = new System.Drawing.Point(88, 101);
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
            // 
            // presetParam1Title
            // 
            this.presetParam1Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.presetParam1Title.Location = new System.Drawing.Point(18, 85);
            this.presetParam1Title.Name = "presetParam1Title";
            this.presetParam1Title.Size = new System.Drawing.Size(60, 13);
            this.presetParam1Title.TabIndex = 7;
            this.presetParam1Title.Text = "Param1";
            this.presetParam1Title.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(86, 59);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(178, 21);
            this.comboBox1.TabIndex = 3;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(86, 28);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(178, 20);
            this.textBox1.TabIndex = 2;
            // 
            // radioCurvePreset
            // 
            this.radioCurvePreset.AutoSize = true;
            this.radioCurvePreset.Location = new System.Drawing.Point(13, 63);
            this.radioCurvePreset.Name = "radioCurvePreset";
            this.radioCurvePreset.Size = new System.Drawing.Size(58, 17);
            this.radioCurvePreset.TabIndex = 1;
            this.radioCurvePreset.TabStop = true;
            this.radioCurvePreset.Text = "Preset:";
            this.radioCurvePreset.UseVisualStyleBackColor = true;
            // 
            // radioCurveEquation
            // 
            this.radioCurveEquation.AutoSize = true;
            this.radioCurveEquation.Location = new System.Drawing.Point(13, 31);
            this.radioCurveEquation.Name = "radioCurveEquation";
            this.radioCurveEquation.Size = new System.Drawing.Size(70, 17);
            this.radioCurveEquation.TabIndex = 0;
            this.radioCurveEquation.TabStop = true;
            this.radioCurveEquation.Text = "Equation:";
            this.radioCurveEquation.UseVisualStyleBackColor = true;
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
            this.groupBox1.Location = new System.Drawing.Point(0, 318);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(340, 166);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Variables";
            // 
            // variableDMinLbl
            // 
            this.variableDMinLbl.AutoSize = true;
            this.variableDMinLbl.Location = new System.Drawing.Point(252, 136);
            this.variableDMinLbl.Name = "variableDMinLbl";
            this.variableDMinLbl.Size = new System.Drawing.Size(27, 13);
            this.variableDMinLbl.TabIndex = 43;
            this.variableDMinLbl.Text = "Min:";
            // 
            // variableDMin
            // 
            this.variableDMin.Location = new System.Drawing.Point(282, 133);
            this.variableDMin.Name = "variableDMin";
            this.variableDMin.Size = new System.Drawing.Size(40, 20);
            this.variableDMin.TabIndex = 42;
            // 
            // variableDMax
            // 
            this.variableDMax.Location = new System.Drawing.Point(282, 107);
            this.variableDMax.Name = "variableDMax";
            this.variableDMax.Size = new System.Drawing.Size(40, 20);
            this.variableDMax.TabIndex = 41;
            // 
            // variableDMaxLbl
            // 
            this.variableDMaxLbl.AutoSize = true;
            this.variableDMaxLbl.Location = new System.Drawing.Point(252, 110);
            this.variableDMaxLbl.Name = "variableDMaxLbl";
            this.variableDMaxLbl.Size = new System.Drawing.Size(30, 13);
            this.variableDMaxLbl.TabIndex = 40;
            this.variableDMaxLbl.Text = "Max:";
            // 
            // variableDValue
            // 
            this.variableDValue.BackColor = System.Drawing.Color.White;
            this.variableDValue.Location = new System.Drawing.Point(282, 86);
            this.variableDValue.Name = "variableDValue";
            this.variableDValue.Size = new System.Drawing.Size(40, 16);
            this.variableDValue.TabIndex = 39;
            this.variableDValue.Text = "0";
            // 
            // variableDTitle
            // 
            this.variableDTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.variableDTitle.Location = new System.Drawing.Point(282, 27);
            this.variableDTitle.Name = "variableDTitle";
            this.variableDTitle.Size = new System.Drawing.Size(40, 13);
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
            this.variableDKnob.Location = new System.Drawing.Point(282, 43);
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
            // 
            // variableCMinLbl
            // 
            this.variableCMinLbl.AutoSize = true;
            this.variableCMinLbl.Location = new System.Drawing.Point(172, 136);
            this.variableCMinLbl.Name = "variableCMinLbl";
            this.variableCMinLbl.Size = new System.Drawing.Size(27, 13);
            this.variableCMinLbl.TabIndex = 36;
            this.variableCMinLbl.Text = "Min:";
            // 
            // variableCMin
            // 
            this.variableCMin.Location = new System.Drawing.Point(202, 133);
            this.variableCMin.Name = "variableCMin";
            this.variableCMin.Size = new System.Drawing.Size(40, 20);
            this.variableCMin.TabIndex = 35;
            // 
            // variableCMax
            // 
            this.variableCMax.Location = new System.Drawing.Point(202, 107);
            this.variableCMax.Name = "variableCMax";
            this.variableCMax.Size = new System.Drawing.Size(40, 20);
            this.variableCMax.TabIndex = 34;
            // 
            // variableCMaxLbl
            // 
            this.variableCMaxLbl.AutoSize = true;
            this.variableCMaxLbl.Location = new System.Drawing.Point(172, 110);
            this.variableCMaxLbl.Name = "variableCMaxLbl";
            this.variableCMaxLbl.Size = new System.Drawing.Size(30, 13);
            this.variableCMaxLbl.TabIndex = 33;
            this.variableCMaxLbl.Text = "Max:";
            // 
            // variableCValue
            // 
            this.variableCValue.BackColor = System.Drawing.Color.White;
            this.variableCValue.Location = new System.Drawing.Point(202, 86);
            this.variableCValue.Name = "variableCValue";
            this.variableCValue.Size = new System.Drawing.Size(40, 16);
            this.variableCValue.TabIndex = 32;
            this.variableCValue.Text = "0";
            // 
            // variableCTitle
            // 
            this.variableCTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.variableCTitle.Location = new System.Drawing.Point(202, 27);
            this.variableCTitle.Name = "variableCTitle";
            this.variableCTitle.Size = new System.Drawing.Size(40, 13);
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
            this.variableCKnob.Location = new System.Drawing.Point(202, 43);
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
            // 
            // variableBMinLbl
            // 
            this.variableBMinLbl.AutoSize = true;
            this.variableBMinLbl.Location = new System.Drawing.Point(92, 136);
            this.variableBMinLbl.Name = "variableBMinLbl";
            this.variableBMinLbl.Size = new System.Drawing.Size(27, 13);
            this.variableBMinLbl.TabIndex = 29;
            this.variableBMinLbl.Text = "Min:";
            // 
            // variableBMin
            // 
            this.variableBMin.Location = new System.Drawing.Point(122, 133);
            this.variableBMin.Name = "variableBMin";
            this.variableBMin.Size = new System.Drawing.Size(40, 20);
            this.variableBMin.TabIndex = 28;
            // 
            // variableBMax
            // 
            this.variableBMax.Location = new System.Drawing.Point(122, 107);
            this.variableBMax.Name = "variableBMax";
            this.variableBMax.Size = new System.Drawing.Size(40, 20);
            this.variableBMax.TabIndex = 27;
            // 
            // variableBMaxLbl
            // 
            this.variableBMaxLbl.AutoSize = true;
            this.variableBMaxLbl.Location = new System.Drawing.Point(92, 110);
            this.variableBMaxLbl.Name = "variableBMaxLbl";
            this.variableBMaxLbl.Size = new System.Drawing.Size(30, 13);
            this.variableBMaxLbl.TabIndex = 26;
            this.variableBMaxLbl.Text = "Max:";
            // 
            // variableBValue
            // 
            this.variableBValue.BackColor = System.Drawing.Color.White;
            this.variableBValue.Location = new System.Drawing.Point(122, 86);
            this.variableBValue.Name = "variableBValue";
            this.variableBValue.Size = new System.Drawing.Size(40, 16);
            this.variableBValue.TabIndex = 25;
            this.variableBValue.Text = "0";
            // 
            // variableBTitle
            // 
            this.variableBTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.variableBTitle.Location = new System.Drawing.Point(122, 27);
            this.variableBTitle.Name = "variableBTitle";
            this.variableBTitle.Size = new System.Drawing.Size(40, 13);
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
            this.variableBKnob.Location = new System.Drawing.Point(122, 43);
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
            // 
            // variableAMinLbl
            // 
            this.variableAMinLbl.AutoSize = true;
            this.variableAMinLbl.Location = new System.Drawing.Point(12, 137);
            this.variableAMinLbl.Name = "variableAMinLbl";
            this.variableAMinLbl.Size = new System.Drawing.Size(27, 13);
            this.variableAMinLbl.TabIndex = 22;
            this.variableAMinLbl.Text = "Min:";
            // 
            // variableAMin
            // 
            this.variableAMin.Location = new System.Drawing.Point(42, 134);
            this.variableAMin.Name = "variableAMin";
            this.variableAMin.Size = new System.Drawing.Size(40, 20);
            this.variableAMin.TabIndex = 21;
            // 
            // variableAMax
            // 
            this.variableAMax.Location = new System.Drawing.Point(42, 108);
            this.variableAMax.Name = "variableAMax";
            this.variableAMax.Size = new System.Drawing.Size(40, 20);
            this.variableAMax.TabIndex = 20;
            // 
            // variableAMaxLbl
            // 
            this.variableAMaxLbl.AutoSize = true;
            this.variableAMaxLbl.Location = new System.Drawing.Point(12, 111);
            this.variableAMaxLbl.Name = "variableAMaxLbl";
            this.variableAMaxLbl.Size = new System.Drawing.Size(30, 13);
            this.variableAMaxLbl.TabIndex = 19;
            this.variableAMaxLbl.Text = "Max:";
            // 
            // variableAValue
            // 
            this.variableAValue.BackColor = System.Drawing.Color.White;
            this.variableAValue.Location = new System.Drawing.Point(42, 87);
            this.variableAValue.Name = "variableAValue";
            this.variableAValue.Size = new System.Drawing.Size(40, 16);
            this.variableAValue.TabIndex = 18;
            this.variableAValue.Text = "0";
            // 
            // variableATitle
            // 
            this.variableATitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.variableATitle.Location = new System.Drawing.Point(42, 28);
            this.variableATitle.Name = "variableATitle";
            this.variableATitle.Size = new System.Drawing.Size(40, 13);
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
            this.variableAKnob.Location = new System.Drawing.Point(42, 44);
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
            // 
            // PluginEditorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.curveGroup);
            this.Controls.Add(this.mappingList);
            this.Controls.Add(this.zedGraphControl1);
            this.Name = "PluginEditorView";
            this.Size = new System.Drawing.Size(641, 487);
            this.curveGroup.ResumeLayout(false);
            this.curveGroup.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ZedGraph.ZedGraphControl zedGraphControl1;
        private LBSoft.IndustrialCtrls.Knobs.LBKnob presetParam1Knob;
        private System.Windows.Forms.Label presetParam1Value;
        private System.Windows.Forms.ListBox mappingList;
        private System.Windows.Forms.GroupBox curveGroup;
        private System.Windows.Forms.RadioButton radioCurvePreset;
        private System.Windows.Forms.RadioButton radioCurveEquation;
        private System.Windows.Forms.Label presetParam1Title;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox textBox1;
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

    }
}

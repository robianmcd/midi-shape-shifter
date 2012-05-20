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
            if (disposing)
            {
                OnDispose();

                if (components != null)
                {
                    components.Dispose();                    
                }
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
            this.mainGraphControl = new ZedGraph.ZedGraphControl();
            this.presetParam1Knob = new LBSoft.IndustrialCtrls.Knobs.LBKnob();
            this.paramContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editParamMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.presetParam1Value = new System.Windows.Forms.Label();
            this.curveGroup = new System.Windows.Forms.GroupBox();
            this.presetParam5Title = new System.Windows.Forms.Label();
            this.presetParam5Value = new System.Windows.Forms.Label();
            this.presetParam5Knob = new LBSoft.IndustrialCtrls.Knobs.LBKnob();
            this.coverUpUglyLine = new System.Windows.Forms.Label();
            this.presetToolStrip = new System.Windows.Forms.ToolStrip();
            this.curvePresetLabel = new System.Windows.Forms.ToolStripLabel();
            this.curvePresetList = new System.Windows.Forms.ToolStripDropDownButton();
            this.openPresetBtn = new System.Windows.Forms.ToolStripButton();
            this.savePresetBtn = new System.Windows.Forms.ToolStripSplitButton();
            this.savePresetMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SavePresetAsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pointYEquationTextBox = new System.Windows.Forms.TextBox();
            this.pointYEquationLabel = new System.Windows.Forms.Label();
            this.pointXEquationLabel = new System.Windows.Forms.Label();
            this.resetGraphBtn = new System.Windows.Forms.Button();
            this.nextEquationBtn = new System.Windows.Forms.Button();
            this.prevEquationBtn = new System.Windows.Forms.Button();
            this.curveEquationLabel = new System.Windows.Forms.Label();
            this.graphOutputTypeImg = new System.Windows.Forms.PictureBox();
            this.graphInputTypeCombo = new System.Windows.Forms.ComboBox();
            this.graphInputLable = new System.Windows.Forms.Label();
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
            this.pointXEquationTextBox = new System.Windows.Forms.TextBox();
            this.curveEquationTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.variableEValue = new System.Windows.Forms.Label();
            this.variableETitle = new System.Windows.Forms.Label();
            this.variableEKnob = new LBSoft.IndustrialCtrls.Knobs.LBKnob();
            this.variableDValue = new System.Windows.Forms.Label();
            this.variableDTitle = new System.Windows.Forms.Label();
            this.variableDKnob = new LBSoft.IndustrialCtrls.Knobs.LBKnob();
            this.variableCValue = new System.Windows.Forms.Label();
            this.variableCTitle = new System.Windows.Forms.Label();
            this.variableCKnob = new LBSoft.IndustrialCtrls.Knobs.LBKnob();
            this.variableBValue = new System.Windows.Forms.Label();
            this.variableBTitle = new System.Windows.Forms.Label();
            this.variableBKnob = new LBSoft.IndustrialCtrls.Knobs.LBKnob();
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
            this.programToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.programList = new System.Windows.Forms.ToolStripDropDownButton();
            this.openProgram = new System.Windows.Forms.ToolStripButton();
            this.saveSplitButton = new System.Windows.Forms.ToolStripSplitButton();
            this.saveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.paramContextMenu.SuspendLayout();
            this.curveGroup.SuspendLayout();
            this.presetToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.graphOutputTypeImg)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.mappingGroupBox.SuspendLayout();
            this.generatorGroupBox.SuspendLayout();
            this.programToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainGraphControl
            // 
            this.mainGraphControl.BackColor = System.Drawing.SystemColors.Control;
            this.mainGraphControl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.mainGraphControl.EditButtons = System.Windows.Forms.MouseButtons.Left;
            this.mainGraphControl.EditModifierKeys = System.Windows.Forms.Keys.None;
            this.mainGraphControl.Enabled = false;
            this.mainGraphControl.IsEnableHEdit = true;
            this.mainGraphControl.IsEnableHPan = false;
            this.mainGraphControl.IsEnableHZoom = false;
            this.mainGraphControl.IsEnableManualEditing = true;
            this.mainGraphControl.IsEnableVEdit = true;
            this.mainGraphControl.IsEnableVPan = false;
            this.mainGraphControl.IsEnableVZoom = false;
            this.mainGraphControl.IsEnableWheelZoom = false;
            this.mainGraphControl.IsShowCopyMessage = false;
            this.mainGraphControl.LinkModifierKeys = System.Windows.Forms.Keys.None;
            this.mainGraphControl.Location = new System.Drawing.Point(6, 45);
            this.mainGraphControl.Name = "mainGraphControl";
            this.mainGraphControl.ScrollGrace = 0D;
            this.mainGraphControl.ScrollMaxX = 0D;
            this.mainGraphControl.ScrollMaxY = 0D;
            this.mainGraphControl.ScrollMaxY2 = 0D;
            this.mainGraphControl.ScrollMinX = 0D;
            this.mainGraphControl.ScrollMinY = 0D;
            this.mainGraphControl.ScrollMinY2 = 0D;
            this.mainGraphControl.Size = new System.Drawing.Size(259, 187);
            this.mainGraphControl.TabIndex = 2;
            this.mainGraphControl.MouseDownEvent += new ZedGraph.ZedGraphControl.ZedMouseEventHandler(this.mainGraphControl_MouseDownEvent);
            this.mainGraphControl.EditDragEvent += new ZedGraph.ZedGraphControl.ZedEditDragHandler(this.mainGraphControl_EditDragEvent);
            // 
            // presetParam1Knob
            // 
            this.presetParam1Knob.BackColor = System.Drawing.Color.Transparent;
            this.presetParam1Knob.ContextMenuStrip = this.paramContextMenu;
            this.presetParam1Knob.DrawRatio = 0.2F;
            this.presetParam1Knob.Enabled = false;
            this.presetParam1Knob.IndicatorColor = System.Drawing.Color.Black;
            this.presetParam1Knob.IndicatorOffset = 7F;
            this.presetParam1Knob.KnobCenter = ((System.Drawing.PointF)(resources.GetObject("presetParam1Knob.KnobCenter")));
            this.presetParam1Knob.KnobColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(193)))), ((int)(((byte)(216)))));
            this.presetParam1Knob.KnobRect = ((System.Drawing.RectangleF)(resources.GetObject("presetParam1Knob.KnobRect")));
            this.presetParam1Knob.Location = new System.Drawing.Point(9, 310);
            this.presetParam1Knob.MaxValue = 1F;
            this.presetParam1Knob.MinValue = 0F;
            this.presetParam1Knob.Name = "presetParam1Knob";
            this.presetParam1Knob.Renderer = null;
            this.presetParam1Knob.ScaleColor = System.Drawing.SystemColors.ControlDarkDark;
            this.presetParam1Knob.Size = new System.Drawing.Size(40, 40);
            this.presetParam1Knob.StepValue = 0.1F;
            this.presetParam1Knob.Style = LBSoft.IndustrialCtrls.Knobs.LBKnob.KnobStyle.Circular;
            this.presetParam1Knob.TabIndex = 15;
            this.presetParam1Knob.Value = 0F;
            this.presetParam1Knob.KnobChangeValue += new LBSoft.IndustrialCtrls.Knobs.KnobChangeValue(this.lbKnob_KnobChangeValue);
            // 
            // paramContextMenu
            // 
            this.paramContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editParamMenuItem});
            this.paramContextMenu.Name = "paramContextMenu";
            this.paramContextMenu.Size = new System.Drawing.Size(95, 26);
            // 
            // editParamMenuItem
            // 
            this.editParamMenuItem.Name = "editParamMenuItem";
            this.editParamMenuItem.Size = new System.Drawing.Size(94, 22);
            this.editParamMenuItem.Text = "Edit";
            this.editParamMenuItem.Click += new System.EventHandler(this.editParamMenuItem_Click);
            // 
            // presetParam1Value
            // 
            this.presetParam1Value.BackColor = System.Drawing.SystemColors.Control;
            this.presetParam1Value.ContextMenuStrip = this.paramContextMenu;
            this.presetParam1Value.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.presetParam1Value.Location = new System.Drawing.Point(5, 349);
            this.presetParam1Value.Name = "presetParam1Value";
            this.presetParam1Value.Size = new System.Drawing.Size(48, 16);
            this.presetParam1Value.TabIndex = 16;
            this.presetParam1Value.Text = "0";
            this.presetParam1Value.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // curveGroup
            // 
            this.curveGroup.BackColor = System.Drawing.SystemColors.Control;
            this.curveGroup.Controls.Add(this.presetParam5Title);
            this.curveGroup.Controls.Add(this.presetParam5Value);
            this.curveGroup.Controls.Add(this.presetParam5Knob);
            this.curveGroup.Controls.Add(this.coverUpUglyLine);
            this.curveGroup.Controls.Add(this.presetToolStrip);
            this.curveGroup.Controls.Add(this.pointYEquationTextBox);
            this.curveGroup.Controls.Add(this.pointYEquationLabel);
            this.curveGroup.Controls.Add(this.pointXEquationLabel);
            this.curveGroup.Controls.Add(this.resetGraphBtn);
            this.curveGroup.Controls.Add(this.nextEquationBtn);
            this.curveGroup.Controls.Add(this.prevEquationBtn);
            this.curveGroup.Controls.Add(this.curveEquationLabel);
            this.curveGroup.Controls.Add(this.graphOutputTypeImg);
            this.curveGroup.Controls.Add(this.graphInputTypeCombo);
            this.curveGroup.Controls.Add(this.graphInputLable);
            this.curveGroup.Controls.Add(this.presetParam4Title);
            this.curveGroup.Controls.Add(this.presetParam4Value);
            this.curveGroup.Controls.Add(this.presetParam4Knob);
            this.curveGroup.Controls.Add(this.mainGraphControl);
            this.curveGroup.Controls.Add(this.presetParam3Title);
            this.curveGroup.Controls.Add(this.presetParam3Value);
            this.curveGroup.Controls.Add(this.presetParam3Knob);
            this.curveGroup.Controls.Add(this.presetParam2Title);
            this.curveGroup.Controls.Add(this.presetParam2Value);
            this.curveGroup.Controls.Add(this.presetParam2Knob);
            this.curveGroup.Controls.Add(this.presetParam1Title);
            this.curveGroup.Controls.Add(this.presetParam1Value);
            this.curveGroup.Controls.Add(this.presetParam1Knob);
            this.curveGroup.Controls.Add(this.pointXEquationTextBox);
            this.curveGroup.Controls.Add(this.curveEquationTextBox);
            this.curveGroup.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.curveGroup.Location = new System.Drawing.Point(362, 35);
            this.curveGroup.Name = "curveGroup";
            this.curveGroup.Size = new System.Drawing.Size(271, 369);
            this.curveGroup.TabIndex = 3;
            this.curveGroup.TabStop = false;
            this.curveGroup.Text = "Transformation";
            // 
            // presetParam5Title
            // 
            this.presetParam5Title.ContextMenuStrip = this.paramContextMenu;
            this.presetParam5Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.presetParam5Title.Location = new System.Drawing.Point(217, 297);
            this.presetParam5Title.Name = "presetParam5Title";
            this.presetParam5Title.Size = new System.Drawing.Size(48, 13);
            this.presetParam5Title.TabIndex = 30;
            this.presetParam5Title.Text = "P5";
            this.presetParam5Title.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // presetParam5Value
            // 
            this.presetParam5Value.BackColor = System.Drawing.SystemColors.Control;
            this.presetParam5Value.ContextMenuStrip = this.paramContextMenu;
            this.presetParam5Value.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.presetParam5Value.Location = new System.Drawing.Point(217, 349);
            this.presetParam5Value.Name = "presetParam5Value";
            this.presetParam5Value.Size = new System.Drawing.Size(48, 16);
            this.presetParam5Value.TabIndex = 32;
            this.presetParam5Value.Text = "0";
            this.presetParam5Value.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // presetParam5Knob
            // 
            this.presetParam5Knob.BackColor = System.Drawing.Color.Transparent;
            this.presetParam5Knob.ContextMenuStrip = this.paramContextMenu;
            this.presetParam5Knob.DrawRatio = 0.2F;
            this.presetParam5Knob.Enabled = false;
            this.presetParam5Knob.IndicatorColor = System.Drawing.Color.Black;
            this.presetParam5Knob.IndicatorOffset = 7F;
            this.presetParam5Knob.KnobCenter = ((System.Drawing.PointF)(resources.GetObject("presetParam5Knob.KnobCenter")));
            this.presetParam5Knob.KnobColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(193)))), ((int)(((byte)(216)))));
            this.presetParam5Knob.KnobRect = ((System.Drawing.RectangleF)(resources.GetObject("presetParam5Knob.KnobRect")));
            this.presetParam5Knob.Location = new System.Drawing.Point(221, 310);
            this.presetParam5Knob.MaxValue = 1F;
            this.presetParam5Knob.MinValue = 0F;
            this.presetParam5Knob.Name = "presetParam5Knob";
            this.presetParam5Knob.Renderer = null;
            this.presetParam5Knob.ScaleColor = System.Drawing.SystemColors.ControlDarkDark;
            this.presetParam5Knob.Size = new System.Drawing.Size(40, 40);
            this.presetParam5Knob.StepValue = 0.1F;
            this.presetParam5Knob.Style = LBSoft.IndustrialCtrls.Knobs.LBKnob.KnobStyle.Circular;
            this.presetParam5Knob.TabIndex = 31;
            this.presetParam5Knob.Value = 0F;
            this.presetParam5Knob.KnobChangeValue += new LBSoft.IndustrialCtrls.Knobs.KnobChangeValue(this.lbKnob_KnobChangeValue);
            // 
            // coverUpUglyLine
            // 
            this.coverUpUglyLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(236)))));
            this.coverUpUglyLine.Location = new System.Drawing.Point(263, 15);
            this.coverUpUglyLine.Name = "coverUpUglyLine";
            this.coverUpUglyLine.Size = new System.Drawing.Size(2, 30);
            this.coverUpUglyLine.TabIndex = 29;
            // 
            // presetToolStrip
            // 
            this.presetToolStrip.AutoSize = false;
            this.presetToolStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(236)))));
            this.presetToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.presetToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.presetToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.curvePresetLabel,
            this.curvePresetList,
            this.openPresetBtn,
            this.savePresetBtn});
            this.presetToolStrip.Location = new System.Drawing.Point(6, 14);
            this.presetToolStrip.Name = "presetToolStrip";
            this.presetToolStrip.Size = new System.Drawing.Size(259, 32);
            this.presetToolStrip.TabIndex = 28;
            this.presetToolStrip.Text = "toolStrip1";
            // 
            // curvePresetLabel
            // 
            this.curvePresetLabel.Name = "curvePresetLabel";
            this.curvePresetLabel.Size = new System.Drawing.Size(42, 29);
            this.curvePresetLabel.Text = "Preset:";
            // 
            // curvePresetList
            // 
            this.curvePresetList.AutoSize = false;
            this.curvePresetList.BackColor = System.Drawing.Color.White;
            this.curvePresetList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.curvePresetList.Image = ((System.Drawing.Image)(resources.GetObject("curvePresetList.Image")));
            this.curvePresetList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.curvePresetList.Margin = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.curvePresetList.Name = "curvePresetList";
            this.curvePresetList.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.curvePresetList.Size = new System.Drawing.Size(155, 22);
            this.curvePresetList.Text = "Line";
            this.curvePresetList.ToolTipText = "Select a transformation preset.";
            // 
            // openPresetBtn
            // 
            this.openPresetBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openPresetBtn.Image = global::MidiShapeShifter.Properties.Resources.imgOpenBlue;
            this.openPresetBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openPresetBtn.Name = "openPresetBtn";
            this.openPresetBtn.Size = new System.Drawing.Size(23, 29);
            this.openPresetBtn.Text = "Open Program";
            this.openPresetBtn.Click += new System.EventHandler(this.openTransformPreset_Click);
            // 
            // savePresetBtn
            // 
            this.savePresetBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.savePresetBtn.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.savePresetMenuItem,
            this.SavePresetAsMenuItem});
            this.savePresetBtn.Image = global::MidiShapeShifter.Properties.Resources.imgSaveBlue;
            this.savePresetBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.savePresetBtn.Name = "savePresetBtn";
            this.savePresetBtn.Size = new System.Drawing.Size(32, 29);
            this.savePresetBtn.Text = "Save Program";
            this.savePresetBtn.ButtonClick += new System.EventHandler(this.saveTransformPreset_Click);
            // 
            // savePresetMenuItem
            // 
            this.savePresetMenuItem.Image = global::MidiShapeShifter.Properties.Resources.imgSaveBlue;
            this.savePresetMenuItem.Name = "savePresetMenuItem";
            this.savePresetMenuItem.Size = new System.Drawing.Size(158, 22);
            this.savePresetMenuItem.Text = "Save Preset";
            this.savePresetMenuItem.Click += new System.EventHandler(this.saveTransformPreset_Click);
            // 
            // SavePresetAsMenuItem
            // 
            this.SavePresetAsMenuItem.Name = "SavePresetAsMenuItem";
            this.SavePresetAsMenuItem.Size = new System.Drawing.Size(158, 22);
            this.SavePresetAsMenuItem.Text = "Save Preset As...";
            this.SavePresetAsMenuItem.Click += new System.EventHandler(this.saveTransformPresetAs_Click);
            // 
            // pointYEquationTextBox
            // 
            this.pointYEquationTextBox.Enabled = false;
            this.pointYEquationTextBox.Location = new System.Drawing.Point(164, 265);
            this.pointYEquationTextBox.Name = "pointYEquationTextBox";
            this.pointYEquationTextBox.Size = new System.Drawing.Size(101, 20);
            this.pointYEquationTextBox.TabIndex = 13;
            this.pointYEquationTextBox.Visible = false;
            this.pointYEquationTextBox.TextChanged += new System.EventHandler(this.pointEquationTextBox_TextChanged);
            // 
            // pointYEquationLabel
            // 
            this.pointYEquationLabel.Location = new System.Drawing.Point(137, 268);
            this.pointYEquationLabel.Name = "pointYEquationLabel";
            this.pointYEquationLabel.Size = new System.Drawing.Size(27, 18);
            this.pointYEquationLabel.TabIndex = 12;
            this.pointYEquationLabel.Text = "Y = ";
            this.pointYEquationLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.pointYEquationLabel.Visible = false;
            // 
            // pointXEquationLabel
            // 
            this.pointXEquationLabel.Location = new System.Drawing.Point(6, 268);
            this.pointXEquationLabel.Name = "pointXEquationLabel";
            this.pointXEquationLabel.Size = new System.Drawing.Size(27, 18);
            this.pointXEquationLabel.TabIndex = 10;
            this.pointXEquationLabel.Text = "X = ";
            this.pointXEquationLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.pointXEquationLabel.Visible = false;
            // 
            // resetGraphBtn
            // 
            this.resetGraphBtn.BackgroundImage = global::MidiShapeShifter.Properties.Resources.imgTrashBlue;
            this.resetGraphBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.resetGraphBtn.Location = new System.Drawing.Point(54, 234);
            this.resetGraphBtn.Margin = new System.Windows.Forms.Padding(0);
            this.resetGraphBtn.Name = "resetGraphBtn";
            this.resetGraphBtn.Size = new System.Drawing.Size(24, 24);
            this.resetGraphBtn.TabIndex = 7;
            this.resetGraphBtn.UseVisualStyleBackColor = true;
            this.resetGraphBtn.EnabledChanged += new System.EventHandler(this.resetGraphBtn_EnabledChanged);
            this.resetGraphBtn.Click += new System.EventHandler(this.resetGraphBtn_Click);
            // 
            // nextEquationBtn
            // 
            this.nextEquationBtn.BackgroundImage = global::MidiShapeShifter.Properties.Resources.imgRightBlue;
            this.nextEquationBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.nextEquationBtn.Location = new System.Drawing.Point(30, 234);
            this.nextEquationBtn.Margin = new System.Windows.Forms.Padding(0);
            this.nextEquationBtn.Name = "nextEquationBtn";
            this.nextEquationBtn.Size = new System.Drawing.Size(24, 24);
            this.nextEquationBtn.TabIndex = 6;
            this.nextEquationBtn.UseVisualStyleBackColor = true;
            this.nextEquationBtn.EnabledChanged += new System.EventHandler(this.nextEquationBtn_EnabledChanged);
            this.nextEquationBtn.Click += new System.EventHandler(this.nextEquationBtn_Click);
            // 
            // prevEquationBtn
            // 
            this.prevEquationBtn.BackgroundImage = global::MidiShapeShifter.Properties.Resources.imgLeftBlue;
            this.prevEquationBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.prevEquationBtn.Location = new System.Drawing.Point(6, 234);
            this.prevEquationBtn.Margin = new System.Windows.Forms.Padding(0);
            this.prevEquationBtn.Name = "prevEquationBtn";
            this.prevEquationBtn.Size = new System.Drawing.Size(24, 24);
            this.prevEquationBtn.TabIndex = 5;
            this.prevEquationBtn.UseVisualStyleBackColor = true;
            this.prevEquationBtn.EnabledChanged += new System.EventHandler(this.prevEquationBtn_EnabledChanged);
            this.prevEquationBtn.Click += new System.EventHandler(this.prevEquationBtn_Click);
            // 
            // curveEquationLabel
            // 
            this.curveEquationLabel.Location = new System.Drawing.Point(6, 268);
            this.curveEquationLabel.Name = "curveEquationLabel";
            this.curveEquationLabel.Size = new System.Drawing.Size(27, 18);
            this.curveEquationLabel.TabIndex = 8;
            this.curveEquationLabel.Text = "Y = ";
            this.curveEquationLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // graphOutputTypeImg
            // 
            this.graphOutputTypeImg.BackColor = System.Drawing.Color.White;
            this.graphOutputTypeImg.Location = new System.Drawing.Point(16, 65);
            this.graphOutputTypeImg.Name = "graphOutputTypeImg";
            this.graphOutputTypeImg.Size = new System.Drawing.Size(19, 117);
            this.graphOutputTypeImg.TabIndex = 25;
            this.graphOutputTypeImg.TabStop = false;
            this.graphOutputTypeImg.Paint += new System.Windows.Forms.PaintEventHandler(this.graphOutputTypeImg_Paint);
            // 
            // graphInputTypeCombo
            // 
            this.graphInputTypeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.graphInputTypeCombo.FormattingEnabled = true;
            this.graphInputTypeCombo.Location = new System.Drawing.Point(105, 202);
            this.graphInputTypeCombo.Name = "graphInputTypeCombo";
            this.graphInputTypeCombo.Size = new System.Drawing.Size(118, 21);
            this.graphInputTypeCombo.TabIndex = 4;
            this.graphInputTypeCombo.SelectedIndexChanged += new System.EventHandler(this.graphInputTypeCombo_SelectedIndexChanged);
            // 
            // graphInputLable
            // 
            this.graphInputLable.BackColor = System.Drawing.Color.White;
            this.graphInputLable.Location = new System.Drawing.Point(70, 205);
            this.graphInputLable.Name = "graphInputLable";
            this.graphInputLable.Size = new System.Drawing.Size(36, 16);
            this.graphInputLable.TabIndex = 3;
            this.graphInputLable.Text = "Input:";
            // 
            // presetParam4Title
            // 
            this.presetParam4Title.ContextMenuStrip = this.paramContextMenu;
            this.presetParam4Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.presetParam4Title.Location = new System.Drawing.Point(164, 297);
            this.presetParam4Title.Name = "presetParam4Title";
            this.presetParam4Title.Size = new System.Drawing.Size(48, 13);
            this.presetParam4Title.TabIndex = 22;
            this.presetParam4Title.Text = "P4";
            this.presetParam4Title.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // presetParam4Value
            // 
            this.presetParam4Value.BackColor = System.Drawing.SystemColors.Control;
            this.presetParam4Value.ContextMenuStrip = this.paramContextMenu;
            this.presetParam4Value.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.presetParam4Value.Location = new System.Drawing.Point(164, 349);
            this.presetParam4Value.Name = "presetParam4Value";
            this.presetParam4Value.Size = new System.Drawing.Size(48, 16);
            this.presetParam4Value.TabIndex = 24;
            this.presetParam4Value.Text = "0";
            this.presetParam4Value.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // presetParam4Knob
            // 
            this.presetParam4Knob.BackColor = System.Drawing.Color.Transparent;
            this.presetParam4Knob.ContextMenuStrip = this.paramContextMenu;
            this.presetParam4Knob.DrawRatio = 0.2F;
            this.presetParam4Knob.Enabled = false;
            this.presetParam4Knob.IndicatorColor = System.Drawing.Color.Black;
            this.presetParam4Knob.IndicatorOffset = 7F;
            this.presetParam4Knob.KnobCenter = ((System.Drawing.PointF)(resources.GetObject("presetParam4Knob.KnobCenter")));
            this.presetParam4Knob.KnobColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(193)))), ((int)(((byte)(216)))));
            this.presetParam4Knob.KnobRect = ((System.Drawing.RectangleF)(resources.GetObject("presetParam4Knob.KnobRect")));
            this.presetParam4Knob.Location = new System.Drawing.Point(168, 310);
            this.presetParam4Knob.MaxValue = 1F;
            this.presetParam4Knob.MinValue = 0F;
            this.presetParam4Knob.Name = "presetParam4Knob";
            this.presetParam4Knob.Renderer = null;
            this.presetParam4Knob.ScaleColor = System.Drawing.SystemColors.ControlDarkDark;
            this.presetParam4Knob.Size = new System.Drawing.Size(40, 40);
            this.presetParam4Knob.StepValue = 0.1F;
            this.presetParam4Knob.Style = LBSoft.IndustrialCtrls.Knobs.LBKnob.KnobStyle.Circular;
            this.presetParam4Knob.TabIndex = 23;
            this.presetParam4Knob.Value = 0F;
            this.presetParam4Knob.KnobChangeValue += new LBSoft.IndustrialCtrls.Knobs.KnobChangeValue(this.lbKnob_KnobChangeValue);
            // 
            // presetParam3Title
            // 
            this.presetParam3Title.ContextMenuStrip = this.paramContextMenu;
            this.presetParam3Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.presetParam3Title.Location = new System.Drawing.Point(111, 297);
            this.presetParam3Title.Name = "presetParam3Title";
            this.presetParam3Title.Size = new System.Drawing.Size(48, 13);
            this.presetParam3Title.TabIndex = 19;
            this.presetParam3Title.Text = "P3";
            this.presetParam3Title.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // presetParam3Value
            // 
            this.presetParam3Value.BackColor = System.Drawing.SystemColors.Control;
            this.presetParam3Value.ContextMenuStrip = this.paramContextMenu;
            this.presetParam3Value.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.presetParam3Value.Location = new System.Drawing.Point(111, 349);
            this.presetParam3Value.Name = "presetParam3Value";
            this.presetParam3Value.Size = new System.Drawing.Size(48, 16);
            this.presetParam3Value.TabIndex = 21;
            this.presetParam3Value.Text = "0";
            this.presetParam3Value.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // presetParam3Knob
            // 
            this.presetParam3Knob.BackColor = System.Drawing.Color.Transparent;
            this.presetParam3Knob.ContextMenuStrip = this.paramContextMenu;
            this.presetParam3Knob.DrawRatio = 0.2F;
            this.presetParam3Knob.Enabled = false;
            this.presetParam3Knob.IndicatorColor = System.Drawing.Color.Black;
            this.presetParam3Knob.IndicatorOffset = 7F;
            this.presetParam3Knob.KnobCenter = ((System.Drawing.PointF)(resources.GetObject("presetParam3Knob.KnobCenter")));
            this.presetParam3Knob.KnobColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(193)))), ((int)(((byte)(216)))));
            this.presetParam3Knob.KnobRect = ((System.Drawing.RectangleF)(resources.GetObject("presetParam3Knob.KnobRect")));
            this.presetParam3Knob.Location = new System.Drawing.Point(115, 310);
            this.presetParam3Knob.MaxValue = 1F;
            this.presetParam3Knob.MinValue = 0F;
            this.presetParam3Knob.Name = "presetParam3Knob";
            this.presetParam3Knob.Renderer = null;
            this.presetParam3Knob.ScaleColor = System.Drawing.SystemColors.ControlDarkDark;
            this.presetParam3Knob.Size = new System.Drawing.Size(40, 40);
            this.presetParam3Knob.StepValue = 0.1F;
            this.presetParam3Knob.Style = LBSoft.IndustrialCtrls.Knobs.LBKnob.KnobStyle.Circular;
            this.presetParam3Knob.TabIndex = 20;
            this.presetParam3Knob.Value = 0F;
            this.presetParam3Knob.KnobChangeValue += new LBSoft.IndustrialCtrls.Knobs.KnobChangeValue(this.lbKnob_KnobChangeValue);
            // 
            // presetParam2Title
            // 
            this.presetParam2Title.ContextMenuStrip = this.paramContextMenu;
            this.presetParam2Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.presetParam2Title.Location = new System.Drawing.Point(58, 297);
            this.presetParam2Title.Name = "presetParam2Title";
            this.presetParam2Title.Size = new System.Drawing.Size(48, 13);
            this.presetParam2Title.TabIndex = 16;
            this.presetParam2Title.Text = "P2";
            this.presetParam2Title.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // presetParam2Value
            // 
            this.presetParam2Value.BackColor = System.Drawing.SystemColors.Control;
            this.presetParam2Value.ContextMenuStrip = this.paramContextMenu;
            this.presetParam2Value.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.presetParam2Value.Location = new System.Drawing.Point(58, 349);
            this.presetParam2Value.Name = "presetParam2Value";
            this.presetParam2Value.Size = new System.Drawing.Size(48, 16);
            this.presetParam2Value.TabIndex = 18;
            this.presetParam2Value.Text = "0";
            this.presetParam2Value.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // presetParam2Knob
            // 
            this.presetParam2Knob.BackColor = System.Drawing.Color.Transparent;
            this.presetParam2Knob.ContextMenuStrip = this.paramContextMenu;
            this.presetParam2Knob.DrawRatio = 0.2F;
            this.presetParam2Knob.Enabled = false;
            this.presetParam2Knob.IndicatorColor = System.Drawing.Color.Black;
            this.presetParam2Knob.IndicatorOffset = 7F;
            this.presetParam2Knob.KnobCenter = ((System.Drawing.PointF)(resources.GetObject("presetParam2Knob.KnobCenter")));
            this.presetParam2Knob.KnobColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(193)))), ((int)(((byte)(216)))));
            this.presetParam2Knob.KnobRect = ((System.Drawing.RectangleF)(resources.GetObject("presetParam2Knob.KnobRect")));
            this.presetParam2Knob.Location = new System.Drawing.Point(62, 310);
            this.presetParam2Knob.MaxValue = 1F;
            this.presetParam2Knob.MinValue = 0F;
            this.presetParam2Knob.Name = "presetParam2Knob";
            this.presetParam2Knob.Renderer = null;
            this.presetParam2Knob.ScaleColor = System.Drawing.SystemColors.ControlDarkDark;
            this.presetParam2Knob.Size = new System.Drawing.Size(40, 40);
            this.presetParam2Knob.StepValue = 0.1F;
            this.presetParam2Knob.Style = LBSoft.IndustrialCtrls.Knobs.LBKnob.KnobStyle.Circular;
            this.presetParam2Knob.TabIndex = 17;
            this.presetParam2Knob.Value = 0F;
            this.presetParam2Knob.KnobChangeValue += new LBSoft.IndustrialCtrls.Knobs.KnobChangeValue(this.lbKnob_KnobChangeValue);
            // 
            // presetParam1Title
            // 
            this.presetParam1Title.ContextMenuStrip = this.paramContextMenu;
            this.presetParam1Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.presetParam1Title.Location = new System.Drawing.Point(5, 297);
            this.presetParam1Title.Name = "presetParam1Title";
            this.presetParam1Title.Size = new System.Drawing.Size(48, 13);
            this.presetParam1Title.TabIndex = 14;
            this.presetParam1Title.Text = "P1";
            this.presetParam1Title.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pointXEquationTextBox
            // 
            this.pointXEquationTextBox.Enabled = false;
            this.pointXEquationTextBox.Location = new System.Drawing.Point(33, 265);
            this.pointXEquationTextBox.Name = "pointXEquationTextBox";
            this.pointXEquationTextBox.Size = new System.Drawing.Size(101, 20);
            this.pointXEquationTextBox.TabIndex = 11;
            this.pointXEquationTextBox.Visible = false;
            this.pointXEquationTextBox.TextChanged += new System.EventHandler(this.pointEquationTextBox_TextChanged);
            // 
            // curveEquationTextBox
            // 
            this.curveEquationTextBox.Enabled = false;
            this.curveEquationTextBox.Location = new System.Drawing.Point(33, 265);
            this.curveEquationTextBox.Name = "curveEquationTextBox";
            this.curveEquationTextBox.Size = new System.Drawing.Size(232, 20);
            this.curveEquationTextBox.TabIndex = 9;
            this.curveEquationTextBox.Text = " ";
            this.curveEquationTextBox.TextChanged += new System.EventHandler(this.curveEquationTextBox_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.variableEValue);
            this.groupBox1.Controls.Add(this.variableETitle);
            this.groupBox1.Controls.Add(this.variableEKnob);
            this.groupBox1.Controls.Add(this.variableDValue);
            this.groupBox1.Controls.Add(this.variableDTitle);
            this.groupBox1.Controls.Add(this.variableDKnob);
            this.groupBox1.Controls.Add(this.variableCValue);
            this.groupBox1.Controls.Add(this.variableCTitle);
            this.groupBox1.Controls.Add(this.variableCKnob);
            this.groupBox1.Controls.Add(this.variableBValue);
            this.groupBox1.Controls.Add(this.variableBTitle);
            this.groupBox1.Controls.Add(this.variableBKnob);
            this.groupBox1.Controls.Add(this.variableAValue);
            this.groupBox1.Controls.Add(this.variableATitle);
            this.groupBox1.Controls.Add(this.variableAKnob);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Location = new System.Drawing.Point(639, 35);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(68, 369);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Variables";
            // 
            // variableEValue
            // 
            this.variableEValue.BackColor = System.Drawing.SystemColors.Control;
            this.variableEValue.ContextMenuStrip = this.paramContextMenu;
            this.variableEValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.variableEValue.Location = new System.Drawing.Point(6, 348);
            this.variableEValue.Name = "variableEValue";
            this.variableEValue.Size = new System.Drawing.Size(56, 16);
            this.variableEValue.TabIndex = 14;
            this.variableEValue.Text = "0";
            this.variableEValue.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // variableETitle
            // 
            this.variableETitle.ContextMenuStrip = this.paramContextMenu;
            this.variableETitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.variableETitle.Location = new System.Drawing.Point(6, 296);
            this.variableETitle.Name = "variableETitle";
            this.variableETitle.Size = new System.Drawing.Size(56, 13);
            this.variableETitle.TabIndex = 12;
            this.variableETitle.Text = "E";
            this.variableETitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // variableEKnob
            // 
            this.variableEKnob.BackColor = System.Drawing.Color.Transparent;
            this.variableEKnob.ContextMenuStrip = this.paramContextMenu;
            this.variableEKnob.DrawRatio = 0.2F;
            this.variableEKnob.IndicatorColor = System.Drawing.Color.Black;
            this.variableEKnob.IndicatorOffset = 7F;
            this.variableEKnob.KnobCenter = ((System.Drawing.PointF)(resources.GetObject("variableEKnob.KnobCenter")));
            this.variableEKnob.KnobColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.variableEKnob.KnobRect = ((System.Drawing.RectangleF)(resources.GetObject("variableEKnob.KnobRect")));
            this.variableEKnob.Location = new System.Drawing.Point(14, 309);
            this.variableEKnob.MaxValue = 1F;
            this.variableEKnob.MinValue = 0F;
            this.variableEKnob.Name = "variableEKnob";
            this.variableEKnob.Renderer = null;
            this.variableEKnob.ScaleColor = System.Drawing.SystemColors.ControlDarkDark;
            this.variableEKnob.Size = new System.Drawing.Size(40, 40);
            this.variableEKnob.StepValue = 0.1F;
            this.variableEKnob.Style = LBSoft.IndustrialCtrls.Knobs.LBKnob.KnobStyle.Circular;
            this.variableEKnob.TabIndex = 13;
            this.variableEKnob.Value = 0F;
            this.variableEKnob.KnobChangeValue += new LBSoft.IndustrialCtrls.Knobs.KnobChangeValue(this.lbKnob_KnobChangeValue);
            // 
            // variableDValue
            // 
            this.variableDValue.BackColor = System.Drawing.SystemColors.Control;
            this.variableDValue.ContextMenuStrip = this.paramContextMenu;
            this.variableDValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.variableDValue.Location = new System.Drawing.Point(6, 278);
            this.variableDValue.Name = "variableDValue";
            this.variableDValue.Size = new System.Drawing.Size(56, 16);
            this.variableDValue.TabIndex = 11;
            this.variableDValue.Text = "0";
            this.variableDValue.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // variableDTitle
            // 
            this.variableDTitle.ContextMenuStrip = this.paramContextMenu;
            this.variableDTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.variableDTitle.Location = new System.Drawing.Point(6, 226);
            this.variableDTitle.Name = "variableDTitle";
            this.variableDTitle.Size = new System.Drawing.Size(56, 13);
            this.variableDTitle.TabIndex = 9;
            this.variableDTitle.Text = "D";
            this.variableDTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // variableDKnob
            // 
            this.variableDKnob.BackColor = System.Drawing.Color.Transparent;
            this.variableDKnob.ContextMenuStrip = this.paramContextMenu;
            this.variableDKnob.DrawRatio = 0.2F;
            this.variableDKnob.IndicatorColor = System.Drawing.Color.Black;
            this.variableDKnob.IndicatorOffset = 7F;
            this.variableDKnob.KnobCenter = ((System.Drawing.PointF)(resources.GetObject("variableDKnob.KnobCenter")));
            this.variableDKnob.KnobColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.variableDKnob.KnobRect = ((System.Drawing.RectangleF)(resources.GetObject("variableDKnob.KnobRect")));
            this.variableDKnob.Location = new System.Drawing.Point(14, 239);
            this.variableDKnob.MaxValue = 1F;
            this.variableDKnob.MinValue = 0F;
            this.variableDKnob.Name = "variableDKnob";
            this.variableDKnob.Renderer = null;
            this.variableDKnob.ScaleColor = System.Drawing.SystemColors.ControlDarkDark;
            this.variableDKnob.Size = new System.Drawing.Size(40, 40);
            this.variableDKnob.StepValue = 0.1F;
            this.variableDKnob.Style = LBSoft.IndustrialCtrls.Knobs.LBKnob.KnobStyle.Circular;
            this.variableDKnob.TabIndex = 10;
            this.variableDKnob.Value = 0F;
            this.variableDKnob.KnobChangeValue += new LBSoft.IndustrialCtrls.Knobs.KnobChangeValue(this.lbKnob_KnobChangeValue);
            // 
            // variableCValue
            // 
            this.variableCValue.BackColor = System.Drawing.SystemColors.Control;
            this.variableCValue.ContextMenuStrip = this.paramContextMenu;
            this.variableCValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.variableCValue.Location = new System.Drawing.Point(6, 208);
            this.variableCValue.Name = "variableCValue";
            this.variableCValue.Size = new System.Drawing.Size(56, 16);
            this.variableCValue.TabIndex = 8;
            this.variableCValue.Text = "0";
            this.variableCValue.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // variableCTitle
            // 
            this.variableCTitle.ContextMenuStrip = this.paramContextMenu;
            this.variableCTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.variableCTitle.Location = new System.Drawing.Point(6, 156);
            this.variableCTitle.Name = "variableCTitle";
            this.variableCTitle.Size = new System.Drawing.Size(56, 13);
            this.variableCTitle.TabIndex = 6;
            this.variableCTitle.Text = "C";
            this.variableCTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // variableCKnob
            // 
            this.variableCKnob.BackColor = System.Drawing.Color.Transparent;
            this.variableCKnob.ContextMenuStrip = this.paramContextMenu;
            this.variableCKnob.DrawRatio = 0.2F;
            this.variableCKnob.IndicatorColor = System.Drawing.Color.Black;
            this.variableCKnob.IndicatorOffset = 7F;
            this.variableCKnob.KnobCenter = ((System.Drawing.PointF)(resources.GetObject("variableCKnob.KnobCenter")));
            this.variableCKnob.KnobColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.variableCKnob.KnobRect = ((System.Drawing.RectangleF)(resources.GetObject("variableCKnob.KnobRect")));
            this.variableCKnob.Location = new System.Drawing.Point(14, 169);
            this.variableCKnob.MaxValue = 1F;
            this.variableCKnob.MinValue = 0F;
            this.variableCKnob.Name = "variableCKnob";
            this.variableCKnob.Renderer = null;
            this.variableCKnob.ScaleColor = System.Drawing.SystemColors.ControlDarkDark;
            this.variableCKnob.Size = new System.Drawing.Size(40, 40);
            this.variableCKnob.StepValue = 0.1F;
            this.variableCKnob.Style = LBSoft.IndustrialCtrls.Knobs.LBKnob.KnobStyle.Circular;
            this.variableCKnob.TabIndex = 7;
            this.variableCKnob.Value = 0F;
            this.variableCKnob.KnobChangeValue += new LBSoft.IndustrialCtrls.Knobs.KnobChangeValue(this.lbKnob_KnobChangeValue);
            // 
            // variableBValue
            // 
            this.variableBValue.BackColor = System.Drawing.SystemColors.Control;
            this.variableBValue.ContextMenuStrip = this.paramContextMenu;
            this.variableBValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.variableBValue.Location = new System.Drawing.Point(6, 138);
            this.variableBValue.Name = "variableBValue";
            this.variableBValue.Size = new System.Drawing.Size(56, 16);
            this.variableBValue.TabIndex = 5;
            this.variableBValue.Text = "0";
            this.variableBValue.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // variableBTitle
            // 
            this.variableBTitle.ContextMenuStrip = this.paramContextMenu;
            this.variableBTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.variableBTitle.Location = new System.Drawing.Point(6, 86);
            this.variableBTitle.Name = "variableBTitle";
            this.variableBTitle.Size = new System.Drawing.Size(56, 13);
            this.variableBTitle.TabIndex = 3;
            this.variableBTitle.Text = "B";
            this.variableBTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // variableBKnob
            // 
            this.variableBKnob.BackColor = System.Drawing.Color.Transparent;
            this.variableBKnob.ContextMenuStrip = this.paramContextMenu;
            this.variableBKnob.DrawRatio = 0.2F;
            this.variableBKnob.IndicatorColor = System.Drawing.Color.Black;
            this.variableBKnob.IndicatorOffset = 7F;
            this.variableBKnob.KnobCenter = ((System.Drawing.PointF)(resources.GetObject("variableBKnob.KnobCenter")));
            this.variableBKnob.KnobColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.variableBKnob.KnobRect = ((System.Drawing.RectangleF)(resources.GetObject("variableBKnob.KnobRect")));
            this.variableBKnob.Location = new System.Drawing.Point(14, 99);
            this.variableBKnob.MaxValue = 1F;
            this.variableBKnob.MinValue = 0F;
            this.variableBKnob.Name = "variableBKnob";
            this.variableBKnob.Renderer = null;
            this.variableBKnob.ScaleColor = System.Drawing.SystemColors.ControlDarkDark;
            this.variableBKnob.Size = new System.Drawing.Size(40, 40);
            this.variableBKnob.StepValue = 0.1F;
            this.variableBKnob.Style = LBSoft.IndustrialCtrls.Knobs.LBKnob.KnobStyle.Circular;
            this.variableBKnob.TabIndex = 4;
            this.variableBKnob.Value = 0F;
            this.variableBKnob.KnobChangeValue += new LBSoft.IndustrialCtrls.Knobs.KnobChangeValue(this.lbKnob_KnobChangeValue);
            // 
            // variableAValue
            // 
            this.variableAValue.BackColor = System.Drawing.SystemColors.Control;
            this.variableAValue.ContextMenuStrip = this.paramContextMenu;
            this.variableAValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.variableAValue.Location = new System.Drawing.Point(6, 68);
            this.variableAValue.Name = "variableAValue";
            this.variableAValue.Size = new System.Drawing.Size(56, 16);
            this.variableAValue.TabIndex = 2;
            this.variableAValue.Text = "0";
            this.variableAValue.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // variableATitle
            // 
            this.variableATitle.ContextMenuStrip = this.paramContextMenu;
            this.variableATitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F);
            this.variableATitle.Location = new System.Drawing.Point(6, 16);
            this.variableATitle.Name = "variableATitle";
            this.variableATitle.Size = new System.Drawing.Size(56, 13);
            this.variableATitle.TabIndex = 0;
            this.variableATitle.Text = "A";
            this.variableATitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // variableAKnob
            // 
            this.variableAKnob.BackColor = System.Drawing.Color.Transparent;
            this.variableAKnob.ContextMenuStrip = this.paramContextMenu;
            this.variableAKnob.DrawRatio = 0.2F;
            this.variableAKnob.IndicatorColor = System.Drawing.Color.Black;
            this.variableAKnob.IndicatorOffset = 7F;
            this.variableAKnob.KnobCenter = ((System.Drawing.PointF)(resources.GetObject("variableAKnob.KnobCenter")));
            this.variableAKnob.KnobColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.variableAKnob.KnobRect = ((System.Drawing.RectangleF)(resources.GetObject("variableAKnob.KnobRect")));
            this.variableAKnob.Location = new System.Drawing.Point(14, 29);
            this.variableAKnob.MaxValue = 1F;
            this.variableAKnob.MinValue = 0F;
            this.variableAKnob.Name = "variableAKnob";
            this.variableAKnob.Renderer = null;
            this.variableAKnob.ScaleColor = System.Drawing.SystemColors.ControlDarkDark;
            this.variableAKnob.Size = new System.Drawing.Size(40, 40);
            this.variableAKnob.StepValue = 0.1F;
            this.variableAKnob.Style = LBSoft.IndustrialCtrls.Knobs.LBKnob.KnobStyle.Circular;
            this.variableAKnob.TabIndex = 1;
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
            this.mappingListView.Size = new System.Drawing.Size(337, 145);
            this.mappingListView.TabIndex = 0;
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
            this.mappingGroupBox.BackColor = System.Drawing.SystemColors.Control;
            this.mappingGroupBox.Controls.Add(this.moveMappingDownBtn);
            this.mappingGroupBox.Controls.Add(this.mappingListView);
            this.mappingGroupBox.Controls.Add(this.moveMappingUpBtn);
            this.mappingGroupBox.Controls.Add(this.addMappingBtn);
            this.mappingGroupBox.Controls.Add(this.editMappingBtn);
            this.mappingGroupBox.Controls.Add(this.deleteMappingBtn);
            this.mappingGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.mappingGroupBox.Location = new System.Drawing.Point(5, 35);
            this.mappingGroupBox.Name = "mappingGroupBox";
            this.mappingGroupBox.Size = new System.Drawing.Size(351, 192);
            this.mappingGroupBox.TabIndex = 1;
            this.mappingGroupBox.TabStop = false;
            this.mappingGroupBox.Text = "Mapings";
            // 
            // moveMappingDownBtn
            // 
            this.moveMappingDownBtn.BackgroundImage = global::MidiShapeShifter.Properties.Resources.imgDownBlue;
            this.moveMappingDownBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.moveMappingDownBtn.Location = new System.Drawing.Point(105, 164);
            this.moveMappingDownBtn.Margin = new System.Windows.Forms.Padding(0);
            this.moveMappingDownBtn.Name = "moveMappingDownBtn";
            this.moveMappingDownBtn.Size = new System.Drawing.Size(24, 24);
            this.moveMappingDownBtn.TabIndex = 5;
            this.moveMappingDownBtn.UseVisualStyleBackColor = true;
            this.moveMappingDownBtn.EnabledChanged += new System.EventHandler(this.moveDownBtn_EnabledChanged);
            this.moveMappingDownBtn.Click += new System.EventHandler(this.moveMappingDownBtn_Click);
            // 
            // moveMappingUpBtn
            // 
            this.moveMappingUpBtn.BackgroundImage = global::MidiShapeShifter.Properties.Resources.imgUpBlue;
            this.moveMappingUpBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.moveMappingUpBtn.Location = new System.Drawing.Point(81, 164);
            this.moveMappingUpBtn.Margin = new System.Windows.Forms.Padding(0);
            this.moveMappingUpBtn.Name = "moveMappingUpBtn";
            this.moveMappingUpBtn.Size = new System.Drawing.Size(24, 24);
            this.moveMappingUpBtn.TabIndex = 4;
            this.moveMappingUpBtn.UseVisualStyleBackColor = true;
            this.moveMappingUpBtn.EnabledChanged += new System.EventHandler(this.moveUpBtn_EnabledChanged);
            this.moveMappingUpBtn.Click += new System.EventHandler(this.moveMappingUpBtn_Click);
            // 
            // addMappingBtn
            // 
            this.addMappingBtn.BackgroundImage = global::MidiShapeShifter.Properties.Resources.imgAddBlue;
            this.addMappingBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.addMappingBtn.Location = new System.Drawing.Point(8, 164);
            this.addMappingBtn.Margin = new System.Windows.Forms.Padding(0);
            this.addMappingBtn.Name = "addMappingBtn";
            this.addMappingBtn.Size = new System.Drawing.Size(24, 24);
            this.addMappingBtn.TabIndex = 1;
            this.addMappingBtn.UseVisualStyleBackColor = true;
            this.addMappingBtn.Click += new System.EventHandler(this.addMappingBtn_Click);
            // 
            // editMappingBtn
            // 
            this.editMappingBtn.BackgroundImage = global::MidiShapeShifter.Properties.Resources.imgEditBlue;
            this.editMappingBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.editMappingBtn.Location = new System.Drawing.Point(57, 164);
            this.editMappingBtn.Margin = new System.Windows.Forms.Padding(0);
            this.editMappingBtn.Name = "editMappingBtn";
            this.editMappingBtn.Size = new System.Drawing.Size(24, 24);
            this.editMappingBtn.TabIndex = 3;
            this.editMappingBtn.UseVisualStyleBackColor = true;
            this.editMappingBtn.EnabledChanged += new System.EventHandler(this.editBtn_EnabledChanged);
            this.editMappingBtn.Click += new System.EventHandler(this.editMappingBtn_Click);
            // 
            // deleteMappingBtn
            // 
            this.deleteMappingBtn.BackgroundImage = global::MidiShapeShifter.Properties.Resources.imgDeleteBlue;
            this.deleteMappingBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.deleteMappingBtn.Location = new System.Drawing.Point(32, 164);
            this.deleteMappingBtn.Margin = new System.Windows.Forms.Padding(0);
            this.deleteMappingBtn.Name = "deleteMappingBtn";
            this.deleteMappingBtn.Size = new System.Drawing.Size(24, 24);
            this.deleteMappingBtn.TabIndex = 2;
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
            this.generatorGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.generatorGroupBox.Location = new System.Drawing.Point(5, 230);
            this.generatorGroupBox.Name = "generatorGroupBox";
            this.generatorGroupBox.Size = new System.Drawing.Size(351, 174);
            this.generatorGroupBox.TabIndex = 2;
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
            this.generatorListView.Size = new System.Drawing.Size(337, 124);
            this.generatorListView.TabIndex = 0;
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
            this.addGeneratorBtn.Location = new System.Drawing.Point(8, 146);
            this.addGeneratorBtn.Margin = new System.Windows.Forms.Padding(0);
            this.addGeneratorBtn.Name = "addGeneratorBtn";
            this.addGeneratorBtn.Size = new System.Drawing.Size(24, 24);
            this.addGeneratorBtn.TabIndex = 1;
            this.addGeneratorBtn.UseVisualStyleBackColor = true;
            this.addGeneratorBtn.Click += new System.EventHandler(this.addGeneratorBtn_Click);
            // 
            // editGeneratorBtn
            // 
            this.editGeneratorBtn.BackgroundImage = global::MidiShapeShifter.Properties.Resources.imgEditBlue;
            this.editGeneratorBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.editGeneratorBtn.Location = new System.Drawing.Point(57, 146);
            this.editGeneratorBtn.Margin = new System.Windows.Forms.Padding(0);
            this.editGeneratorBtn.Name = "editGeneratorBtn";
            this.editGeneratorBtn.Size = new System.Drawing.Size(24, 24);
            this.editGeneratorBtn.TabIndex = 3;
            this.editGeneratorBtn.UseVisualStyleBackColor = true;
            this.editGeneratorBtn.EnabledChanged += new System.EventHandler(this.editBtn_EnabledChanged);
            this.editGeneratorBtn.Click += new System.EventHandler(this.editGeneratorBtn_Click);
            // 
            // deleteGeneratorBtn
            // 
            this.deleteGeneratorBtn.BackgroundImage = global::MidiShapeShifter.Properties.Resources.imgDeleteBlue;
            this.deleteGeneratorBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.deleteGeneratorBtn.Location = new System.Drawing.Point(32, 146);
            this.deleteGeneratorBtn.Margin = new System.Windows.Forms.Padding(0);
            this.deleteGeneratorBtn.Name = "deleteGeneratorBtn";
            this.deleteGeneratorBtn.Size = new System.Drawing.Size(24, 24);
            this.deleteGeneratorBtn.TabIndex = 2;
            this.deleteGeneratorBtn.UseVisualStyleBackColor = true;
            this.deleteGeneratorBtn.EnabledChanged += new System.EventHandler(this.deleteBtn_EnabledChanged);
            this.deleteGeneratorBtn.Click += new System.EventHandler(this.deleteGeneratorBtn_Click);
            // 
            // programToolStrip
            // 
            this.programToolStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(236)))));
            this.programToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.programToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel2,
            this.programList,
            this.openProgram,
            this.saveSplitButton});
            this.programToolStrip.Location = new System.Drawing.Point(0, 0);
            this.programToolStrip.Name = "programToolStrip";
            this.programToolStrip.Size = new System.Drawing.Size(712, 32);
            this.programToolStrip.TabIndex = 0;
            this.programToolStrip.Text = "Tool Strip";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(56, 29);
            this.toolStripLabel2.Text = "Program:";
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
            this.programList.ToolTipText = "Select a program.";
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
            // PluginEditorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.programToolStrip);
            this.Controls.Add(this.generatorGroupBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.mappingGroupBox);
            this.Controls.Add(this.curveGroup);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "PluginEditorView";
            this.Size = new System.Drawing.Size(712, 409);
            this.paramContextMenu.ResumeLayout(false);
            this.curveGroup.ResumeLayout(false);
            this.curveGroup.PerformLayout();
            this.presetToolStrip.ResumeLayout(false);
            this.presetToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.graphOutputTypeImg)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.mappingGroupBox.ResumeLayout(false);
            this.generatorGroupBox.ResumeLayout(false);
            this.programToolStrip.ResumeLayout(false);
            this.programToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ZedGraph.ZedGraphControl mainGraphControl;
        private LBSoft.IndustrialCtrls.Knobs.LBKnob presetParam1Knob;
        private System.Windows.Forms.Label presetParam1Value;
        private System.Windows.Forms.GroupBox curveGroup;
        private System.Windows.Forms.Label presetParam1Title;
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
        private System.Windows.Forms.Label variableDValue;
        private System.Windows.Forms.Label variableDTitle;
        private LBSoft.IndustrialCtrls.Knobs.LBKnob variableDKnob;
        private System.Windows.Forms.Label variableCValue;
        private System.Windows.Forms.Label variableCTitle;
        private LBSoft.IndustrialCtrls.Knobs.LBKnob variableCKnob;
        private System.Windows.Forms.Label variableBValue;
        private System.Windows.Forms.Label variableBTitle;
        private LBSoft.IndustrialCtrls.Knobs.LBKnob variableBKnob;
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
        private System.Windows.Forms.ToolStrip programToolStrip;
        private System.Windows.Forms.ToolStripButton openProgram;
        private System.Windows.Forms.ColumnHeader generatorPeriodColHeader;
        private System.Windows.Forms.ColumnHeader generatorLoopColHeader;
        private System.Windows.Forms.ColumnHeader generatorEnabledColHeader;
        private System.Windows.Forms.ToolStripDropDownButton programList;
        private System.Windows.Forms.ToolStripSplitButton saveSplitButton;
        private System.Windows.Forms.ToolStripMenuItem saveMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsMenuItem;
        private System.Windows.Forms.Label variableEValue;
        private System.Windows.Forms.Label variableETitle;
        private LBSoft.IndustrialCtrls.Knobs.LBKnob variableEKnob;
        private System.Windows.Forms.ComboBox graphInputTypeCombo;
        private System.Windows.Forms.Label graphInputLable;
        private System.Windows.Forms.PictureBox graphOutputTypeImg;
        private System.Windows.Forms.Label curveEquationLabel;
        private System.Windows.Forms.Button resetGraphBtn;
        private System.Windows.Forms.Button nextEquationBtn;
        private System.Windows.Forms.Button prevEquationBtn;
        private System.Windows.Forms.TextBox pointYEquationTextBox;
        private System.Windows.Forms.Label pointYEquationLabel;
        private System.Windows.Forms.Label pointXEquationLabel;
        private System.Windows.Forms.TextBox pointXEquationTextBox;
        private System.Windows.Forms.ToolStrip presetToolStrip;
        private System.Windows.Forms.ToolStripButton openPresetBtn;
        private System.Windows.Forms.ToolStripSplitButton savePresetBtn;
        private System.Windows.Forms.ToolStripMenuItem savePresetMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SavePresetAsMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton curvePresetList;
        private System.Windows.Forms.ToolStripLabel curvePresetLabel;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.Label coverUpUglyLine;
        private System.Windows.Forms.Label presetParam5Title;
        private System.Windows.Forms.Label presetParam5Value;
        private LBSoft.IndustrialCtrls.Knobs.LBKnob presetParam5Knob;
        private System.Windows.Forms.ContextMenuStrip paramContextMenu;
        private System.Windows.Forms.ToolStripMenuItem editParamMenuItem;

    }
}

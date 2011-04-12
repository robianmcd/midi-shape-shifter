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
            this.testKnob = new LBSoft.IndustrialCtrls.Knobs.LBKnob();
            this.testKnobDisplay = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // zedGraphControl1
            // 
            this.zedGraphControl1.Location = new System.Drawing.Point(376, 14);
            this.zedGraphControl1.Name = "zedGraphControl1";
            this.zedGraphControl1.ScrollGrace = 0;
            this.zedGraphControl1.ScrollMaxX = 0;
            this.zedGraphControl1.ScrollMaxY = 0;
            this.zedGraphControl1.ScrollMaxY2 = 0;
            this.zedGraphControl1.ScrollMinX = 0;
            this.zedGraphControl1.ScrollMinY = 0;
            this.zedGraphControl1.ScrollMinY2 = 0;
            this.zedGraphControl1.Size = new System.Drawing.Size(322, 294);
            this.zedGraphControl1.TabIndex = 1;
            // 
            // testKnob
            // 
            this.testKnob.BackColor = System.Drawing.Color.Transparent;
            this.testKnob.DrawRatio = 0.2F;
            this.testKnob.IndicatorColor = System.Drawing.Color.Black;
            this.testKnob.IndicatorOffset = 8F;
            this.testKnob.KnobCenter = ((System.Drawing.PointF)(resources.GetObject("testKnob.KnobCenter")));
            this.testKnob.KnobColor = System.Drawing.Color.Silver;
            this.testKnob.KnobRect = ((System.Drawing.RectangleF)(resources.GetObject("testKnob.KnobRect")));
            this.testKnob.Location = new System.Drawing.Point(376, 314);
            this.testKnob.MaxValue = 1F;
            this.testKnob.MinValue = 0F;
            this.testKnob.Name = "testKnob";
            this.testKnob.Renderer = null;
            this.testKnob.ScaleColor = System.Drawing.SystemColors.ControlDarkDark;
            this.testKnob.Size = new System.Drawing.Size(40, 40);
            this.testKnob.StepValue = 0.1F;
            this.testKnob.Style = LBSoft.IndustrialCtrls.Knobs.LBKnob.KnobStyle.Circular;
            this.testKnob.TabIndex = 2;
            this.testKnob.Value = 0F;
            this.testKnob.KnobChangeValue += new LBSoft.IndustrialCtrls.Knobs.KnobChangeValue(this.testKnob_KnobChangeValue);
            // 
            // testKnobDisplay
            // 
            this.testKnobDisplay.BackColor = System.Drawing.Color.White;
            this.testKnobDisplay.Location = new System.Drawing.Point(376, 357);
            this.testKnobDisplay.Name = "testKnobDisplay";
            this.testKnobDisplay.Size = new System.Drawing.Size(40, 16);
            this.testKnobDisplay.TabIndex = 4;
            this.testKnobDisplay.Text = "0";
            // 
            // PluginEditorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.testKnobDisplay);
            this.Controls.Add(this.testKnob);
            this.Controls.Add(this.zedGraphControl1);
            this.Name = "PluginEditorView";
            this.Size = new System.Drawing.Size(711, 442);
            this.ResumeLayout(false);

        }

        #endregion

        private ZedGraph.ZedGraphControl zedGraphControl1;
        private LBSoft.IndustrialCtrls.Knobs.LBKnob testKnob;
        private System.Windows.Forms.Label testKnobDisplay;

    }
}

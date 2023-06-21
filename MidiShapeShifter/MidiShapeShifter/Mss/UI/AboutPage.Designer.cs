namespace MidiShapeShifter.Mss.UI
{
    partial class AboutPage
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
            this.OkBtn = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.titleLbl = new System.Windows.Forms.Label();
            this.versionLbl = new System.Windows.Forms.Label();
            this.homePageLbl = new System.Windows.Forms.Label();
            this.homePageLink = new System.Windows.Forms.LinkLabel();
            this.authors = new System.Windows.Forms.Label();
            this.thanksLbl = new System.Windows.Forms.Label();
            this.versionValueLbl = new System.Windows.Forms.Label();
            this.vstNetLink = new System.Windows.Forms.LinkLabel();
            this.iconzaLink = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            //
            // OkBtn
            //
            this.OkBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.OkBtn.Location = new System.Drawing.Point(103, 174);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(75, 23);
            this.OkBtn.TabIndex = 3;
            this.OkBtn.Text = "Ok";
            this.OkBtn.UseVisualStyleBackColor = true;
            //
            // pictureBox1
            //
            this.pictureBox1.Image = global::MidiShapeShifter.Properties.Resources.imgLogoWide;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 63);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            //
            // titleLbl
            //
            this.titleLbl.AllowDrop = true;
            this.titleLbl.AutoSize = true;
            this.titleLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLbl.Location = new System.Drawing.Point(118, 12);
            this.titleLbl.Name = "titleLbl";
            this.titleLbl.Size = new System.Drawing.Size(136, 16);
            this.titleLbl.TabIndex = 5;
            this.titleLbl.Text = "MIDI Shape Shifter";
            this.titleLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // versionLbl
            //
            this.versionLbl.AutoSize = true;
            this.versionLbl.Location = new System.Drawing.Point(118, 33);
            this.versionLbl.Name = "versionLbl";
            this.versionLbl.Size = new System.Drawing.Size(45, 13);
            this.versionLbl.TabIndex = 6;
            this.versionLbl.Text = "Version:";
            //
            // homePageLbl
            //
            this.homePageLbl.AutoSize = true;
            this.homePageLbl.Location = new System.Drawing.Point(9, 83);
            this.homePageLbl.Name = "homePageLbl";
            this.homePageLbl.Size = new System.Drawing.Size(66, 13);
            this.homePageLbl.TabIndex = 7;
            this.homePageLbl.Text = "Home Page:";
            //
            // homePageLink
            //
            this.homePageLink.AutoSize = true;
            this.homePageLink.Location = new System.Drawing.Point(81, 83);
            this.homePageLink.Name = "homePageLink";
            this.homePageLink.Size = new System.Drawing.Size(220, 13);
            this.homePageLink.TabIndex = 9;
            this.homePageLink.TabStop = true;
            this.homePageLink.Text = "https://github.com/robianmcd/midi-shape-shifter";
            this.homePageLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.homePageLink_LinkClicked);
            //
            // Authors
            //
            this.authors.AutoSize = true;
            this.authors.Location = new System.Drawing.Point(9, 106);
            this.authors.Name = "authors";
            this.authors.Size = new System.Drawing.Size(67, 26);
            this.authors.TabIndex = 10;
            this.authors.Text = "Created By:\nRob (@robianmcd), Amin Yahyaabadi (@aminya)";
            //
            // thanksLbl
            //
            this.thanksLbl.AutoSize = true;
            this.thanksLbl.Location = new System.Drawing.Point(9, 146);
            this.thanksLbl.Name = "thanksLbl";
            this.thanksLbl.Size = new System.Drawing.Size(67, 13);
            this.thanksLbl.TabIndex = 10;
            this.thanksLbl.Text = "Powered By:";
            //
            // versionValueLbl
            //
            this.versionValueLbl.AutoSize = true;
            this.versionValueLbl.Location = new System.Drawing.Point(169, 33);
            this.versionValueLbl.Name = "versionValueLbl";
            this.versionValueLbl.Size = new System.Drawing.Size(28, 13);
            this.versionValueLbl.TabIndex = 11;
            this.versionValueLbl.Text = "x.x.x";
            //
            // vstNetLink
            //
            this.vstNetLink.Location = new System.Drawing.Point(19, 166);
            this.vstNetLink.Margin = new System.Windows.Forms.Padding(0);
            this.vstNetLink.Name = "vstNetLink";
            this.vstNetLink.Size = new System.Drawing.Size(50, 13);
            this.vstNetLink.TabIndex = 13;
            this.vstNetLink.TabStop = true;
            this.vstNetLink.Text = "VST.Net";
            this.vstNetLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.vstNetLink_LinkClicked);
            //
            // iconzaLink
            //
            this.iconzaLink.Location = new System.Drawing.Point(19, 186);
            this.iconzaLink.Margin = new System.Windows.Forms.Padding(0);
            this.iconzaLink.Name = "iconzaLink";
            this.iconzaLink.Size = new System.Drawing.Size(50, 13);
            this.iconzaLink.TabIndex = 14;
            this.iconzaLink.TabStop = true;
            this.iconzaLink.Text = "Iconza";
            this.iconzaLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.iconzaLink_LinkClicked);
            //
            // AboutPage
            //
            this.AcceptButton = this.OkBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.OkBtn;
            this.ClientSize = new System.Drawing.Size(400, 400);
            this.Controls.Add(this.iconzaLink);
            this.Controls.Add(this.vstNetLink);
            this.Controls.Add(this.versionValueLbl);
            this.Controls.Add(this.authors);
            this.Controls.Add(this.thanksLbl);
            this.Controls.Add(this.homePageLink);
            this.Controls.Add(this.homePageLbl);
            this.Controls.Add(this.versionLbl);
            this.Controls.Add(this.titleLbl);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.OkBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "AboutPage";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AboutPage";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OkBtn;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label titleLbl;
        private System.Windows.Forms.Label versionLbl;
        private System.Windows.Forms.Label homePageLbl;
        private System.Windows.Forms.LinkLabel homePageLink;
        private System.Windows.Forms.Label authors;
        private System.Windows.Forms.Label thanksLbl;
        private System.Windows.Forms.Label versionValueLbl;
        private System.Windows.Forms.LinkLabel vstNetLink;
        private System.Windows.Forms.LinkLabel iconzaLink;
    }
}

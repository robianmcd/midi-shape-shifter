using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace MidiShapeShifter.Mss.UI
{
    public partial class AboutPage : Form
    {
        public AboutPage()
        {
            InitializeComponent();

            string majorVersion = Assembly.GetExecutingAssembly().GetName().Version.Major.ToString();
            string minorVersion = Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString();
            string buildVersion = Assembly.GetExecutingAssembly().GetName().Version.Build.ToString();
            this.versionValueLbl.Text = string.Format("{0}.{1}.{2}", majorVersion, minorVersion, buildVersion);
        }

        private void homePageLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://code.google.com/p/midi-shape-shifter/");
        }

        private void vstNetLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://vstnet.codeplex.com/");
        }

        private void iconzaLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://iconza.com/");
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MidiShapeShifter
{
    public partial class MappingDlg : Form
    {

        public MappingEntry mappingEntry = new MappingEntry();
        public bool useMappingEntryForDefaultValues = false;

        public MappingDlg()
        {
            InitializeComponent();
        }
    }
}

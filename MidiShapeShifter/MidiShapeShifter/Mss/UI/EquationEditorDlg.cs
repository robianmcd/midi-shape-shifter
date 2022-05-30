using System;
using System.Drawing;
using System.Windows.Forms;

namespace MidiShapeShifter.Mss.UI
{
    public partial class EquationEditorDlg : Form
    {
        Control parentControl;

        public string equation = "";
        public int cursorPosition = 0;

        public EquationEditorDlg()
        {
            InitializeComponent();
        }

        public void Init(string equationStr, int initialCursorPosition, string insertCharacter, Control parentControl)
        {
            this.parentControl = parentControl;

            this.equationTextBox.Text =
                    equationStr.Substring(0, initialCursorPosition) +
                    insertCharacter +
                    equationStr.Substring(initialCursorPosition, equationStr.Length - initialCursorPosition);
            this.equationTextBox.SelectionStart = initialCursorPosition + insertCharacter.Length;
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            this.equation = this.equationTextBox.Text;
            this.cursorPosition = this.equationTextBox.SelectionStart;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void PopupEquationDlg_Load(object sender, EventArgs e)
        {
            Point storedLocation = Properties.Settings.Default.EquationEditorRelLocation;
            Point absParentPos = this.parentControl.PointToScreen(Point.Empty);

            this.Location = new Point(absParentPos.X + storedLocation.X, absParentPos.Y + storedLocation.Y);
            this.Size = Properties.Settings.Default.EquationEditorSize;
        }

        private void PopupEquationDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            Point absParentPos = this.parentControl.PointToScreen(Point.Empty);

            Properties.Settings.Default.EquationEditorRelLocation =
                new Point(this.Location.X - absParentPos.X, this.Location.Y - absParentPos.Y);
            Properties.Settings.Default.EquationEditorSize = this.Size;
            Properties.Settings.Default.Save();
        }


    }
}

using System.Windows.Forms;

namespace MidiShapeShifter.UI
{
    public partial class PluginEditorView : UserControl
    {
        public PluginEditorView()
        {
            InitializeComponent();
        }

        private void testKnob_KnobChangeValue(object sender, LBSoft.IndustrialCtrls.Knobs.LBKnobEventArgs e)
        {
            testKnobDisplay.Text = System.Math.Round((double) testKnob.Value, 2).ToString();
        }
    }
}

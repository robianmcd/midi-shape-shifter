using System.Windows.Forms;

namespace MidiShapeShifter.UI
{
    public partial class PluginEditorView : UserControl
    {
        public PluginEditorView()
        {
            InitializeComponent();
        }

        private void presetParam1Knob_KnobChangeValue(object sender, LBSoft.IndustrialCtrls.Knobs.LBKnobEventArgs e)
        {
            presetParam1Value.Text = System.Math.Round((double) presetParam1Knob.Value, 2).ToString();
        }
    }
}

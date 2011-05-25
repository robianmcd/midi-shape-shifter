using Jacobi.Vst.Core;
using Jacobi.Vst.Framework;
using Jacobi.Vst.Framework.Plugin;

namespace MidiShapeShifter.Framework
{
    /// <summary>
    /// This object is a dummy AudioProcessor only to be able to output Midi during the Audio processing cycle.
    /// </summary>
    public class DummyAudioHandler : VstPluginAudioProcessorBase
    {
        // some defaults
        private static readonly int AudioInputCount = 2;
        private static readonly int AudioOutputCount = 2;
        private static readonly int InitialTailSize = 0;

        private Plugin _plugin;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public DummyAudioHandler(Plugin plugin)
            : base(AudioInputCount, AudioOutputCount, InitialTailSize)
        {
            _plugin = plugin;
        }

        /// <summary>
        /// Called by the host to allow the plugin to process audio samples.
        /// </summary>
        /// <param name="inChannels">Never null.</param>
        /// <param name="outChannels">Never null.</param>
        public override void Process(VstAudioBuffer[] inChannels, VstAudioBuffer[] outChannels)
        {
            // calling the base class transfers input samples to the output channels unchanged (bypass).
            base.Process(inChannels, outChannels);

            // check to see if we need to output midi here
            if (_plugin.MidiHandler.SyncWithAudioProcessor)
            {
                _plugin.MidiHandler.ProcessCurrentEvents();
            }
        }
    }
}

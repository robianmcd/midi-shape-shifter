using Jacobi.Vst.Core;
using Jacobi.Vst.Framework;
using Jacobi.Vst.Framework.Plugin;

using MidiShapeShifter.CSharpUtil;
using MidiShapeShifter.Mss.Relays;

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

        protected IHostInfoReceiver hostInfoReceiver;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public DummyAudioHandler()
            : base(AudioInputCount, AudioOutputCount, InitialTailSize)
        {
        }

        public void Init(IHostInfoReceiver hostInfoReceiver)
        {
            this.hostInfoReceiver = hostInfoReceiver;
        }

        /// <summary>
        /// Called by the host to allow the plugin to process audio samples.
        /// </summary>
        /// <param name="inChannels">Never null.</param>
        /// <param name="outChannels">Never null.</param>
        public override void Process(VstAudioBuffer[] inChannels, VstAudioBuffer[] outChannels)
        {
            long cycleEndTimestampInTicks = System.DateTime.Now.Ticks;

            this.hostInfoReceiver.ReceiveProcessingCycleEndTimestampInTicks(cycleEndTimestampInTicks);

            // calling the base class transfers input samples to the output channels unchanged (bypass).
            base.Process(inChannels, outChannels);
        }
    }
}

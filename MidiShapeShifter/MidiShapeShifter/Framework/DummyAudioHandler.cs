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

        protected IHostInfoInputPort hostInfoInputPort;
        protected VstTimeInfoTransmitter timeInfoTransmitter;

        protected IVstHost vstHost;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public DummyAudioHandler()
            : base(AudioInputCount, AudioOutputCount, InitialTailSize)
        {
            timeInfoTransmitter = new VstTimeInfoTransmitter();
        }

        public void Init(IHostInfoInputPort hostInfoInputPort)
        {
            timeInfoTransmitter.Init(hostInfoInputPort);
            this.hostInfoInputPort = hostInfoInputPort;
        }

        //This cannot be done during Init() because the IVstHost is still null
        public void InitVstHost(IVstHost vstHost)
        {
            this.vstHost = vstHost;
        }

        /// <summary>
        /// Called by the host to allow the plugin to process audio samples.
        /// </summary>
        /// <param name="inChannels">Never null.</param>
        /// <param name="outChannels">Never null.</param>
        public override void Process(VstAudioBuffer[] inChannels, VstAudioBuffer[] outChannels)
        {
            long cycleEndTimestampInTicks = System.DateTime.Now.Ticks;

            IVstHostSequencer midiHostSeq = this.vstHost.GetInstance<IVstHostSequencer>();
            VstTimeInfo timeInfo = midiHostSeq.GetTime(VstTimeInfoTransmitter.RequiredTimeInfoFlags);

            this.timeInfoTransmitter.TransmitTimeInfoToRelay(timeInfo);
            this.hostInfoInputPort.ReceiveProcessingCycleEndTimestampInTicks(cycleEndTimestampInTicks);

            // calling the base class transfers input samples to the output channels unchanged (bypass).
            base.Process(inChannels, outChannels);
        }
    }
}

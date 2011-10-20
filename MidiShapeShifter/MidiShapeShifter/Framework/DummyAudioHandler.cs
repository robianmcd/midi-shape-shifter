using Jacobi.Vst.Core;
using Jacobi.Vst.Framework;
using Jacobi.Vst.Framework.Plugin;

using MidiShapeShifter.CSharpUtil;
using MidiShapeShifter.Mss.Relays;

namespace MidiShapeShifter.Framework
{
    /// <summary>
    ///     This plugin does not receive or output audio. However an implimentation of VstPluginAudioProcessorBase is 
    ///     still necessary becuase the plugin needs send midi to the host at the end of each the audio processing 
    ///     cycle. 
    /// </summary>
    public class DummyAudioHandler : VstPluginAudioProcessorBase
    {
        // some defaults
        private static readonly int AudioInputCount = 2;
        private static readonly int AudioOutputCount = 2;
        private static readonly int InitialTailSize = 0;

        //Receives information about the audio processing cycle and sends it out to which ever classes need to know 
        //about it.
        protected IHostInfoInputPort hostInfoInputPort;

        //Interprets a VstTimeInfo object and sends the relavent information out to the HostInfoRelay.
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
        /// Called by the host to allow the plugin to process audio samples. This method is used to inform the rest of 
        /// the plugin that the audio processing cycle is ending.
        /// </summary>
        public override void Process(VstAudioBuffer[] inChannels, VstAudioBuffer[] outChannels)
        {
            long cycleEndTimestampInTicks = System.DateTime.Now.Ticks;

            IVstHostSequencer midiHostSeq = this.vstHost.GetInstance<IVstHostSequencer>();
            VstTimeInfo timeInfo = midiHostSeq.GetTime(VstTimeInfoTransmitter.RequiredTimeInfoFlags);

            this.timeInfoTransmitter.TransmitTimeInfoToRelay(timeInfo, cycleEndTimestampInTicks);

            //Informs the HostInfoRelay that the audio processing cycle is ending.
            this.hostInfoInputPort.ReceiveProcessingCycleEndTimestampInTicks(cycleEndTimestampInTicks);

            // calling the base class transfers input samples to the output channels unchanged (bypass).
            base.Process(inChannels, outChannels);
        }
    }
}

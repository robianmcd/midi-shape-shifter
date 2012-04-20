using System;
using System.Diagnostics;

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

        protected long sampleTime = -1;

        //Receives information about the audio processing cycle and sends it out to which ever classes need to know 
        //about it.
        private Func<IHostInfoInputPort> getHostInfoInputPort;
        protected IHostInfoInputPort hostInfoInputPort { get { return this.getHostInfoInputPort(); } }

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

        public void Init(Func<IHostInfoInputPort>getHostInfoInputPort)
        {
            timeInfoTransmitter.Init(getHostInfoInputPort);
            this.getHostInfoInputPort = getHostInfoInputPort;
        }

        //This cannot be done during Init() because the IVstHost is still null
        public void InitVstHost(IVstHost vstHost)
        {
            this.vstHost = vstHost;
        }

        //When the plugin is deserialized a new instance of the classes in the MSS namespace will be 
        //created. These classes will assume that the sample time starts at 0.
        public void OnDeserialized()
        {
            this.sampleTime = -1;
        }

        /// <summary>
        /// Called by the host to allow the plugin to process audio samples. This method is used to inform the rest of 
        /// the plugin that the audio processing cycle is ending.
        /// </summary>
        public override void Process(VstAudioBuffer[] inChannels, VstAudioBuffer[] outChannels)
        {
            sampleTime += inChannels[0].SampleCount;

            IVstHostSequencer midiHostSeq = this.vstHost.GetInstance<IVstHostSequencer>();
            VstTimeInfo timeInfo = midiHostSeq.GetTime(VstTimeInfoTransmitter.RequiredTimeInfoFlags);

            this.timeInfoTransmitter.TransmitTimeInfoToRelay(timeInfo, sampleTime);

            //Informs the HostInfoRelay that the audio processing cycle is ending.
            this.hostInfoInputPort.TriggerProcessingCycleEnd(sampleTime);

            // calling the base class transfers input samples to the output channels unchanged (bypass).
            base.Process(inChannels, outChannels);
        }

    }
}

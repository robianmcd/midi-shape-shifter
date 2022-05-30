
using MidiShapeShifter.Mss.Relays;

namespace MidiShapeShifter.Mss
{
    /// <summary>
    ///     Responcible for passing processing cycle end messages from the HostInfoRelay to the WetMssEventRelay which
    ///     can trigger events in the WetMssEventRelay to be sent out to the host.
    /// </summary>
    public class SendMssEventsToHostTrigger
    {
        protected IWetMssEventInputPort wetMssEventInputPort;

        public void Init(IHostInfoOutputPort hostInfoOutputPort, IWetMssEventInputPort wetMssEventInputPort)
        {
            hostInfoOutputPort.ProcessingCycleEnd += new ProcessingCycleEndEventHandler(hostInfoOutputPort_ProcessingCycleEndSampleTimeRecieved);
            this.wetMssEventInputPort = wetMssEventInputPort;
        }

        protected void hostInfoOutputPort_ProcessingCycleEndSampleTimeRecieved(long sampleTimeAtEndOfCycle)
        {
            this.wetMssEventInputPort.OnProcessingCycleEnd(sampleTimeAtEndOfCycle);
        }
    }
}

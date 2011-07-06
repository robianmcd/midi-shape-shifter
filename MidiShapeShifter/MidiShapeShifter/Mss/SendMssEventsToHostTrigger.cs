﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            hostInfoOutputPort.ProcessingCycleEndTimestampRecieved += new ProcessingCycleEndTimestampRecievedEventHandler(hostInfoOutputPort_ProcessingCycleEndTimestampRecieved);
            this.wetMssEventInputPort = wetMssEventInputPort;
        }

        protected void hostInfoOutputPort_ProcessingCycleEndTimestampRecieved(long cycleEndTimestampInTicks)
        {
            this.wetMssEventInputPort.OnProcessingCycleEnd(cycleEndTimestampInTicks);
        }
    }
}

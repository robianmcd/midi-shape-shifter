using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.Relays
{
    public class HostInfoRelay : IHostInfoReceiver, IHostInfoEchoer
    {
        public event ProcessingCycleEndTimestampRecievedEventHandler ProcessingCycleEndTimestampRecieved;


        public void ReceiveProcessingCycleEndTimestampInTicks(long cycleEndTimeStampInTicks)
        {
            if (ProcessingCycleEndTimestampRecieved != null)
            {
                ProcessingCycleEndTimestampRecieved(cycleEndTimeStampInTicks);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.Relays
{
    /// <summary>
    ///     Accepts information from the host and sends a messages notifying any subscribers of the information.
    /// </summary>
    /// <remarks>
    ///     This class is used to pass host information from the "Framework" namespace to the "Mss" namespace.
    /// </remarks>
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.Relays
{
    public delegate void ProcessingCycleEndTimestampRecievedEventHandler(long cycleEndTimestampInTicks);


    public interface IHostInfoEchoer
    {
        event ProcessingCycleEndTimestampRecievedEventHandler ProcessingCycleEndTimestampRecieved;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Jacobi.Vst.Core;

using MidiShapeShifter.Mss.Relays;

namespace MidiShapeShifter.Framework
{
    public class VstTimeInfoTransmitter
    {
        //might need VstTimeInfoFlags.BarStartPositionValid
        public static readonly VstTimeInfoFlags RequiredTimeInfoFlags = VstTimeInfoFlags.ClockValid |
                                                                       VstTimeInfoFlags.PpqPositionValid |
                                                                       VstTimeInfoFlags.TempoValid |
                                                                       VstTimeInfoFlags.TimeSignatureValid;

        protected IHostInfoInputPort hostInfoInputPort;

        public void Init(IHostInfoInputPort hostInfoInputPort)
        {
            this.hostInfoInputPort = hostInfoInputPort;
        }

        public void TransmitTimeInfoToRelay(VstTimeInfo timeInfo)
        {
            //Ensures that the host is transmitting all the required time info.
            Debug.Assert((timeInfo.Flags & RequiredTimeInfoFlags) == RequiredTimeInfoFlags);

            this.hostInfoInputPort.ReceiveSampleRate(timeInfo.SampleRate);
        }
    }
}

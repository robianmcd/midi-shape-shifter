using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MidiShapeShifter.Mss.Relays;
using MidiShapeShifter.Mss.Mapping;

namespace MidiShapeShifter.Mss.Generator
{
    // TODO: comment this calss
    public class MssEventGenerator
    {
        protected IHostInfoOutputPort hostInfoOutputPort;
        protected IDryMssEventInputPort dryMssEventInputPort;

        protected GeneratorMappingManager generatorMappingMgr;

        public int PreviousGenId { get; private set; }

        public MssEventGenerator()
        {
            PreviousGenId = -1;
        }

        public void Init(IHostInfoOutputPort hostInfoOutputPort, 
                         IWetMssEventOutputPort wetMssEventOutputPort, 
                         IDryMssEventInputPort dryMssEventInputPort,
                         GeneratorMappingManager generatorMappingMgr)
        {
            wetMssEventOutputPort.WetMssEventsReceived += new WetMssEventsReceivedEventHandler(WetMssEventOutputPort_WetMssEventsReceived);

            this.hostInfoOutputPort = hostInfoOutputPort;
            this.dryMssEventInputPort = dryMssEventInputPort;

            this.generatorMappingMgr = generatorMappingMgr;
        }

        protected void WetMssEventOutputPort_WetMssEventsReceived(List<MssEvent> mssEventList)
        {
            foreach(MssEvent mssEvent in mssEventList)
            {
                if (mssEvent.mssMsg.Type == MssMsgType.GeneratorToggle) 
                {
                    //TODO: deal with GeneratorToggle message
                }
            }
        }

    }
}

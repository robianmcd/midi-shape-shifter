using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ninject;
using MidiShapeShifter.Ioc;

using MidiShapeShifter.Mss.Relays;
using MidiShapeShifter.Mss.Mapping;
using MidiShapeShifter.Mss.Parameters;

namespace MidiShapeShifter.Mss
{
    /// <summary>
    ///     Responsible for listening to dry MssEvents coming through the DryMssEventRelay, ensuring that they get 
    ///     processed and passing them out to the WetMssEventRelay.
    /// </summary>
    public class DryMssEventHandler
    {
        /// <summary>
        ///     Used to process incomming MssEvents
        /// </summary>
        protected IMssMsgProcessor mssMsgProcessor;

        /// <summary>
        ///     Receives MssEvents once they have been processed
        /// </summary>
        protected IWetMssEventInputPort wetMssEventInputPort;

        public DryMssEventHandler()
        {
            this.mssMsgProcessor = IocMgr.Kernel.Get<IMssMsgProcessor>();
        }

        /// <summary>
        ///     Initializes this DryMssEventHandler. Other methods in this class should not be called beofre calling 
        ///     Init().
        /// </summary>
        /// <param name="dryMssEventOutputPort">Sends unprocessed MssEvents from the host.</param>
        /// <param name="wetMssEventInputPort">Receives processed MssEvents to be sent back to the host</param>
        /// <param name="mappingMgr">The MappingManager that will be used by mssMsgProcessor</param>
        public void Init(IDryMssEventOutputPort dryMssEventOutputPort, 
                         IWetMssEventInputPort wetMssEventInputPort, 
                         MappingManager mappingMgr,
                         IMssParameterViewer mssParameters)
        {
            this.mssMsgProcessor.Init(mappingMgr, mssParameters);

            this.wetMssEventInputPort = wetMssEventInputPort;

            dryMssEventOutputPort.DryMssEventRecieved += new DryMssEventRecievedEventHandler(dryMssEventOutputPort_DryMssEventRecieved);
        }

        /// <summary>
        ///     Event handler for MssEvents coming
        /// </summary>
        /// <param name="dryMssEvent"></param>
        protected void dryMssEventOutputPort_DryMssEventRecieved(MssEvent dryMssEvent)
        {
            //Process incoming MssEvent
            List<MssMsg> mssMessages = this.mssMsgProcessor.ProcessMssMsg(dryMssEvent.mssMsg);

            List<MssEvent> wetEventList = new List<MssEvent>(mssMessages.Count);
            //Convert the list of processed MssMsgs into a list of MssEvents.
            foreach (MssMsg mssMsg in mssMessages)
            {
                MssEvent wetEvent = new MssEvent();
                wetEvent.mssMsg = mssMsg;
                wetEvent.sampleTime = dryMssEvent.sampleTime;
                wetEventList.Add(wetEvent);
            }

            //Send the processed MssEvents to the WetMssEventRelay
            this.wetMssEventInputPort.ReceiveWetMssEventList(wetEventList);
        }
    }
}

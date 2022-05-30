
using MidiShapeShifter.Ioc;
using MidiShapeShifter.Mss.Mapping;
using MidiShapeShifter.Mss.Parameters;
using MidiShapeShifter.Mss.Relays;
using Ninject;

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
                         IMappingManager mappingMgr,
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
            foreach (MssMsg mssMsg in this.mssMsgProcessor.ProcessMssMsg(dryMssEvent.mssMsg))
            {
                MssEvent wetEvent = new MssEvent
                {
                    mssMsg = mssMsg,
                    sampleTime = dryMssEvent.sampleTime
                };

                //Send the processed MssEvent to the WetMssEventRelay
                this.wetMssEventInputPort.ReceiveWetMssEvent(wetEvent);
            }
        }
    }
}

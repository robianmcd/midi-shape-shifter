using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MidiShapeShifter.CSharpUtil
{
    public class SendRateLimittedEvent<eventParamsType>
    {

        protected AutoResetEvent paramInfoRecieved;
        protected Task eventSender;

        protected object eventParamsLock;
        protected eventParamsType nextEventParams;

        public SendRateLimittedEvent(int minMillsBetweenEvents, Action<eventParamsType> callEvent)
        {
            this.paramInfoRecieved = new AutoResetEvent(false);
            eventParamsLock = new object();

            eventSender = new Task(() => { 
                while (true) {
                    this.paramInfoRecieved.WaitOne();
                    lock (this.eventParamsLock) {
                        callEvent(this.nextEventParams);
                    }
                    Thread.Sleep(minMillsBetweenEvents);
                }
            });

            eventSender.Start();
        }

        public void SendEvent(eventParamsType eventParams) {
            lock(this.eventParamsLock) {
                this.nextEventParams = eventParams;
                this.paramInfoRecieved.Set();
            }
        }
    }
}

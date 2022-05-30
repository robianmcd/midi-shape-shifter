using MidiShapeShifter.CSharpUtil;
using NUnit.Framework;
using System.Threading;

namespace MidiShapeShifterTest.CSharpUtil
{
    internal class SendRateLimittedEventTest
    {
        //I was getting some random failures with this set to 500.
        protected const int EVENT_WAIT_TIMEOUT = 1000;

        [Test]
        public void SendEvent_SingleEvent_EventIsSent()
        {
            AutoResetEvent eventSent = new AutoResetEvent(false);

            SendRateLimittedEvent<int> limittedEvent = new SendRateLimittedEvent<int>(100, num =>
            {
                eventSent.Set();
            });

            limittedEvent.SendEvent(1);

            bool signalRecieved = eventSent.WaitOne();

            Assert.IsTrue(signalRecieved);
        }

        [Test]
        public void SendEvent_multipleEvents_OnlyTheFirstAndLastEventsAreSent()
        {
            AutoResetEvent eventSent = new AutoResetEvent(false);

            int numberSetByEvent = -1;
            SendRateLimittedEvent<int> limittedEvent = new SendRateLimittedEvent<int>(100, num =>
            {
                numberSetByEvent = num;
                eventSent.Set();
            });

            limittedEvent.SendEvent(1);

            eventSent.WaitOne(1000);
            Assert.AreEqual(1, numberSetByEvent);

            limittedEvent.SendEvent(2);
            limittedEvent.SendEvent(3);
            limittedEvent.SendEvent(4);

            Assert.AreEqual(1, numberSetByEvent);

            limittedEvent.SendEvent(5);

            eventSent.WaitOne(1000);
            Assert.AreEqual(5, numberSetByEvent);

        }

        [Test]
        public void SendEvent_multipleEvents_LastEventIsOutputAfterTimeIsUp()
        {
            AutoResetEvent eventSent = new AutoResetEvent(false);

            int numberSetByEvent = -1;
            SendRateLimittedEvent<int> limittedEvent = new SendRateLimittedEvent<int>(100, num =>
            {
                numberSetByEvent = num;
                eventSent.Set();
            });

            limittedEvent.SendEvent(1);

            eventSent.WaitOne(1000);
            Assert.AreEqual(1, numberSetByEvent);

            limittedEvent.SendEvent(2);
            limittedEvent.SendEvent(3);
            limittedEvent.SendEvent(4);

            Assert.AreEqual(1, numberSetByEvent);

            Thread.Sleep(200);

            Assert.AreEqual(4, numberSetByEvent);

        }

    }
}

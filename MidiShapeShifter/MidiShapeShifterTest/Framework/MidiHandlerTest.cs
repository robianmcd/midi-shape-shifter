using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using Moq;

using Jacobi.Vst.Core;
using Jacobi.Vst.Framework;

using MidiShapeShifter.Framework;
using MidiShapeShifter.Midi;
using MidiShapeShifter.Mss;
using MidiShapeShifter.Mss.Relays;

namespace MidiShapeShifterTest.Framework
{
    [TestFixture]
    class MidiHandlerTest
    {
        protected IDryMssEventInputPort dryMssEventInputPort;
        protected IWetMssEventOutputPort wetMssEventOutputPort;
        protected IHostInfoOutputPort hostInfoOutputPort;

        protected int numDryEventsReceivedByRelay;
        protected int sampleRate;
        protected const int DEFAULT_SAMPLE_RATE = 44000;

        [SetUp]
        public void Init()
        {
            this.numDryEventsReceivedByRelay = 0;
            this.sampleRate = DEFAULT_SAMPLE_RATE;

            DryMssEventRelay dryMssEventRelay = new DryMssEventRelay();
            dryMssEventRelay.DryMssEventRecieved += new DryMssEventRecievedEventHandler(dryMssEventInputPort_DryMssEventRecieved);
            this.dryMssEventInputPort = dryMssEventRelay;

            this.wetMssEventOutputPort = new WetMssEventRelay();

            HostInfoRelay hostInfoRelay = new HostInfoRelay();
            hostInfoRelay.StartUpdate();
            hostInfoRelay.ReceiveSampleRateDuringUpdate(this.sampleRate);
            hostInfoRelay.FinishUpdate();
            this.hostInfoOutputPort = hostInfoRelay;
        }

        [Test]
        public void Process_NoteOn_SentToRelay()
        {
            VstEventCollection vstEvents = new VstEventCollection();
            vstEvents.Add(Factory_VstMidiEvent_Basic(0, CreateMidiDataWithDefaultValues(MssMsgType.NoteOn)));

            Test_Process_VstEventCollection_SentToReplay(vstEvents, 1);
        }

        [Test]
        public void Process_MultipleMsgTypes_SentToRelay()
        {
            VstEventCollection vstEvents = new VstEventCollection();
            vstEvents.Add(Factory_VstMidiEvent_Basic(0, CreateMidiDataWithDefaultValues(MssMsgType.NoteOn)));
            vstEvents.Add(Factory_VstMidiEvent_Basic(0, CreateMidiDataWithDefaultValues(MssMsgType.NoteOff)));
            vstEvents.Add(Factory_VstMidiEvent_Basic(0, CreateMidiDataWithDefaultValues(MssMsgType.CC)));
            vstEvents.Add(Factory_VstMidiEvent_Basic(0, CreateMidiDataWithDefaultValues(MssMsgType.ChanAftertouch)));
            vstEvents.Add(Factory_VstMidiEvent_Basic(0, CreateMidiDataWithDefaultValues(MssMsgType.PitchBend)));
            vstEvents.Add(Factory_VstMidiEvent_Basic(0, CreateMidiDataWithDefaultValues(MssMsgType.PolyAftertouch)));

            Test_Process_VstEventCollection_SentToReplay(vstEvents, 6);
        }

        [Test]
        public void Process_SysExEvent_NothingSentToRelay()
        {
            VstEventCollection vstEvents = new VstEventCollection();
            VstMidiSysExEvent unsupportedEvent = new VstMidiSysExEvent(0, new byte[3]);
            vstEvents.Add(unsupportedEvent);

            Test_Process_VstEventCollection_SentToReplay(vstEvents, 0);
        }

        [Test]
        public void Process_SomeSupportedSomeUnsupporedEvents_SomeSentToRelay()
        {
            VstEventCollection vstEvents = new VstEventCollection();

            //Supported event type
            vstEvents.Add(Factory_VstMidiEvent_Basic(0, CreateMidiDataWithDefaultValues(MssMsgType.PitchBend)));
            
            //Unsupported event type
            VstMidiSysExEvent unsupportedEvent = new VstMidiSysExEvent(0, new byte[3]);
            vstEvents.Add(unsupportedEvent);

            //Supported event type
            vstEvents.Add(Factory_VstMidiEvent_Basic(0, CreateMidiDataWithDefaultValues(MssMsgType.PolyAftertouch)));

            Test_Process_VstEventCollection_SentToReplay(vstEvents, 2);
        }

        [Test]
        public void ConvertVstMidiEventToMssEvent_StandardMsg_CreatesAssociatedMssEvent()
        {
            Test_MsgConversion(MssMsgType.NoteOn, 1, 64, 100, false);
        }

        //There is a special case for pitch bend because the second and third byte of a pitch bend's midi data are 
        //treated as one number in an MssMsg unlike other message types.
        [Test]
        public void ConvertVstMidiEventToMssEvent_PitchBend_CreatesAssociatedMssEvent()
        {
            Test_MsgConversion(MssMsgType.PitchBend, 8, 12, 34, false);
        }

        [Test]
        public void ConvertVstMidiEventToMssEvent_ExtremeValues_CreatesAssociatedMssEvent()
        {
            Test_MsgConversion(MssMsgType.PolyAftertouch, 16, 0, 127, false);
        }

        [Test]
        public void ConvertMssEventToVstMidiEvent_StandardMsg_CreatesAssociatedMssEvent()
        {
            Test_MsgConversion(MssMsgType.NoteOn, 1, 64, 100, true);
        }

        //There is a special case for pitch bend because the second and third byte of a pitch bend's midi data are 
        //treated as one number in an MssMsg unlike other message types.
        [Test]
        public void ConvertMssEventToVstMidiEvent_PitchBend_CreatesAssociatedMssEvent()
        {
            Test_MsgConversion(MssMsgType.PitchBend, 8, 12, 34, true);
        }

        [Test]
        public void ConvertMssEventToVstMidiEvent_ExtremeValues_CreatesAssociatedMssEvent()
        {
            Test_MsgConversion(MssMsgType.PolyAftertouch, 16, 0, 127, true);
        }

        [Test]
        public void ConvertMssEventToVstMidiEvent_NoValidConversion_ReturnsNull()
        {
            MidiHandlerProtectedWrapper midiHandler = Factory_MidiHandler_Basic();

            long sampleTime = 123456;

            MssEvent internalEvent = new MssEvent();
            internalEvent.mssMsg = new MssMsg(MssMsgType.Generator, 0, 0, 0);
            internalEvent.sampleTime = sampleTime;

            VstMidiEvent convertedEvent = midiHandler.ConvertMssEventToVstMidiEventWrapper(
                internalEvent, sampleTime, this.hostInfoOutputPort.SampleRate);

            Assert.IsNull(convertedEvent);
        }

        //*******************************Helpers*******************************


        //if testMssToMidi is true then this method tests ConvertMssEventToVstMidiEvent. Otherwise this method tests ConvertVstMidiEventToMssEvent
        protected void Test_MsgConversion(MssMsgType msgType, int midiChannel, int midiParam1, int midiParam2, bool testMssToMidi)
        {
            long sampleTimeAtStartOfCycle = 12345;
            int deltaFrames = 789;

            MidiHandlerProtectedWrapper midiHandler = Factory_MidiHandler_Basic();

            MssEvent mssEvent = new MssEvent();

            if (msgType == MssMsgType.PitchBend)
            {
                mssEvent.mssMsg = new MssMsg(msgType, midiChannel, MssMsgUtil.UNUSED_MSS_MSG_DATA, (midiParam2 << 7) + midiParam1);
            }
            else
            {
                mssEvent.mssMsg = new MssMsg(msgType, midiChannel, midiParam1, midiParam2);
            }
            mssEvent.sampleTime = sampleTimeAtStartOfCycle + deltaFrames;

            byte[] midiData = MidiUtil.CreateMidiData(msgType, midiChannel, (byte)midiParam1, (byte)midiParam2);
            VstMidiEvent midiEvent = new VstMidiEvent(deltaFrames, 0, 0, midiData, 0, 0x00);

            if (testMssToMidi == true)
            {
                MssEvent convertedMssEvent = midiHandler.ConvertVstMidiEventToMssEventWrapper(
                    midiEvent, sampleTimeAtStartOfCycle, this.hostInfoOutputPort.SampleRate);
                Assert.AreEqual(mssEvent, convertedMssEvent);
            }
            else
            {
                VstMidiEvent ConvertedMidiEvent = midiHandler.ConvertMssEventToVstMidiEventWrapper(
                    mssEvent, sampleTimeAtStartOfCycle, this.hostInfoOutputPort.SampleRate);
                Assert.AreEqual(midiEvent.Data, ConvertedMidiEvent.Data);
                Assert.AreEqual(midiEvent.DeltaFrames, ConvertedMidiEvent.DeltaFrames);
            }
        }

        protected void Test_Process_VstEventCollection_SentToReplay(VstEventCollection vstEvents, int NumMssCompatibleEvents)
        {
            MidiHandlerProtectedWrapper midiHandler = Factory_MidiHandler_Basic();
            midiHandler.Process(vstEvents);

            Assert.AreEqual(numDryEventsReceivedByRelay, NumMssCompatibleEvents);
        }

        public void dryMssEventInputPort_DryMssEventRecieved(MssEvent dryMssEvent)
        {
            numDryEventsReceivedByRelay++;
        }

        protected byte[] CreateMidiDataWithDefaultValues(MssMsgType msgType)
        {
            return MidiUtil.CreateMidiData(msgType, 1, 0x00, 0x00);
        }

        protected MidiHandlerProtectedWrapper Factory_MidiHandler_Basic()
        {
            MidiHandlerProtectedWrapper midiHandler = new MidiHandlerProtectedWrapper();
            midiHandler.Init(this.dryMssEventInputPort, this.wetMssEventOutputPort, this.hostInfoOutputPort);

            var vstHostMock = new Mock<IVstHost>();
            midiHandler.InitVstHost(vstHostMock.Object);

            return midiHandler;
        }

        protected VstMidiEvent Factory_VstMidiEvent_Basic(int deltaFrames, byte[] midiData)
        {
            return new VstMidiEvent(deltaFrames, 0, 0, midiData, 0, 0x00);
        }
    }
}

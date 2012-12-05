using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ninject;
using MidiShapeShifter.Ioc;

using NUnit.Framework;
using Moq;

using MidiShapeShifter.Mss.Generator;
using MidiShapeShifter.Mss;
using MidiShapeShifter.Mss.Relays;
using MidiShapeShifter.Mss.Parameters;

namespace MidiShapeShifterTest.Mss.Generator
{
    [TestFixture]
    class MssEventGeneratorTest
    {
        //Object to run tests on
        protected MssEventGenerator eventGenerator;

        protected Mock<IMssMsgProcessor> msgProcessorMock;

        protected Mock<IHostInfoRelay> hostInfoRelayMock;
        protected Mock<IDryMssEventRelay> dryEventRelayMock;
        protected Mock<IWetMssEventRelay> wetEventRelayMock;

        protected Mock<IGeneratorMappingManager> genMappingMgrMock;

        protected Mock<IMssParameterViewer> mssParameterViewerMock;

        protected List<IGeneratorMappingEntry> genMappingEntryList = new List<IGeneratorMappingEntry>();

        //Host info
        protected double host_SampleRate;
        protected bool host_SampleRateIsInitialized;

        protected double host_Tempo;
        protected bool host_TempoIsInitialized;

        protected int host_TimeSignatureNumerator;
        protected int host_TimeSignatureDenominator;
        protected bool host_TimeSignatureIsInitialized;

        protected bool host_TransportPlaying;
        protected bool host_TransportPlayingIsInitialized;

        protected long host_CalculatedBarZeroSampleTime;
        protected bool host_CalculatedBarZeroIsInitialized;

        protected bool host_BarPosIsInitialized;

        protected const int DUMMY_CYCLE_END_TIME = 12345;
        protected const double BAR_POS_INCREMENT = 0.25;

        protected List<MssEvent> generatedEventList = new List<MssEvent>();
 
        [SetUp]
        public void Init()
        {
            this.msgProcessorMock = new Mock<IMssMsgProcessor>();
            this.msgProcessorMock.Setup(processor => processor.ProcessMssMsg(It.IsAny<MssMsg>()))
                                 .Returns((MssMsg msg) => ProcessMssMsg_CopyData3(msg));
            IocMgr.Kernel.Rebind<IMssMsgProcessor>().ToConstant(msgProcessorMock.Object);

            this.mssParameterViewerMock = new Mock<IMssParameterViewer>();

            this.hostInfoRelayMock = new Mock<IHostInfoRelay>();

            //Link host info member variables to hostInfoRelayMock
            this.hostInfoRelayMock.Setup(relay => relay.SampleRate)
                                  .Returns(() => this.host_SampleRate);
            this.hostInfoRelayMock.Setup(relay => relay.SampleRateIsInitialized)
                                  .Returns(() => this.host_SampleRateIsInitialized);

            this.hostInfoRelayMock.Setup(relay => relay.Tempo)
                                  .Returns(() => this.host_Tempo);
            this.hostInfoRelayMock.Setup(relay => relay.TempoIsInitialized)
                                  .Returns(() => this.host_TempoIsInitialized);

            this.hostInfoRelayMock.Setup(relay => relay.TimeSignatureNumerator)
                                  .Returns(() => this.host_TimeSignatureNumerator);
            this.hostInfoRelayMock.Setup(relay => relay.TimeSignatureDenominator)
                                  .Returns(() => this.host_TimeSignatureDenominator);
            this.hostInfoRelayMock.Setup(relay => relay.TimeSignatureIsInitialized)
                                  .Returns(() => this.host_TimeSignatureIsInitialized);

            this.hostInfoRelayMock.Setup(relay => relay.CalculatedBarZeroSampleTime)
                                  .Returns(() => this.host_CalculatedBarZeroSampleTime);
            this.hostInfoRelayMock.Setup(relay => relay.CalculatedBarZeroIsInitialized)
                                  .Returns(() => this.host_CalculatedBarZeroIsInitialized);

            this.hostInfoRelayMock.Setup(relay => relay.TransportPlaying)
                                  .Returns(() => this.host_TransportPlaying);
            this.hostInfoRelayMock.Setup(relay => relay.TransportPlayingIsInitialized)
                                  .Returns(() => this.host_TransportPlayingIsInitialized);

            this.hostInfoRelayMock.Setup(relay => relay.BarPosIsInitialized)
                                  .Returns(() => this.host_BarPosIsInitialized);

            InitializeHostInfo_Defaults();

            this.hostInfoRelayMock.Setup(relay => relay.GetBarPosAtSampleTime(It.IsAny<long>()))
                .Returns((long sampleTime) => GetBarPosAtSampleTime_IncrementByOneQuarter(sampleTime));

            this.dryEventRelayMock = new Mock<IDryMssEventRelay>();

            //Setup ReceiveDryMssEvent() so that it adds all received events to generatedEvents.
            this.dryEventRelayMock.Setup(relay => relay.ReceiveDryMssEvent(It.IsAny<MssEvent>()))
                .Callback((MssEvent mssEvent) => generatedEventList.Add(mssEvent));

            this.wetEventRelayMock = new Mock<IWetMssEventRelay>();


            this.genMappingMgrMock = new Mock<IGeneratorMappingManager>();

            //Link up genMappingMgrMock with genMappingEntryList;
            this.genMappingMgrMock.Setup(mgr => mgr.GetNumEntries()).Returns(() => this.genMappingEntryList.Count);
            //TODO: this should really return a deep copy of genMappingEntryList instead of a shallow copy
            this.genMappingMgrMock.Setup(mgr => mgr.GetCopyOfMappingEntryList()).Returns(() => new List<IGeneratorMappingEntry>(this.genMappingEntryList));

            eventGenerator = new MssEventGenerator();
            eventGenerator.Init(this.hostInfoRelayMock.Object,
                                this.wetEventRelayMock.Object,
                                this.dryEventRelayMock.Object,
                                this.genMappingMgrMock.Object,
                                this.mssParameterViewerMock.Object);
        }

        [TearDown]
        public void Dispose()
        {
            this.genMappingEntryList.Clear();
            this.generatedEventList.Clear();

            this.nextBarPos = 0;
        }
        
        [Test]
        public void OnUpdate_EmptyMappingMgr_NoEventsGenerated()
        {
            triggerCycleEnd(DUMMY_CYCLE_END_TIME);

            Assert.IsEmpty(this.generatedEventList);
        }

        [Test]
        public void OnUpdate_HostInfoUninitialized_NoEventsGenerated()
        {
            this.host_SampleRateIsInitialized = false;
            this.host_CalculatedBarZeroIsInitialized = false;
            this.host_TempoIsInitialized = false;
            this.host_TimeSignatureIsInitialized = false;
            this.host_TransportPlayingIsInitialized = false;
            this.host_BarPosIsInitialized = false;

            Mock<IGeneratorMappingEntry> beatSyncedMapEntry = MockFactory_IGeneratorMappingEntry();
            beatSyncedMapEntry.Object.GenConfigInfo.PeriodType = GenPeriodType.BeatSynced;

            Mock<IGeneratorMappingEntry> timeBasedMapEntry = MockFactory_IGeneratorMappingEntry();
            timeBasedMapEntry.Object.GenConfigInfo.PeriodType = GenPeriodType.Time;

            this.genMappingEntryList.Add(beatSyncedMapEntry.Object);
            this.genMappingEntryList.Add(timeBasedMapEntry.Object);

            triggerCycleEnd(DUMMY_CYCLE_END_TIME);


            Assert.IsEmpty(this.generatedEventList);
        }

        [Test]
        public void OnUpdate_TransportStoppedForBeatGen_NoEventsGenerated()
        {
            this.host_TransportPlaying = false;

            Mock<IGeneratorMappingEntry> mapEntryMock = MockFactory_IGeneratorMappingEntry();
            mapEntryMock.Object.GenConfigInfo.PeriodType = GenPeriodType.BeatSynced;

            this.genMappingEntryList.Add(mapEntryMock.Object);

            triggerCycleEnd(DUMMY_CYCLE_END_TIME);

            Assert.IsEmpty(this.generatedEventList);
        }

        [Test]
        public void OnUpdate_GeneratorDisabled_NoEventsGenerated()
        {
            Mock<IGeneratorMappingEntry> mapEntryMock = MockFactory_IGeneratorMappingEntry();
            mapEntryMock.Object.GenConfigInfo.Enabled = false;

            this.genMappingEntryList.Add(mapEntryMock.Object);

            triggerCycleEnd(DUMMY_CYCLE_END_TIME);

            Assert.IsEmpty(this.generatedEventList);
        }

        [Test]
        public void OnUpdate_UninitializedGenerator_GeneratorGetInitialized()
        {
            Mock<IGeneratorMappingEntry> mapEntryMock = MockFactory_IGeneratorMappingEntry();
            mapEntryMock.Object.GenHistoryInfo.Initialized = false;

            this.genMappingEntryList.Add(mapEntryMock.Object);

            triggerCycleEnd(DUMMY_CYCLE_END_TIME);

            Assert.IsTrue(mapEntryMock.Object.GenHistoryInfo.Initialized);
        }

        [Test]
        public void OnUpdate_MultipleUpdatesInCycle_MultipleEventsGenerated()
        {
            Mock<IGeneratorMappingEntry> mapEntryMock = MockFactory_IGeneratorMappingEntry();

            this.genMappingEntryList.Add(mapEntryMock.Object);

            triggerCycleEnd(MssEventGenerator.SAMPLES_PER_GENERATOR_UPDATE * 3);

            Assert.AreEqual(3, this.generatedEventList.Count);
        }

        [Test]
        public void OnUpdate_MultipleUpdatesInCycle_PeriodPositionWrapsAround()
        {
            Mock<IGeneratorMappingEntry> mapEntryMock = MockFactory_IGeneratorMappingEntry();

            this.genMappingEntryList.Add(mapEntryMock.Object);

            triggerCycleEnd(MssEventGenerator.SAMPLES_PER_GENERATOR_UPDATE * 5);

            Assert.AreEqual(BAR_POS_INCREMENT * 0 % 1, this.generatedEventList[0].mssMsg.Data3);
            Assert.AreEqual(BAR_POS_INCREMENT * 1 % 1, this.generatedEventList[1].mssMsg.Data3);
            Assert.AreEqual(BAR_POS_INCREMENT * 2 % 1, this.generatedEventList[2].mssMsg.Data3);
            Assert.AreEqual(BAR_POS_INCREMENT * 3 % 1, this.generatedEventList[3].mssMsg.Data3);
            Assert.AreEqual(BAR_POS_INCREMENT * 4 % 1, this.generatedEventList[4].mssMsg.Data3);
        }

        [Test]
        public void OnUpdate_MultipleGenerators_EventGeneratedForEachGenerator()
        {
            Mock<IGeneratorMappingEntry> beatSyncedMapEntryMock = MockFactory_IGeneratorMappingEntry();
            beatSyncedMapEntryMock.Object.GenConfigInfo.PeriodType = GenPeriodType.BeatSynced;
            
            Mock<IGeneratorMappingEntry> timeBasedMapEntryMock = MockFactory_IGeneratorMappingEntry();
            timeBasedMapEntryMock.Object.GenConfigInfo.PeriodType = GenPeriodType.Time;

            this.genMappingEntryList.Add(beatSyncedMapEntryMock.Object);
            this.genMappingEntryList.Add(timeBasedMapEntryMock.Object);

            triggerCycleEnd(MssEventGenerator.SAMPLES_PER_GENERATOR_UPDATE);

            Assert.AreEqual(2, this.generatedEventList.Count);
        }

        [Test]
        public void OnUpdate_MultipleGenerators_GeneratorEventsGenerated()
        {
            Mock<IGeneratorMappingEntry> beatSyncedMapEntryMock = MockFactory_IGeneratorMappingEntry();
            beatSyncedMapEntryMock.Object.GenConfigInfo.PeriodType = GenPeriodType.BeatSynced;

            Mock<IGeneratorMappingEntry> timeBasedMapEntryMock = MockFactory_IGeneratorMappingEntry();
            timeBasedMapEntryMock.Object.GenConfigInfo.PeriodType = GenPeriodType.Time;

            this.genMappingEntryList.Add(beatSyncedMapEntryMock.Object);
            this.genMappingEntryList.Add(timeBasedMapEntryMock.Object);

            triggerCycleEnd(MssEventGenerator.SAMPLES_PER_GENERATOR_UPDATE);

            //We subtract 1 because sample time starts at 0.
            int expectedSampleTime = MssEventGenerator.SAMPLES_PER_GENERATOR_UPDATE - 1;

            Assert.AreEqual(expectedSampleTime, this.generatedEventList[0].sampleTime);
            Assert.AreEqual(MssMsgType.Generator, this.generatedEventList[0].mssMsg.Type);
            Assert.AreEqual(expectedSampleTime, this.generatedEventList[1].sampleTime);
            Assert.AreEqual(MssMsgType.Generator, this.generatedEventList[1].mssMsg.Type);
        }

        protected void triggerCycleEnd(long sampleTime)
        {
            hostInfoRelayMock.Raise(hostRelay => 
                hostRelay.BeforeProcessingCycleEnd += null, sampleTime);
            hostInfoRelayMock.Raise(hostRelay =>
                hostRelay.ProcessingCycleEnd += null, sampleTime);
        }

        protected List<MssMsg> ProcessMssMsg_CopyData3(MssMsg mssMsg)
        {
            MssMsg msg = new MssMsg();
            msg.Type = MssMsgType.Generator;
            msg.Data1 = 0;
            msg.Data2 = MssMsgUtil.UNUSED_MSS_MSG_DATA;
            msg.Data3 = mssMsg.Data3;

            List<MssMsg> retList = new List<MssMsg>();
            retList.Add(msg);
            return retList;
        }

        protected double nextBarPos = 0;
        protected double GetBarPosAtSampleTime_IncrementByOneQuarter(long dummySampleTime)
        {
            double barPos = this.nextBarPos;
            this.nextBarPos += BAR_POS_INCREMENT;
            return barPos;
        }

        protected void InitializeHostInfo_Defaults()
        {
            this.host_SampleRate = 44100;
            this.host_SampleRateIsInitialized = true;

            this.host_Tempo = 120;
            this.host_TempoIsInitialized = true;

            this.host_TimeSignatureNumerator = 4;
            this.host_TimeSignatureDenominator = 4;
            this.host_TimeSignatureIsInitialized = true;

            this.host_TransportPlaying = true;
            this.host_TransportPlayingIsInitialized = true;

            this.host_CalculatedBarZeroSampleTime = 0;
            this.host_CalculatedBarZeroIsInitialized = true;

            this.host_BarPosIsInitialized = true;
        }

        protected Mock<IGeneratorMappingEntry> MockFactory_IGeneratorMappingEntry()
        {
            var genEntryMock = new Mock<IGeneratorMappingEntry>();

            GenEntryConfigInfo configInfo = Factory_GenEntryConfigInfo_Default();
            genEntryMock.Setup(genEntry => genEntry.GenConfigInfo).Returns(configInfo);

            GenEntryHistoryInfo historyInfo = Factory_GenEntryHistoryInfo_Uninitilized();
            genEntryMock.Setup(genEntry => genEntry.GenHistoryInfo).Returns(historyInfo);

            var inMsgRangeMock = new Mock<IMssMsgRange>();
            inMsgRangeMock.Setup(range => range.MsgType).Returns(MssMsgType.RelBarPeriodPos);
            inMsgRangeMock.Setup(range => range.Data1RangeBottom).Returns(MssMsgUtil.UNUSED_MSS_MSG_DATA);
            inMsgRangeMock.Setup(range => range.Data1RangeTop).Returns(MssMsgUtil.UNUSED_MSS_MSG_DATA);
            inMsgRangeMock.Setup(range => range.Data2RangeBottom).Returns(MssMsgUtil.UNUSED_MSS_MSG_DATA);
            inMsgRangeMock.Setup(range => range.Data2RangeTop).Returns(MssMsgUtil.UNUSED_MSS_MSG_DATA);

            var outMsgRangeMock = new Mock<IMssMsgRange>();
            outMsgRangeMock.Setup(range => range.MsgType).Returns(MssMsgType.Generator);
            outMsgRangeMock.Setup(range => range.Data1RangeBottom).Returns(0);
            outMsgRangeMock.Setup(range => range.Data1RangeTop).Returns(0);
            outMsgRangeMock.Setup(range => range.Data2RangeBottom).Returns(MssMsgUtil.UNUSED_MSS_MSG_DATA);
            outMsgRangeMock.Setup(range => range.Data2RangeTop).Returns(MssMsgUtil.UNUSED_MSS_MSG_DATA);

            genEntryMock.Setup(genEntry => genEntry.InMssMsgRange).Returns(inMsgRangeMock.Object);
            genEntryMock.Setup(genEntry => genEntry.OutMssMsgRange).Returns(outMsgRangeMock.Object);

            return genEntryMock;
        }

        protected GenEntryConfigInfo Factory_GenEntryConfigInfo_Default()
        {
            //Don't need to bother mocking GenEntryConfigInfo as it does not have any internal
            //logic that needs testing.
            GenEntryConfigInfo configInfo = new GenEntryConfigInfo();
            configInfo.InitWithDefaultValues();
            return configInfo;
        }

        protected GenEntryHistoryInfo Factory_GenEntryHistoryInfo_Uninitilized()
        {
            //Don't need to bother mocking GenEntryHistoryInfo as it does not have any internal
            //logic that needs testing.
            GenEntryHistoryInfo historyInfo = new GenEntryHistoryInfo();
            return historyInfo;
        }

    } //class MssEventGeneratorTest
} //namespace MidiShapeShifterTest.Mss.Generator

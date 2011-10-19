using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using Moq;

using MidiShapeShifter.Mss;
using MidiShapeShifter.Mss.Generator;
using MidiShapeShifter.Mss.Mapping;

namespace MidiShapeShifterTest.Mss.Generator
{
    [TestFixture]
    public class GeneratorMappingManagerTest
    {
        [Test]
        public void Constructor_DefaultConstructor_DoesntContainAnyEntries()
        {
            GeneratorMappingManager genMgr = Factory_GeneratorMappingManager_Default();
            Assert.AreEqual(0, genMgr.GetNumEntries());
        }

        [Test]
        public void AddGenMappingEntry_SingleEntry_SuccessfullyAdded()
        {
            GeneratorMappingManager genMgr = Factory_GeneratorMappingManager_Default();
            Mock<IGeneratorMappingEntry> genEntryMock = MockFactory_IGeneratorMappingEntry();

            genMgr.AddGenMappingEntry(genEntryMock.Object);

            Assert.AreEqual(1, genMgr.GetNumEntries());
            Assert.AreEqual(genEntryMock.Object, genMgr.GetGenMappingEntryByIndex(0));
            Assert_ConfigInfoIdIsInitialized(genEntryMock.Object.GenConfigInfo);
        }

        [Test]
        public void CreateAndAddEntryFromGenInfo_DefaultGenInfo_SuccessfullyAdded()
        {
            GeneratorMappingManager genMgr = Factory_GeneratorMappingManager_Default();
            GenEntryConfigInfo genConfigInfo = Factory_GenEntryConfigInfo_Default();

            genMgr.CreateAndAddEntryFromGenInfo(genConfigInfo);

            Assert.AreEqual(1, genMgr.GetNumEntries());
        }

        [Test]
        public void CreateAndAddEntryFromGenInfo_BeatSyncedGenInfo_CorrectlyInitializedInRangeType()
        {
            GeneratorMappingManager genMgr = Factory_GeneratorMappingManager_Default();
            GenEntryConfigInfo genConfigInfo = Factory_GenEntryConfigInfo_Default();
            genConfigInfo.PeriodType = GenPeriodType.BeatSynced;

            genMgr.CreateAndAddEntryFromGenInfo(genConfigInfo);

            IGeneratorMappingEntry genEntry = genMgr.GetGenMappingEntryByIndex(0);
            Assert.AreEqual(MssMsgType.RelBarPeriodPos, genEntry.InMssMsgRange.MsgType);
        }

        [Test]
        public void CreateAndAddEntryFromGenInfo_TimeBasedGenInfo_CorrectlyInitializedInRangeType()
        {
            GeneratorMappingManager genMgr = Factory_GeneratorMappingManager_Default();
            GenEntryConfigInfo genConfigInfo = Factory_GenEntryConfigInfo_Default();
            genConfigInfo.PeriodType = GenPeriodType.Time;

            genMgr.CreateAndAddEntryFromGenInfo(genConfigInfo);

            IGeneratorMappingEntry genEntry = genMgr.GetGenMappingEntryByIndex(0);
            Assert.AreEqual(MssMsgType.RelTimePeriodPos, genEntry.InMssMsgRange.MsgType);
        }

        [Test]
        public void CreateAndAddEntryFromGenInfo_DefaultGenInfo_CorrectlyInitializedInRangeBounds()
        {
            GeneratorMappingManager genMgr = Factory_GeneratorMappingManager_Default();
            GenEntryConfigInfo genConfigInfo = Factory_GenEntryConfigInfo_Default();

            genMgr.CreateAndAddEntryFromGenInfo(genConfigInfo);

            IGeneratorMappingEntry genEntry = genMgr.GetGenMappingEntryByIndex(0);
            Assert.AreEqual(genConfigInfo.Id, 
                            genEntry.InMssMsgRange.Data1RangeBottom);
            Assert.AreEqual(genConfigInfo.Id, 
                            genEntry.InMssMsgRange.Data1RangeTop);
            Assert.AreEqual(MssMsgUtil.UNUSED_MSS_MSG_DATA,
                            genEntry.InMssMsgRange.Data2RangeBottom);
            Assert.AreEqual(MssMsgUtil.UNUSED_MSS_MSG_DATA,
                            genEntry.InMssMsgRange.Data2RangeTop);
        }

        [Test]
        public void CreateAndAddEntryFromGenInfo_DefaultGenInfo_CorrectlyInitializedOutRange()
        {
            GeneratorMappingManager genMgr = Factory_GeneratorMappingManager_Default();
            GenEntryConfigInfo genConfigInfo = Factory_GenEntryConfigInfo_Default();

            genMgr.CreateAndAddEntryFromGenInfo(genConfigInfo);

            IGeneratorMappingEntry genEntry = genMgr.GetGenMappingEntryByIndex(0);
            Assert.AreEqual(genEntry.GenConfigInfo.Id, genEntry.OutMssMsgRange.Data1RangeBottom);
            Assert.AreEqual(genEntry.GenConfigInfo.Id, genEntry.OutMssMsgRange.Data1RangeTop);
            Assert.AreEqual(MssMsgUtil.UNUSED_MSS_MSG_DATA,
                            genEntry.OutMssMsgRange.Data2RangeBottom);
            Assert.AreEqual(MssMsgUtil.UNUSED_MSS_MSG_DATA,
                            genEntry.OutMssMsgRange.Data2RangeTop);
            Assert.AreEqual(MssMsgType.Generator, genEntry.OutMssMsgRange.MsgType);
        }

        [Test]
        public void CreateAndAddEntryFromGenInfo_DefaultGenInfo_CorrectlyInitializedInfoClassses()
        {
            GeneratorMappingManager genMgr = Factory_GeneratorMappingManager_Default();
            GenEntryConfigInfo genConfigInfo = Factory_GenEntryConfigInfo_Default();

            genMgr.CreateAndAddEntryFromGenInfo(genConfigInfo);

            IGeneratorMappingEntry genEntry = genMgr.GetGenMappingEntryByIndex(0);
            //GenHistoryInfo should not be initialized until the first MSS Event is generated
            Assert.IsFalse(genEntry.GenHistoryInfo.Initialized);
            Assert_ConfigInfoIdIsInitialized(genConfigInfo);
            Assert.IsNotNull(genEntry.CurveShapeInfo);
        }

        [Test]
        public void RemoveGenMappingEntry_RemoveSecondEntry_SuccessfullyRemoved()
        {
            GeneratorMappingManager genMgr = Factory_GeneratorMappingManager_Default();
            Mock<IGeneratorMappingEntry> genEntryMock1 = MockFactory_IGeneratorMappingEntry();
            Mock<IGeneratorMappingEntry> genEntryMock2 = MockFactory_IGeneratorMappingEntry();
            Mock<IGeneratorMappingEntry> genEntryMock3 = MockFactory_IGeneratorMappingEntry();

            genMgr.AddGenMappingEntry(genEntryMock1.Object);
            genMgr.AddGenMappingEntry(genEntryMock2.Object);
            genMgr.AddGenMappingEntry(genEntryMock3.Object);

            genMgr.RemoveGenMappingEntry(1);

            Assert.AreEqual(2, genMgr.GetNumEntries());
            Assert.AreEqual(genEntryMock1.Object, genMgr.GetGenMappingEntryByIndex(0));
            Assert.AreEqual(genEntryMock3.Object, genMgr.GetGenMappingEntryByIndex(1));
        }

        [Test]
        public void GetGenMappingEntryById_RetrieveSecondEntry_SuccessfullyRetrieved()
        {
            GeneratorMappingManager genMgr = Factory_GeneratorMappingManager_Default();
            Mock<IGeneratorMappingEntry> genEntryMock1 = MockFactory_IGeneratorMappingEntry();
            Mock<IGeneratorMappingEntry> genEntryMock2 = MockFactory_IGeneratorMappingEntry();
            Mock<IGeneratorMappingEntry> genEntryMock3 = MockFactory_IGeneratorMappingEntry();
            genMgr.AddGenMappingEntry(genEntryMock1.Object);
            genMgr.AddGenMappingEntry(genEntryMock2.Object);
            genMgr.AddGenMappingEntry(genEntryMock3.Object);

            IGeneratorMappingEntry retrievedEntry =
                    genMgr.GetGenMappingEntryById(genEntryMock2.Object.GenConfigInfo.Id);

            Assert.AreEqual(genEntryMock2.Object, retrievedEntry);
        }

        [Test]
        public void GetAssociatedEntries_EmptyManager_NoEntriesRetrieved()
        {
            GeneratorMappingManager genMgr = Factory_GeneratorMappingManager_Default();
            MssMsg inputMsg = Factory_MssMsg_CustomTypeAndData1(MssMsgType.RelBarPeriodPos, 0);

            IEnumerable<IMappingEntry> retrievedEntries = genMgr.GetAssociatedEntries(inputMsg);

            Assert.AreEqual(0, retrievedEntries.Count());
        }

        [Test]
        public void GetAssociatedEntries_NonGeneratorMessage_NoEntriesRetrieved()
        {
            GeneratorMappingManager genMgr = Factory_GeneratorMappingManager_Default();
            MssMsg inputMsg = Factory_MssMsg_CustomTypeAndData1(MssMsgType.CC, 0);

            IEnumerable<IMappingEntry> retrievedEntries = genMgr.GetAssociatedEntries(inputMsg);

            Assert.AreEqual(0, retrievedEntries.Count());
        }

        [Test]
        public void GetAssociatedEntries_MatchesAnEntry_CorrectEntryRetrieved()
        {
            GeneratorMappingManager genMgr = Factory_GeneratorMappingManager_Default();
            Mock<IGeneratorMappingEntry> genEntryMock1 = MockFactory_IGeneratorMappingEntry();
            Mock<IGeneratorMappingEntry> genEntryMock2 = MockFactory_IGeneratorMappingEntry();
            Mock<IGeneratorMappingEntry> genEntryMock3 = MockFactory_IGeneratorMappingEntry();
            genMgr.AddGenMappingEntry(genEntryMock1.Object);
            genMgr.AddGenMappingEntry(genEntryMock2.Object);
            genMgr.AddGenMappingEntry(genEntryMock3.Object);
            MssMsg inputMsg = Factory_MssMsg_CustomTypeAndData1(
                    MssMsgType.RelTimePeriodPos, genEntryMock2.Object.GenConfigInfo.Id);

            IEnumerable<IMappingEntry> retrievedEntries = genMgr.GetAssociatedEntries(inputMsg);

            Assert.AreEqual(1, retrievedEntries.Count());
            Assert.AreEqual(genEntryMock2.Object, retrievedEntries.First());
        }

        protected GeneratorMappingManager Factory_GeneratorMappingManager_Default()
        {
            return new GeneratorMappingManager();
        }

        protected GenEntryConfigInfo Factory_GenEntryConfigInfo_Default()
        {
            //Don't need to bother mocking GenEntryConfigInfo as it does not have any internal
            //logic that needs testing.
            GenEntryConfigInfo configInfo = new GenEntryConfigInfo();
            configInfo.InitWithDefaultValues();
            return configInfo;
        }

        protected MssMsg Factory_MssMsg_CustomTypeAndData1(MssMsgType msgType, double data1)
        {
            return new MssMsg(msgType, data1, MssMsgUtil.UNUSED_MSS_MSG_DATA, 0);
        }

        protected Mock<IGeneratorMappingEntry> MockFactory_IGeneratorMappingEntry()
        {
            var genEntryMock = new Mock<IGeneratorMappingEntry>();

            GenEntryConfigInfo configInfo = Factory_GenEntryConfigInfo_Default();
            genEntryMock.Setup(genEntry => genEntry.GenConfigInfo).Returns(configInfo);
            return genEntryMock;
        }

        protected void Assert_ConfigInfoIdIsInitialized(GenEntryConfigInfo configInfo)
        {
            Assert.AreNotEqual(GenEntryConfigInfo.UNINITIALIZED_ID, configInfo.Id);
        }
    }
}

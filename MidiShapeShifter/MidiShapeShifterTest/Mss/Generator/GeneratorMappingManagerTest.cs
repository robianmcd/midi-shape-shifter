using MidiShapeShifter.CSharpUtil;
using MidiShapeShifter.Mss;
using MidiShapeShifter.Mss.Generator;
using MidiShapeShifter.Mss.Mapping;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

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

            int newId = genMgr.AddMappingEntry(genEntryMock.Object);

            Assert.AreEqual(1, genMgr.GetNumEntries());
            Assert.AreEqual(genEntryMock.Object.Id, genMgr.GetCopyOfMappingEntryById(newId).Value.Id);
            Assert_GenEntryIsInitialized(genEntryMock.Object);
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
        public void CreateAndAddEntryFromGenInfo_BarsGenInfo_CorrectlyInitializedInRangeType()
        {
            GeneratorMappingManager genMgr = Factory_GeneratorMappingManager_Default();
            GenEntryConfigInfo genConfigInfo = Factory_GenEntryConfigInfo_Default();
            genConfigInfo.PeriodType = GenPeriodType.Bars;

            int newId = genMgr.CreateAndAddEntryFromGenInfo(genConfigInfo);

            IReturnStatus<IGeneratorMappingEntry> getCopyRetStatus = genMgr.GetCopyOfMappingEntryById(newId);
            Assert.IsTrue(getCopyRetStatus.IsValid);
            Assert.AreEqual(MssMsgType.RelBarPeriodPos, getCopyRetStatus.Value.InMssMsgRange.MsgType);
        }

        [Test]
        public void CreateAndAddEntryFromGenInfo_TimeBasedGenInfo_CorrectlyInitializedInRangeType()
        {
            GeneratorMappingManager genMgr = Factory_GeneratorMappingManager_Default();
            GenEntryConfigInfo genConfigInfo = Factory_GenEntryConfigInfo_Default();
            genConfigInfo.PeriodType = GenPeriodType.Time;

            int newId = genMgr.CreateAndAddEntryFromGenInfo(genConfigInfo);

            IReturnStatus<IGeneratorMappingEntry> getCopyRetStatus = genMgr.GetCopyOfMappingEntryById(newId);
            Assert.IsTrue(getCopyRetStatus.IsValid);
            Assert.AreEqual(MssMsgType.RelTimePeriodPos, getCopyRetStatus.Value.InMssMsgRange.MsgType);
        }

        [Test]
        public void CreateAndAddEntryFromGenInfo_DefaultGenInfo_CorrectlyInitializedInRangeBounds()
        {
            GeneratorMappingManager genMgr = Factory_GeneratorMappingManager_Default();
            GenEntryConfigInfo genConfigInfo = Factory_GenEntryConfigInfo_Default();

            int newId = genMgr.CreateAndAddEntryFromGenInfo(genConfigInfo);

            IReturnStatus<IGeneratorMappingEntry> getCopyRetStatus = genMgr.GetCopyOfMappingEntryById(newId);
            Assert.IsTrue(getCopyRetStatus.IsValid);
            IGeneratorMappingEntry genEntry = getCopyRetStatus.Value;
            Assert.AreEqual(genEntry.Id,
                            genEntry.InMssMsgRange.Data1RangeBottom);
            Assert.AreEqual(genEntry.Id,
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

            int newId = genMgr.CreateAndAddEntryFromGenInfo(genConfigInfo);

            IReturnStatus<IGeneratorMappingEntry> getCopyRetStatus = genMgr.GetCopyOfMappingEntryById(newId);
            Assert.IsTrue(getCopyRetStatus.IsValid);
            IGeneratorMappingEntry genEntry = getCopyRetStatus.Value;

            Assert.AreEqual(genEntry.Id, genEntry.OutMssMsgRange.Data1RangeBottom);
            Assert.AreEqual(genEntry.Id, genEntry.OutMssMsgRange.Data1RangeTop);
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

            int newId = genMgr.CreateAndAddEntryFromGenInfo(genConfigInfo);

            IGeneratorMappingEntry genEntry = genMgr.GetCopyOfMappingEntryById(newId).Value;
            //GenHistoryInfo should not be initialized until the first MSS Event is generated
            Assert.IsFalse(genEntry.GenHistoryInfo.Initialized);
            Assert_GenEntryIsInitialized(genEntry);
            Assert.IsNotNull(genEntry.CurveShapeInfo);
        }

        [Test]
        public void RemoveGenMappingEntry_RemoveSecondEntry_SuccessfullyRemoved()
        {
            GeneratorMappingManager genMgr = Factory_GeneratorMappingManager_Default();
            Mock<IGeneratorMappingEntry> genEntryMock1 = MockFactory_IGeneratorMappingEntry();
            Mock<IGeneratorMappingEntry> genEntryMock2 = MockFactory_IGeneratorMappingEntry();
            Mock<IGeneratorMappingEntry> genEntryMock3 = MockFactory_IGeneratorMappingEntry();

            int id1 = genMgr.AddMappingEntry(genEntryMock1.Object);
            int id2 = genMgr.AddMappingEntry(genEntryMock2.Object);
            int id3 = genMgr.AddMappingEntry(genEntryMock3.Object);

            genMgr.RemoveMappingEntry(id2);

            Assert.AreEqual(2, genMgr.GetNumEntries());
            Assert.AreEqual(genEntryMock1.Object.Id, genMgr.GetCopyOfMappingEntryById(id1).Value.Id);
            Assert.AreEqual(genEntryMock3.Object.Id, genMgr.GetCopyOfMappingEntryById(id3).Value.Id);
        }

        [Test]
        public void GetGenMappingEntryById_RetrieveSecondEntry_SuccessfullyRetrieved()
        {
            GeneratorMappingManager genMgr = Factory_GeneratorMappingManager_Default();
            Mock<IGeneratorMappingEntry> genEntryMock1 = MockFactory_IGeneratorMappingEntry();
            Mock<IGeneratorMappingEntry> genEntryMock2 = MockFactory_IGeneratorMappingEntry();
            Mock<IGeneratorMappingEntry> genEntryMock3 = MockFactory_IGeneratorMappingEntry();
            int id1 = genMgr.AddMappingEntry(genEntryMock1.Object);
            int id2 = genMgr.AddMappingEntry(genEntryMock2.Object);
            int id3 = genMgr.AddMappingEntry(genEntryMock3.Object);

            IGeneratorMappingEntry retrievedEntry =
                    genMgr.GetCopyOfMappingEntryById(genEntryMock2.Object.Id).Value;

            Assert.AreEqual(genEntryMock2.Object.Id, retrievedEntry.Id);
        }

        [Test]
        public void GetAssociatedEntries_EmptyManager_NoEntriesRetrieved()
        {
            GeneratorMappingManager genMgr = Factory_GeneratorMappingManager_Default();
            MssMsg inputMsg = Factory_MssMsg_CustomTypeAndData1(MssMsgType.RelBarPeriodPos, 0);

            IEnumerable<IMappingEntry> retrievedEntries = genMgr.GetCopiesOfMappingEntriesForMsg(inputMsg);

            Assert.AreEqual(0, retrievedEntries.Count());
        }

        [Test]
        public void GetAssociatedEntries_NonGeneratorMessage_NoEntriesRetrieved()
        {
            GeneratorMappingManager genMgr = Factory_GeneratorMappingManager_Default();
            MssMsg inputMsg = Factory_MssMsg_CustomTypeAndData1(MssMsgType.CC, 0);

            IEnumerable<IMappingEntry> retrievedEntries = genMgr.GetCopiesOfMappingEntriesForMsg(inputMsg);

            Assert.AreEqual(0, retrievedEntries.Count());
        }

        [Test]
        public void GetAssociatedEntries_MatchesAnEntry_CorrectEntryRetrieved()
        {
            GeneratorMappingManager genMgr = Factory_GeneratorMappingManager_Default();
            Mock<IGeneratorMappingEntry> genEntryMock1 = MockFactory_IGeneratorMappingEntry();
            Mock<IGeneratorMappingEntry> genEntryMock2 = MockFactory_IGeneratorMappingEntry();
            Mock<IGeneratorMappingEntry> genEntryMock3 = MockFactory_IGeneratorMappingEntry();
            genMgr.AddMappingEntry(genEntryMock1.Object);
            genMgr.AddMappingEntry(genEntryMock2.Object);
            genMgr.AddMappingEntry(genEntryMock3.Object);
            MssMsg inputMsg = Factory_MssMsg_CustomTypeAndData1(
                    MssMsgType.RelTimePeriodPos, genEntryMock2.Object.Id);

            IEnumerable<IMappingEntry> retrievedEntries = genMgr.GetCopiesOfMappingEntriesForMsg(inputMsg);

            Assert.AreEqual(1, retrievedEntries.Count());
            Assert.AreEqual(genEntryMock2.Object.Id, retrievedEntries.First().Id);
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
            genEntryMock.Setup(genEntry => genEntry.Clone()).Returns(genEntryMock.Object);
            return genEntryMock;
        }

        protected void Assert_GenEntryIsInitialized(IGeneratorMappingEntry genEntry)
        {
            Assert.AreNotEqual(GeneratorMappingEntry.UNINITIALIZED_ID, genEntry.Id);
        }
    }
}

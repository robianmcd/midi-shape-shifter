using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using NUnit.Framework;
using Moq;

using MidiShapeShifter.Mss;
using MidiShapeShifter.Mss.Mapping;
using MidiShapeShifter.Mss.MssMsgInfoTypes;


namespace MidiShapeShifterTest.Mss.Mapping
{
    [TestFixture]
    public class MappingManagerTest
    {
        
        protected const bool DEFAULT_OVERRIDE_DUPLICATES = false;
        protected const MssMsgType DEFAULT_MSG_TYPE = MssMsgType.NoteOn;
        //useful for when you need to test two message types that do not match. Other types do not need to be tested here
        //because MappingManager should not have any logic specific to a particular type.
        protected const MssMsgType SECONDARY_MSG_TYPE = MssMsgType.CC;

        [Test]
        public void AddMappingEntry_SingleEntry_SuccessfullyAdded()
        {
            MappingManager mappingMgr = Factory_MappingManager_Default();
            IMappingEntry mappingEntry = Factory_IMappingEntry_Basic();

            int newId = mappingMgr.AddMappingEntry(mappingEntry);

            Assert.AreEqual(mappingEntry.Id, mappingMgr.GetCopyOfMappingEntryById(newId).Value.Id);
        }

        [Test]
        public void AddMappingEntry_SingleEntry_AddsExactlyOneEntry()
        {
            MappingManager mappingMgr = Factory_MappingManager_Default();
            IMappingEntry mappingEntry = Factory_IMappingEntry_Basic();

            mappingMgr.AddMappingEntry(mappingEntry);

            Assert.AreEqual(1, mappingMgr.GetNumEntries());
        }

        [Test]
        public void RemoveMappingEntry_ManagerHasOneEntry_ExistingEntryIsRemoved()
        {
            MappingManager mappingMgr = Factory_MappingManager_Default();
            IMappingEntry mappingEntry = Factory_IMappingEntry_Basic();

            mappingMgr.AddMappingEntry(mappingEntry);
            mappingMgr.RemoveMappingEntry(0);

            Assert.AreEqual(0, mappingMgr.GetNumEntries());
        }

        [Test]
        public void MoveEntryUp_MoveBottomEntryUpInManagerThatHasTwoEntries_EntriesAreSwapped()
        {
            MappingManager mappingMgr = Factory_MappingManager_Default();
            IMappingEntry mappingEntry1 = Factory_IMappingEntry_MapsIdenticalMidiMsgRanges(DEFAULT_MSG_TYPE, 1, 1, 0, 0);
            IMappingEntry mappingEntry2 = Factory_IMappingEntry_MapsIdenticalMidiMsgRanges(DEFAULT_MSG_TYPE, 2, 2, 3, 3);

            mappingMgr.AddMappingEntry(mappingEntry1);
            mappingMgr.AddMappingEntry(mappingEntry2);

            mappingMgr.MoveEntryUp(1);

            List<IMappingEntry> mappingEntryList = mappingMgr.GetCopyOfMappingEntryList();

            Assert.AreEqual(mappingEntry2.Id, mappingEntryList[0].Id);
            Assert.AreEqual(mappingEntry1.Id, mappingEntryList[1].Id);
        }

        [Test]
        public void MoveEntryUp_MoveThirdEntryUpInManagerThatHasFourEntries_OnlyMiddleTwoEntriesAreSwapped()
        {
            MappingManager mappingMgr = Factory_MappingManager_Default();
            IMappingEntry mappingEntry1 = Factory_IMappingEntry_MapsIdenticalMidiMsgRanges(DEFAULT_MSG_TYPE, 1, 1, 0, 0);
            IMappingEntry mappingEntry2 = Factory_IMappingEntry_MapsIdenticalMidiMsgRanges(DEFAULT_MSG_TYPE, 2, 2, 0, 0);
            IMappingEntry mappingEntry3 = Factory_IMappingEntry_MapsIdenticalMidiMsgRanges(DEFAULT_MSG_TYPE, 3, 3, 0, 0);
            IMappingEntry mappingEntry4 = Factory_IMappingEntry_MapsIdenticalMidiMsgRanges(DEFAULT_MSG_TYPE, 4, 4, 0, 0);

            mappingMgr.AddMappingEntry(mappingEntry1);
            mappingMgr.AddMappingEntry(mappingEntry2);
            mappingMgr.AddMappingEntry(mappingEntry3);
            mappingMgr.AddMappingEntry(mappingEntry4);

            mappingMgr.MoveEntryUp(2);

            List<IMappingEntry> mappingEntryList = mappingMgr.GetCopyOfMappingEntryList();

            Assert.AreEqual(mappingEntry1.Id, mappingEntryList[0].Id);
            Assert.AreEqual(mappingEntry3.Id, mappingEntryList[1].Id);
            Assert.AreEqual(mappingEntry2.Id, mappingEntryList[2].Id);
            Assert.AreEqual(mappingEntry4.Id, mappingEntryList[3].Id);
        }

        [Test]
        public void MoveEntryDown_MoveTopEntryDownInManagerThatHasTwoEntries_EntriesAreSwapped()
        {
            MappingManager mappingMgr = Factory_MappingManager_Default();
            IMappingEntry mappingEntry1 = Factory_IMappingEntry_MapsIdenticalMidiMsgRanges(DEFAULT_MSG_TYPE, 1, 1, 0, 0);
            IMappingEntry mappingEntry2 = Factory_IMappingEntry_MapsIdenticalMidiMsgRanges(DEFAULT_MSG_TYPE, 2, 2, 3, 3);

            mappingMgr.AddMappingEntry(mappingEntry1);
            mappingMgr.AddMappingEntry(mappingEntry2);

            mappingMgr.MoveEntryDown(0);

            List<IMappingEntry> mappingEntryList = mappingMgr.GetCopyOfMappingEntryList();

            Assert.AreEqual(mappingEntry2.Id, mappingEntryList[0].Id);
            Assert.AreEqual(mappingEntry1.Id, mappingEntryList[1].Id);
        }

        [Test]
        public void MoveEntryDown_MoveSecondEntryDownInManagerThatHasFourEntries_OnlyMiddleTwoEntriesAreSwapped()
        {
            MappingManager mappingMgr = Factory_MappingManager_Default();
            IMappingEntry mappingEntry1 = Factory_IMappingEntry_MapsIdenticalMidiMsgRanges(DEFAULT_MSG_TYPE, 1, 1, 0, 0);
            IMappingEntry mappingEntry2 = Factory_IMappingEntry_MapsIdenticalMidiMsgRanges(DEFAULT_MSG_TYPE, 2, 2, 0, 0);
            IMappingEntry mappingEntry3 = Factory_IMappingEntry_MapsIdenticalMidiMsgRanges(DEFAULT_MSG_TYPE, 3, 3, 0, 0);
            IMappingEntry mappingEntry4 = Factory_IMappingEntry_MapsIdenticalMidiMsgRanges(DEFAULT_MSG_TYPE, 4, 4, 0, 0);

            mappingMgr.AddMappingEntry(mappingEntry1);
            mappingMgr.AddMappingEntry(mappingEntry2);
            mappingMgr.AddMappingEntry(mappingEntry3);
            mappingMgr.AddMappingEntry(mappingEntry4);

            mappingMgr.MoveEntryDown(1);

            List<IMappingEntry> mappingEntryList = mappingMgr.GetCopyOfMappingEntryList();

            Assert.AreEqual(mappingEntry1.Id,mappingEntryList[0].Id);
            Assert.AreEqual(mappingEntry3.Id,mappingEntryList[1].Id);
            Assert.AreEqual(mappingEntry2.Id,mappingEntryList[2].Id);
            Assert.AreEqual(mappingEntry4.Id,mappingEntryList[3].Id);
        }

        [Test]
        public void GetAssociatedEntries_MsgMatchesOneEntry_TheAssociatedEntryIsReturned()
        {
            MappingManager mappingMgr = Factory_MappingManager_Default();
            IMappingEntry mappingEntry = Factory_IMappingEntry_MapsIdenticalMidiMsgRanges(DEFAULT_MSG_TYPE, 1, 3, 0, 10);
            int newId = mappingMgr.AddMappingEntry(mappingEntry);

            MssMsg msg = Factory_MssMsg_InitializedValues(
                DEFAULT_MSG_TYPE, /*same as type in manager*/
                2, /*between 1 and 3*/
                5, /*between 0 and 10*/
                100 /*doesn't need to match anything*/);

            var matchingEntries = mappingMgr.GetCopiesOfMappingEntriesForMsg(msg);
            Assert.IsTrue(matchingEntries.Any(entry => entry.Id == newId));
        }

        [Test]
        public void GetAssociatedEntries_MsgOnlyMatchesPartsOfEntriesInMgr_TheEnumerationReturnedIsEmpty()
        {
            MappingManager mappingMgr = Factory_MappingManager_Default();
            
            IMappingEntry mappingEntry1 = Factory_IMappingEntry_MapsIdenticalMidiMsgRanges(DEFAULT_MSG_TYPE, 1, 1, 3, 127);
            IMappingEntry mappingEntry2 = Factory_IMappingEntry_MapsIdenticalMidiMsgRanges(DEFAULT_MSG_TYPE, 2, 16, 2, 2);
            IMappingEntry mappingEntry3 = Factory_IMappingEntry_MapsIdenticalMidiMsgRanges(SECONDARY_MSG_TYPE, 1, 1, 2, 2);

            mappingMgr.AddMappingEntry(mappingEntry1);
            mappingMgr.AddMappingEntry(mappingEntry2);
            mappingMgr.AddMappingEntry(mappingEntry3);

            MssMsg msg = Factory_MssMsg_InitializedValues(
                DEFAULT_MSG_TYPE, /*matches entries 1 and 2*/
                1, /*matches entries 1 and 3*/
                2, /*matches entries 2 and 3*/
                100 /*doesn't need to match anything*/);

            var matchingEntries = mappingMgr.GetCopiesOfMappingEntriesForMsg(msg);

            Assert.IsEmpty(matchingEntries.ToList());
        }

        [Test]
        public void GetAssociatedEntries_MsgMatchesFirstTwoEntriesAndOverrideDuplicatesIsTrueForTheThirdEntry_FirstTwoEntriesAreReturned()
        {
            MappingManager mappingMgr = Factory_MappingManager_Default();

            IMappingEntry mappingEntry1 = Factory_IMappingEntry_MapsIdenticalMidiMsgRanges(DEFAULT_MSG_TYPE, 1, 16, 0, 127);
            IMappingEntry mappingEntry2 = Factory_IMappingEntry_MapsIdenticalMidiMsgRanges(DEFAULT_MSG_TYPE, 1, 8, 0, 64);
            IMappingEntry mappingEntry3 = Factory_IMappingEntry_MapsIdenticalMidiMsgRanges(DEFAULT_MSG_TYPE, 7, 16, 60, 127);
            mappingEntry3.OverrideDuplicates = true;

            int id1 = mappingMgr.AddMappingEntry(mappingEntry1);
            int id2 = mappingMgr.AddMappingEntry(mappingEntry2);
            int id3 = mappingMgr.AddMappingEntry(mappingEntry3);

            MssMsg msg = Factory_MssMsg_InitializedValues(
                DEFAULT_MSG_TYPE, /*matches all*/
                7, /*matches all*/
                10, /*only matches entries 1 and 2*/
                100 /*doesn't need to match anything*/);

            var matchingEntries = mappingMgr.GetCopiesOfMappingEntriesForMsg(msg);

            Assert.IsTrue(matchingEntries.Any(entry => entry.Id == id1));
            Assert.IsTrue(matchingEntries.Any(entry => entry.Id == id2));
            Assert.IsFalse(matchingEntries.Any(entry => entry.Id == id3));
        }

        [Test]
        public void GetAssociatedEntries_MultipleMatches_AllMatchingEntriesAreReturned()
        {
            TestCase_GetAssociatedEntries_ManagerWithThreeOverlappingEntriesThatMatchAMsg(
                false, false, false,
                true, true, true);
        }

        [Test]
        public void GetAssociatedEntries_MultipleMatchesButOverrideDuplicatesIsTrueForOne_TheOverrideDuplicatesEntryIsTheOnlyOneReturned()
        {
            TestCase_GetAssociatedEntries_ManagerWithThreeOverlappingEntriesThatMatchAMsg(
                false, true, false,
                false, true, false);
        }

        [Test]
        public void GetAssociatedEntries_MultipleMatchesButOverrideDuplicatesIsTrueForSome_TheFirstOverrideDuplicatesEntryIsTheOnlyOneReturned()
        {
            TestCase_GetAssociatedEntries_ManagerWithThreeOverlappingEntriesThatMatchAMsg(
                true, false, true,
                true, false, false);
        }

        //*******************************Helpers*******************************

        protected MappingManager Factory_MappingManager_Default()
        {
            return new MappingManager();
        }

        protected IMappingEntry Factory_IMappingEntry_Basic()
        {
            return Factory_IMappingEntry_MapsIdenticalMidiMsgRanges(DEFAULT_MSG_TYPE, 1, 1, 0, 0);
        }

        protected IMappingEntry Factory_IMappingEntry_MapsIdenticalMidiMsgRanges(
            MssMsgType msgType, 
            int chanRangeBottom, int chanRamgeTop, 
            int paramRangeBottom, int paramRangeTop)
        {
            MssMsgRange inMsgRange = new MssMsgRange();
            inMsgRange.InitPublicMembers(msgType, chanRangeBottom, chanRamgeTop, paramRangeBottom, paramRangeTop);

            MssMsgRange outMsgRange = new MssMsgRange();
            outMsgRange.InitPublicMembers(msgType, chanRangeBottom, chanRamgeTop, paramRangeBottom, paramRangeTop);

            IMappingEntry mapEntry = new MappingEntry();
            mapEntry.InMssMsgRange = inMsgRange;
            mapEntry.OutMssMsgRange = outMsgRange;
            mapEntry.OverrideDuplicates = DEFAULT_OVERRIDE_DUPLICATES;

            CurveShapeInfo curveInfo = new CurveShapeInfo();
            curveInfo.InitWithDefaultValues();
            mapEntry.CurveShapeInfo = curveInfo;

            return mapEntry;
        }

        protected MssMsg Factory_MssMsg_InitializedValues(MssMsgType msgType, int data1, int data2, int data3)
        {
            return new MssMsg(msgType, data1, data2, data3);
        }

        protected void TestCase_GetAssociatedEntries_ManagerWithThreeOverlappingEntriesThatMatchAMsg(
            bool Entry1Override, bool Entry2Override, bool Entry3Override, 
            bool Entry1Matches, bool Entry2Matches, bool Entry3Matches)
        {
            MappingManager mappingMgr = Factory_MappingManager_Default();

            IMappingEntry mappingEntry1 = Factory_IMappingEntry_MapsIdenticalMidiMsgRanges(DEFAULT_MSG_TYPE, 1, 16, 0, 127);
            mappingEntry1.OverrideDuplicates = Entry1Override;
            IMappingEntry mappingEntry2 = Factory_IMappingEntry_MapsIdenticalMidiMsgRanges(DEFAULT_MSG_TYPE, 4, 5, 10, 20);
            mappingEntry2.OverrideDuplicates = Entry2Override;
            IMappingEntry mappingEntry3 = Factory_IMappingEntry_MapsIdenticalMidiMsgRanges(DEFAULT_MSG_TYPE, 5, 5, 15, 15);
            mappingEntry3.OverrideDuplicates = Entry3Override;

            int id1 = mappingMgr.AddMappingEntry(mappingEntry1);
            int id2 = mappingMgr.AddMappingEntry(mappingEntry2);
            int id3 = mappingMgr.AddMappingEntry(mappingEntry3);

            MssMsg msg = Factory_MssMsg_InitializedValues(
                DEFAULT_MSG_TYPE, /*matches all entries*/
                5, /*matches all*/
                15, /*matches all*/
                100 /*doesn't need to match anything*/);

            var matchingEntries = mappingMgr.GetCopiesOfMappingEntriesForMsg(msg);

            Assert.IsTrue(matchingEntries.Any(entry => entry.Id == id1) == Entry1Matches);
            Assert.IsTrue(matchingEntries.Any(entry => entry.Id == id2) == Entry2Matches);
            Assert.IsTrue(matchingEntries.Any(entry => entry.Id == id3) == Entry3Matches);
        }
    }
}

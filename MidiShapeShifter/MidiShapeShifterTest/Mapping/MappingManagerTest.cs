using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MidiShapeShifter;
using MidiShapeShifter.Mapping;

namespace MidiShapeShifterTest.Mapping
{
    [TestFixture]
    public class MappingManagerTest
    {
        [Test]
        public void AddMappingEntry_SingleEntry()
        {
            /*MappingManager mappingMgr = new MappingManager();
            MappingEntry mapEntry = new MappingEntry(1, 2, 1, 2, MssMsgUtil.MssMsgType.NoteOn,
                                                     1, 2, 1, 2, MssMsgUtil.MssMsgType.NoteOn,
                                                     0, false);

            Assert.IsTrue(mappingMgr.AddMappingEntry(mapEntry));
            Assert.AreEqual(1, mappingMgr.MappingEntries.Count);
            Assert.AreEqual(mapEntry, mappingMgr.MappingEntries[0]);*/
        }

        [Test]
        public void AddMappingEntry_MultipleEntries()
        {
            /*MappingManager mappingMgr = new MappingManager();
            MappingEntry mapEntry1 = new MappingEntry(0, 127, 1, 16, MssMsgUtil.MssMsgType.NoteOn,
                                                     0, 127, 1, 16, MssMsgUtil.MssMsgType.NoteOn,
                                                     0, false);
            MappingEntry mapEntry2 = new MappingEntry(1, 1, 1, 16, MssMsgUtil.MssMsgType.NoteOn,
                                                     1, 1, 1, 16, MssMsgUtil.MssMsgType.NoteOn,
                                                     1, true);
            MappingEntry mapEntry3 = new MappingEntry(64, 64, 1, 1, MssMsgUtil.MssMsgType.CC,
                                                     65, 65, 1, 1, MssMsgUtil.MssMsgType.CC,
                                                     2, false);

            Assert.IsTrue(mappingMgr.AddMappingEntry(mapEntry1));
            Assert.IsTrue(mappingMgr.AddMappingEntry(mapEntry2));
            Assert.IsTrue(mappingMgr.AddMappingEntry(mapEntry3));
            Assert.AreEqual(3, mappingMgr.MappingEntries.Count);
            Assert.AreEqual(mapEntry1, mappingMgr.MappingEntries[0]);
            Assert.AreEqual(mapEntry2, mappingMgr.MappingEntries[1]);
            Assert.AreEqual(mapEntry3, mappingMgr.MappingEntries[2]);*/
        }
    }
}

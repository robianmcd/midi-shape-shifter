using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MidiShapeShifter;

namespace MidiShapeShifterTest
{
    [TestFixture]
    public class MappingManagerTest
    {
        [Test]
        public void AddTest()
        {
            MappingManager mappingMgr = new MappingManager();
            MappingEntry mapEntry = new MappingEntry();

            mapEntry.inMsgRange = new MidiHelper.MidiMsgRange();
            mapEntry.inMsgRange.bottomParam = 1;
            mapEntry.inMsgRange.topParam = 2;
            mapEntry.inMsgRange.bottomChannel = 1;
            mapEntry.inMsgRange.topChannel = 2;
            mapEntry.inMsgRange.msgType = MidiHelper.MidiMsgType.NoteOn;

            mapEntry.outMsgRange = new MidiHelper.MidiMsgRange();
            mapEntry.outMsgRange.bottomParam = 1;
            mapEntry.outMsgRange.topParam = 2;
            mapEntry.outMsgRange.bottomChannel = 1;
            mapEntry.outMsgRange.topChannel = 2;
            mapEntry.outMsgRange.msgType = MidiHelper.MidiMsgType.NoteOn;

            mapEntry.priority = 0;
            mapEntry.overrideDuplicates = false;

            Assert.IsTrue(mappingMgr.AddMappingEntry(mapEntry));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ninject;
using MidiShapeShifter.Ioc;

using NUnit.Framework;
using Moq;

using MidiShapeShifter.CSharpUtil;
using MidiShapeShifter.Mss;
using MidiShapeShifter.Mss.Mapping;
using MidiShapeShifter.Mss.MssMsgInfoTypes;
using MidiShapeShifter.Mss.Generator;
using MidiShapeShifter.Mss.Evaluation;
using MidiShapeShifter.Mss.Parameters;

namespace MidiShapeShifterTest.Mss
{
    [TestFixture]
    class MssMsgProcessorTest
    {
        protected static readonly CurveShapeInfo DEFAULT_CURVE_SHAPE_INFO;

        static MssMsgProcessorTest()
        {
            DEFAULT_CURVE_SHAPE_INFO = new CurveShapeInfo();
            DEFAULT_CURVE_SHAPE_INFO.InitWithDefaultValues();
        }

        [Test]
        public void ProcessMssMsg_NoMappings_MsgIsUnaffected()
        {
            MssMsg inputMsg = Factory_MssMsg(MssMsgType.NoteOn, 1, 64, 100);
            
            List<IMappingEntry> matchingEntries = new List<IMappingEntry>();

            List<MssMsg> desiredReturnedMsgList = new List<MssMsg>();
            desiredReturnedMsgList.Add((MssMsg)inputMsg.Clone());

            Test_ProcessMssMsg(inputMsg, matchingEntries, desiredReturnedMsgList, false);
        }

        [Test]
        public void ProcessMssMsg_MapsToSameMsg_MsgIsUnaffected()
        {
            MssMsg inputMsg = Factory_MssMsg(MssMsgType.NoteOff, 1, 64, 0);

            List<IMappingEntry> matchingEntries = new List<IMappingEntry>();
            IMappingEntry mapsNoteOffToSameMsg = Factory_IMappingEntry(
                MssMsgType.NoteOff, 1, 1, 64, 64,
                MssMsgType.NoteOff, 1, 1, 64, 64);
            matchingEntries.Add(mapsNoteOffToSameMsg);

            List<MssMsg> desiredReturnedMsgList = new List<MssMsg>();
            desiredReturnedMsgList.Add((MssMsg)inputMsg.Clone());

            Test_ProcessMssMsg(inputMsg, matchingEntries, desiredReturnedMsgList, false);
        }

        [Test]
        public void ProcessMssMsg_MapsTypeData1AndData2_MsgIsProperlyMapped()
        {
            MssMsg inputMsg = Factory_MssMsg(MssMsgType.NoteOff, 1, 64, 0);

            List<IMappingEntry> matchingEntries = new List<IMappingEntry>();
            IMappingEntry mappingEntry = Factory_IMappingEntry(
                MssMsgType.NoteOff, 1, 1, 64, 64,
                MssMsgType.CC, 2, 2, 100, 100);
            matchingEntries.Add(mappingEntry);

            List<MssMsg> desiredReturnedMsgList = new List<MssMsg>();
            desiredReturnedMsgList.Add(Factory_MssMsg(MssMsgType.CC, 2, 100, 0));

            Test_ProcessMssMsg(inputMsg, matchingEntries, desiredReturnedMsgList, false);
        }

        [Test]
        public void ProcessMssMsg_MapsToRangeOfSameSize_MsgIsProperlyMapped()
        {
            MssMsg inputMsg = Factory_MssMsg(MssMsgType.PolyAftertouch, 16, 64, 100);

            List<IMappingEntry> matchingEntries = new List<IMappingEntry>();
            IMappingEntry mappingEntry = Factory_IMappingEntry(
                MssMsgType.PolyAftertouch, 15, 16, 60, 70,
                MssMsgType.PolyAftertouch, 1, 2, 50, 60);
            matchingEntries.Add(mappingEntry);

            List<MssMsg> desiredReturnedMsgList = new List<MssMsg>();
            desiredReturnedMsgList.Add(Factory_MssMsg(MssMsgType.PolyAftertouch, 2, 54, 100));

            Test_ProcessMssMsg(inputMsg, matchingEntries, desiredReturnedMsgList, false);
        }

        [Test]
        public void ProcessMssMsg_MapsToRangeOfDifferentSize_MsgIsProperlyMapped()
        {
            MssMsg inputMsg = Factory_MssMsg(MssMsgType.NoteOn, 2, 10, 100);

            List<IMappingEntry> matchingEntries = new List<IMappingEntry>();
            IMappingEntry mappingEntry = Factory_IMappingEntry(
                MssMsgType.NoteOn, 1, 3, 9, 10,
                MssMsgType.NoteOn, 1, 5, 0, 127);
            matchingEntries.Add(mappingEntry);

            List<MssMsg> desiredReturnedMsgList = new List<MssMsg>();
            desiredReturnedMsgList.Add(Factory_MssMsg(MssMsgType.NoteOn, 3, 127, 100));

            Test_ProcessMssMsg(inputMsg, matchingEntries, desiredReturnedMsgList, false);
        }

        [Test]
        public void ProcessMssMsg_MultipleMappings_MsgIsProperlyMapped()
        {
            MssMsg inputMsg = Factory_MssMsg(MssMsgType.NoteOn, 1, 64, 100);

            List<IMappingEntry> matchingEntries = new List<IMappingEntry>();
            IMappingEntry mappingEntry = Factory_IMappingEntry(
                MssMsgType.NoteOn, 1, 1, 64, 64,
                MssMsgType.NoteOn, 2, 2, 65, 65);
            matchingEntries.Add(mappingEntry);

            IMappingEntry mappingEntry2 = Factory_IMappingEntry(
                MssMsgType.NoteOn, 1, 1, 64, 64,
                MssMsgType.CC, 3, 3, 66, 66);
            matchingEntries.Add(mappingEntry2);

            List<MssMsg> desiredReturnedMsgList = new List<MssMsg>();
            desiredReturnedMsgList.Add(Factory_MssMsg(MssMsgType.NoteOn, 2, 65, 100));
            desiredReturnedMsgList.Add(Factory_MssMsg(MssMsgType.CC, 3, 66, 100));

            Test_ProcessMssMsg(inputMsg, matchingEntries, desiredReturnedMsgList, false);
        }

        [Test]
        public void ProcessMssMsg_MapsCCToPitchbend_MsgIsProperlyMapped()
        {
            MssMsg inputMsg = Factory_MssMsg(MssMsgType.CC, 1, 10, 127);

            List<IMappingEntry> matchingEntries = new List<IMappingEntry>();
            IMappingEntry mappingEntry = Factory_IMappingEntry(
                MssMsgType.CC, 1, 1, 10, 10,
                MssMsgType.PitchBend, 1, 1, 10, 10);
            matchingEntries.Add(mappingEntry);

            List<MssMsg> desiredReturnedMsgList = new List<MssMsg>();
            desiredReturnedMsgList.Add(Factory_MssMsg(MssMsgType.PitchBend, 1, 10, 16383));

            Test_ProcessMssMsg(inputMsg, matchingEntries, desiredReturnedMsgList, false);
        }

        [Test]
        public void ProcessMssMsg_MapsOutsideOfRange_NoMsgsAreReturned()
        {
            MssMsg inputMsg = Factory_MssMsg(MssMsgType.NoteOn, 1, 10, 64);

            List<IMappingEntry> matchingEntries = new List<IMappingEntry>();
            IMappingEntry mapsNoteOffToSameMsg = Factory_IMappingEntry(
                MssMsgType.NoteOn, 1, 1, 10, 10,
                MssMsgType.NoteOn, 1, 1, 10, 10);
            matchingEntries.Add(mapsNoteOffToSameMsg);

            List<MssMsg> desiredReturnedMsgList = new List<MssMsg>();

            Test_ProcessMssMsg(inputMsg, matchingEntries, desiredReturnedMsgList, true);
        }

        /// <summary>
        /// Tests the ProcessMssMsg() method.
        /// </summary>
        /// <param name="inputMsg">The message to be precessed</param>
        /// <param name="matchingEntries">
        ///     Mapping entries where <paramref name="inputMsg"/> is in the input message range
        /// </param>
        /// <param name="desiredReturnedMsgList"> List of that should be returned by ProcessMssMsg() </param>
        /// <param name="doubleData3"> 
        ///     Simulates the equation "x*2" in each mapping if true. Simulates the equation "x" in each mapping if 
        ///     false. 
        /// </param>
        protected void Test_ProcessMssMsg(MssMsg inputMsg, 
                                         List<IMappingEntry> matchingEntries, 
                                         List<MssMsg> desiredReturnedMsgList,
                                         bool doubleData3)
        {
            int inputMultiple;
            if (doubleData3 == true)
            {
                inputMultiple = 2;
            }
            else
            {
                inputMultiple = 1;
            }

            //Setup the MssEvaluator that msgProcessor will use so that it always just returns the input
            var mssEvanuatorMock = new Mock<IEvaluator>();
            mssEvanuatorMock.Setup(evaluator => evaluator.Evaluate(It.IsAny<EvaluationCurveInput>()))
                            .Returns((EvaluationCurveInput input) => new ReturnStatus<double>(input.RelData3 * inputMultiple, true));
            IocMgr.Kernel.Rebind<IEvaluator>().ToConstant(mssEvanuatorMock.Object);
            
            MssMsgProcessor msgProcessor = new MssMsgProcessor();

            var mapMgrMock = new Mock<IMappingManager>();
            mapMgrMock.Setup(mapMgr => mapMgr.GetCopiesOfMappingEntriesForMsg(inputMsg)).Returns(matchingEntries);

            var mssParameterViewerMock = new Mock<IMssParameterViewer>();

            msgProcessor.Init(mapMgrMock.Object, mssParameterViewerMock.Object);

            List<MssMsg> returnedMsgList = msgProcessor.ProcessMssMsg(inputMsg);

            //Ensure returnedMsgList and desiredReturnedMsgList contain the same elements.
            foreach(MssMsg returnedMsg in returnedMsgList) 
            {
                int foundIndex = desiredReturnedMsgList.FindIndex(
                    desiredReturnedMsg => desiredReturnedMsg.Equals(returnedMsg));
                if (foundIndex == -1)
                {
                    Assert.Fail("The desired MssMsgs were not returned.");
                    return;
                }
                else
                {
                    //Ensures that each message is desiredReturnedMsgs is only matched to a 
                    //message in returnedMsgs once.
                    desiredReturnedMsgList.RemoveAt(foundIndex);
                }
            }
            Assert.IsTrue(desiredReturnedMsgList.Count == 0, "Some of the desired MssMSgs were not returned");
        }

        protected MssMsg Factory_MssMsg(MssMsgType msgType, int data1, int data2, int data3)
        {
            return new MssMsg(msgType, data1, data2, data3);
        }

        protected IMappingEntry Factory_IMappingEntry(
            MssMsgType inMsgType, int inData1Bottom, int inData1Top, int inData2Bottom, int inData2Top,
            MssMsgType outMsgType, int outData1Bottom, int outData1Top, int outData2Bottom, int outData2Top)
        {
            MssMsgRange inMsgRange = new MssMsgRange();
            inMsgRange.InitPublicMembers(inMsgType, inData1Bottom, inData1Top, inData2Bottom, inData2Top);

            MssMsgRange outMsgRange = new MssMsgRange();
            outMsgRange.InitPublicMembers(outMsgType, outData1Bottom, outData1Top, outData2Bottom, outData2Top);

            IMappingEntry mappingEntry = new MappingEntry();
            mappingEntry.InitAllMembers(inMsgRange, outMsgRange, false, DEFAULT_CURVE_SHAPE_INFO);

            return mappingEntry;
        }
    }
}

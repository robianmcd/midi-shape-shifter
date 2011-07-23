using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

using MidiShapeShifter.Mss;
using MidiShapeShifter.Mss.Mapping;

namespace MidiShapeShifterTest.Mss.Mapping
{
    [TestFixture]
    class EntryFieldInterpretingUtilsTest
    {
        [Test]
        public void InterpretAsInt_ValidInteger_SuccessfullyExtractsInt()
        {
            int outValue;
            bool successfullyInturpreted = EntryFieldInterpretingUtils.InterpretAsInt("25", out outValue);

            Assert.IsTrue(successfullyInturpreted);
            Assert.AreEqual(25, outValue);
        }

        [Test]
        public void InterpretAsInt_ValidIntegerWithWhitespace_SuccessfullyExtractsInt()
        {
            int outValue;
            bool successfullyInturpreted = EntryFieldInterpretingUtils.InterpretAsInt("  12  ", out outValue);

            Assert.IsTrue(successfullyInturpreted);
            Assert.AreEqual(12, outValue);
        }

        [Test]
        public void InterpretAsInt_NonIntegerString_CannotExtractInt()
        {
            int outValue;
            bool successfullyInturpreted = EntryFieldInterpretingUtils.InterpretAsInt("1b", out outValue);

            Assert.IsFalse(successfullyInturpreted);
        }

        [Test]
        public void InterpretAsRangeAllStr_UppercaseVersionOfRangeAllString_ReturnsTrue()
        {
            bool stringIsRangeAllStr = 
                EntryFieldInterpretingUtils.InterpretAsRangeAllStr(MssMsgUtil.RANGE_ALL_STR.ToUpper());

            Assert.IsTrue(stringIsRangeAllStr);
        }

        [Test]
        public void InterpretAsRangeAllStr_NonRangeAllString_ReturnsFalse()
        {
            bool stringIsRangeAllStr = EntryFieldInterpretingUtils.InterpretAsRangeAllStr("16");

            Assert.IsFalse(stringIsRangeAllStr);
        }

        [Test]
        public void InterpretAsRange_ValidIntegerRange_SuccessfullyExtractsRange()
        {
            int outRangeTop;
            int outRangeBottom;
            bool successfullyInturpreted = 
                EntryFieldInterpretingUtils.InterpretAsRange("10 - 20", out outRangeTop, out outRangeBottom);

            Assert.IsTrue(successfullyInturpreted);
            Assert.AreEqual(10, outRangeBottom);
            Assert.AreEqual(20, outRangeTop);
        }

        [Test]
        public void InterpretAsRange_BackwardsIntegerRange_SuccessfullyAssignSmallerNumberToRangeBottomAndLargerNumberToRangeTop()
        {
            int outRangeTop;
            int outRangeBottom;
            bool successfullyInturpreted =
                EntryFieldInterpretingUtils.InterpretAsRange("85-6", out outRangeTop, out outRangeBottom);

            Assert.IsTrue(successfullyInturpreted);
            Assert.AreEqual(6, outRangeBottom);
            Assert.AreEqual(85, outRangeTop);
        }

        [Test]
        public void InterpretAsRange_SingleInteger_ReturnsFalse()
        {
            int outRangeTop;
            int outRangeBottom;
            bool successfullyInturpreted = 
                EntryFieldInterpretingUtils.InterpretAsRange("9", out outRangeTop, out outRangeBottom);

            Assert.IsFalse(successfullyInturpreted);
        }

        [Test]
        public void InterpretAsRange_RangeOfLetters_ReturnsFalse()
        {
            int outRangeTop;
            int outRangeBottom;
            bool successfullyInturpreted =
                EntryFieldInterpretingUtils.InterpretAsRange("a-z", out outRangeTop, out outRangeBottom);

            Assert.IsFalse(successfullyInturpreted);
        }
    }
}

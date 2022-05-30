
using MidiShapeShifter.Mss;
using MidiShapeShifter.Mss.Mapping;
using NUnit.Framework;

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
        public void InterpretAsInt_DecimalInput_CannotExtractInt()
        {
            int outValue;
            bool successfullyInturpreted = EntryFieldInterpretingUtils.InterpretAsInt("1.5", out outValue);

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
        public void InterpretAsRangeOfInts_ValidIntegerRange_SuccessfullyExtractsRange()
        {
            int outRangeTop;
            int outRangeBottom;
            bool successfullyInturpreted =
                EntryFieldInterpretingUtils.InterpretAsRangeOfInts("10 - 20", out outRangeTop, out outRangeBottom);

            Assert.IsTrue(successfullyInturpreted);
            Assert.AreEqual(10, outRangeBottom);
            Assert.AreEqual(20, outRangeTop);
        }

        [Test]
        public void InterpretAsRangeOfInts_BackwardsIntegerRange_SuccessfullyAssignSmallerNumberToRangeBottomAndLargerNumberToRangeTop()
        {
            int outRangeTop;
            int outRangeBottom;
            bool successfullyInturpreted =
                EntryFieldInterpretingUtils.InterpretAsRangeOfInts("85-6", out outRangeTop, out outRangeBottom);

            Assert.IsTrue(successfullyInturpreted);
            Assert.AreEqual(6, outRangeBottom);
            Assert.AreEqual(85, outRangeTop);
        }

        [Test]
        public void InterpretAsRangeOfInts_SingleInteger_ReturnsFalse()
        {
            int outRangeTop;
            int outRangeBottom;
            bool successfullyInturpreted =
                EntryFieldInterpretingUtils.InterpretAsRangeOfInts("9", out outRangeTop, out outRangeBottom);

            Assert.IsFalse(successfullyInturpreted);
        }

        [Test]
        public void InterpretAsRangeOfInts_RangeOfLetters_ReturnsFalse()
        {
            int outRangeTop;
            int outRangeBottom;
            bool successfullyInturpreted =
                EntryFieldInterpretingUtils.InterpretAsRangeOfInts("a-z", out outRangeTop, out outRangeBottom);

            Assert.IsFalse(successfullyInturpreted);
        }

        [Test]
        public void InterpretAsRangeOfInts_RangeWithDecimal_ReturnsFalse()
        {
            int outRangeTop;
            int outRangeBottom;
            bool successfullyInturpreted =
                EntryFieldInterpretingUtils.InterpretAsRangeOfInts("10.3-15", out outRangeTop, out outRangeBottom);

            Assert.IsFalse(successfullyInturpreted);
        }
    }
}

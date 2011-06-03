using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.Mapping
{
    /// <summary>
    /// EntryFieldInterpretingUtils provides various static methods that will attempt to interpret and extract values 
    /// from user input. This class is intented to be used to valitate user input in the MappingDlg.
    /// </summary>
    public static class EntryFieldInterpretingUtils
    {
        /// <summary>
        ///     Attempts to interpret <paramref name="input"/> as an integer.
        /// </summary>
        /// <param name="input">User input.</param>
        /// <param name="value">User input as an integer.</param>
        /// <returns>True if <paramref name="input"/> could be converted into an integer.</returns>
        public static bool InterpretAsInt(string input, out int value)
        {
            if (int.TryParse(input.Trim(), out value))
            {
                return true;
            }
            else 
            {
                return false;
            }
        }

        /// <summary>
        ///     Attempts to interpret <paramref name="input"/> as a string representing all values in a range. Eg. the 
        ///     word "All".
        /// </summary>
        /// <param name="input">User input.</param>
        /// <returns>True if <paramref name="input"/> is the RangeAll string.</returns>
        public static bool InterpretAsRangeAllStr(string input)
        {
            return string.Equals(input.Trim(), MssMsgUtil.RANGE_ALL_STR, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        ///     Attempts to interpret <paramref name="input"/> as a range of integers. Eg. "25-100".
        /// </summary>
        /// <param name="input">User input.</param>
        /// <param name="rangeTop">The higher number in the range.</param>
        /// <param name="rangeBottom">The lower number in the range.</param>
        /// <returns>True if <paramref name="input"/> could be inturpreted as a range of integers.</returns>
        public static bool InterpretAsRange(string input, out int rangeTop, out int rangeBottom)
        {
            bool validRangeStructure;
            input = input.Trim();

            string[] rangeArr = input.Split('-');
            if (rangeArr.Length == 2)
            {
                //Trys to convert the first number into an integer.
                bool rangeIsInts = int.TryParse(rangeArr[0].Trim(), out rangeBottom);
                //Trys to convert the second number into an integer.
                rangeIsInts &= int.TryParse(rangeArr[1].Trim(), out rangeTop);
                if (rangeIsInts)
                {
                    validRangeStructure = true;
                }
                else
                {
                    rangeBottom = MssMsgUtil.RANGE_INVALID;
                    rangeTop = MssMsgUtil.RANGE_INVALID;
                    validRangeStructure = false;
                }
            }
            else
            {
                rangeBottom = MssMsgUtil.RANGE_INVALID;
                rangeTop = MssMsgUtil.RANGE_INVALID;
                validRangeStructure = false;
            }

            //swap the top and bottom of the range if the bottom is greater then the top
            if (rangeBottom > rangeTop)
            {
                int rangeBottomBackup = rangeBottom;
                rangeBottom = rangeTop;
                rangeTop = rangeBottomBackup;
            }

            return validRangeStructure;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mapping
{
    static class EntryFieldInterpretingUtils
    {
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

        public static bool InterpretAsRangeAllStr(string input)
        {
            return string.Equals(input.Trim(), MssMsgUtil.RANGE_ALL_STR, StringComparison.OrdinalIgnoreCase);
        }

        public static bool InterpretAsRange(string input, out int rangeTop, out int rangeBottom)
        {
            bool validRangeStructure;
            input = input.Trim();

            string[] rangeArr = input.Split('-');
            if (rangeArr.Length == 2)
            {
                bool rangeIsInts = int.TryParse(rangeArr[0].Trim(), out rangeBottom);
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

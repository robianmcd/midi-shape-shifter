using System.Collections.Generic;
using System.Diagnostics;

namespace MidiShapeShifter.Mss
{
    //This class should be refactored into other classes and deleted
    public static class MssMsgUtil
    {
        

        //RANGE_ALL_STR is used to represent a midi message ranges that convere all channels or all parameter values
        public const string RANGE_ALL_STR = "All";
        public const int RANGE_INVALID = -1;
        public const int MIN_CHANNEL = 1;
        public const int MAX_CHANNEL = 16;
        public const int MIN_PARAM = 0;
        public const int MAX_PARAM = 127;

        public static bool isValidParamValue(int value)
        {
            return value >= MIN_PARAM && value <= MAX_PARAM;
        }

        public static bool isValidChannel(int channel)
        {
            return channel >= MIN_CHANNEL && channel <= MAX_CHANNEL;
        }
    }
}

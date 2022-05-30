namespace MidiShapeShifter.Mss
{
    public static class MssMsgUtil
    {
        public const int UNUSED_MSS_MSG_DATA = -1;
        public const string UNUSED_MSS_MSG_STRING = "N/A";

        public const int MIN_RELATIVE_MSG_VAL = 0;
        public const int MAX_RELATIVE_MSG_VAL = 1;

        //RANGE_ALL_STR is used to represent a midi message ranges that convere all channels or all parameter values
        public const string RANGE_ALL_STR = "All";
        public const int RANGE_INVALID = -1;
    }
}

namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    /// <summary>
    /// Contains static information and utility functions for an MSS message. Also contains 
    /// information and utility functions that may rely on the state of this class or other 
    /// classes. An instance of this may require custom initialization.
    /// </summary>
    public interface IMssMsgInfo : IStaticMssMsgInfo
    {
        string ConvertData1ToString(double Data1);
        string ConvertData2ToString(double Data2);
        string ConvertData3ToString(double Data3);
    }
}

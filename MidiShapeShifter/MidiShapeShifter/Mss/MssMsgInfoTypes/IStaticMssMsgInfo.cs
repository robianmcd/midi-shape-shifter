using System;

using MidiShapeShifter.Mss;

namespace MidiShapeShifter.Mss.MssMsgInfoTypes
{
    /// <summary>
    /// Contains information and utility functions for an MSS message type that do not require any 
    /// initialization, dependencies, or state. 
    /// </summary>
    public interface IStaticMssMsgInfo
    {
        string Data1Name { get; }
        string Data2Name { get; }
        string Data3Name { get; }

        string GetDataFieldName(MssMsgDataField field);

        double MaxData1Value { get; }
        double MaxData2Value { get; }
        double MaxData3Value { get; }

        double MinData1Value { get; }
        double MinData2Value { get; }
        double MinData3Value { get; }

        MssMsgType MsgType { get; }
    }
}

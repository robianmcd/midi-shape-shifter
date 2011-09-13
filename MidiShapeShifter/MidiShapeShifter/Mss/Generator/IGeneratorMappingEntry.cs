using System;
using MidiShapeShifter.Mss.Mapping;

namespace MidiShapeShifter.Mss.Generator
{
    public interface IGeneratorMappingEntry : IMappingEntry
    {
        GenEntryConfigInfo GenConfigInfo { get; set; }
        GenEntryHistoryInfo GenHistoryInfo { get; set; }
        string GetReadableEnabledStatus();
        string GetReadableLoopStatus();
        string GetReadablePeriod();
        void InitAllMembers(MidiShapeShifter.Mss.MssMsgRange inMsgRange, MidiShapeShifter.Mss.MssMsgRange outMsgRange, bool overrideDuplicates, MidiShapeShifter.Mss.CurveShapeInfo curveShapeInfo, GenEntryConfigInfo generatorInfo);
    }
}

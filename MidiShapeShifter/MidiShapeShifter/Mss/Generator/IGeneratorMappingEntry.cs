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
        void InitAllMembers(MidiShapeShifter.Mss.IMssMsgRange inMsgRange, MidiShapeShifter.Mss.IMssMsgRange outMsgRange, bool overrideDuplicates, MidiShapeShifter.Mss.CurveShapeInfo curveShapeInfo, GenEntryConfigInfo generatorInfo);
    }
}

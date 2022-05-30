using MidiShapeShifter.Mss.Mapping;

namespace MidiShapeShifter.Mss.Generator
{
    public interface IGeneratorMappingManager : IGraphableMappingManager<IGeneratorMappingEntry>
    {
        int CreateAndAddEntryFromGenInfo(GenEntryConfigInfo genInfo);
        bool UpdateEntryWithNewGenInfo(GenEntryConfigInfo genInfo, int id);
    }
}

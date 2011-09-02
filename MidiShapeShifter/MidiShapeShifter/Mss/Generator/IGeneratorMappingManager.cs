using System;
using MidiShapeShifter.Mss.Mapping;

namespace MidiShapeShifter.Mss.Generator
{
    public interface IGeneratorMappingManager : IGraphableMappingManager
    {
        void AddGenMappingEntry(GeneratorMappingEntry newEntry);
        void CreateAndAddEntryFromGenInfo(GenEntryConfigInfo genInfo);
        GeneratorMappingEntry GetGenMappingEntryById(int id);
        GeneratorMappingEntry GetGenMappingEntryByIndex(int index);
        void RemoveGenMappingEntry(int index);
        void UpdateEntryWithNewGenInfo(GenEntryConfigInfo genInfo);
    }
}

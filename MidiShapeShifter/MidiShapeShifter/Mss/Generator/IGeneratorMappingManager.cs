using System;
using MidiShapeShifter.Mss.Mapping;

namespace MidiShapeShifter.Mss.Generator
{
    public interface IGeneratorMappingManager : IGraphableMappingManager
    {
        void AddGenMappingEntry(IGeneratorMappingEntry newEntry);
        void CreateAndAddEntryFromGenInfo(GenEntryConfigInfo genInfo);
        IGeneratorMappingEntry GetGenMappingEntryById(int id);
        IGeneratorMappingEntry GetGenMappingEntryByIndex(int index);
        int GetIndexById(int id);
        void RemoveGenMappingEntry(int index);
        void UpdateEntryWithNewGenInfo(GenEntryConfigInfo genInfo);
    }
}

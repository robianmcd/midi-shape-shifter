using System;
namespace MidiShapeShifter.Mss.Mapping
{
    public interface IMappingManager : IGraphableMappingManager
    {
        void AddMappingEntry(MappingEntry newEntry);
        void MoveEntryDown(int index);
        void MoveEntryUp(int index);
        void RemoveMappingEntry(int index);
    }
}

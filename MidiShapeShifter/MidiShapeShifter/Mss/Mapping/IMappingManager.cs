using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MidiShapeShifter.Mss.Mapping
{
    public interface IMappingManager : IGraphableMappingManager
    {
        ReadOnlyCollection<IMappingEntry> readOnlyMappingEntryList { get; }

        void AddMappingEntry(IMappingEntry newEntry);
        void MoveEntryDown(int index);
        void MoveEntryUp(int index);
        void RemoveMappingEntry(int index);
    }
}

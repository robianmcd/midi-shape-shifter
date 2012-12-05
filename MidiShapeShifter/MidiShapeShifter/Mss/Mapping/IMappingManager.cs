using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MidiShapeShifter.Mss.Mapping
{
    public interface IMappingManager : IGraphableMappingManager<IMappingEntry>
    {
        bool MoveEntryDown(int id);
        bool MoveEntryUp(int id);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.Mapping
{
    public interface IGraphableMappingManager
    {
        int GetNumEntries();
        IEnumerable<IMappingEntry> GetAssociatedEntries(MidiShapeShifter.Mss.MssMsg inputMsg);
        IMappingEntry GetMappingEntry(int index);
    }
}

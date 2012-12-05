using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MidiShapeShifter.CSharpUtil;

namespace MidiShapeShifter.Mss.Mapping
{
    public interface IBaseGraphableMappingManager
    {
        IEnumerable<IMappingEntry> GetCopiesOfIMappingEntriesForMsg(MidiShapeShifter.Mss.MssMsg inputMsg);
        IReturnStatus<IMappingEntry> GetCopyOfIMappingEntryById(int id);
        IEnumerable<IMappingEntry> GetCopyOfIMappingEntryList();
        bool RemoveMappingEntry(int id);
        int GetIndexById(int id);
        int GetNumEntries();
    }
}

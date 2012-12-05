using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MidiShapeShifter.CSharpUtil;

namespace MidiShapeShifter.Mss.Mapping
{
    public interface IGraphableMappingManager<MappingEntryType> : IBaseGraphableMappingManager
        where MappingEntryType : IMappingEntry
    {
        IEnumerable<MappingEntryType> GetCopiesOfMappingEntriesForMsg(MidiShapeShifter.Mss.MssMsg inputMsg);
        IReturnStatus<MappingEntryType> GetCopyOfMappingEntryById(int id);
        List<MappingEntryType> GetCopyOfMappingEntryList();

        int AddMappingEntry(MappingEntryType newEntry);
        bool RunFuncOnMappingEntry(int id, GraphableMappingManager<MappingEntryType>.MappingEntryAccessor mappingEntryAccessor);
    }
}

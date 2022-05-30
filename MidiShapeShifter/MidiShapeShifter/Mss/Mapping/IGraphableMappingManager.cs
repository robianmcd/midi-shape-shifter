using MidiShapeShifter.CSharpUtil;
using System.Collections.Generic;

namespace MidiShapeShifter.Mss.Mapping
{
    public interface IGraphableMappingManager<MappingEntryType> : IBaseGraphableMappingManager
        where MappingEntryType : IMappingEntry
    {
        new IEnumerable<MappingEntryType> GetCopiesOfMappingEntriesForMsg(MidiShapeShifter.Mss.MssMsg inputMsg);
        new IReturnStatus<MappingEntryType> GetCopyOfMappingEntryById(int id);
        new List<MappingEntryType> GetCopyOfMappingEntryList();
        bool RunFuncOnMappingEntry(int id, MappingEntryAccessor<MappingEntryType> mappingEntryAccessor);

        int AddMappingEntry(MappingEntryType newEntry);
        bool ReplaceMappingEntry(MappingEntryType mappingEntry);
    }
}

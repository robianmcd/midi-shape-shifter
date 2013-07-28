using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MidiShapeShifter.CSharpUtil;

namespace MidiShapeShifter.Mss.Mapping
{
    public interface IBaseGraphableMappingManager
    {
        IEnumerable<IMappingEntry> GetCopiesOfMappingEntriesForMsg(MidiShapeShifter.Mss.MssMsg inputMsg);
        IReturnStatus<IMappingEntry> GetCopyOfMappingEntryById(int id);
        IEnumerable<IMappingEntry> GetCopyOfMappingEntryList();
        List<int> GetEntryIdList();
        bool RemoveMappingEntry(int id);
        int GetNumEntries();
        int GetMappingEntryIndexById(int id);
        int GetMappingEntryIdByIndex(int index);


        IReturnStatus<CurveShapeInfo> GetCopyOfCurveShapeInfoById(int id);

        bool RunFuncOnMappingEntry(int id, MappingEntryAccessor<IMappingEntry> mappingEntryAccessor);

    }
}

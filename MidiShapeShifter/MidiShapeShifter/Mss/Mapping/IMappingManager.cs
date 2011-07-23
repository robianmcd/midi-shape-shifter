using System;
namespace MidiShapeShifter.Mss.Mapping
{
    public interface IMappingManager
    {
        void AddMappingEntry(MappingEntry newEntry);
        System.Collections.Generic.IEnumerable<MappingEntry> GetAssociatedEntries(MidiShapeShifter.Mss.MssMsg inputMsg);
        System.Windows.Forms.ListViewItem GetListViewRow(int index);
        MappingEntry GetMappingEntry(int index);
        int GetNumEntries();
        void MoveEntryDown(int index);
        void MoveEntryUp(int index);
        void RemoveMappingEntry(int index);
    }
}

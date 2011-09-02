using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.Mapping
{
    public interface IGraphableMappingManager
    {
        int GetNumEntries();
        IEnumerable<MappingEntry> GetAssociatedEntries(MidiShapeShifter.Mss.MssMsg inputMsg);
        System.Windows.Forms.ListViewItem GetListViewRow(int index);
        MappingEntry GetMappingEntry(int index);
    }
}

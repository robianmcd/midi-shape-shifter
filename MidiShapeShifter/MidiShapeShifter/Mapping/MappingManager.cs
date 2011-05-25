using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;
using System.Windows.Forms;

namespace MidiShapeShifter.Mapping
{
    //This class is responsible for storing and interpreting MappingEntries
    public class MappingManager
    {
        protected List<MappingEntry> mappingEntries = new List<MappingEntry>();
        /*public List<MappingEntry> MappingEntries { 
            get 
            {
                return _mappingEntries;
            } 
            private set 
            {
                _mappingEntries = value;
            } 
        }*/

        public void AddMappingEntry(MappingEntry newEntry) 
        {
            mappingEntries.Add(newEntry);
        }

        public void RemoveMappingEntry(int index) 
        {
            if (index >= 0 && index < mappingEntries.Count)
            {
                mappingEntries.RemoveAt(index);
            }
            else
            {
                //invalid index
                Debug.Assert(false);
            }
        }

        public MappingEntry GetMappingEntry(int index)
        {
            if (index >= 0 && index < mappingEntries.Count)
            {
                return mappingEntries[index];
            }
            else
            {
                //invalid index
                Debug.Assert(false);
                return null;
            }
        }

        public int GetNumEntries()
        {
            return mappingEntries.Count;
        }

        public void MoveEntryUp(int index) 
        {
            if (index >= 1 && index < mappingEntries.Count)
            {
                MappingEntry tempEntry = mappingEntries[index];
                mappingEntries[index] = mappingEntries[index - 1];
                mappingEntries[index - 1] = tempEntry;
            }
            else
            {
                //invalid index
                Debug.Assert(false);
            }
        }

        public void MoveEntryDown(int index)
        {
            if (index >= 0 && index < mappingEntries.Count - 1)
            {
                MappingEntry tempEntry = mappingEntries[index];
                mappingEntries[index] = mappingEntries[index + 1];
                mappingEntries[index + 1] = tempEntry;
            }
            else
            {
                //invalid index
                Debug.Assert(false);
            }
        }

        public ListViewItem GetListViewRow(int index)
        { 
            if (index >= 0 && index < mappingEntries.Count)
            {
                MappingEntry entry = mappingEntries[index];
                ListViewItem mappingItem = new ListViewItem(entry.GetReadableMsgType(MappingEntry.IO.Input));
                mappingItem.SubItems.Add(entry.InMssMsgInfo.Field1);
                mappingItem.SubItems.Add(entry.InMssMsgInfo.Field2);

                mappingItem.SubItems.Add(entry.GetReadableMsgType(MappingEntry.IO.Output));
                mappingItem.SubItems.Add(entry.OutMssMsgInfo.Field1);
                mappingItem.SubItems.Add(entry.OutMssMsgInfo.Field2);

                mappingItem.SubItems.Add(entry.GetReadableOverrideDuplicates());

                return mappingItem;
            }
            else
            {
                //invalid index
                Debug.Assert(false);
                return null;
            }
        }

        public IEnumerable<MappingEntry> GetAssociatedEntries(MssMsg inputMsg) 
        {
            var associatedEntiresQuery =
                from entry in mappingEntries
                where entry.InMssMsgInfo.MatchesMssMsg(inputMsg)
                select entry;

            //deal with input overrides
            var inputOverrideEntriesQuery =
                from entry in associatedEntiresQuery
                where entry.OverrideDuplicates == true
                select entry;

            var inputOverrideEntriesList = inputOverrideEntriesQuery.ToList();
            if (inputOverrideEntriesList.Count > 0)
            {
                associatedEntiresQuery =
                from entry in associatedEntiresQuery
                where entry == inputOverrideEntriesList[0]
                select entry;
            }

            return associatedEntiresQuery;
        }
    }
}

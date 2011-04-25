using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MidiShapeShifter
{
    public class MappingManager
    {
        private List<MappingEntry> _mappingEntries = new List<MappingEntry>();
        public List<MappingEntry> MappingEntries { 
            get 
            {
                return _mappingEntries;
            } 
            private set 
            {
                _mappingEntries = value;
            } 
        }

        public bool AddMappingEntry(MappingEntry newEntry) 
        {
            //TODO: Valadate newEntry

            MappingEntries.Add(newEntry);
            return true;
        }

        public bool removeMappingEntry(int priorityIndex) 
        {
            bool entryFound = false;

            foreach (MappingEntry entry in MappingEntries) 
            {
                if (entry.priority == priorityIndex)
                {
                    MappingEntries.Remove(entry);
                    entryFound = true;
                }
                else if (entryFound)
                {
                    entry.priority--;
                }
            }

            return entryFound;
        }


        public bool MoveEntryUp(int index) 
        {
            if (index >= 1 && index < MappingEntries.Count)
            {
                MappingEntries[index].priority--;
                MappingEntries[index - 1].priority++;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool MoveEntryDown(int index)
        {
            if (index >= 0 && index < MappingEntries.Count - 1)
            {
                MappingEntries[index].priority++;
                MappingEntries[index + 1].priority--; 
                return true;
            }
            else
            {
                return false;
            }
        }

        public IEnumerable<MappingEntry> GetAssociatedEntries(MidiHelper.MidiMsg inputMsg) 
        {
            var associatedEntiresQuery =
                from entry in MappingEntries
                where entry.inMsgRange.msgType == inputMsg.type &&
                      ValueIsInRange(inputMsg.channel, entry.inMsgRange.bottomChannel, entry.inMsgRange.topChannel) &&
                      ValueIsInRange(inputMsg.param1, entry.inMsgRange.bottomParam, entry.inMsgRange.topParam)
                select entry;

            //deal with input overrides
            var inputOverrideEntriesQuery =
                from entry in associatedEntiresQuery
                where entry.overrideDuplicates == true
                orderby entry.priority ascending
                select entry;

            var inputOverrideEntriesList = inputOverrideEntriesQuery.ToList();
            if (inputOverrideEntriesList.Count > 0)
            {
                associatedEntiresQuery =
                from entry in associatedEntiresQuery
                where entry.priority == inputOverrideEntriesList[0].priority
                select entry;    
            }

            return associatedEntiresQuery;
        }

        protected bool ValueIsInRange(int value, int bottomOfRange, int topOfRange) 
        {
            return (value >= bottomOfRange && value <= topOfRange);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MidiShapeShifter
{
    class MappingManager
    {
        //The following constants are used to specify midi message ranges that convere all channels or all 
        //parameter values
        public const int RANGE_ALL_INT = -1;
        public const string RANGE_ALL_STR = "All";

        //Each instance represents a range of midi messages. For example: All note on message from C1 to C2
        public class MidiMsgRange 
        {
            public MidiHelper.MidiMsgType msgType;
            public int topChannel;
            public int bottomChannel;
            public int topParam;
            public int bottomParam;

        }

        public class MappingEntry
        {
            public MidiMsgRange inMsgRange;
            public MidiMsgRange outMsgRange;

            //If there are multiple mapping entries with overlapping in or out ranges then a single midi message can 
            //generate several or there could be several messages that map to one. This can be disabled by setting 
            //either of the override duplicates flags to true
            public bool overrideInDuplicates;
            public bool overrideOutDuplicates;

            //specifies the order in which the override duplicates flags are considered. For example if there are two
            //mapping entries with an overlapping inMsgRange and overrideInDuplicates set to true then the one 
            //with a priority closer to 0 will override the other. This priority also corresponds to the index if an 
            //entry in the listbox it is displayed in. Each entry should have a unique priority.
            public int priority;
        }

        public List<MappingEntry> MappingEntries { get; private set; }

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
                where entry.overrideInDuplicates == true
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

            //deal with output overrides

            //TODO: what happens if this is empty? do we block the message or pass it through?
            return associatedEntiresQuery;
        }

        protected bool ValueIsInRange(int value, int bottomOfRange, int topOfRange) 
        {
            return (value >= bottomOfRange && value <= topOfRange) ||
                   (topOfRange == RANGE_ALL_INT && bottomOfRange == RANGE_ALL_INT);
        }
    }
}

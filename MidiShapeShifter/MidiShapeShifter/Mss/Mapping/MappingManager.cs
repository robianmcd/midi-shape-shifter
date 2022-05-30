using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace MidiShapeShifter.Mss.Mapping
{

    /// <summary>
    ///     The MappingManager is responsible for storing, retrieving and interpreting MappingEntry objects.
    /// </summary>
    [DataContract]
    public class MappingManager : GraphableMappingManager<IMappingEntry>, IMappingManager
    {
        /// <summary>
        ///     Moves the MappingEntry specified by <paramref name="id"/> up one element in the list.
        /// </summary>
        /// <remarks>
        ///     The order of MappingEntry objects in the list can affect which MappingEntry objects are returned by 
        ///     GetAssociatedEntries().
        /// </remarks>
        /// <returns> True if an entry is successfully moved up in the list</returns>
        public bool MoveEntryUp(int id)
        {
            lock (MssComponentHub.criticalSectioinLock)
            {
                int entryIndex = GetMappingEntryIndexById(id);
                if (entryIndex == -1 || entryIndex == 0)
                {
                    return false;
                }
                else
                {
                    IMappingEntry tempEntry = mappingEntryList[entryIndex];
                    mappingEntryList[entryIndex] = mappingEntryList[entryIndex - 1];
                    mappingEntryList[entryIndex - 1] = tempEntry;

                    return true;
                }
            }
        }

        /// <summary>
        ///     Moves the MappingEntry specified by <paramref name="id"/> down one element in the list.
        /// </summary>
        /// <remarks>
        ///     The order of <see cref="MappingEntry"/> objects in the list can affect which MappingEntry objects are 
        ///     returned by GetAssociatedEntries().
        /// </remarks>
        /// <returns> True if an entry is successfully moved up in the list</returns>
        public bool MoveEntryDown(int id)
        {
            lock (MssComponentHub.criticalSectioinLock)
            {
                int entryIndex = GetMappingEntryIndexById(id);
                if (entryIndex == -1 || entryIndex == this.mappingEntryList.Count - 1)
                {
                    return false;
                }
                else
                {
                    IMappingEntry tempEntry = mappingEntryList[entryIndex];
                    mappingEntryList[entryIndex] = mappingEntryList[entryIndex + 1];
                    mappingEntryList[entryIndex + 1] = tempEntry;

                    return true;
                }
            }
        }

        /// <summary>
        ///     Queries the MappingEntry objects in the MappingManager and returns each one that matches InMssMsgRange 
        ///     <paramref name="inputMsg"/>. For example, if the MappingManager contained a MappingEntry whose 
        ///     InMssMsgRange described a range of NoteOn MIDI messages, then that MappingEntry would be returned if 
        ///     <paramref name="inputMsg"/> was a NoteOn message that fell into InMssMsgRange's range.
        ///     
        ///     If multiple MappingEntry objects match <paramref name="inputMsg"/> and one or more has OverrideDuplicates set
        ///     to true then the one with the lowest index will be returned.
        /// </summary>
        /// <param name="inputMsg">MssMsg to query the MappingManager with.</param>
        /// <returns>An enumeration of MappingEntry objects that match <paramref name="inputMsg"/>.</returns>
        public override IEnumerable<IMappingEntry> GetCopiesOfMappingEntriesForMsg(MssMsg inputMsg)
        {
            lock (MssComponentHub.criticalSectioinLock)
            {
                var associatedEntiresQuery =
                    from entry in mappingEntryList
                    where entry.InMssMsgRange.MsgIsInRange(inputMsg)
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

                //Select() will return a querry which references the original list, so if mappingEntryList 
                //was changed then the querry would also be changed. This is why there is a call to 
                //ToList() This creates a seperate list that is not linked to mappingEntryList.
                return associatedEntiresQuery.Select(entry => (IMappingEntry)entry.Clone()).ToList();
            }
        }

    }
}

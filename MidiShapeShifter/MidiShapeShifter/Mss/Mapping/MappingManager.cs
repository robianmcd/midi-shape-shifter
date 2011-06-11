using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;
using System.Windows.Forms;

namespace MidiShapeShifter.Mss.Mapping
{

    /// <summary>
    ///     The MappingManager is responsible for storing, retrieving and interpreting MappingEntry objects.
    /// </summary>
    public class MappingManager
    {
        /// <summary>
        ///     List that stores all of the MappingEntry objects. The mapping list box on the main GUI is basically a 
        ///     visualization of this list.
        /// </summary>
        protected List<MappingEntry> mappingEntries = new List<MappingEntry>();

        public void AddMappingEntry(MappingEntry newEntry) 
        {
            mappingEntries.Add(newEntry);
        }


        /// <remarks>
        ///     Precondition: <paramref name="index"/> must be a valid index in the MappingManager's list of 
        ///     MappingEntry objects.
        /// </remarks>
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


        /// <remarks>
        ///     Precondition: <paramref name="index"/> must be a valid index in the MappingManager's list of 
        ///     MappingEntry objects.
        /// </remarks>
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

        /// <summary>
        ///     Moves the MappingEntry specified by <paramref name="index"/> up one element in the list.
        /// </summary>
        /// <remarks>
        ///     The order of MappingEntry objects in the list can affect which MappingEntry objects are returned by 
        ///     GetAssociatedEntries().
        ///     
        ///     Precondition: <paramref name="index"/> must be a valid index in the MappingManager's list of 
        ///     MappingEntry objects. 
        ///     
        ///     Precondition: <paramref name="index"/> cannot refer to the first element of the list as this element 
        ///     cannot move any further up.
        /// </remarks>
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

        /// <summary>
        ///     Moves the MappingEntry specified by <paramref name="index"/> down one element in the list.
        /// </summary>
        /// <remarks>
        ///     The order of <see cref="MappingEntry"/> objects in the list can affect which MappingEntry objects are 
        ///     returned by GetAssociatedEntries().
        ///     
        ///     Precondition: <paramref name="index"/> must be a valid index in the MappingManager's list of 
        ///     MappingEntry objects.  
        ///     
        ///     Precondition: <paramref name="index"/> cannot refer to the last element of the list as this element 
        ///     cannot move any further down.
        /// </remarks>
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

        /// <summary>
        ///     Creates a ListViewItem based on the MappingEntry specified by <paramref name="index"/>. This 
        ///     ListViewItem is intended to be used in the PluginEditorView's mapping list box.
        /// </summary>
        /// <returns>The ListViewItem representation of a MappingEntry</returns>
        public ListViewItem GetListViewRow(int index)
        { 
            if (index >= 0 && index < mappingEntries.Count)
            {
                MappingEntry entry = mappingEntries[index];
                ListViewItem mappingItem = new ListViewItem(entry.GetReadableMsgType(IoType.Input));
                mappingItem.SubItems.Add(entry.InMssMsgInfo.Field1);
                mappingItem.SubItems.Add(entry.InMssMsgInfo.Field2);

                mappingItem.SubItems.Add(entry.GetReadableMsgType(IoType.Output));
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

        /// <summary>
        ///     Queries the MappingEntry objects in the MappingManager and returns each one that matches InMssMsgInfo 
        ///     <paramref name="inputMsg"/>. For example, if the MappingManager contained a MappingEntry whose 
        ///     InMssMsgInfo described a range of NoteOn MIDI messages, then that MappingEntry would be returned if 
        ///     <paramref name="inputMsg"/> was a NoteOn message that fell into InMssMsgInfo's range.
        ///     
        ///     If multiple MappingEntry objects match <paramref name="inputMsg"/> and one or more has OverrideDuplicates set
        ///     to true then the one with the lowest index will be returned.
        /// </summary>
        /// <param name="inputMsg">MssMsg to query the MappingManager with.</param>
        /// <returns>An enumeration of MappingEntry objects that match <paramref name="inputMsg"/>.</returns>
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MidiShapeShifter.CSharpUtil;
using System.Runtime.Serialization;

namespace MidiShapeShifter.Mss.Mapping
{
    [DataContract]
    public abstract class GraphableMappingManager<MappingEntryType> : IGraphableMappingManager<MappingEntryType>
        where MappingEntryType : IMappingEntry
    {
        public delegate void MappingEntryAccessor(MappingEntryType genEntry);

        abstract public IEnumerable<MappingEntryType> GetCopiesOfMappingEntriesForMsg(MidiShapeShifter.Mss.MssMsg inputMsg);

        /// <summary>
        ///     A unique id will be associated with each element in mappingEntryList. This field 
        ///     keeps track of the next available id.
        /// </summary>
        [DataMember(Name = "NextId")]
        protected int nextId = 0;


        /// <summary>
        ///     List that stores all of the MappingEntry objects.
        /// </summary>
        [DataMember(Name = "MappingEntryList")]
        protected List<MappingEntryType> mappingEntryList = new List<MappingEntryType>();

        protected Object memberLock = new Object();

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            this.memberLock = new Object();
        }

        public IEnumerable<IMappingEntry> GetCopiesOfIMappingEntriesForMsg(MidiShapeShifter.Mss.MssMsg inputMsg) {
            return (IEnumerable<IMappingEntry>)GetCopiesOfMappingEntriesForMsg(inputMsg);
        }

        public IReturnStatus<IMappingEntry> GetCopyOfIMappingEntryById(int id) {
            return (IReturnStatus<IMappingEntry>)GetCopyOfMappingEntryById(id);
        }

        public IEnumerable<IMappingEntry> GetCopyOfIMappingEntryList()
        {
            return (IEnumerable<IMappingEntry>)GetCopyOfMappingEntryList();
        }

        /// <summary>
        /// Adds a copy of newEntry to this manager's list. Calling this function will also set 
        /// the unique ID corresponding to newEntry and its copy.
        /// </summary>
        /// <returns>Returns the id of the newly added entry</returns>
        public int AddMappingEntry(MappingEntryType newEntry)
        {
            lock (this.memberLock)
            {
                newEntry.Id = this.nextId;
                this.nextId++;
                //TODO: we should be adding a copy not the real thing.
                mappingEntryList.Add(newEntry);

                return newEntry.Id;
            }
        }

        /// <remarks>
        ///     Delete the entry with the specified id if it exists. Returns true if an entry was 
        ///     successfully removed.
        /// </remarks>
        public bool RemoveMappingEntry(int id)
        {
            lock (this.memberLock)
            {
                MappingEntryType mappingEntry = GetMappingEntryById(id);
                if (mappingEntry == null)
                {
                    return false;
                }
                else
                {
                    mappingEntryList.Remove(mappingEntry);
                    return true;
                }
            }
        }

        /// <summary>
        ///     Get the MappingEntry in this Mapping Manager with a the same ID
        ///     as <paramref name="id"/>. Returns null if the GeneratorMappingEntry cannot be
        ///     found
        /// </summary>
        protected MappingEntryType GetMappingEntryById(int id)
        {
            lock (this.memberLock)
            {
                return mappingEntryList.Find(entry => entry.Id == id);
            }
        }

        /// <remarks>
        ///     Gets a copy the mapping entry associated with the specified id.
        /// </remarks>
        public IReturnStatus<MappingEntryType> GetCopyOfMappingEntryById(int id)
        {
            lock (this.memberLock)
            {
                MappingEntryType mappingEntry = GetMappingEntryById(id);
                if (mappingEntry == null)
                {
                    return new ReturnStatus<MappingEntryType>();
                }
                else
                {
                    //TODO: clone mappingEntry
                    MappingEntryType copyOfEntry = mappingEntry;
                    return new ReturnStatus<MappingEntryType>(copyOfEntry, true);
                }
            }
        }

        public List<MappingEntryType> GetCopyOfMappingEntryList()
        {

            lock (this.memberLock)
            {
                //TODO: copy the contents of genMappingEntryList into entryListCopy
                List<MappingEntryType> entryListCopy = this.mappingEntryList;
                return entryListCopy;
            }

        }

        public bool RunFuncOnMappingEntry(int id, MappingEntryAccessor mappingEntryAccessor)
        {
            lock (this.memberLock)
            {
                MappingEntryType mappingEntry = GetMappingEntryById(id);

                if (mappingEntry == null)
                {
                    return false;
                }
                else
                {
                    mappingEntryAccessor(mappingEntry);
                    return true;
                }
            }
        }

        /// <summary>
        /// Returns the index of the Mapping Entry specified by id or -1 if the id was not
        /// found.
        /// </summary>
        public int GetIndexById(int id)
        {
            lock (this.memberLock)
            {
                return mappingEntryList.FindIndex(entry => entry.Id == id);
            }
        }

        public int GetNumEntries() {
            lock (this.memberLock)
            {
                return mappingEntryList.Count;
            }
        }


    }
}

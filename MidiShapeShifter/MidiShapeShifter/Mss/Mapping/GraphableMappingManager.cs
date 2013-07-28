using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MidiShapeShifter.CSharpUtil;
using System.Runtime.Serialization;
using MidiShapeShifter.Mss.Generator;
using System.Threading;

namespace MidiShapeShifter.Mss.Mapping
{
    public delegate void MappingEntryAccessor<in ParamEntryType>(ParamEntryType genEntry);

    //For an explination of why we must specify that MappingEntryType is a class see this post:
    //http://stackoverflow.com/questions/13813347
    [DataContract]
    public abstract class GraphableMappingManager<MappingEntryType> : IGraphableMappingManager<MappingEntryType>
        where MappingEntryType : class, IMappingEntry
    {

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

        [OnSerializing]
        protected void OnSerializing(StreamingContext context)
        {
            Monitor.Enter(this.memberLock);
        }

        [OnSerialized]
        protected void OnSerialized(StreamingContext context)
        {
            Monitor.Exit(this.memberLock);
        }

        [OnDeserialized]
        protected void OnDeserialized(StreamingContext context)
        {
            this.memberLock = new Object();
        }


        IEnumerable<IMappingEntry> IBaseGraphableMappingManager.GetCopiesOfMappingEntriesForMsg(MidiShapeShifter.Mss.MssMsg inputMsg) {
            return (IEnumerable<IMappingEntry>)GetCopiesOfMappingEntriesForMsg(inputMsg);
        }

        IReturnStatus<IMappingEntry> IBaseGraphableMappingManager.GetCopyOfMappingEntryById(int id)
        {
            return (IReturnStatus<IMappingEntry>)GetCopyOfMappingEntryById(id);
        }

        IEnumerable<IMappingEntry> IBaseGraphableMappingManager.GetCopyOfMappingEntryList()
        {
            return (IEnumerable<IMappingEntry>)GetCopyOfMappingEntryList();
        }

        bool IBaseGraphableMappingManager.RunFuncOnMappingEntry(int id, MappingEntryAccessor<IMappingEntry> mappingEntryAccessor)
        {
            return RunFuncOnMappingEntry(id, mappingEntryAccessor as MappingEntryAccessor<MappingEntryType>);
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
                mappingEntryList.Add((MappingEntryType)newEntry.Clone());

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

        /// <summary>
        /// Returns the index of the Mapping Entry specified by id or -1 if the id was not
        /// found.
        /// </summary>
        public int GetMappingEntryIndexById(int id)
        {
            lock (this.memberLock)
            {
                return mappingEntryList.FindIndex(entry => entry.Id == id);
            }
        }

        public int GetMappingEntryIdByIndex(int index) {
            lock (this.memberLock)
            {
                return mappingEntryList[index].Id;
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
                    MappingEntryType copyOfEntry = (MappingEntryType)mappingEntry.Clone();
                    return new ReturnStatus<MappingEntryType>(copyOfEntry, true);
                }
            }
        }

        public List<MappingEntryType> GetCopyOfMappingEntryList()
        {

            lock (this.memberLock)
            {
                List<MappingEntryType> entryListCopy = this.mappingEntryList.Clone();
                return entryListCopy;
            }

        }

        public bool RunFuncOnMappingEntry(int id, MappingEntryAccessor<MappingEntryType> mappingEntryAccessor)
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

        public int GetNumEntries() {
            lock (this.memberLock)
            {
                return mappingEntryList.Count;
            }
        }




        public IReturnStatus<CurveShapeInfo> GetCopyOfCurveShapeInfoById(int id)
        {
            lock (this.memberLock)
            {
                MappingEntryType matchingEntry = GetMappingEntryById(id);
                if (matchingEntry == null)
                {
                    return new ReturnStatus<CurveShapeInfo>();
                }
                else
                {
                    CurveShapeInfo curveInfoClone = matchingEntry.CurveShapeInfo.Clone();
                    return new ReturnStatus<CurveShapeInfo>(curveInfoClone);
                }
            }
        }

        public bool ReplaceMappingEntry(MappingEntryType mappingEntry)
        {
            lock (this.memberLock)
            {
                int index = GetMappingEntryIndexById(mappingEntry.Id);
                if (index != -1)
                {
                    this.mappingEntryList[index] = (MappingEntryType)mappingEntry.Clone();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<int> GetEntryIdList() {
            List<int> entryIdList = new List<int>();

            lock (this.memberLock) {
                foreach (MappingEntryType entry in this.mappingEntryList) {
                    entryIdList.Add(entry.Id);
                }
            }

            return entryIdList;
        }


    }
}

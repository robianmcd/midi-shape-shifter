using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

using MidiShapeShifter.Mss.Mapping;
using MidiShapeShifter.Mss.MssMsgInfoTypes;
using System.Runtime.Serialization;

namespace MidiShapeShifter.Mss.Generator
{
    /// <summary>
    ///     The GeneratorMappingManager is responsible for storing, retrieving and interpreting 
    ///     GeneratorMappingEntry objects.
    /// </summary>
    [DataContract]
    public class GeneratorMappingManager : IGeneratorMappingManager
    {

        /// <summary>
        /// Each GeneratorMappingEntry has a unique ID. nextGenId keeps track of the next 
        /// available unique ID.
        /// </summary>
        [DataMember(Name = "NextGenId")]
        protected int nextGenId = 0;

        /// <summary>
        ///     List that stores all of the GeneratorMappingEntry objects. Each instance of 
        ///     GeneratorMappingEntry in this list corresponds to a row in the generator list view
        ///     on the PluginEditorView dialog.
        /// </summary>
        [DataMember(Name = "GenMappingEntryList")]
        protected List<IGeneratorMappingEntry> genMappingEntryList = new List<IGeneratorMappingEntry>();

        /// <summary>
        /// Adds newEntry to this manager's list. Calling this function will also initialize 
        /// the unique ID corresponding to newEntry
        /// </summary>
        public void AddGenMappingEntry(IGeneratorMappingEntry newEntry)
        {
            newEntry.GenConfigInfo.Id = this.nextGenId;
            this.nextGenId++;
            genMappingEntryList.Add(newEntry);
        }

        /// <summary>
        /// Creates a new GeneratorMappingEntry based on the info in genInfo. The newly created 
        /// GeneratorMappingEntry will be stored in this GeneratorMappingManager.
        /// </summary>
        public void CreateAndAddEntryFromGenInfo(GenEntryConfigInfo genInfo)
        {
            genInfo.Id = this.nextGenId;
            this.nextGenId++;

            IGeneratorMappingEntry mappingEntry = new GeneratorMappingEntry();

            InitializeEntryFromGenInfo(genInfo, mappingEntry);

            //Add new mapping entry to list
            genMappingEntryList.Add(mappingEntry);
        }

        /// <summary>
        /// Regererates an existing GeneratorMappingEntry based on genInfo. genInfo's ID must be
        /// the same as an ID in this GeneratorMappingManager.
        /// </summary>
        public void UpdateEntryWithNewGenInfo(GenEntryConfigInfo genInfo)
        {
            IGeneratorMappingEntry mappingEntry = GetGenMappingEntryById(genInfo.Id);

            InitializeEntryFromGenInfo(genInfo, mappingEntry);
        }

        /// <summary>
        /// populate mappingEntry's members based on the information in genInfo
        /// </summary>
        protected void InitializeEntryFromGenInfo(GenEntryConfigInfo genInfo, 
                                                  IGeneratorMappingEntry mappingEntry)
        {
            //Sets mappingEntry.GeneratorInfo
            mappingEntry.GenConfigInfo = genInfo;

            //Sets mappingEntry.inMsgRange
            IMssMsgRange inMsgRange = new MssMsgRange();

            if (genInfo.PeriodType == GenPeriodType.BeatSynced)
            {
                inMsgRange.InitPublicMembers(MssMsgType.RelBarPeriodPos,
                                          genInfo.Id,
                                          MssMsgUtil.UNUSED_MSS_MSG_DATA);
            }
            else if (genInfo.PeriodType == GenPeriodType.Time)
            {
                inMsgRange.InitPublicMembers(MssMsgType.RelTimePeriodPos,
                                          genInfo.Id,
                                          MssMsgUtil.UNUSED_MSS_MSG_DATA);
            }
            else
            {
                //Unknown period type
                Debug.Assert(false);
            }


            mappingEntry.InMssMsgRange = inMsgRange;

            //Sets mappingEntry.outMsgRange
            IMssMsgRange outMsgRange = new MssMsgRange();
            outMsgRange.InitPublicMembers(MssMsgType.Generator,
                                       genInfo.Id,
                                       MssMsgUtil.UNUSED_MSS_MSG_DATA);

            mappingEntry.OutMssMsgRange = outMsgRange;

            //Sets mappingEntry.OverrideDuplicates
            mappingEntry.OverrideDuplicates = false;

            //Sets mappingEntry.CurveShapeInfo. This function is also used to reinitialize 
            //mappingEntry so sometimes CurveShapeInfo will already be initialized.
            if (mappingEntry.CurveShapeInfo == null)
            {
                mappingEntry.CurveShapeInfo = new CurveShapeInfo();
                mappingEntry.CurveShapeInfo.InitWithDefaultValues();
            }
            
        }

        /// <remarks>
        ///     Precondition: <paramref name="index"/> must be a valid index in the 
        ///     GeneratorMappingManager's list of GeneratorMappingEntry objects.
        /// </remarks>
        public void RemoveGenMappingEntry(int index)
        {
            if (index >= 0 && index < genMappingEntryList.Count)
            {
                genMappingEntryList.RemoveAt(index);
            }
            else
            {
                //invalid index
                Debug.Assert(false);
            }
        }

        /// <summary>
        ///     Creates a ListViewItem based on the GeneratorMappingEntry specified by 
        ///     <paramref name="index"/>. This ListViewItem is intended to be used in the 
        ///     PluginEditorView's generator list box.
        /// </summary>
        /// <returns>The ListViewItem representation of a GeneratorMappingEntry</returns>
        public ListViewItem GetListViewRow(int index)
        {
            if (index >= 0 && index < genMappingEntryList.Count)
            {
                IGeneratorMappingEntry entry = genMappingEntryList[index];
                ListViewItem genMappingItem = new ListViewItem(entry.GenConfigInfo.Name);
                genMappingItem.SubItems.Add(entry.GetReadablePeriod());
                genMappingItem.SubItems.Add(entry.GetReadableLoopStatus());
                genMappingItem.SubItems.Add(entry.GetReadableEnabledStatus());

                return genMappingItem;
            }
            else
            {
                //invalid index
                Debug.Assert(false);
                return null;
            }
        }


        /// <remarks>
        ///     Precondition: <paramref name="index"/> must be a valid index in the 
        ///     GeneratorMappingManager's list of GeneratorMappingEntry objects.
        /// </remarks>
        public IGeneratorMappingEntry GetGenMappingEntryByIndex(int index)
        {
            if (index >= 0 && index < genMappingEntryList.Count)
            {
                return genMappingEntryList[index];
            }
            else
            {
                //invalid index
                Debug.Assert(false);
                return null;
            }
        }

        /// <summary>
        ///     Get the GeneratorMappingEntry in this GeneratorMappingManager with a the same ID
        ///     as <paramref name="id"/>. Returns null if the GeneratorMappingEntry cannot be
        ///     found
        /// </summary>
        public IGeneratorMappingEntry GetGenMappingEntryById(int id)
        { 
            return genMappingEntryList.Find(entry => entry.GenConfigInfo.Id == id);
        }

        public int GetNumEntries()
        {
            return genMappingEntryList.Count;
        }

        /// <summary>
        /// Returns the index of the GeneratorMappingEntry specified by id or -1 if the id was not
        /// found.
        /// </summary>
        public int GetIndexById(int id)
        {
            return genMappingEntryList.FindIndex(entry => entry.GenConfigInfo.Id == id);
        }

        /// <summary>
        /// Returns a list of MappingEntries from the GeneratorMappingEntries stored in this 
        /// GeneratorMappingManager. A GeneratorMappingEntry will be in this list if 
        /// <paramref name="inputMsg"/> falls into it's input range. Due the the nature of the 
        /// GeneratorMappingEntries stored in this GeneratorMappingManager, the returned list will 
        /// only ever contain a maximum of one element. This is because a GeneratorMappingEntry's
        /// input range will only accept one message that has a unique id.
        /// </summary>
        public IEnumerable<IMappingEntry> GetAssociatedEntries(MssMsg inputMsg)
        {
            List<IMappingEntry> associatedEntryList = new List<IMappingEntry>();
            if (inputMsg.Type == MssMsgType.RelBarPeriodPos || inputMsg.Type == MssMsgType.RelTimePeriodPos)
            {
                IGeneratorMappingEntry associatedEntry = 
                        GetGenMappingEntryById((int)inputMsg.Data1);
                if (associatedEntry != null)
                {
                    associatedEntryList.Add(associatedEntry);
                }
            }

            return associatedEntryList;
        }

        public IMappingEntry GetMappingEntry(int index)
        {
            return GetGenMappingEntryByIndex(index);
        }
    }
}

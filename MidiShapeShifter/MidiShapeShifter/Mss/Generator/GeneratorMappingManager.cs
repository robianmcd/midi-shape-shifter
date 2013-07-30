using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

using MidiShapeShifter.Mss.Mapping;
using MidiShapeShifter.Mss.MssMsgInfoTypes;
using System.Runtime.Serialization;
using MidiShapeShifter.CSharpUtil;

namespace MidiShapeShifter.Mss.Generator
{

    /// <summary>
    ///     The GeneratorMappingManager is responsible for storing, retrieving and interpreting 
    ///     GeneratorMappingEntry objects.
    /// </summary>
    [DataContract]
    public class GeneratorMappingManager : GraphableMappingManager<IGeneratorMappingEntry>, IGeneratorMappingManager
    {
        /// <summary>
        /// Creates a new GeneratorMappingEntry based on the info in genInfo. The newly created 
        /// GeneratorMappingEntry will be stored in this GeneratorMappingManager.
        /// <returns>Returns the id of the newly created entry.</returns>
        /// </summary>
        public int CreateAndAddEntryFromGenInfo(GenEntryConfigInfo genInfo)
        {
            lock (MssComponentHub.criticalSectioinLock)
            {
                IGeneratorMappingEntry mappingEntry = new GeneratorMappingEntry();
                int curId = this.nextId;
                this.nextId++;

                InitializeEntryFromGenInfo(genInfo, curId, mappingEntry);

                //Add new mapping entry to list
                this.mappingEntryList.Add(mappingEntry);

                return mappingEntry.Id;
            }
        }

        /// <summary>
        /// Regererates an existing GeneratorMappingEntry based on genInfo. genInfo's ID must be
        /// the same as an ID in this GeneratorMappingManager.
        /// </summary>
        public bool UpdateEntryWithNewGenInfo(GenEntryConfigInfo genInfo, int id)
        {
            lock (MssComponentHub.criticalSectioinLock)
            {
                IGeneratorMappingEntry mappingEntry = GetMappingEntryById(id);
                if (mappingEntry == null){
                    return false;
                }
                else {
                    InitializeEntryFromGenInfo(genInfo, id, mappingEntry);
                    return true;
                }
            }
        }

        /// <summary>
        /// populate mappingEntry's members based on the information in genInfo
        /// </summary>
        protected void InitializeEntryFromGenInfo(GenEntryConfigInfo genInfo, int id,
                                                  IGeneratorMappingEntry mappingEntry)
        {
            //Sets mappingEntry.Id
            mappingEntry.Id = id;

            //Sets mappingEntry.GeneratorInfo
            mappingEntry.GenConfigInfo = genInfo;

            //Sets mappingEntry.inMsgRange
            IMssMsgRange inMsgRange = new MssMsgRange();

            switch (genInfo.PeriodType)
            {
                case GenPeriodType.Bars:
                case GenPeriodType.BeatSynced:
                    inMsgRange.InitPublicMembers(MssMsgType.RelBarPeriodPos,
                                          id,
                                          MssMsgUtil.UNUSED_MSS_MSG_DATA);
                    break;

                case GenPeriodType.Time:
                    inMsgRange.InitPublicMembers(MssMsgType.RelTimePeriodPos,
                                          id,
                                          MssMsgUtil.UNUSED_MSS_MSG_DATA);
                    break;

                default:
                    //Unknown period type
                    Debug.Assert(false);
                    break;
            }

            mappingEntry.InMssMsgRange = inMsgRange;

            //Sets mappingEntry.outMsgRange
            IMssMsgRange outMsgRange = new MssMsgRange();
            outMsgRange.InitPublicMembers(MssMsgType.Generator,
                                       id,
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

        /// <summary>
        /// Returns a list of MappingEntries from the GeneratorMappingEntries stored in this 
        /// GeneratorMappingManager. A GeneratorMappingEntry will be in this list if 
        /// <paramref name="inputMsg"/> falls into it's input range. Due the the nature of the 
        /// GeneratorMappingEntries stored in this GeneratorMappingManager, the returned list will 
        /// only ever contain a maximum of one element. This is because a GeneratorMappingEntry's
        /// input range will only accept one message that has a unique id.
        /// </summary>
        public override IEnumerable<IGeneratorMappingEntry> GetCopiesOfMappingEntriesForMsg(MssMsg inputMsg)
        {
            lock (MssComponentHub.criticalSectioinLock)
            {
                List<IGeneratorMappingEntry> associatedEntryList = new List<IGeneratorMappingEntry>();
                if (inputMsg.Type == MssMsgType.RelBarPeriodPos || inputMsg.Type == MssMsgType.RelTimePeriodPos)
                {
                    IReturnStatus<IGeneratorMappingEntry> associatedEntryCopy = 
                            GetCopyOfMappingEntryById((int)inputMsg.Data1);
                    if (associatedEntryCopy.IsValid)
                    {
                        associatedEntryList.Add(associatedEntryCopy.Value);
                    }
                }

                return associatedEntryList;
            }
        }

    }
}

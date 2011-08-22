using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

using MidiShapeShifter.Mss.Mapping;

namespace MidiShapeShifter.Mss.Generator
{
    // TODO: comment this calss
    public class GeneratorMappingManager
    {

        protected int nextGenId = 0;

        protected List<GeneratorMappingEntry> genMappingEntryList = new List<GeneratorMappingEntry>();

        public void AddGenMappingEntry(GeneratorMappingEntry newEntry)
        {
            newEntry.GeneratorInfo.Id = this.nextGenId;
            this.nextGenId++;
            genMappingEntryList.Add(newEntry);
        }

        public void CreateAndAddEntryFromGenInfo(GeneratorMappingEntryInfo genInfo)
        {
            genInfo.Id = this.nextGenId;
            this.nextGenId++;

            GeneratorMappingEntry mappingEntry = new GeneratorMappingEntry();

            InitializeEntryFromGenInfo(genInfo, mappingEntry);

            //Add new mapping entry to list
            genMappingEntryList.Add(mappingEntry);
        }

        //Precondition: GenInfo's ID must correspond to an existing entry in genMappingEntryList.
        public void UpdateEntryWithNewGenInfo(GeneratorMappingEntryInfo genInfo)
        {
            GeneratorMappingEntry mappingEntry = GetGenMappingEntryById(genInfo.Id);

            InitializeEntryFromGenInfo(genInfo, mappingEntry);
        }

        protected void InitializeEntryFromGenInfo(GeneratorMappingEntryInfo genInfo, GeneratorMappingEntry mappingEntry)
        {
            //Set mappingEntry.GeneratorInfo
            mappingEntry.GeneratorInfo = genInfo;

            //Set mappingEntry.inMsgRange
            MssMsgRange inMsgRange = new MssMsgRange();
            if (genInfo.PeriodType == GenPeriodType.Beats)
            {
                inMsgRange.InitAllMembers(MssMsgType.RelBarPeriodPos,
                                          MssMsgUtil.UNUSED_MSS_MSG_DATA,
                                          MssMsgUtil.UNUSED_MSS_MSG_DATA);
            }
            else if (genInfo.PeriodType == GenPeriodType.Time)
            {
                inMsgRange.InitAllMembers(MssMsgType.RelTimePeriodPos,
                                          MssMsgUtil.UNUSED_MSS_MSG_DATA,
                                          MssMsgUtil.UNUSED_MSS_MSG_DATA);
            }
            else
            {
                //Unknown period type
                Debug.Assert(false);
            }


            mappingEntry.InMssMsgRange = inMsgRange;

            //Set mappingEntry.outMsgRange
            MssMsgRange outMsgRange = new MssMsgRange();
            outMsgRange.InitAllMembers(MssMsgType.Generator,
                                       genInfo.Id,
                                       MssMsgUtil.UNUSED_MSS_MSG_DATA);

            mappingEntry.OutMssMsgRange = outMsgRange;

            //Set mappingEntry.OverrideDuplicates
            mappingEntry.OverrideDuplicates = false;

            //Set mappingEntry.CurveShapeInfo
            CurveShapeInfo curveInfo = new CurveShapeInfo();
            curveInfo.InitWithDefaultValues();
            mappingEntry.CurveShapeInfo = curveInfo;
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
                GeneratorMappingEntry entry = genMappingEntryList[index];
                ListViewItem genMappingItem = new ListViewItem(entry.GeneratorInfo.Name);
                genMappingItem.SubItems.Add(entry.GetReadablePeriod());
                genMappingItem.SubItems.Add(entry.GetReadableLoopStatus());
                genMappingItem.SubItems.Add(entry.GetReadableIsGeneratingStatus());

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
        public GeneratorMappingEntry GetGenMappingEntryByIndex(int index)
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

        public GeneratorMappingEntry GetGenMappingEntryById(int id)
        { 
            return genMappingEntryList.Find(entry => entry.GeneratorInfo.Id == id);
        }

        public int GetNumEntries()
        {
            return genMappingEntryList.Count;
        }
    }
}

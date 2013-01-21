using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using MidiShapeShifter.Mss.Generator;
using System.Diagnostics;
using MidiShapeShifter.CSharpUtil;

namespace MidiShapeShifter.Mss.Mapping
{
    public enum GraphableEntryType { Mapping, Generator }

    [DataContract]
    public class ActiveMappingInfo
    {
        [DataMember]
        public int ActiveGraphableEntryId = -1;
        [DataMember]
        public GraphableEntryType ActiveGraphableEntryType;

        protected IMappingManager mappingManager;
        protected IGeneratorMappingManager genMappingManager;

        public void InitNonserializableMembers(IMappingManager mappingMgr, 
                         IGeneratorMappingManager genMappingMgr) 
        {
            this.mappingManager = mappingMgr;
            this.genMappingManager = genMappingMgr;
        }


        /// <summary>
        /// Gets the active mapping entry. The active mapping entry is the one the user has most 
        /// recently selected in the PluginEditorView. This method is similar to the 
        /// ActiveGraphableEntry property in PluginEditorView.
        /// </summary>
        public IMappingEntry GetActiveMappingCopy()
        {
            if (this.ActiveGraphableEntryId < 0)
            {
                return null;
            }
            else
            {
                IMappingEntry activeEntry;
                IBaseGraphableMappingManager activeEntryManager;
                if (this.ActiveGraphableEntryType == GraphableEntryType.Mapping)
                {
                    activeEntryManager = this.mappingManager;
                }
                else if (this.ActiveGraphableEntryType == GraphableEntryType.Generator)
                {
                    activeEntryManager = this.genMappingManager;
                }
                else
                {
                    //Unknown MappingType
                    Debug.Assert(false);
                    return null;
                }

                IReturnStatus<IMappingEntry> getCopyRetStatus =
                        activeEntryManager.GetCopyOfMappingEntryById(this.ActiveGraphableEntryId);
                Debug.Assert(getCopyRetStatus.IsValid);

                activeEntry = getCopyRetStatus.Value;

                return activeEntry;
            }
        }

        public bool GetActiveMappingExists() {
            return this.ActiveGraphableEntryId >= 0;
        }

        public IBaseGraphableMappingManager GetActiveGraphableEntryManager()
        {
            IBaseGraphableMappingManager activeEntryManager;

            if (this.ActiveGraphableEntryType == GraphableEntryType.Mapping)
            {
                activeEntryManager = this.mappingManager;
            }
            else if (this.ActiveGraphableEntryType == GraphableEntryType.Generator)
            {
                activeEntryManager = this.genMappingManager;
            }
            else
            {
                //Unknown MappingType
                Debug.Assert(false);
                return null;
            }

            return activeEntryManager;
        }

        public IMappingManager GetActiveMappingManager()
        {
            if (this.ActiveGraphableEntryId < 0 || this.ActiveGraphableEntryType != GraphableEntryType.Mapping)
            {
                Debug.Assert(false);
                return null;
            }
            else
            {
                return this.mappingManager;
            }
        }

        public IGeneratorMappingManager GetActiveGeneratorMappingManager()
        {
            if (this.ActiveGraphableEntryId < 0 || this.ActiveGraphableEntryType != GraphableEntryType.Generator)
            {
                Debug.Assert(false);
                return null;
            }
            else
            {
                return this.genMappingManager;
            }
        }

    }
}

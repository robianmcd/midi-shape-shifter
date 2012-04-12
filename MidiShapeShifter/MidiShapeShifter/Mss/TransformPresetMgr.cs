using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;

using MidiShapeShifter.CSharpUtil;
using MidiShapeShifter.Mss.Mapping;
using MidiShapeShifter.Mss.UI;
using MidiShapeShifter.Mss.Generator;

namespace MidiShapeShifter.Mss
{
    /// <summary>
    /// The class keeps track of available transformation preset. The active transformation
    /// preset indicated by this class is just a reference to the ActiveTransformPreset which
    /// is stored in the CurveShapeInfo for the active mapping entry. For more info see
    /// BaseSettingsFileMgr.cs.
    /// </summary>
    public class TransformPresetMgr : BaseSettingsFileMgr
    {
        public const string TRANSFORM_PRESET_FILE_EXTENSION = "tpst";

        SerializablePluginEditorInfo pluginEditorInfo;
        MappingManager mappingMgr;
        GeneratorMappingManager genMappingMgr;

        public TransformPresetMgr()
        {
            ConstructNonSerializableMembers();
        }

        /// <summary>
        /// Get the active transformation preset from the active mapping.
        /// </summary>
        public override SettingsFileInfo ActiveSettingsFile
        {
            get
            {
                if (getActiveMapping() != null)
                {
                    return getActiveMapping().CurveShapeInfo.ActiveTransformPreset;
                }
                else
                {
                    return null;
                }
            }
            protected set
            {
                if (getActiveMapping() != null)
                {
                    getActiveMapping().CurveShapeInfo.ActiveTransformPreset = value;
                }
                else
                {
                    //It doesn't make sense to set this when the active mapping is null.
                    Debug.Assert(false);
                }
            }
        }

        public void Init(SerializablePluginEditorInfo pluginEditorInfo, 
                         MappingManager mappingMgr, 
                         GeneratorMappingManager generatorMappingMgr)
        {
            this.pluginEditorInfo = pluginEditorInfo;
            this.mappingMgr = mappingMgr;
            this.genMappingMgr = generatorMappingMgr;

            InitBase();
        }

        /// <summary>
        /// Gets the active mapping entry. The active mapping entry is the one the user has most 
        /// recently selected in the PluginEditorView. This method is similar to the 
        /// ActiveGraphableEntry property in PluginEditorView.
        /// </summary>
        protected IMappingEntry getActiveMapping()
        {
            if (this.pluginEditorInfo.activeGraphableEntryIndex < 0)
            {
                return null;
            }
            else
            {
                IMappingEntry activeEntry;

                if (this.pluginEditorInfo.activeGraphableEntryType == GraphableEntryType.Mapping)
                {
                    activeEntry = this.mappingMgr.GetMappingEntry(
                            this.pluginEditorInfo.activeGraphableEntryIndex);
                }
                else if (this.pluginEditorInfo.activeGraphableEntryType == GraphableEntryType.Generator)
                {
                    activeEntry = this.genMappingMgr.GetGenMappingEntryByIndex(
                            this.pluginEditorInfo.activeGraphableEntryIndex);
                }
                else
                {
                    //Unknown MappingType
                    Debug.Assert(false);
                    return null;
                }

                return activeEntry;
            }
        }

        public override string SettingsFileExtension
        {
            get { return TRANSFORM_PRESET_FILE_EXTENSION; }
        }

        public override string TypeOfSettingsName
        {
            get { return "Tramsformation Preset"; }
        }

        public override string RootFolderForFactorySettings
        {
            get { return MssFileSystemLocations.FactoryTransformPresetFolder; }
        }

        public override string RootFolderForUserSettings
        {
            get { return MssFileSystemLocations.UserTransformPresetFolder; }
        }

        protected override bool EnforceUniqueSettingsFileNames
        {
            get { return false; }
        }

        protected override SettingsFileInfo CreateDefaultActiveSettingsFileInfo()
        {
            return CreateDefaultTransformPresetFileInfo();
        }

        public static SettingsFileInfo CreateDefaultTransformPresetFileInfo()
        {
            SettingsFileInfo defaultActiveProgram = new SettingsFileInfo();
            //Sets the default program
            defaultActiveProgram.Init(MssFileSystemLocations.FactoryProgramsFolder + "Line." +
                                      TRANSFORM_PRESET_FILE_EXTENSION);

            return defaultActiveProgram;
        }

        protected override void SaveActiveSettingsToFileStream(System.IO.FileStream fs)
        {
            if (getActiveMapping() == null)
            {
                //If the active mapping is null then the buttons that would trigger this call 
                //should be disabled.
                Debug.Assert(false);
                return;
            }

            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(fs, getActiveMapping().CurveShapeInfo);
        }

        protected override void LoadSettingsFromFileStream(System.IO.FileStream fs)
        {
            if (getActiveMapping() == null)
            {
                //If the active mapping is null then the buttons that would trigger this call 
                //should be disabled.
                Debug.Assert(false);
                return;
            }

            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Binder = new DeserializationBinderForPlugins();
            getActiveMapping().CurveShapeInfo = (CurveShapeInfo)formatter.Deserialize(fs);
        }

    }
}

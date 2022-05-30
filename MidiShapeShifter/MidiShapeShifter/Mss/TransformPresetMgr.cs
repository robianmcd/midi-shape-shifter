using MidiShapeShifter.CSharpUtil;
using MidiShapeShifter.Mss.Mapping;
using System.Runtime.Serialization;

namespace MidiShapeShifter.Mss
{
    /// <summary>
    /// The class keeps track of available transformation preset. The active transformation
    /// preset indicated by this class is just a reference to the ActiveTransformPreset which
    /// is stored in the CurveShapeInfo for the active mapping entry. For more info see
    /// BaseSettingsFileMgr.cs.
    /// </summary>
    [DataContract]
    public class TransformPresetMgr : BaseSettingsFileMgr
    {
        public const string TRANSFORM_PRESET_FILE_EXTENSION = "msst";
        public const string DEFAULT_TRANSFORM_PRESET_NAME = "Line";
        protected ActiveMappingInfo activeMappingInfo;

        public TransformPresetMgr()
        {
            ConstructNonSerializableMembers();
        }

        public override string ActiveSettingsFileName
        {
            get
            {
                if (this.activeMappingInfo.GetActiveMappingExists() == false)
                {
                    return null;
                }

                IBaseGraphableMappingManager activeMappingManager = activeMappingInfo.GetActiveGraphableEntryManager();

                string activeSettingsFileName = null;
                activeMappingManager.RunFuncOnMappingEntry(activeMappingInfo.ActiveGraphableEntryId,
                        (mappingEntry) => activeSettingsFileName = mappingEntry.ActiveTransformPresetName);

                return activeSettingsFileName;
            }
            protected set
            {
                IBaseGraphableMappingManager activeMappingManager = activeMappingInfo.GetActiveGraphableEntryManager();

                activeMappingManager.RunFuncOnMappingEntry(activeMappingInfo.ActiveGraphableEntryId,
                        (mappingEntry) => mappingEntry.ActiveTransformPresetName = value);
            }
        }

        public void Init(ActiveMappingInfo activeMappingInfo)
        {
            this.activeMappingInfo = activeMappingInfo;

            InitBase();
        }

        public override string SettingsFileExtension => TRANSFORM_PRESET_FILE_EXTENSION;

        public override string TypeOfSettingsName => "Tramsformation Preset";

        public override string RootFolderForFactorySettings => MssFileSystemLocations.FactoryTransformPresetFolder;

        public override string RootFolderForUserSettings => MssFileSystemLocations.UserTransformPresetFolder;

        protected override bool EnforceUniqueSettingsFileNames => false;

        protected override SettingsFileInfo CreateDefaultActiveSettingsFileInfo()
        {
            return CreateDefaultTransformPresetFileInfo();
        }

        public static SettingsFileInfo CreateDefaultTransformPresetFileInfo()
        {
            SettingsFileInfo defaultActiveProgram = new SettingsFileInfo();
            //Sets the default program
            defaultActiveProgram.Init(MssFileSystemLocations.FactoryProgramsFolder + DEFAULT_TRANSFORM_PRESET_NAME +
                                      "." + TRANSFORM_PRESET_FILE_EXTENSION);

            return defaultActiveProgram;
        }

        protected override void SaveActiveSettingsToFileStream(System.IO.FileStream fs)
        {
            IBaseGraphableMappingManager activeMappingManager = activeMappingInfo.GetActiveGraphableEntryManager();

            activeMappingManager.RunFuncOnMappingEntry(activeMappingInfo.ActiveGraphableEntryId, (mappingEntry) =>
                    ContractSerializer.Serialize<CurveShapeInfo>(fs, mappingEntry.CurveShapeInfo));
        }

        protected override void LoadSettingsFromFileStream(System.IO.FileStream fs)
        {
            IBaseGraphableMappingManager activeMappingManager = activeMappingInfo.GetActiveGraphableEntryManager();

            activeMappingManager.RunFuncOnMappingEntry(activeMappingInfo.ActiveGraphableEntryId, (mappingEntry) =>
                    mappingEntry.CurveShapeInfo = ContractSerializer.Deserialize<CurveShapeInfo>(fs));
        }

    }
}

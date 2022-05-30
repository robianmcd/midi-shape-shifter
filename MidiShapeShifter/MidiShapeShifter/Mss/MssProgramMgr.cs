using System.IO;
using System.Runtime.Serialization;

namespace MidiShapeShifter.Mss
{

    public delegate void SaveProgramRequestEventHandler(Stream saveLocation);
    public delegate void LoadProgramRequestEventHandler(Stream loadLocation);
    public delegate void ActiveProgramChangedEventHandler(string programName);

    /// <summary>
    /// MssProgramMgr is responsible for storing the programs available in the plugin. The list of
    /// programs is made up of factory programs and user programs. Factory program are distributed
    /// with the Midi Shape Shifter installer and user programs can be created by the user. This
    /// Class should be used by other classes to coordinate program information with the host and 
    /// to set the active program. See BaseSettingsFileMgr for more info.
    /// </summary>
    [DataContract]
    public class MssProgramMgr : BaseSettingsFileMgr
    {
        public const string DEFAULT_PROGRAM_NAME = "Blank";

        /// <summary>
        /// SaveProgramRequest is sent out when the active program should be saved to the specified
        /// stream.
        /// </summary>
        public event SaveProgramRequestEventHandler SaveProgramRequest;

        /// <summary>
        /// SaveProgramRequest is sent out when the active program should be loaded from the 
        /// specified stream.
        /// </summary>
        public event LoadProgramRequestEventHandler LoadProgramRequest;

        /// <summary>
        /// ActiveProgramChanged will be raised when MssProgramMgr's active program changes. This 
        /// should be used to coordinate the active program shown in the plugin's GUI and the 
        /// active program shown in the host.
        /// </summary>
        public event ActiveProgramChangedEventHandler ActiveProgramChanged;


        [DataMember(Name = "ActiveSettingsFileName")]
        protected string _activeSettingsFileName;
        public override string ActiveSettingsFileName
        {
            get { return this._activeSettingsFileName; }
            protected set { this._activeSettingsFileName = value; }
        }


        public MssProgramMgr()
        {
            ConstructNonSerializableMembers();
        }

        public void Init()
        {
            //Sets the default program
            this.ActiveSettingsFileName = DEFAULT_PROGRAM_NAME;

            InitBase();
        }

        public override string SettingsFileExtension
        {
            get { return "mssp"; }
        }

        public override string TypeOfSettingsName
        {
            get { return "MSS Program"; }
        }

        public override string RootFolderForFactorySettings
        {
            get { return MssFileSystemLocations.FactoryProgramsFolder; }
        }

        public override string RootFolderForUserSettings
        {
            get { return MssFileSystemLocations.UserProgramsFolder; }
        }

        protected override bool EnforceUniqueSettingsFileNames
        {
            get { return true; }
        }

        protected override SettingsFileInfo CreateDefaultActiveSettingsFileInfo()
        {
            SettingsFileInfo defaultActiveProgram = new SettingsFileInfo();
            //Sets the default program
            defaultActiveProgram.Init(MssFileSystemLocations.FactoryProgramsFolder + "Blank." +
                                        this.SettingsFileExtension);

            return defaultActiveProgram;
        }

        protected override void SaveActiveSettingsToFileStream(FileStream fs)
        {
            if (this.SaveProgramRequest != null)
            {
                this.SaveProgramRequest(fs);
            }
        }

        protected override void LoadSettingsFromFileStream(FileStream fs)
        {
            //We don't need to set this.ActiveSettingsFile because it will be deserialized when the
            //new instance of MssComponentHub is loaded

            if (this.LoadProgramRequest != null)
            {
                this.LoadProgramRequest(fs);
            }
        }

        protected override void OnActiveSettingsFileChanged()
        {
            if (this.ActiveProgramChanged != null)
            {
                this.ActiveProgramChanged(this.ActiveSettingsFileName);
            }
        }


    }
}

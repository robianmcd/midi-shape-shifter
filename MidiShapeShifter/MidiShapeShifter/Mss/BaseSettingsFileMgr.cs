using MidiShapeShifter.CSharpUtil;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace MidiShapeShifter.Mss
{
    /// <summary>
    /// Keeps track of available settings such as programs or transformation presets. The type of
    /// settings will be determined by the class implimenting BaseSettingsFileMgr. This class will 
    /// search the file system to create a collection of available settings files. This
    /// collection is available as a list or in tree form where nodes represent folders on the file
    /// system. This class is responsible for keeping track of the settings that are currently 
    /// in use and saving/loading them to/from the file system.
    /// </summary>
    [DataContract]
    public abstract class BaseSettingsFileMgr
    {
        /// <summary>
        /// The file extension for a settings file.
        /// </summary>
        public abstract string SettingsFileExtension { get; }

        /// <summary>
        /// A short description of the type of settings this class manages. Eg. "Mss Program".
        /// </summary>
        public abstract string TypeOfSettingsName { get; }

        /// <summary>
        /// The root folder that contains settings files distributed with the installer.
        /// </summary>
        public abstract string RootFolderForFactorySettings { get; }

        /// <summary>
        /// The root folder that contains settings files created by the user
        /// </summary>
        public abstract string RootFolderForUserSettings { get; }

        /// <summary>
        /// Determines whether all settings files should have unique names. If this is true
        /// and two settings files have the same file name then (copy) will be appended to 
        /// one of them.
        /// </summary>
        protected abstract bool EnforceUniqueSettingsFileNames { get; }

        public abstract string ActiveSettingsFileName { get; protected set; }

        public SettingsFileInfo GetActiveSettingsFile()
        {
            return GetSettingsFromName(this.ActiveSettingsFileName);
        }

        private FileTreeFolderNode<SettingsFileInfo> _settingsFileTree;
        /// <summary>
        /// Stores the tree representation of the available settings files.
        /// </summary>
        public FileTreeFolderNode<SettingsFileInfo> SettingsFileTree
        {
            get { return this._settingsFileTree; }
            private set { this._settingsFileTree = value; }
        }

        protected List<SettingsFileInfo> _flatSettingsFileList;
        /// <summary>
        /// Stores the list of available settings files.
        /// </summary>
        public List<SettingsFileInfo> FlatSettingsFileList
        {
            get { return this._flatSettingsFileList; }
            protected set { this._flatSettingsFileList = value; }
        }


        protected virtual void ConstructNonSerializableMembers()
        {
            this.SettingsFileTree = new FileTreeFolderNode<SettingsFileInfo>();
            this.FlatSettingsFileList = new List<SettingsFileInfo>();
        }

        public void InitBase()
        {
            InitializeNonSerializableMembers();
        }

        protected void InitializeNonSerializableMembers()
        {
            ReinitializeSettingsFileCollections();
        }

        [OnDeserializing]
        protected void OnDeserializing(StreamingContext context)
        {
            ConstructNonSerializableMembers();
        }

        [OnDeserialized]
        protected void OnDeserialized(StreamingContext context)
        {
            InitializeNonSerializableMembers();
        }

        /// <summary>
        /// Reinitializes the collections containing available settings files. These collections 
        /// are populated from serializations of settings stored on the file system.
        /// </summary>
        protected void ReinitializeSettingsFileCollections()
        {

            //Clear the previously stored settings files
            this.SettingsFileTree = new FileTreeFolderNode<SettingsFileInfo>();
            this.FlatSettingsFileList.Clear();

            this.SettingsFileTree.Init("root", null);

            //Adds settings files found in the the factory settings folder to the collections.
            List<SettingsFileInfo> factorySettings = this.SettingsFileTree.AddDirectory(
                    this.RootFolderForFactorySettings,
                    CreateSettingsFileInfo,
                    SettingsFileExtension);
            AddNewSettingsFileInfoToFlatList(factorySettings);

            //Adds settings files found in the the user settings folder to the collections.
            List<SettingsFileInfo> userSettings = this.SettingsFileTree.AddDirectory(
                    this.RootFolderForUserSettings,
                    CreateSettingsFileInfo,
                    SettingsFileExtension);
            AddNewSettingsFileInfoToFlatList(userSettings);
        }

        /// <summary>
        /// Creates an instance of SettingsFileInfo that represents the default settings file to 
        /// be set as active.
        /// </summary>
        protected abstract SettingsFileInfo CreateDefaultActiveSettingsFileInfo();

        /// <summary>
        /// Create an instance of SettingsFileInfo from a file path.
        /// </summary>
        protected SettingsFileInfo CreateSettingsFileInfo(string filePath)
        {
            SettingsFileInfo progInfo = new SettingsFileInfo();
            progInfo.Init(filePath);
            return progInfo;
        }

        /// <summary>
        /// Adds the SettingsFileInfo objects in newSettingsFilesList to this.FlatSettingsFileList.
        /// If EnforceUniqueSettingsFileNames is set to true then any name conflicts will be
        /// resolved here.
        /// </summary>
        protected void AddNewSettingsFileInfoToFlatList(List<SettingsFileInfo> newSettingsFilesList)
        {
            foreach (SettingsFileInfo newSettingsFile in newSettingsFilesList)
            {
                if (this.EnforceUniqueSettingsFileNames == true)
                {
                    //If the new settings file is not active then give it a unique name.
                    if (newSettingsFile.Name.Equals(this.ActiveSettingsFileName) == false)
                    {
                        while (this.FlatSettingsFileList.Find(
                            (SettingsFileInfo existingSettings) => existingSettings.Name == newSettingsFile.Name)
                            != null)
                        {
                            newSettingsFile.Name += " (copy)";
                        }
                    }
                }
                this.FlatSettingsFileList.Add(newSettingsFile);
            }
        }

        /// <summary>
        /// Get the filter string used in a file dialog for saving/loading settings.
        /// </summary>
        public string GetSettingsFileFilter()
        {
            return this.TypeOfSettingsName + " (*." + this.SettingsFileExtension + ")" +
                    "|*." + this.SettingsFileExtension +
                    "|All files (*.*)|*.*";
        }

        /// <summary>
        /// Save the active settings to fs
        /// </summary>
        protected abstract void SaveActiveSettingsToFileStream(FileStream fs);

        /// <summary>
        /// Load settings from fs and store them where ever active settings are store. For example
        /// a transform preset would be stored in the active mapping's CurveShapeInfo.
        /// </summary>
        protected abstract void LoadSettingsFromFileStream(FileStream fs);

        /// <summary>
        /// Called when the active settings are replaced with a new set of active settings.
        /// </summary>
        protected virtual void OnActiveSettingsFileChanged()
        {
            //DO nothing by default.
        }

        /// <summary>
        /// Save the active settings over their previous serialization. If the active settings are
        /// factory settings or if a file does not already exist for them then the user will be 
        /// able to choose a file to save them to.
        /// </summary>
        public void SaveActiveSettingsFile()
        {
            SettingsFileInfo activeSettingsFile = GetActiveSettingsFile();

            if (activeSettingsFile.FileLocationType == SettingsFileLocationType.Factory ||
                File.Exists(activeSettingsFile.FilePath) == false)
            {
                SaveAsActiveSettingsFile();
            }
            else
            {
                FileStream newProgramStream = new
                    FileStream(activeSettingsFile.FilePath, FileMode.Truncate);
                SaveActiveSettingsToFileStream(newProgramStream);
                newProgramStream.Close();

                ReinitializeSettingsFileCollections();
            }
        }

        /// <summary>
        /// Save the active settings to a file specified by the user.
        /// </summary>
        public void SaveAsActiveSettingsFile()
        {
            SettingsFileInfo activeSettingsFile = GetActiveSettingsFile();

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = this.GetSettingsFileFilter();

            if (activeSettingsFile.FileLocationType == SettingsFileLocationType.User)
            {
                dlg.InitialDirectory = Path.GetDirectoryName(activeSettingsFile.FilePath);
                dlg.FileName = Path.GetFileName(activeSettingsFile.FilePath);
            }
            else
            {
                //Factory settings should not be overwritten by the user so the default directory
                //will be the root folder for uesr settings.
                dlg.InitialDirectory = this.RootFolderForUserSettings;
                dlg.FileName = activeSettingsFile.Name + " (copy)." + SettingsFileExtension;
            }

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                //Create a SettingsFileInfo instance for the newly created file.
                SettingsFileInfo newActiveSettingsFile = new SettingsFileInfo();
                newActiveSettingsFile.Init(dlg.FileName);
                this.ActiveSettingsFileName = newActiveSettingsFile.Name;

                OnActiveSettingsFileChanged();

                FileStream newSettingsStream = new FileStream(dlg.FileName, FileMode.Create);
                //Save the settings to the user specified location.
                SaveActiveSettingsToFileStream(newSettingsStream);
                newSettingsStream.Close();

                ReinitializeSettingsFileCollections();
            }
        }

        public SettingsFileInfo GetSettingsFromName(string settingsName)
        {
            //Search for the SettingsFileInfo instance associated with settingsName
            SettingsFileInfo settingsInfo = this.FlatSettingsFileList.Find(
                (SettingsFileInfo curProgram) => curProgram.Name == settingsName);

            //settingsInfo could be null if the active settings are external (not user settings or 
            //factory settings) as this would mean they are not in the FlatSettingsFileList
            if (settingsInfo == null)
            {
                settingsInfo = new SettingsFileInfo();
                settingsInfo.InitAsExternal(this.ActiveSettingsFileName);
            }

            return settingsInfo;
        }

        /// <summary>
        /// Loads and active the settings stored in a file whose name is specified by 
        /// newActiveSettingsFileName.
        /// </summary>
        public void LoadAndActivateSettingsFromName(string newActiveSettingsFileName)
        {
            if (newActiveSettingsFileName == this.ActiveSettingsFileName)
            {
                return;
            }

            SettingsFileInfo newActiveProgram = GetSettingsFromName(newActiveSettingsFileName);

            LoadAndActivateSettingsFromSettingsFileInfo(newActiveProgram);
        }

        /// <summary>
        /// Loads and active the settings stored at programFilePath
        /// </summary>
        public void LoadAndActivateSettingsFromPath(string programFilePath)
        {
            if (programFilePath == GetActiveSettingsFile().FilePath)
            {
                return;
            }

            SettingsFileInfo newSettingsFileInfo = CreateSettingsFileInfo(programFilePath);
            LoadAndActivateSettingsFromSettingsFileInfo(newSettingsFileInfo);
        }

        /// <summary>
        /// Loads and active the settings specified by newActiveProgram
        /// </summary>
        public void LoadAndActivateSettingsFromSettingsFileInfo(SettingsFileInfo newActiveProgram)
        {
            if (newActiveProgram != GetActiveSettingsFile())
            {
                try
                {
                    FileStream loadProgramStream = new FileStream(newActiveProgram.FilePath, FileMode.Open);
                    LoadSettingsFromFileStream(loadProgramStream);
                    loadProgramStream.Close();

                    this.ActiveSettingsFileName = newActiveProgram.Name;
                }
                catch (FileNotFoundException)
                {

                }
            }
        }

    }
}

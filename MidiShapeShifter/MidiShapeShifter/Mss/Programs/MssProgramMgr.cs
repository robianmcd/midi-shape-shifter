using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Windows.Forms;

namespace MidiShapeShifter.Mss.Programs
{

    public delegate void SaveProgramRequestEventHandler(Stream saveLocation);
    public delegate void LoadProgramRequestEventHandler(Stream loadLocation);
    public delegate void ActiveProgramChangedEventHandler(string programName);


    [Serializable]
    public class MssProgramMgr
    {
        [field:NonSerialized]
        public event SaveProgramRequestEventHandler SaveProgramRequest;
        [field: NonSerialized]
        public event LoadProgramRequestEventHandler LoadProgramRequest;
        [field: NonSerialized]
        public event ActiveProgramChangedEventHandler ActiveProgramChanged;

        public MssProgramInfo ActiveProgram { get; protected set; }

        [NonSerialized]
        private MssProgramTreeNode _programTree;
        public MssProgramTreeNode ProgramTree
        {
            get { return this._programTree; }
            private set { this._programTree = value; }
        }

        [NonSerialized]
        protected List<MssProgramInfo> _flatProgramList;
        public List<MssProgramInfo> FlatProgramList { get { return this._flatProgramList; }
                                                      protected set { this._flatProgramList = value; } }

        public MssProgramMgr()
        {
            ConstructNonSerializableMembers();
        }

        protected void ConstructNonSerializableMembers()
        {
            this.ProgramTree = new MssProgramTreeNode();
            this.FlatProgramList = new List<MssProgramInfo>();
        }


        public void Init()
        {
            this.ActiveProgram = new MssProgramInfo();
            this.ActiveProgram.Init(MssProgramType.Factory, 
                                    MssFileSystemLocations.FactoryProgramsFolder + "Blank." + 
                                        MssProgramInfo.MSS_PROGRAM_FILE_EXT);

            InitializeNonSerializableMembers();
        }

        protected void InitializeNonSerializableMembers()
        {
            ReinitializeProgramCollections();
        }

        protected void ReinitializeProgramCollections()
        {
            this.ProgramTree = new MssProgramTreeNode();
            this.FlatProgramList.Clear();

            this.FlatProgramList.Add(this.ActiveProgram);

            this.ProgramTree.Init("root", null);

            List<MssProgramInfo> factoryPrograms = this.ProgramTree.AddDirectory(
                    MssFileSystemLocations.FactoryProgramsFolder, MssProgramType.Factory);
            AddNewProgramsToFlatProgramList(factoryPrograms);

            List<MssProgramInfo> userPrograms = this.ProgramTree.AddDirectory(
                MssFileSystemLocations.UserProgramsFolder, MssProgramType.User);
            AddNewProgramsToFlatProgramList(userPrograms);            
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


        protected void AddNewProgramsToFlatProgramList(List<MssProgramInfo> newProgramList)
        {
            foreach(MssProgramInfo newProgram in newProgramList)
            {
                //If the new program is not the active program then give it a unique name.
                if (newProgram.Equals(this.ActiveProgram) == false)
                {
                    while (this.FlatProgramList.Find(
                        (MssProgramInfo existingProgram) => existingProgram.Name == newProgram.Name)
                        != null)
                    {
                        newProgram.Name += " (copy)";
                    }
                }
                this.FlatProgramList.Add(newProgram);
            }
        }

        public void SaveActiveProgram()
        {
            if (this.ActiveProgram.ProgramType == MssProgramType.Factory || 
                File.Exists(this.ActiveProgram.FilePath) == false)
            {
                SaveActiveProgramAsNewProgram();
            }
            else
            {
                if (this.SaveProgramRequest != null)
                {
                    FileStream newProgramStream = new 
                        FileStream(this.ActiveProgram.FilePath, FileMode.Truncate);
                    this.SaveProgramRequest(newProgramStream);
                    newProgramStream.Close();
                }

                ReinitializeProgramCollections();
            }            
        }

        public void SaveActiveProgramAsNewProgram()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = MssProgramInfo.MSS_PROGRAM_FILE_FILTER;

            if (this.ActiveProgram.ProgramType == MssProgramType.User)
            {
                dlg.InitialDirectory = Path.GetDirectoryName(this.ActiveProgram.FilePath);
                dlg.FileName = Path.GetFileName(this.ActiveProgram.FilePath);
            }
            else
            {
                dlg.InitialDirectory = MssFileSystemLocations.UserProgramsFolder;
                dlg.FileName = ActiveProgram.Name + " (copy)." + MssProgramInfo.MSS_PROGRAM_FILE_EXT;
            }

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                MssProgramInfo newActiveProgram = new MssProgramInfo();
                MssProgramType newActiveProgramType;
                if (dlg.FileName.StartsWith(MssFileSystemLocations.UserProgramsFolder))
                {
                    newActiveProgramType = MssProgramType.User;
                }
                else if (dlg.FileName.StartsWith(MssFileSystemLocations.FactoryProgramsFolder))
                {
                    newActiveProgramType = MssProgramType.Factory;
                }
                else 
                {
                    newActiveProgramType = MssProgramType.External;
                }

                newActiveProgram.Init(newActiveProgramType, dlg.FileName);
                this.ActiveProgram = newActiveProgram;

                if (this.ActiveProgramChanged != null)
                {
                    this.ActiveProgramChanged(newActiveProgram.Name);
                }
                
                if (this.SaveProgramRequest != null)
                {
                    FileStream newProgramStream = new FileStream(dlg.FileName, FileMode.Create);
                    this.SaveProgramRequest(newProgramStream);
                    newProgramStream.Close();
                }

                ReinitializeProgramCollections();
            }
        }

        public void ActivateProgramByName(string newActiveProgramName)
        {
            if (newActiveProgramName == this.ActiveProgram.Name)
            {
                return;
            }

            MssProgramInfo newActiveProgram = this.FlatProgramList.Find(
                (MssProgramInfo curProgram) => curProgram.Name == newActiveProgramName);

            if (newActiveProgram != null)
            {
                ActivateProgramByPath(newActiveProgram.FilePath);
            }
        }

        public void ActivateProgramByMssProgramInfo(MssProgramInfo newActiveProgram)
        {
            if (newActiveProgram != this.ActiveProgram)
            {
                ActivateProgramByPath(newActiveProgram.FilePath);
            }
        }

        public void ActivateProgramByPath(string programFilePath)
        {
            if (programFilePath == this.ActiveProgram.FilePath)
            {
                return;
            }

            //We don't need to set this.ActiveProgram because it will be deserialized when the
            //new instance of MssComponentHub is loaded

            if (this.LoadProgramRequest != null)
            {
                try
                {
                    FileStream loadProgramStream = new
                        FileStream(programFilePath, FileMode.Open);

                    this.LoadProgramRequest(loadProgramStream);
                    loadProgramStream.Close();

                }
                catch (FileNotFoundException)
                {

                }
            }
        }

    }
}

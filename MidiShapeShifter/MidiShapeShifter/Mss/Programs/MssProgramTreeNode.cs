using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MidiShapeShifter.Mss.Programs
{
    public class MssProgramTreeNode
    {
        public string NodeName { get; protected set; }

        protected MssProgramTreeNode parentNode;

        public List<MssProgramInfo> ChildProgramsList {get; protected set;}
        public List<MssProgramTreeNode> ChildTreeNodesList { get; protected set; }

        public MssProgramTreeNode()
        {
            this.ChildProgramsList = new List<MssProgramInfo>();
            this.ChildTreeNodesList = new List<MssProgramTreeNode>();
        }

        public void Init(string nodeName, MssProgramTreeNode parentNode)
        {
            this.NodeName = nodeName;
            this.parentNode = parentNode;
        }

        public List<MssProgramInfo> AddDirectory(string dirPath, MssProgramType programTypeInDir)
        {
            List<MssProgramInfo> programsCreated = new List<MssProgramInfo>();

            string[] programPathArr = 
                Directory.GetFiles(dirPath, "*." + MssProgramInfo.MSS_PROGRAM_FILE_EXT);
            //Add programs in dirPath
            foreach (string programPath in programPathArr)
            {
                MssProgramInfo newProgram = new MssProgramInfo();
                newProgram.Init(programTypeInDir, programPath);
                programsCreated.Add(newProgram);
                this.ChildProgramsList.Add(newProgram);
            }


            // Recurse into subdirectories of this directory.
            string[] subDirPathArr = Directory.GetDirectories(dirPath);
            foreach (string subDirPath in subDirPathArr)
            {
                // Do not iterate through reparse points
                if (File.GetAttributes(subDirPath).HasFlag(FileAttributes.ReparsePoint) == false)
                {
                    //In this case the method GetFileNameWithoutExtension() gets the directory name.
                    string childNodeName = Path.GetFileNameWithoutExtension(subDirPath);
                    MssProgramTreeNode childNode = GetChildNodeFromName(childNodeName);
                    bool newChildNodeCreated = false;
                    if (childNode == null)
                    {
                        childNode = new MssProgramTreeNode();
                        childNode.Init(childNodeName, this);
                        newChildNodeCreated = true;
                    }

                    List<MssProgramInfo> childPrograms = childNode.AddDirectory(subDirPath, programTypeInDir);
                    if (childPrograms.Count > 0)
                    {
                        if (newChildNodeCreated)
                        {
                            this.ChildTreeNodesList.Add(childNode);
                        }

                        programsCreated.AddRange(childPrograms);
                    }
                }
            }
            return programsCreated;
        }

        protected MssProgramTreeNode GetChildNodeFromName(string newChildNodeName)
        { 
            foreach (MssProgramTreeNode childNode in this.ChildTreeNodesList)
            {
                if (childNode.NodeName == newChildNodeName)
                {
                    return childNode;
                }
            }

            return null;
        }

    }
}

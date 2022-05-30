using System;
using System.Collections.Generic;
using System.IO;

namespace MidiShapeShifter.CSharpUtil
{
    //TODO: This tree should just store the file path as a string instead of a file info object. 
    //The other info in programinfo can be pulled out of the file path from the program manager.
    //That wont work because the program manager needs to flat list and tree to reference the same
    //program info.
    public class FileTreeFolderNode<FileTreeFileInfo>
    {
        public string NodeName { get; protected set; }

        protected FileTreeFolderNode<FileTreeFileInfo> parentNode;

        public List<FileTreeFileInfo> ChildFileList { get; protected set; }
        public List<FileTreeFolderNode<FileTreeFileInfo>> ChildFolderList { get; protected set; }

        public FileTreeFolderNode()
        {
            this.ChildFileList = new List<FileTreeFileInfo>();
            this.ChildFolderList = new List<FileTreeFolderNode<FileTreeFileInfo>>();
        }

        public void Init(string nodeName, FileTreeFolderNode<FileTreeFileInfo> parentNode)
        {
            this.NodeName = nodeName;
            this.parentNode = parentNode;
        }

        public List<FileTreeFileInfo> AddDirectory(string dirPath,
                                                   Func<string, FileTreeFileInfo> CreateFileInfo,
                                                   string fileExtension)
        {
            List<FileTreeFileInfo> programsCreated = new List<FileTreeFileInfo>();

            string[] programPathArr =
                Directory.GetFiles(dirPath, "*." + fileExtension);
            //Add programs in dirPath
            foreach (string programPath in programPathArr)
            {
                FileTreeFileInfo newProgram = CreateFileInfo(programPath);
                programsCreated.Add(newProgram);
                this.ChildFileList.Add(newProgram);
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
                    FileTreeFolderNode<FileTreeFileInfo> childNode = GetChildNodeFromName(childNodeName);
                    bool newChildNodeCreated = false;
                    if (childNode == null)
                    {
                        childNode = new FileTreeFolderNode<FileTreeFileInfo>();
                        childNode.Init(childNodeName, this);
                        newChildNodeCreated = true;
                    }

                    List<FileTreeFileInfo> childPrograms = childNode.AddDirectory(subDirPath, CreateFileInfo, fileExtension);
                    if (childPrograms.Count > 0)
                    {
                        if (newChildNodeCreated)
                        {
                            this.ChildFolderList.Add(childNode);
                        }

                        programsCreated.AddRange(childPrograms);
                    }
                }
            }
            return programsCreated;
        }

        protected FileTreeFolderNode<FileTreeFileInfo> GetChildNodeFromName(string newChildNodeName)
        {
            foreach (FileTreeFolderNode<FileTreeFileInfo> childNode in this.ChildFolderList)
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

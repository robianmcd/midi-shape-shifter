using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace MidiShapeShifter.Mss.Programs
{
    public enum MssProgramType { Factory, User, External }

    [Serializable]
    public class MssProgramInfo
    {
        public const string MSS_PROGRAM_FILE_EXT = "mpgm";

        public string Name { get; set; }

        public MssProgramType ProgramType{ get; private set; }
        public string FilePath { get; private set; }

        public MssProgramInfo()
        { 
            
        }

        public void Init(MssProgramType programType, string filePath)
        {
            Debug.Assert(programType != MssProgramType.External);

            this.ProgramType = programType;
            this.Name = Path.GetFileNameWithoutExtension(filePath);
            this.FilePath = filePath;
        }

        public override bool Equals(object o)
        {
            MssProgramInfo compareToProgInfo = (MssProgramInfo)o;
            return this.Name == compareToProgInfo.Name &&
                   this.ProgramType == compareToProgInfo.ProgramType &&
                   this.FilePath == compareToProgInfo.FilePath;
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + (int)this.Name.GetHashCode();
            hash = (hash * 7) + this.ProgramType.GetHashCode();
            hash = (hash * 7) + this.FilePath.GetHashCode();
            return hash;
        }
    }
}

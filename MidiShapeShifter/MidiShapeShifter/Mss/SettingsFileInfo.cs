using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace MidiShapeShifter.Mss
{
    public enum SettingsFileLocationType { Factory, User, External }

    [Serializable]
    public class SettingsFileInfo
    {
        public string Name { get; set; }

        public SettingsFileLocationType FileLocationType{ get; private set; }
        public string FilePath { get; private set; }

        public SettingsFileInfo()
        { 
            
        }

        public void Init(string filePath)
        {
            this.Name = Path.GetFileNameWithoutExtension(filePath);
            this.FilePath = filePath;

            if (filePath.StartsWith(MssFileSystemLocations.FactorySettingsFolder))
            {
                this.FileLocationType = SettingsFileLocationType.Factory;
            }
            else if (filePath.StartsWith(MssFileSystemLocations.UserSettingsFolder))
            {
                this.FileLocationType = SettingsFileLocationType.User;
            }
            else
            {
                this.FileLocationType = SettingsFileLocationType.External;
            }
        }

        public override bool Equals(object o)
        {
            SettingsFileInfo compareToProgInfo = (SettingsFileInfo)o;
            return this.Name == compareToProgInfo.Name &&
                   this.FileLocationType == compareToProgInfo.FileLocationType &&
                   this.FilePath == compareToProgInfo.FilePath;
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + (int)this.Name.GetHashCode();
            hash = (hash * 7) + this.FileLocationType.GetHashCode();
            hash = (hash * 7) + this.FilePath.GetHashCode();
            return hash;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Microsoft.Win32;

namespace MidiShapeShifter.Mss
{
    public static class MssFileSystemLocations
    {


        public static string SettingsFolder 
        {
            get 
            {
                RegistryKey mssLocalMachineRegKey = CreateMssLocalMachineRegKey();
                string settingsFolder = mssLocalMachineRegKey.GetValue("SettingsFolder","").ToString();
                mssLocalMachineRegKey.Close();
                Debug.Assert(settingsFolder != "");

                return settingsFolder;
            }
        }

        public static string FactorySettingsFolder { 
            get 
            {
                return SettingsFolder + @"Factory Settings\";
            } 
        }

        public static string UserSettingsFolder
        {
            get
            {
                return SettingsFolder + @"User Settings\";
            }
        }

        public static string FactoryProgramsFolder
        {
            get
            {
                return FactorySettingsFolder + @"Programs\";
            }
        }

        public static string UserProgramsFolder
        {
            get
            {
                return UserSettingsFolder + @"Programs\";
            }
        }

        public static string FactoryTransformPresetFolder
        {
            get
            {
                return FactorySettingsFolder + @"Transformation Presets\";
            }
        }

        public static string UserTransformPresetFolder
        {
            get
            {
                return UserSettingsFolder + @"Transformation Presets\";
            }
        }

        private static RegistryKey CreateMssLocalMachineRegKey()
        {
            RegistryKey localMachine32 = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry32);
            return localMachine32.OpenSubKey(@"SOFTWARE\" + MssConstants.APP_NAME);
        }
    }


}

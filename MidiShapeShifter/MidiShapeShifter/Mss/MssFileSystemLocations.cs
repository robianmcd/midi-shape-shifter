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

        private static RegistryKey CreateMssLocalMachineRegKey()
        {
            return Registry.LocalMachine.OpenSubKey(@"SOFTWARE\MIDI Shape Shifter");
        }
    }


}

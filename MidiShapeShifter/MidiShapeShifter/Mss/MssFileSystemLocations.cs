using Microsoft.Win32;
using System.Diagnostics;

namespace MidiShapeShifter.Mss
{
    public static class MssFileSystemLocations
    {


        public static string SettingsFolder
        {
            get
            {
                RegistryKey mssLocalMachineRegKey = CreateMssLocalMachineRegKey();
                string settingsFolder = mssLocalMachineRegKey.GetValue("SettingsFolder", "").ToString();
                mssLocalMachineRegKey.Close();
                Debug.Assert(settingsFolder != "");

                return settingsFolder;
            }
        }

        public static string FactorySettingsFolder => SettingsFolder + @"Factory Settings\";

        public static string UserSettingsFolder => SettingsFolder + @"User Settings\";

        public static string FactoryProgramsFolder => FactorySettingsFolder + @"Programs\";

        public static string UserProgramsFolder => UserSettingsFolder + @"Programs\";

        public static string FactoryTransformPresetFolder => FactorySettingsFolder + @"Transformation Presets\";

        public static string UserTransformPresetFolder => UserSettingsFolder + @"Transformation Presets\";

        private static RegistryKey CreateMssLocalMachineRegKey()
        {
            RegistryKey localMachine32 = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry32);
            return localMachine32.OpenSubKey(@"SOFTWARE\" + MssConstants.APP_NAME);
        }
    }


}

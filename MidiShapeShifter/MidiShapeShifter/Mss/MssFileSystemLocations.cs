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
                RegistryKey mssLocalMachineRegKey = CreateMssCurrentUserRegKey();
                var settingsFolder = mssLocalMachineRegKey.GetValue("SettingsFolder");

                // if doesn't exist, create it
                if (settingsFolder == null)
                {
                    mssLocalMachineRegKey.SetValue("SettingsFolder", MssConstants.DEFAULT_SETTINGS_FOLDER);
                    settingsFolder = mssLocalMachineRegKey.GetValue("SettingsFolder");
                }
                Debug.Assert(settingsFolder != null);

                mssLocalMachineRegKey.Close();

                return settingsFolder;
            }
        }

        public static string FactorySettingsFolder => SettingsFolder + @"Factory Settings\";

        public static string UserSettingsFolder => SettingsFolder + @"User Settings\";

        public static string FactoryProgramsFolder => FactorySettingsFolder + @"Programs\";

        public static string UserProgramsFolder => UserSettingsFolder + @"Programs\";

        public static string FactoryTransformPresetFolder => FactorySettingsFolder + @"Transformation Presets\";

        public static string UserTransformPresetFolder => UserSettingsFolder + @"Transformation Presets\";

        private static RegistryKey CreateMssCurrentUserRegKey()
        {
            RegistryKey localMachine32 = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.CurrentUser, RegistryView.Registry32);
            return localMachine32.OpenSubKey(@"SOFTWARE\" + MssConstants.APP_NAME) ??
                   localMachine32.CreateSubKey(@"SOFTWARE\" + MssConstants.APP_NAME);
        }
    }


}

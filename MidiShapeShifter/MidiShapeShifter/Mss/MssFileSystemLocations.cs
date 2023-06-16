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
                var settingsFolderObject = mssLocalMachineRegKey.GetValue("SettingsFolder");

                // if doesn't exist, create it
                string settingsFolder = "";
                if (settingsFolderObject == null)
                {
                    // set it to the current user's Documents/MidiShapeShifter folder
                    string current_user_documents_folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
                    Debug.Assert(current_user_documents_folder != "");
                    string default_settings_folder = current_user_documents_folder + @"\" + MssConstants.APP_FOLDER_NAME + @"\";

                    mssLocalMachineRegKey.SetValue("SettingsFolder", default_settings_folder);
                    settingsFolder = default_settings_folder;
                }
                Debug.Assert(settingsFolder != "");

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

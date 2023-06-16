using Microsoft.Win32;
using System.Diagnostics;

namespace MidiShapeShifter.Mss
{
    public static class MssFileSystemLocations
    {
        public static string default_settings_folder()
        {
            // set it to the current user's Documents/MidiShapeShifter folder
            string current_user_documents_folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            Debug.Assert(current_user_documents_folder != "");
            return current_user_documents_folder + @"\" + MssConstants.APP_FOLDER_NAME + @"\";
        }

        public static string SettingsFolder
        {
            get
            {
                RegistryKey mssLocalMachineRegKey = CreateMssCurrentUserRegKey();
                Debug.Assert(mssLocalMachineRegKey != null);
                object settingsFolderObject = mssLocalMachineRegKey.GetValue("SettingsFolder");

                // if doesn't exist, create it
                string settingsFolder = "";
                if (settingsFolderObject == null)
                {
                    settingsFolder = default_settings_folder();

                    try
                    {
                        mssLocalMachineRegKey.SetValue("SettingsFolder", settingsFolder, RegistryValueKind.ExpandString);
                    }
                    catch (System.Exception err)
                    {
                        Logger.Warning(0, "Unable to create registry key for SettingsFolder. Try again as administrator or ignore the warning:" + err.Message);
                    }
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
            RegistryKey current_user = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.CurrentUser, RegistryView.Registry32);
            return current_user.OpenSubKey(@"SOFTWARE\" + MssConstants.APP_NAME) ??
                   current_user.CreateSubKey(@"SOFTWARE\" + MssConstants.APP_NAME);
        }
    }


}

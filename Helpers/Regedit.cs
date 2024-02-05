using Microsoft.Win32;

namespace Frame.Helpers
{
    internal class Regedit
    {
        private const string taskbarPATH = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\StuckRects3";
        private const string windowsTheme = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize";
        
        /*
         * SystemUsesLightTheme
         * AppsUseLightTheme
         * 
         */
     

        public static void OpenKey()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(taskbarPATH, true))
            {
                if(key != null)
                {
                    byte[] keyValue = (byte[])key.GetValue("Settings");

                    if (keyValue != null && keyValue.Length > 8)
                    {

                        keyValue[8] = 3;
                    }
                    key.SetValue("Settings", value: keyValue);
                }
            }
        }
    }
}

using System;
using System.IO;
using Microsoft.Win32;
using Printer.Properties;

namespace Printer
{
    public static class SettingsService
    {
        public static string GetConnectionString()
        {
            return GetConnectionString(null, true);
        }
        public static string GetConnectionString(bool oledbString)
        {
            return GetConnectionString(null, oledbString);
        }

        public static string GetConnectionString(string connectionString, bool oledbString)
        {
            if (!string.IsNullOrEmpty(connectionString))
                return connectionString;

            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Nuansa\NCI Medismart\3.00\Database", RegistryKeyPermissionCheck.ReadSubTree);
            if (key == null)
            {
                throw new NullReferenceException(@"Tidak bisa baca registry HKCU\Software\Nuansa\NCI Medismart\3.00\Database");
            }
            for (int i = 0; i < 11; i++)
            {
                var value = (string)key.GetValue("key" + i);
                if (value.Contains("User ID") || value.Contains("Password"))
                    continue;
                if (oledbString || !(value.Contains("Provider") || value.Contains("Locale") || value.Contains("Use Procedure") || value.Contains("Auto Translate")))
                    connectionString = connectionString + value;
                
            }
            connectionString += string.Format(";Password={0};User ID={1}", Settings.Default.DatabasePassword, Settings.Default.DatabaseUser);
            return connectionString;
        }


        public static string GetReportFolder()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                                Settings.Default.ProgramFolder + @"\Reports\");
        }

        public static string GetProgramFolder()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                                              Settings.Default.ProgramFolder + @"\");
        }

        public static string CreateReportName(string kasir, bool pendaftaran, string kdCustomerReport)
        {
            string modul = pendaftaran ? "PEN" : "BIL";
            return string.Format("CR{0}-{1}-{2}.rpt", modul, kasir, kdCustomerReport.Substring(8));
        }
    }
}
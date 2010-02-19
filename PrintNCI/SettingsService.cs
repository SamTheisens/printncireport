using System;
using Microsoft.Win32;
using PrintNCI.Properties;

namespace PrintNCI
{
    public static class SettingsService
    {
        public static string GetConnectionString(string connectionString)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("Nuansa").OpenSubKey("NCI Medismart").OpenSubKey("3.00").OpenSubKey("Database");
            if (key == null)
            {
                throw new NullReferenceException("Tidak bisa baca registry.");
            }
            for (int i = 0; i < 11; i++)
            {
                var value = (string)key.GetValue("key" + i);
                if (!(value.Contains("User ID") || value.Contains("Password")))
                    connectionString = connectionString + value;
            }
            connectionString += ";Password=pocopoco;User ID=sa";
            return connectionString;
        }

        public static string GetExecutableUgd(string kelompokPasien)
        {
            if (Program.printStatus)
                return Settings.Default.ExecutablePrintStatus;

            switch (kelompokPasien)
            {
                case Program.kelompokInHealth:
                    return Settings.Default.ExecutablePrintInHealth;
                case Program.kelompokAskes:
                    return Settings.Default.ExecutablePrintAskes;
                case Program.kelompokJamkesmas:
                    return Settings.Default.ExecutablePrintJamkesmasRWJ;
                case Program.kelompokJamkesda:
                    return Settings.Default.ExecutablePrintJamkesdaRWJ;
                default:
                    return "";
            }
        }

        public static string GetExecutableTpp(string kelompokPasien, int bagian)
        {
            if (Program.printStatus)
                return Settings.Default.ExecutablePrintStatus;

            if (bagian == 3)
            {
                return Settings.Default.ExecutablePrintJamkesmasRWI;
            }
            switch (kelompokPasien)
            {
                case Program.kelompokInHealth:
                    return "";
                case Program.kelompokAskes:
                    return "";
                case Program.kelompokJamkesmas:
                    return Settings.Default.ExecutablePrintJamkesmasRWJ;
                case Program.kelompokJamkesda:
                    return Settings.Default.ExecutablePrintJamkesdaRWJ;
                default:
                    return "";
            }
        }
    }
}

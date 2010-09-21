using System;
using System.Security.Permissions;
using Microsoft.Win32;
using Printer.Properties;

namespace Printer
{
    public static class SettingsService
    {
        public static string GetConnectionString(string connectionString)
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
                if (!(value.Contains("User ID") || value.Contains("Password")))
                    connectionString = connectionString + value;
            }
            connectionString += string.Format(";Password={0};User ID={1}", Settings.Default.DatabasePassword, Settings.Default.DatabaseUser);
            return connectionString;
        }

        public static string GetExecutableUgd(Options options, string kelompokPasien)
        {
            if (options.KartuBerobat)
                return Settings.Default.ExecutablePrintKartuBerobat;

            if (options.Status)
                return Settings.Default.ExecutablePrintStatus;

            switch (kelompokPasien)
            {
                case NCIPrinter.kelompokInHealth:
                    return Settings.Default.ExecutablePrintInHealth;
                case NCIPrinter.kelompokAskes:
                    return Settings.Default.ExecutablePrintAskes;
                case NCIPrinter.kelompokJamkesmas:
                    return Settings.Default.ExecutablePrintJamkesmasRWJ;
                case NCIPrinter.kelompokJamkesda:
                    return Settings.Default.ExecutablePrintJamkesdaRWJ;
                default:
                    return "";
            }
        }

        public static string GetExecutableTpp(Options options, string kelompokPasien)
        {
            if (options.Status)
                return Settings.Default.ExecutablePrintStatus;

            if (options.KartuBerobat)
                return Settings.Default.ExecutablePrintKartuBerobat;

            if (options.Tracer)
                return Settings.Default.ExecutablePrintTracer;

            if (options.KdBagian == 1)
            {
                return Settings.Default.ExecutablePrintJamkesmasRWI;
            }
            switch (kelompokPasien)
            {
                case NCIPrinter.kelompokInHealth:
                    return "";
                case NCIPrinter.kelompokAskes:
                    return "";
                case NCIPrinter.kelompokJamkesmas:
                    return Settings.Default.ExecutablePrintJamkesmasRWJ;
                case NCIPrinter.kelompokJamkesda:
                    return Settings.Default.ExecutablePrintJamkesdaRWJ;
                default:
                    return "";
            }
        }
    }
}
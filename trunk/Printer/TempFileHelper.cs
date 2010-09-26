using System;
using System.Collections;
using System.IO;
using Printer.Properties;

namespace Printer
{
    public static class TempFileHelper
    {
        public static void ReadBill(out string noTransaksi, out string kdKasir)
        {
            string line = GetLine("NCIPrintBill.TMP");
            var strArray = line.Split('#');
            kdKasir = strArray[0];
            noTransaksi = strArray[1];
            
            Logger.Logger.WriteLog("Kasir: " + kdKasir + " Transaksi: " + noTransaksi);
        }

        public static string GetLine(string tempFile)
        {
            var filepath = Path.Combine(GetUserTempPath(), tempFile);
            if (!File.Exists(filepath))
                Logger.Logger.WriteLog(string.Format("Temp file {0} tidak ada", filepath));
            var reader = new StreamReader(filepath);
            var line = reader.ReadLine();
            Logger.Logger.WriteLog(string.Format("Yang di dalam tmpfile {0}: {1}", filepath, line));
            reader.Close();
            return line;
        }

        public static void WriteTempFileStatus(string kdPasien, byte bagian)
        {
            string tempFileName = bagian == 1
                                      ? Settings.Default.RITempFileName
                                      : Settings.Default.RJTempFileName;

            string filePath = Path.Combine(GetUserTempPath(),tempFileName);
            var writer = new StreamWriter(filePath);
            Logger.Logger.WriteLog(string.Format("Writing tempfile: {0}", filePath));
            writer.Write(kdPasien);
            writer.Write('\r');
            writer.Write('\n');
            writer.Write('\f');
            writer.Write('\r');
            writer.Write('\n');
            writer.Flush();
            writer.Close();
        }

        public static string ReadStatus(Options options)
        {
            string tempFileName = options.KdBagian == 1
                                      ? Settings.Default.RITempFileName
                                      : Settings.Default.RJTempFileName;
            string line = GetLine(tempFileName);
            var strArray = line.Split('#');
            return strArray[0];
        }
        private static string GetUserTempPath()
        {
            IntPtr l_ptrEnvironment;
            ProcessUtilities.SECURITY_ATTRIBUTES l_oSecurityAttributes;
            ProcessUtilities.STARTUPINFO l_oStartupInfo;

            ProcessUtilities.LoadUserProfile(out l_oSecurityAttributes, out l_ptrEnvironment, out l_oStartupInfo);
            if (l_ptrEnvironment == IntPtr.Zero)
            {
                return Path.GetTempPath();
            }
            string dir = Environment.GetEnvironmentVariable("USERPROFILE");
            if (dir != null) return Path.Combine(dir, Path.Combine("Local Settings", "Temp"));
            return "";
        }
    }
}
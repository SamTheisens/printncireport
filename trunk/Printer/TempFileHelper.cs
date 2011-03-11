﻿using System;
using System.Collections.Generic;
using System.IO;
using Printer.Properties;

namespace Printer
{
    public static class TempFileHelper
    {
        private const string tempFileName = "NCICetakStatus.tmp";

        public static void ReadBill(out string noTransaksi, out string kdKasir)
        {
            string line = GetLine(Settings.Default.ReadBillTmpFileName);
            var strArray = line.Split('#');
            kdKasir = strArray[0].Length < strArray[1].Length ? strArray[0] : strArray[1];
            noTransaksi = strArray[0].Length < strArray[1].Length ? strArray[1] : strArray[0];
            
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

        public static void WriteTempFileBill(string noTransaksi, string kdKasir)
        {
            string filePath = Path.Combine(GetUserTempPath(), tempFileName);
            var writer = new StreamWriter(filePath);
            Logger.Logger.WriteLog(string.Format("Writing tempfile: {0}", filePath));
            writer.Write(noTransaksi + '-' + kdKasir);
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
        public static void ModifyExecutable(string fileName, string procedure, string report)
        {
            var stream = File.OpenWrite(fileName);
            string reportFileName = report;

            if (!File.Exists(Path.Combine(SettingsService.GetReportFolder(), reportFileName)))
                throw new Exception(string.Format("Report {0} belum ada", reportFileName));

            stream.Seek(25980L, SeekOrigin.Begin);
            var procedureByteList = ConvertToByteList("EXEC " + (procedure + "                     ").Substring(0,17));
            stream.Write(procedureByteList, 0, procedureByteList.Length);

            stream.Seek(29806L, SeekOrigin.Begin);
            var byteList = ConvertToByteList(reportFileName);
            stream.Write(byteList, 0, byteList.Length);

            stream.Flush();
            stream.Close();
            Logger.Logger.WriteLog(string.Format("Ditulis dalam file {0} procedure: {1} report {2}", fileName, procedure,
                                                 report));
        }

        private static byte[] ConvertToByteList(string unicodeString)
        {
            var byteList = new List<byte>();
            foreach (char c in unicodeString)
            {
                byteList.AddRange(BitConverter.GetBytes(c));
            }
            return byteList.ToArray();
        }
    }
}
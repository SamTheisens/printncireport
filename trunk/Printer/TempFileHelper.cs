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
            var filepath = Path.Combine(Path.GetTempPath(), tempFile);
            if (!File.Exists(filepath))
                Logger.Logger.WriteLog(string.Format("Temp file {0} tidak ada", filepath));
            var reader = new StreamReader(filepath);
            var line = reader.ReadLine();
            Logger.Logger.WriteLog(string.Format("Yang di dalam tmpfile: {0}", line));
            reader.Close();
            return line;
        }

        public static void WriteTempFileStatus(string kdPasien, byte bagian)
        {
            string tempFileName = bagian == 1
                                      ? Settings.Default.RITempFileName
                                      : Settings.Default.RJTempFileName;

            var writer = new StreamWriter(Path.GetTempPath() + tempFileName);
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
    }
}
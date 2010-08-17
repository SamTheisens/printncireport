using System.IO;
using PrintNCI.Properties;

namespace PrintNCI
{
    public static class TempFileHelper
    {
        public static string ReadBill(out string noTransaksi)
        {
            string line = GetLine("NCIPrintBill.TMP");
            var strArray = line.Split('#');
            var kdKasir = strArray[0];
            noTransaksi = strArray[1];

            Logger.WriteLog("Kasir: " + kdKasir + " Transaksi: " + noTransaksi);
            return kdKasir;
        }

        public static string GetLine(string tempFile)
        {
            var filepath = Path.Combine(Path.GetTempPath(), tempFile);
            if (!File.Exists(filepath))
                Logger.WriteLog(string.Format("Temp file {0} tidak ada", filepath));
            var reader = new StreamReader(filepath);
            var line = reader.ReadLine();
            Logger.WriteLog(string.Format("Yang di dalam tmpfile: {0}", line));
            reader.Close();
            return line;
        }

        public static void WriteTempFileStatus(string kdPasien)
        {
            var writer = new StreamWriter(Path.GetTempPath() + "NCICetakStatus.tmp");
            writer.Write(kdPasien);
            writer.Write('\r');
            writer.Write('\n');
            writer.Write('\f');
            writer.Write('\r');
            writer.Write('\n');
            writer.Flush();
            writer.Close();
        }
        public static string ReadStatus()
        {
            string tempFileName = Settings.Default.RawatInap
                                      ? Settings.Default.RITempFileName
                                      : Settings.Default.RJTempFileName;
            string line = GetLine(tempFileName);
            var strArray = line.Split('#');
            return strArray[0];
        }


    }
}

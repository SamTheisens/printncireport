using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using PrintNCI.Properties;

namespace PrintNCI
{

    public class Program
    {
        public const string kelompokJamkesmas = "JAMKESMAS";
        public const string kelompokJamkesda = "JAMKESDA";
        public const string kelompokAskes = "ASKES NEGERI";
        public const string kelompokInHealth = "IN HEALTH";

        public static bool printStatus;
        // Methods

        private static void Main(string[] args)
        {
            var argList = new List<string>(args);
            if (argList.Count == 0 || argList.Contains("help"))
            {
                Console.WriteLine(Resources.Usage);
                return;
            }
            if (argList.Contains("settings"))
            {
                var window = new SettingsWindow();
                window.ShowDialog();
                return;
            }
            printStatus = argList.Contains("status");
            if (!printStatus && !argList.Contains("bill"))
            {
                Console.WriteLine(Resources.Usage);
                return;
            }
            
            try
            {
                string noTransaksi;
                string kdPasien;
                string kelompokPasien;
                int bagian = Settings.Default.RawatInap ? 3 : 0;
                var tglMasuk = new DateTime();
                bool baru = false;
                string connectionString = "";
                bool mauPrint = true; 

                connectionString = SettingsService.GetConnectionString(connectionString);

                if (args.Length > 0 && args[0] == "test")
                {
                    kdPasien = "0-20-99-71";
                    kelompokPasien = kelompokJamkesda;
                }
                else
                {
                    if (printStatus)
                    {
                        kdPasien = ReadStatus(connectionString, out kelompokPasien, out tglMasuk, out baru);
                        mauPrint = GetMauPrint(tglMasuk, baru);
                    }
                    else
                    {
                        string kdKasir = ReadBill(out noTransaksi);
                        DatabaseService.GetVisitInfo(connectionString, kdKasir, noTransaksi, out kdPasien,
                                                     out kelompokPasien, out bagian);
                    }
                }

                Logger.WriteLog(string.Format("Sedang print pasien #{0}. Kelompok pasien: {1}.", kdPasien, kelompokPasien));
                WriteTempFileStatus(kdPasien);
                string executable = Settings.Default.UntukUGD ? SettingsService.GetExecutableUgd(kelompokPasien) : SettingsService.GetExecutableTpp(kelompokPasien, bagian);
                
                if (executable.Length == 0)
                {
                    Logger.WriteLog(string.Format("Maaf. Pasien {0} pasien {1}. Tidak bisa print Jaminan.", kdPasien,
                                                    kelompokPasien));
                    Thread.Sleep(4000);
                }
                else if(mauPrint)
                    PrintStatus(executable, connectionString, kdPasien);
            }
            catch (Exception exception)
            {
                Logger.WriteLog("Maaf, ada masalah. Tolong panggil Instalasi SIMRS.");
                Logger.WriteLog(exception.Message);
                ConsoleKeyInfo info = Console.ReadKey();
            }
        }

        private static bool GetMauPrint(DateTime tglMasuk, bool baru)
        {
            bool mauPrint = true;
            if (DateTime.Today.Date != tglMasuk.Date)
            {
                mauPrint =
                    MessageBox.Show(
                        string.Format(
                            "Tanggal kunjungan pasien, {0} bukan hari ini.\nLupa daftarkan kunjungan? (Ctrl-K)",
                            tglMasuk.ToShortDateString()), "Tanggal kunjungan salah?", MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.OK;
            }
            if (printStatus && !baru)
            {
                mauPrint =
                    MessageBox.Show("Pasien ini bukan pasien baru.\nMau print status?", "Pasien Lama",
                                    MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation,
                                    MessageBoxDefaultButton.Button2) == DialogResult.OK;
            }
            return mauPrint;
        }


        private static void PrintStatus(string executable, string connectionString, string kdPasien)
        {
            string NCIPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                                          @"Pendaftaran\");

            string fileName = Path.Combine(NCIPath, executable);
            Logger.WriteLog("Sedang start " + fileName);
            using (var process = new Process())
            {
                process.StartInfo.FileName = fileName;
                process.StartInfo.Arguments = "/e";
                process.Start();
            }
        }

        private static string ReadStatus(string connectionString, out string kelompokPasien, out DateTime tglMasuk, out bool baru)
        {
            string tempFileName = Settings.Default.RawatInap
                                      ? Settings.Default.RITempFileName
                                      : Settings.Default.RJTempFileName;
            string line = GetLine(tempFileName);
            var strArray = line.Split('#');
            var kdPasien = strArray[0];
            kelompokPasien = DatabaseService.GetKelompokPasien(connectionString, kdPasien, out tglMasuk, out baru);
            Logger.WriteLog("KdPasien: " + kdPasien);
            return kdPasien;
        }

        private static string ReadBill(out string noTransaksi)
        {
            string line = GetLine("NCIPrintBill.TMP");
            var strArray = line.Split('#');
            var kdKasir = strArray[0];
            noTransaksi = strArray[1];

            Logger.WriteLog("Kasir: " + kdKasir + " Transaksi: " + noTransaksi);
            return kdKasir;
        }

        private static string GetLine(string tempFile)
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

        private static void WriteTempFileStatus(string kdPasien)
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
    }
}
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using PrintNCI.Properties;

namespace PrintNCI
{
    public sealed class NCIPrinter
    {
        private readonly Options options;
        public NCIPrinter(Options options)
        {
            this.options = options;
        }
        public void Print()
        {
            if (options.Settings)
            {
                var window = new SettingsWindow();
                window.ShowDialog();
                return;
            }
            if (!options.Status && !options.Billing && !options.Test)
            {
                Console.WriteLine(Resources.Usage);
                return;
            }

            var visitInfo = new PatientVisitInfo();
            byte bagian = Settings.Default.RawatInap ? (byte) 3 : (byte) 0;
            const string connectionString = "";
            bool mauPrint = true;

            var filler = new DatabaseService(SettingsService.GetConnectionString(connectionString));

            if (options.Status)
            {
                visitInfo = filler.GetKelompokPasien(TempFileHelper.ReadStatus());
                mauPrint = GetMauPrint(visitInfo.TglMasuk, visitInfo.Baru);
            }
            else
            {
                string kdKasir;
                string noTransaksi;
                if (options.Test)
                {
                    kdKasir = "01";
                    noTransaksi = "0144599";
                }
                else
                {
                    kdKasir = TempFileHelper.ReadBill(out noTransaksi);
                }
                visitInfo = filler.GetVisitInfo(kdKasir, noTransaksi);
            }

            Logger.WriteLog(string.Format("Sedang print pasien #{0}. Kelompok pasien: {1}.", visitInfo.KdPasien,
                                          visitInfo.KelompokPasien));
            TempFileHelper.WriteTempFileStatus(visitInfo.KdPasien);
            string executable = Settings.Default.UntukUGD
                                    ? SettingsService.GetExecutableUgd(options.Status, visitInfo.KelompokPasien)
                                    : SettingsService.GetExecutableTpp(options.Status, visitInfo.KelompokPasien, bagian);

            if (executable.Length == 0)
            {
                Logger.WriteLog(string.Format("Maaf. Pasien {0} pasien {1}. Tidak bisa print Jaminan.",
                                              visitInfo.KdPasien,
                                              visitInfo.KelompokPasien));
                Thread.Sleep(4000);
            }
            else if (mauPrint)
                Print(executable);

            if (Settings.Default.UpdateTracer)
                UpdateTracer(filler, visitInfo);
        }


        private void UpdateTracer(DatabaseService service, PatientVisitInfo visitInfo)
        {
            short status;
            short jaminan;
            bool hasPrintStatus = service.GetPrintStatus(visitInfo, out status, out jaminan);
            status = options.Status ? (short)(status + 1) : status;
            jaminan = options.Status ? jaminan : (short)(jaminan + 1);

            service.UpdatePrintStatus(visitInfo, status, jaminan, hasPrintStatus);
        }

        private bool GetMauPrint(DateTime tglMasuk, bool? baru)
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
            if (options.Status && !baru.Value)
            {
                mauPrint =
                    MessageBox.Show("Pasien ini bukan pasien baru.\nMau print status?", "Pasien Lama",
                                    MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation,
                                    MessageBoxDefaultButton.Button2) == DialogResult.OK;
            }
            return mauPrint;
        }


        private static void Print(string executable)
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
 
    }
}

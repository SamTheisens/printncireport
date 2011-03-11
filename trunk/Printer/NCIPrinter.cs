using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Printer.Properties;

namespace Printer
{
    public sealed class NCIPrinter
    {
        private readonly Options options;
        private readonly string connectionString;
        public NCIPrinter(Options options)
        {
            this.options = options;
        }
        public NCIPrinter(Options options, string connectionString)
            : this(options)
        {
            this.connectionString = connectionString;
        }

        public void Print()
        {
            string kdKasir;
            string noTransaksi;

            var visitInfo = new PatientVisitInfo();
            options.SkipVerify = true;

            var service = new DatabaseService(SettingsService.GetConnectionString(true));

            //if (options.Status || options.KartuBerobat || options.Tracer || !string.IsNullOrEmpty(options.KdPasien))
            //{
            //    if (!string.IsNullOrEmpty(options.KdUnit) && !string.IsNullOrEmpty(options.TglMasuk))
            //        visitInfo = service.GetVisitInfo(options);
            //    else
            //        visitInfo = service.GetVisitInfo(string.IsNullOrEmpty(options.KdPasien)
            //                                             ? TempFileHelper.ReadStatus(options)
            //                                             : options.KdPasien);
            //}
            //else if (options.Billing || options.Kasir)
            //{
            if (options.Pendaftaran)
            {
                visitInfo = service.GetVisitInfoKasir(options.KdKasir, TempFileHelper.ReadStatus(options));
            }
            else
            {
                TempFileHelper.ReadBill(out noTransaksi, out kdKasir);
                if (string.IsNullOrEmpty(options.KdKasir))
                    options.KdKasir = kdKasir;

                visitInfo = service.GetVisitInfo(options.KdKasir, noTransaksi);
            }

            options.KdBagian = visitInfo.KdBagian.Value;

            if (!options.SkipVerify && !GetMauPrint(visitInfo.TglMasuk, visitInfo.Baru))
                return;

            if (!string.IsNullOrEmpty(options.Sjp) && options.Pendaftaran)
                UpdateSjp(visitInfo, options.Sjp);

            Logger.Logger.WriteLog(string.Format("Sedang print pasien #{0}. Kelompok pasien: {1}.", visitInfo.KdPasien,
                                                 visitInfo.KelompokPasien));

            service.IncreaseNota(visitInfo.KdKasir, visitInfo.NoTransaksi, visitInfo.KdKelompokPasien);

            TempFileHelper.WriteTempFileBill(visitInfo.NoTransaksi, visitInfo.KdKasir);

            // determine executable
            var executable = service.GetExecutable(visitInfo.KdKelompokPasien, visitInfo.KdKasir, Settings.Default.Pendaftaran);

            if (executable.FileName.Length == 0)
            {
                Logger.Logger.WriteLog(string.Format("Maaf. Pasien {0} pasien {1}. Tidak bisa print Jaminan.",
                                              visitInfo.KdPasien,
                                              visitInfo.KelompokPasien));
                Thread.Sleep(4000);
            }
            else TryPrint(executable, options);


            if (Settings.Default.UpdateTracer)
                UpdateTracer(visitInfo);
        }

        private void UpdateTracer(PatientVisitInfo visitInfo)
        {
            PrintHelper.WaitUntilPrinted(options);
            CommitToDatabase(visitInfo);
            Logger.Logger.WriteLog(string.Format("-- Cetak berhasil untuk pasien {0}", visitInfo.KdPasien));
        }

        private static void TryPrint(Report report, Options options)
        {
            if (options.PrintQueue)
                PrintHelper.PrintQueueEmpty(options);
            Print(report);
        }

        private static void UpdateSjp(PatientVisitInfo visitInfo, string noSJP)
        {
            var service = new DatabaseService(SettingsService.GetConnectionString(true));
            bool update = service.GetSjp(visitInfo);

            service = new DatabaseService(SettingsService.GetConnectionString(true));
            service.UpdateSjp(visitInfo, noSJP, update);
        }

        private void CommitToDatabase(PatientVisitInfo visitInfo)
        {
            short status;
            short jaminan;
            short kartu_berobat;
            short tracer;
            var service = new DatabaseService(SettingsService.GetConnectionString(true));
            bool hasPrintStatus = service.GetPrintStatus(visitInfo, out status, out jaminan, out kartu_berobat, out tracer);
            status = options.Status ? (short)(status + 1) : status;
            jaminan = options.Pendaftaran ? (short)(jaminan + 1) : jaminan;
            kartu_berobat = options.KartuBerobat ? (short)(kartu_berobat + 1) : kartu_berobat;
            tracer = options.Tracer ? (short)(tracer + 1) : tracer;

            service = new DatabaseService(SettingsService.GetConnectionString(true));
            service.UpdatePrintStatus(visitInfo, status, jaminan, kartu_berobat, tracer, hasPrintStatus);
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

        public static void Print(Report report)
        {
            string fileName = Path.Combine(SettingsService.GetProgramFolder(), "Print_Report.exe");
            TempFileHelper.ModifyExecutable(fileName, report.Procedure, report.FileName);

            try
            {
                Logger.Logger.WriteLog(
                    string.Format("Hasil start start proces {0}:  {1}",
                                  ProcessUtilities.CreateUIProcessForServiceRunningAsLocalSystem(fileName, "")
                                  , fileName));

            }
            catch (Exception ex)
            {
                Logger.Logger.WriteLog(string.Format("Ada masalah dengan start proses {0}. Masalah: {1}", fileName,
                                                     ex.Message));
            }
        }
    }
}
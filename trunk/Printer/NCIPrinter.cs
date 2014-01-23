using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using Printer.Properties;

namespace Printer
{
    public sealed class NCIPrinter
    {
        private readonly Options _options;
        private readonly string _connectionString;
        public NCIPrinter(Options options)
        {
            _options = options;
        }
        public NCIPrinter(Options options, string connectionString)
            : this(options)
        {
            this._connectionString = connectionString;
        }

        public void Print()
        {
            string kdKasir;
            string noTransaksi;

            var visitInfo = new PatientVisitInfo();
            _options.SkipVerify = true;

            var service = new DatabaseService(SettingsService.GetConnectionString(true));

            if (_options.CommandLine)
            {
                visitInfo = service.GetVisitInfo(_options);
            }
            else if (_options.Pendaftaran)
            {
                visitInfo = service.GetVisitInfoKasir(_options.KdKasir, TempFileHelper.ReadStatus(_options));
            }
            else
            {
                TempFileHelper.ReadBill(out noTransaksi, out kdKasir);
                if (string.IsNullOrEmpty(_options.KdKasir))
                    _options.KdKasir = kdKasir;

                visitInfo = service.GetVisitInfo(_options.KdKasir, noTransaksi);
            }

            _options.KdBagian = visitInfo.KdBagian.Value;

            if (!_options.SkipVerify && !GetMauPrint(visitInfo.TglMasuk, visitInfo.Baru))
                return;

            if (!string.IsNullOrEmpty(_options.Sjp) && _options.Pendaftaran)
                UpdateSjp(visitInfo, _options.Sjp);

            Logger.Logger.WriteLog(string.Format("Sedang print pasien #{0}. Kelompok pasien: {1}.", visitInfo.KdPasien,
                                                 visitInfo.KelompokPasien));

            if (Settings.Default.Pendaftaran)
            {
                TempFileHelper.WriteTempFileStatus(_options.KdPasien);
            } else
            {
                service.IncreaseNota(visitInfo.KdKasir, visitInfo.NoTransaksi, visitInfo.KdKelompokPasien);
                TempFileHelper.WriteTempFileBill(visitInfo.NoTransaksi, visitInfo.KdKasir);
            }

            // determine executable
            var executable = service.GetExecutable(visitInfo.KdKelompokPasien, visitInfo.KdKasir, Settings.Default.Pendaftaran);

            if (executable.FileName.Length == 0)
            {
                Logger.Logger.WriteLog(string.Format("Maaf. Pasien {0} pasien {1}. Tidak bisa print Jaminan.",
                                              visitInfo.KdPasien,
                                              visitInfo.KelompokPasien));
                Thread.Sleep(4000);
            }
            else TryPrint(executable, _options);


            if (Settings.Default.UpdateTracer)
                UpdateTracer(visitInfo);
        }

        private void UpdateTracer(PatientVisitInfo visitInfo)
        {
            PrintHelper.WaitUntilPrinted(_options);
            CommitToDatabase(visitInfo);
            Logger.Logger.WriteLog(string.Format("-- Cetak berhasil untuk pasien {0}", visitInfo.KdPasien));
        }

        private static void TryPrint(Report report, Options options)
        {
            if (options.PrintQueue)
                PrintHelper.PrintQueueEmpty(options);
            Print(report);
        }

        private static void UpdateSjp(PatientVisitInfo visitInfo, string noSjp)
        {
            var service = new DatabaseService(SettingsService.GetConnectionString(true));
            bool update = service.GetSjp(visitInfo);

            service = new DatabaseService(SettingsService.GetConnectionString(true));
            service.UpdateSjp(visitInfo, noSjp, update);
        }

        private void CommitToDatabase(PatientVisitInfo visitInfo)
        {
            short status;
            short jaminan;
            short kartuBerobat;
            short tracer;
            var service = new DatabaseService(SettingsService.GetConnectionString(true));
            bool hasPrintStatus = service.GetPrintStatus(visitInfo, out status, out jaminan, out kartuBerobat, out tracer);
            status = _options.Status ? (short)(status + 1) : status;
            jaminan = _options.Pendaftaran ? (short)(jaminan + 1) : jaminan;
            kartuBerobat = _options.KartuBerobat ? (short)(kartuBerobat + 1) : kartuBerobat;
            tracer = _options.Tracer ? (short)(tracer + 1) : tracer;

            service = new DatabaseService(SettingsService.GetConnectionString(true));
            service.UpdatePrintStatus(visitInfo, status, jaminan, kartuBerobat, tracer, hasPrintStatus);
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
            if (_options.Status && !baru.Value)
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

            const string printKey = @"Software\Nuansa\NCI Medismart\3.00\Module\RegRWJ\Print";
            RegistryKey key = Registry.CurrentUser.OpenSubKey(printKey, RegistryKeyPermissionCheck.ReadWriteSubTree) ??
                              Registry.CurrentUser.CreateSubKey(printKey);
            key.SetValue("PrinterBill", report.Printer);

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
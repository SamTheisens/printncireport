﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Printing;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using Printer.Properties;

namespace Printer
{
    public sealed class NCIPrinter
    {
        public const string kelompokJamkesmas = "JAMKESMAS";
        public const string kelompokJamkesda = "JAMKESDA";
        public const string kelompokAskes = "ASKES NEGERI";
        public const string kelompokInHealth = "IN HEALTH";

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
            var visitInfo = new PatientVisitInfo();


            var service = new DatabaseService(SettingsService.GetConnectionString(connectionString));

            if (options.Status || options.KartuBerobat || options.Tracer || !string.IsNullOrEmpty(options.KdPasien))
            {
                if (!string.IsNullOrEmpty(options.KdUnit) && !string.IsNullOrEmpty(options.TglMasuk))
                    visitInfo = service.GetVisitInfo(options);
                else
                    visitInfo = service.GetVisitInfo(string.IsNullOrEmpty(options.KdPasien)
                                                         ? TempFileHelper.ReadStatus(options)
                                                         : options.KdPasien);
            }
            else if (options.Billing)
            {
                string kdKasir;
                string noTransaksi;
                TempFileHelper.ReadBill(out noTransaksi, out kdKasir);
                visitInfo = service.GetVisitInfo(kdKasir, noTransaksi);
                options.KdBagian = visitInfo.KdBagian.Value;
            }

            if (!options.SkipVerify && !GetMauPrint(visitInfo.TglMasuk, visitInfo.Baru))
                return;

            if (!string.IsNullOrEmpty(options.Sjp) && options.Billing)
                UpdateSjp(visitInfo, options.Sjp);

            Logger.Logger.WriteLog(string.Format("Sedang print pasien #{0}. Kelompok pasien: {1}.", visitInfo.KdPasien,
                                          visitInfo.KelompokPasien)
                                          );
            TempFileHelper.WriteTempFileStatus(visitInfo.KdPasien, options.KdBagian);

            string executable = (options.KdBagian == 3)
                                    ? SettingsService.GetExecutableUgd(options, visitInfo.KelompokPasien)
                                    : SettingsService.GetExecutableTpp(options, visitInfo.KelompokPasien);

            CheckPrintQueue(options.Tracer ? "PrinterKartu" : "PrinterStatus");
            if (executable.Length == 0)
            {
                Logger.Logger.WriteLog(string.Format("Maaf. Pasien {0} pasien {1}. Tidak bisa print Jaminan.",
                                              visitInfo.KdPasien,
                                              visitInfo.KelompokPasien));
                Thread.Sleep(4000);
            }
            else Print(executable);

            if (Settings.Default.UpdateTracer)
                UpdateTracer(visitInfo);
        }

        private void UpdateSjp(PatientVisitInfo visitInfo, string noSJP)
        {
            var service = new DatabaseService(SettingsService.GetConnectionString(connectionString));
            bool update = service.GetSjp(visitInfo);

            service = new DatabaseService(SettingsService.GetConnectionString(connectionString));
            service.UpdateSjp(visitInfo, noSJP, update);
        }

        private void UpdateTracer(PatientVisitInfo visitInfo)
        {
            short status;
            short jaminan;
            short kartu_berobat;
            short tracer;
            var service = new DatabaseService(SettingsService.GetConnectionString(connectionString));
            bool hasPrintStatus = service.GetPrintStatus(visitInfo, out status, out jaminan, out kartu_berobat, out tracer);
            status = options.Status ? (short)(status + 1) : status;
            jaminan = options.Billing ? (short)(jaminan + 1) : jaminan;
            kartu_berobat = options.KartuBerobat ? (short)(kartu_berobat + 1) : kartu_berobat;
            tracer = options.Tracer ? (short)(tracer + 1) : tracer;

            service = new DatabaseService(SettingsService.GetConnectionString(connectionString));
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


        private static void Print(string executable)
        {
            string programPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                                              Settings.Default.ProgramFolder + @"\");

            string fileName = Path.Combine(programPath, executable);

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

        private static bool CheckPrintQueue(string regkey)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(
                @"Software\Nuansa\NCI Medismart\3.00\Module\BillRWJ\Print", RegistryKeyPermissionCheck.ReadSubTree);
            string printer = key.GetValue(regkey).ToString();

            PrintQueueCollection printQueues;
            using (var server = new PrintServer())
            {
                printQueues = server.GetPrintQueues();
            }
            foreach (PrintQueue pq in printQueues)
            {
                Thread.Sleep(200); // 2 seconds of "Application.DoEvents(), not thread sleep
                if (pq.Name == printer)
                {
                    try
                    {
                        if (pq.NumberOfJobs > 0)
                        {
                            DateTime Bailout = DateTime.Now.AddSeconds(10);
                            string ErrMsg = "notyetretreived";
                            while (Bailout > DateTime.Now && ErrMsg != string.Empty)
                            {
                                try
                                {
                                    var Jobs = pq.GetPrintJobInfoCollection();
                                    Thread.Sleep(500);
                                    foreach (PrintSystemJobInfo Job in Jobs)
                                    {
                                        ErrMsg = string.Empty;
                                        if (Job.Name.Contains("Tracer") || Job.Name.Contains("Status"))
                                            return false;
                                    }
                                }
                                catch (Exception k)
                                {
                                    ErrMsg = k.Message;
                                    Logger.Logger.WriteLog(string.Format("{0}: {1}", pq.Name, k.Message));
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Logger.WriteLog(ex.Message);
                    }
                    Logger.Logger.WriteLog(string.Format("\t{0}\t{1}", pq.Name, DateTime.Now.ToString("HH:mm:ss.fff")));
                }
            }
            return true;
        }
    }
}
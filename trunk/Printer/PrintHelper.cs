using System;
using System.Printing;
using System.Threading;
using System.Windows.Forms;
using Printer.Properties;

namespace Printer
{
    public class PrintException : Exception
    {
        public string Printer { get; private set; }

        public PrintException(string printer, string message) : base(message)
        {
            Printer = printer;
        }
    }
    public class PrintHelper
    {
        public static void WaitUntilPrinted(Options options)
        {
            // wait until queue entered
            for (int i = 0; !CheckPrintQueue(options.Tracer ? Settings.Default.TracerPrinter : Settings.Default.StatusPrinter); i++)
            {
                if (i % 10 == 0) Logger.Logger.WriteLog(string.Format("Check masuk print queue {0}: {1}", i,
                                                     options.Tracer
                                                         ? Settings.Default.TracerPrinter
                                                         : Settings.Default.StatusPrinter));
                Thread.Sleep(100);
                if (i > Settings.Default.InQueueTimeout * 10)
                    ThrowPrintException(options, "tunggu masuk antri");
            }
            // wait until out of queue
            for (int i = 0; CheckPrintQueue(options.Tracer ? Settings.Default.TracerPrinter : Settings.Default.StatusPrinter); i++)
            {
                if (i % 10 == 0) Logger.Logger.WriteLog(string.Format("Check keluar print queue {0}: {1}", i,
                                                     options.Tracer
                                                         ? Settings.Default.TracerPrinter
                                                         : Settings.Default.StatusPrinter));
                Thread.Sleep(100);
                if (i > Settings.Default.OutQueueTimeout * 10)
                    ThrowPrintException(options, "tunggu keluar antri");
            }
        }

        public static void PrintQueueEmpty(Options options)
        {
            if (CheckPrintQueue(options.Tracer ? Settings.Default.TracerPrinter : Settings.Default.StatusPrinter))
                ThrowPrintException(options, "cek antrian kosong");
        }

        private static void ThrowPrintException(Options options, string position)
        {
            throw new PrintException(options.Tracer ? "Tracer" : "Status",
                                     string.Format("Di {1}, printernya macet. Tadi coba cetak untuk pasien {0}", options.KdPasien, position));
        }


        private static bool CheckPrintQueue(string printer)
        {
            try
            {
                PrintQueueCollection printQueues;
                using (var server = new PrintServer())
                {
                    printQueues = server.GetPrintQueues();
                }
                foreach (PrintQueue pq in printQueues)
                {
                    Application.DoEvents();
                    pq.Refresh();
                    if (pq.IsOffline)
                        return false;
                    if (pq.Name == printer)
                    {
                        var Jobs = pq.GetPrintJobInfoCollection();
                        foreach (PrintSystemJobInfo job in Jobs)
                        {
                            if (job.Name.Contains("Tracer") || job.Name.Contains("Status"))
                                return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Logger.WriteLog(string.Format("message: {0} source: {1}", ex.Message, ex.Source));
            }
            return false;
        }
    }
}

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
                Thread.Sleep(1000);
                if (i > Settings.Default.InQueueTimeout)
                    ThrowPrintException(options);
            }
            // wait until out of queue
            for (int i = 0; CheckPrintQueue(options.Tracer ? Settings.Default.TracerPrinter : Settings.Default.StatusPrinter); i++)
            {
                Thread.Sleep(1000);
                if (i > Settings.Default.OutQueueTimeout)
                    ThrowPrintException(options);
            }
        }

        public static void PrintQueueEmpty(Options options)
        {
            if (CheckPrintQueue(options.Tracer ? Settings.Default.TracerPrinter : Settings.Default.StatusPrinter))
                ThrowPrintException(options);
        }

        private static void ThrowPrintException(Options options)
        {
            throw new PrintException(options.Tracer ? "Tracer" : "Status",
                                     string.Format("Printernya macet. Tadi coba cetak untuk pasien {0}", options.KdPasien));
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

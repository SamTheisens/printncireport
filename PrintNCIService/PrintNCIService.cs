using System;
using System.Data;
using System.ServiceProcess;
using System.Threading;
using Printer;
using PrintNCIService.Properties;

namespace PrintNCIService
{
    public partial class PrintNCIService : ServiceBase
    {
        private Timer timer;
        private PrintTimer printTimer;
        public PrintNCIService()
        {
            InitializeComponent();
            printTimer = new PrintTimer();
        }
        public void Test()
        {
            new PrintTimer().Check(null);
        }

        protected override void OnStart(string[] args)
        {
            timer = new Timer(printTimer.Check, null, 3000, Settings.Default.PollingFrequencyInMillis);
        }

        protected override void OnStop()
        {
            timer.Dispose();
        }
    }
    class PrintTimer
    {
        private bool lastTracer;
        public void Check(object sender)
        {
            if (lastTracer)
            {
                PrintStatus();
                lastTracer = false;
            }
            else
            {
                PrintTracer();
                lastTracer = true;
            }
        }
        private static void PrintTracer()
        {
            var reader =
                GetReader("SELECT TOP 1 * FROM {0} WHERE (TRACER = 0 OR TRACER IS NULL) AND BARU = 0 ORDER BY Jam ASC");
            if (!reader.Read())
                return;
            var options = GetOptions(reader);
            options.Tracer = true;
            options.SkipVerify = true;
            new NCIPrinter(options, GetConnctionString()).Print();
        }

        private static void PrintStatus()
        {
            var reader = GetReader("SELECT TOP 1 * FROM {0} WHERE (STATUS = 0 OR STATUS IS NULL) AND BARU = 1 ORDER BY Jam ASC");
            if (!reader.Read())
                return;
            var options = GetOptions(reader);
            options.Status = true;
            options.SkipVerify = true;
            new NCIPrinter(options, GetConnctionString()).Print();
        }

        private static IDataReader GetReader(string query)
        {
            var service = new DatabaseService(GetConnctionString());
            return service.ExecuteQuery(string.Format(query, Settings.Default.TracerView));
        }

        private static Options GetOptions(IDataRecord reader)
        {

            var options = new Options
            {
                KdPasien = reader["KD_PASIEN"].ToString(),
                KdUnit = reader["KD_UNIT"].ToString(),
                UrutMasuk = (short)reader["URUT_MASUK"],
                KdBagian = (byte)reader["KD_BAGIAN"],
                TglMasuk = reader["TGL_MASUK"].ToString(),
            };
            return options;
        }

        private static string GetConnctionString()
        {
            return
                String.Format(
                    "Data Source={0};Initial Catalog={1};Locale Identifier=1033;Connect Timeout=15;Use Procedure for Prepare=1;",
                    Settings.Default.ServerHostName, Settings.Default.DatabaseName) +
                "Auto Translate=True;Packet Size=4096;Provider=SQLOLEDB.1;User ID=sa;Password=pocopoco";
        }
    }
}



using System;
using System.Data;
using System.ServiceProcess;
using Printer;
using PrintNCIService.Properties;

namespace PrintNCIService
{
    public partial class PrintNCIService : ServiceBase
    {
        public PrintNCIService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer.Interval = Settings.Default.PollingFrequencyInMillis;
            timer.Start();
            timer.Tick += timer_Tick;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                PrintStatus();
                PrintTracer();
            }
            catch (Exception ex)
            {
                eventLog.WriteEntry(string.Format("Error dengan print: {0} ", ex.Message));
            }
        }

        private static void PrintTracer()
        {
            var options = GetOptions(GetReader("SELECT TOP 1 * FROM {0} WHERE TRACER = 0 "));
            options.Tracer = true;
            new NCIPrinter(options).Print();
        }

        private static void PrintStatus()
        {
            var options = GetOptions(GetReader("SELECT TOP 1 * FROM {0} WHERE STATUS = 0 "));
            options.Status = true;
            new NCIPrinter(options).Print();
        }

        private static IDataReader GetReader(string query)
        {
            var service = new DatabaseService(SettingsService.GetConnectionString(""));
            return service.ExecuteQuery(string.Format(query, Settings.Default.TracerView));
        }

        private static Options GetOptions(IDataReader reader)
        {
            reader.Read();
            var visitInfo = DatabaseService.FillVisitInfo(reader);
            var options = new Options();
            options.Tracer = true;
            options.KdPasien = visitInfo.KdPasien;
            options.KdUnit = visitInfo.KdUnit;
            options.UrutMasuk = visitInfo.UrutMasuk.Value;
            options.KdBagian = visitInfo.KdBagian.Value;
            return options;
        }

        protected override void OnStop()
        {
            timer.Stop();
        }
    }
}

using System;
using System.Data;
using System.ServiceModel;
using System.ServiceModel.Description;
using Printer;
using PrintNCIService.Properties;

namespace PrintNCIService
{
    class PrintTimer
    {
        private bool lastTracer;
        private readonly PrintService service;
        private ServiceHost serviceHost;
        public PrintTimer()
        {
            service = new PrintService();
            StartService();

        }
        public void Stop()
        {
            if (serviceHost == null)
                return;

            serviceHost.Close();
            serviceHost = null;

        }
        public void Check(object sender)
        {
            if (lastTracer && Settings.Default.CetakStatus)
            {
                PrintStatus();
                lastTracer = false;
            }
            else if (Settings.Default.CetakTracer)
            {
                PrintTracer();
                lastTracer = true;
            }
        }

        private void PrintTracer()
        {
            try
            {
                var reader =
                    GetReader(
                        "SELECT TOP 1 * FROM {0} WHERE (TRACER = 0 OR TRACER IS NULL) AND BARU = 0 ORDER BY Jam ASC");
                if (!reader.Read())
                    return;
                var options = GetOptions(reader);
                options.Tracer = true;
                options.SkipVerify = true;
                new NCIPrinter(options, GetConnctionString()).Print();
            }
            catch (PrintException pe)
            {
                var errorString = string.Format("{0}: {1}", pe.Printer, pe.Message);
                service.TracerQueue = errorString;
                Logger.Logger.WriteLog(string.Format("Print Error {0}", errorString));
                return;
            }
            service.TracerQueue = string.Empty;
        }

        private void PrintStatus()
        {
            try
            {
                var reader =
                    GetReader(
                        "SELECT TOP 1 * FROM {0} WHERE (STATUS = 0 OR STATUS IS NULL) AND BARU = 1 ORDER BY Jam ASC");
                if (!reader.Read())
                    return;
                var options = GetOptions(reader);
                options.Status = true;
                options.SkipVerify = true;
                new NCIPrinter(options, GetConnctionString()).Print();
            }
            catch (PrintException pe)
            {
                var errorString = string.Format("{0}: {1}", pe.Printer, pe.Message);
                service.StatusQueue = errorString;
                Logger.Logger.WriteLog(string.Format("Print Error {0}", errorString));
                return;
            }
            service.StatusQueue = string.Empty;
        }

        private static IDataReader GetReader(string query)
        {
            var dbService = new DatabaseService(GetConnctionString());
            return dbService.ExecuteQuery(string.Format(query, Settings.Default.TracerView));
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

        private void StartService()
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
            }

            serviceHost = new ServiceHost(service);
            serviceHost.AddServiceEndpoint(typeof(IPrintService), new WSHttpBinding(),
                                           "http://localhost:8000/PrintNCIService/service");
            serviceHost.Description.Behaviors.Find<ServiceDebugBehavior>().IncludeExceptionDetailInFaults = true;
            serviceHost.Description.Behaviors.Add(new ServiceMetadataBehavior { HttpGetEnabled = false });
            serviceHost.Open(TimeSpan.FromSeconds(3.0));
        }
    }
}

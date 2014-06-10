using System.Data;
using CrystalDecisions.CrystalReports.Engine;


namespace Printer.Services
{
    class ReportService
    {
        private readonly DatabaseService _databaseService;
        public ReportService()
        {
            _databaseService = new DatabaseService(SettingsService.GetConnectionString());
        }

        public ReportDocument CreateReport(Report reportParam)
        {
            var report = new ReportDocument();
            report.Load(reportParam.FileName);
            var
                reader = DatabaseService.ExecuteQuery(string.Format("EXEC {0} '{1}'", reportParam.Procedure, reportParam.Parameter));
            var dataTable = new DataTable(report.Database.Tables[0].Name);
            dataTable.Load(reader);
            report.SetDataSource(dataTable);
            return report;
        }

        public void PrintReport(Report reportParam)
        {
            var report = CreateReport(reportParam);
            report.PrintOptions.PrinterName = reportParam.Printer;
            report.PrintToPrinter(1, false, 0, 1);
        }

        private DatabaseService DatabaseService
        {
            get { return _databaseService; }
        }


    }
}

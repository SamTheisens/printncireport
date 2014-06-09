using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;

namespace Printer
{
    public partial class PrintPreviewForm : Form
    {
        private readonly string _currentStoredProcedure;
        private readonly string _currentReportFile;
        private readonly ReportDocument _report = new ReportDocument();

        public PrintPreviewForm()
        {
            InitializeComponent();
        }

        public PrintPreviewForm(string currentStoredProcedure, string currentReportFile)
        {
            _currentStoredProcedure = currentStoredProcedure;
            _currentReportFile = currentReportFile;

            InitializeComponent();
        }

        private void PrintPreviewFormLoad(object sender, EventArgs e)
        {

        }

        private void PreviewButtonClick(object sender, EventArgs e)
        {

            try
            {
                _report.Load(_currentReportFile);

                var service = new DatabaseService(SettingsService.GetConnectionString());


                IDataReader
                    reader = service.ExecuteQuery(string.Format("EXEC {0} '{1}'", _currentStoredProcedure, spParameter.Text));
                

                var dataTable = new DataTable(_report.Database.Tables[0].Name);

                dataTable.Load(reader);


                _report.SetDataSource(dataTable);


                crystalReportViewer1.ReportSource = _report;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(CultureInfo.CurrentCulture, ex.Message));
            }
        }

        


    }
}

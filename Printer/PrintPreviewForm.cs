using System;
using System.ComponentModel;
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
        private readonly BackgroundWorker _worker = new BackgroundWorker();
        private readonly ReportDocument _report = new ReportDocument();

        public PrintPreviewForm()
        {
            InitializeComponent();
        }

        public PrintPreviewForm(string currentStoredProcedure, string currentReportFile)
        {
            _currentStoredProcedure = currentStoredProcedure;
            _currentReportFile = currentReportFile;

            _worker.DoWork += worker_DoWork;
            _worker.RunWorkerCompleted += _worker_RunWorkerCompleted;
           

            InitializeComponent();
            
        }

        void _worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var report = new ReportDocument();
            report.Load(_currentReportFile);
            var dataset = e.Result as IDataReader;


            report.SetDataSource(dataset);
            crystalReportViewer1.ReportSource = report;

        }

        private void PrintPreviewForm_Load(object sender, EventArgs e)
        {


//            this.reportViewer.RefreshReport();
        }

        private void previewButton_Click(object sender, EventArgs e)
        {


            try
            {
                _report.Load(_currentReportFile);

                var service = new DatabaseService(SettingsService.GetConnectionString());


                IDataReader
                    reader = service.ExecuteQuery(string.Format("EXEC {0} '{1}'", _currentStoredProcedure, spParameter.Text));
                reader.Read();

                var dataTable = new DataTable(_report.Database.Tables[0].Name);

                dataTable.Load(reader);


                _report.SetDataSource(dataTable);


                crystalReportViewer1.ReportSource = _report;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(CultureInfo.CurrentCulture, ex.Message));
            }
            this.previewButton.Invoke((Action)(() =>
                                          {

                                          }));

            //this.reportViewer.LocalReport.ReportPath = _currentReportFile;
            //this.reportViewer.RefreshReport();

        }

        

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            var service = new DatabaseService(SettingsService.GetConnectionString());


            IDataReader
                dataset = service.ExecuteQuery(string.Format("EXEC {0} '{1}'", _currentStoredProcedure, spParameter.Text));
            e.Result = dataset;
        }


    }
}

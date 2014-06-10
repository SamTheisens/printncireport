using System;
using System.Globalization;
using System.Windows.Forms;
using Printer.Services;

namespace Printer.Forms
{
    public partial class PrintPreviewForm : Form
    {
        private readonly ReportService _reportService;
        private Report _report;

        public PrintPreviewForm()
        {
            InitializeComponent();
        }

        public PrintPreviewForm(Report report)
        {
            _report = report;
            _reportService = new ReportService();

            InitializeComponent();
        }

        private void PrintPreviewFormLoad(object sender, EventArgs e)
        {

        }

        private void PreviewButtonClick(object sender, EventArgs e)
        {
            try
            {
                crystalReportViewer1.ReportSource = _reportService.CreateReport(_report);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(CultureInfo.CurrentCulture, ex.Message));
            }
        }

        private void PrintButtonClick(object sender, EventArgs e)
        {
            try
            {

             _reportService.PrintReport(_report);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(CultureInfo.CurrentCulture, ex.Message));
            }
        }

        private void SpParameterTextChanged(object sender, EventArgs e)
        {
            _report.Parameter = spParameter.Text;
        }

        


    }
}

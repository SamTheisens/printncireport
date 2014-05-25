using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Printer
{
    public partial class PrintPreviewForm : Form
    {
        private readonly string _currentStoredProcedure;
        private readonly string _currentReportFile;

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

        private void PrintPreviewForm_Load(object sender, EventArgs e)
        {

            this.reportViewer.RefreshReport();
        }

        private void previewButton_Click(object sender, EventArgs e)
        {
           
        }


    }
}

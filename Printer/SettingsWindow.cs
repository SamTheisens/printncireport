using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Printer.Properties;

namespace Printer
{
    public partial class SettingsWindow : Form
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private string _reportName;
        private bool _loaded;
        private string _kasir;
        private bool _pendaftaran;
        

        public String CurrentStoredProcedure
        {
            get { return dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[3].Value.ToString(); }
        }

        public String CurrentReportFile
        {
            get { return SettingsService.GetReportFolder() + _reportName; }
        }

        private void SettingsWindow_Load(object sender, EventArgs e)
        {
            
            propertyGrid.SelectedObject = Settings.Default;

            try
            {
                var service = new DatabaseService(SettingsService.GetConnectionString());
                service.ExecuteQuery(File.ReadAllText("RSUD_REPORTS.sql"));
                kasirTableAdapter.Connection.ConnectionString = SettingsService.GetConnectionString(false);
                cUSTOMERTableAdapter.Connection.ConnectionString = SettingsService.GetConnectionString(false);
                proceduresTableAdapter.Connection.ConnectionString = SettingsService.GetConnectionString(false);
                kasirTableAdapter.Connection.ConnectionString = SettingsService.GetConnectionString(false);
                reportsTableAdapter.Connection.ConnectionString = SettingsService.GetConnectionString(false);

                SetKasir();
                _pendaftaran = Settings.Default.Pendaftaran;

                printerListBindingSource.DataSource = new BindingSource(new PrinterList(), null);
                cUSTOMERTableAdapter.Fill(rSKUPANGDataSet.CUSTOMER);
                kasirTableAdapter.Fill(rSKUPANGDataSet.KasirTable);
                proceduresTableAdapter.Fill(rSKUPANGDataSet.procedures);
                
                kasirComboBox.SelectedValue = _kasir;
                billingRadioButton.Checked = !_pendaftaran;
                pendaftaranRadioButton.Checked = _pendaftaran;
                UpdateReportsTable();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            _loaded = true;
        }

        private void SetKasir()
        {
            _kasir = string.Format("{0,2:D2}", Settings.Default.Kasir);
        }

        private void SettingsWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.Save();
        }

        private void dataGridView1_EditingControlShowing_1(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            var comboBox = e.Control as ComboBox;
            if (comboBox != null)
            {
                comboBox.SelectedIndexChanged -= comboBox_SelectedIndexChanged;
                comboBox.SelectedIndexChanged += comboBox_SelectedIndexChanged;
            }
        }

        void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var combo = (ComboBox) sender;
            if (!isInterestingEvent(combo))
                return;

            string kdCustomerReport;
            string namaSp;
            string printer;
            var kdCustomer = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString();
            var kdKasir = kasirComboBox.SelectedValue.ToString();

            FillVariables(combo, out namaSp, out printer, out kdCustomerReport);

            if (reportsTableAdapter.NeedInsert(kdCustomer, kdKasir, _pendaftaran) == null)
            {
                reportsTableAdapter.InsertReport(kdKasir, _pendaftaran, kdCustomer, kdCustomerReport, namaSp, printer);
            }
            else
            {
                if (string.IsNullOrEmpty(kdCustomerReport) && string.IsNullOrEmpty(printer))
                    reportsTableAdapter.UpdateStoredProcedure(namaSp, kdCustomer, kdKasir, _pendaftaran);

                else if (string.IsNullOrEmpty(kdCustomerReport) && string.IsNullOrEmpty(namaSp))
                    reportsTableAdapter.UpdatePrinter(printer, kdCustomer, kdKasir, _pendaftaran);

                else
                    reportsTableAdapter.UpdateCustomerReport(kdCustomerReport,  kdCustomer, kdKasir, _pendaftaran);
            }
        }
        private Boolean isInterestingEvent(ComboBox combo)
        {
            if (combo == null)
                return false;
            if (combo.SelectedValue is string)
                return true;
            if (combo.SelectedValue is KeyValuePair<int, string>)
                return true;
            return false;
        }

        private static void FillVariables(ListControl combo, out string namaSp, out string printer, out string kdCustomerReport)
        {
            if (combo.ValueMember.Equals("KD_CUSTOMER"))
            {
                kdCustomerReport = combo.SelectedValue.ToString();
                namaSp = null;
                printer = null;
            }
            else if (combo.SelectedValue is KeyValuePair<int, string>)
            {
                kdCustomerReport = null;
                namaSp = null;
                printer = ((KeyValuePair<int, string>)combo.SelectedValue).Value;
            }
            else
            {
                printer = null;
                kdCustomerReport = null;
                namaSp = combo.SelectedValue.ToString();
            }
        }

        private void kasirComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            var selectedItem = (ComboBox)sender;
            if (selectedItem.SelectedValue == null)
                return;

            SetKasir();
            UpdateReportsTable();
        }

        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell == null)
                return;
            var rowIndex = dataGridView1.CurrentCell.RowIndex;
            string kdCustomerReport = dataGridView1.Rows[rowIndex].Cells[2].Value.ToString();
            if (string.IsNullOrEmpty(kdCustomerReport))
            {
                reportTextBox.Text = string.Format("Laporan dan Stored Procedure untuk {0} belum dipilih.",
                                                   dataGridView1.Rows[rowIndex].Cells[0].Value);
            }
            else
            {
                _reportName = SettingsService.CreateReportName(string.Format("{0:D2}", _kasir),
                                                              _pendaftaran, kdCustomerReport);
                reportTextBox.Text = _reportName;
            }
            if (!File.Exists(CurrentReportFile))
            {
                reportPresentTextBox.Text = _reportName + " belum ada di Reports: " + SettingsService.GetReportFolder();
                reportPreviewButton.Enabled = false;
            }
            else
            {
                reportPresentTextBox.Text = "";
                reportPreviewButton.Enabled = true;
            }
        }

        private void buttonLaporan_Click(object sender, EventArgs e)
        {
            var service = new DatabaseService(SettingsService.GetConnectionString());
            
            var dataset = service.ExecuteQuery(string.Format("EXEC {0} ''", CurrentStoredProcedure));

            ttxFolderBrowserDialog.SelectedPath = SettingsService.GetReportFolder();

            DialogResult result = ttxFolderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                var file =
                    new StreamWriter(
                        Path.Combine(ttxFolderBrowserDialog.SelectedPath, _reportName.Replace(".rpt", ".ttx")), false);
                file.Write(service.ExtractTtx(dataset.GetSchemaTable()));
                file.Flush();
                file.Close();
            }
            MessageBox.Show(string.Format("Berhasil membuat .ttx datasource untuk report {0}", _reportName));
        }

        private void billingRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            _pendaftaran = !((RadioButton) sender).Checked;
            SetRadioButtons();
        }

        private void pendaftaranRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            _pendaftaran = ((RadioButton)sender).Checked;
            SetRadioButtons();
        }

        private void SetRadioButtons()
        {
            Settings.Default.Pendaftaran = _pendaftaran;
            pendaftaranRadioButton.Checked = _pendaftaran;
            billingRadioButton.Checked = !_pendaftaran;
            UpdateReportsTable();
        }
        private void UpdateReportsTable()
        {
            textBoxKdUnit.Text = kasirComboBox.SelectedValue.ToString();
            _kasir = kasirComboBox.SelectedValue.ToString();
            reportsTableAdapter.Fill(rSKUPANGDataSet.ReportsTable, _kasir, _pendaftaran);            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void reportPreviewButton_Click(object sender, EventArgs e)
        {
            var form = new PrintPreviewForm(CurrentStoredProcedure, CurrentReportFile);
            form.ShowDialog();
        }

    }
}

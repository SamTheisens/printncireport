using System;
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

        private string reportName;
        private bool loaded;
        private string kasir;
        private bool pendaftaran;

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
                pendaftaran = Settings.Default.Pendaftaran;

                printerListBindingSource.DataSource = new PrinterList();
                cUSTOMERTableAdapter.Fill(rSKUPANGDataSet.CUSTOMER);
                kasirTableAdapter.Fill(rSKUPANGDataSet.KasirTable);
                proceduresTableAdapter.Fill(rSKUPANGDataSet.procedures);
                
                kasirComboBox.SelectedValue = kasir;
                billingRadioButton.Checked = !pendaftaran;
                pendaftaranRadioButton.Checked = pendaftaran;
                UpdateReportsTable();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            loaded = true;
        }

        private void SetKasir()
        {
            kasir = string.Format("{0,2:D2}", Settings.Default.Kasir);
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
            if (combo == null || !(combo.SelectedValue is string))
                return;
            string kdCustomerReport;
            string namaSP;
            string printer;
            var kdCustomer = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString();
            var kdKasir = kasirComboBox.SelectedValue.ToString();

            FillVariables(combo, out namaSP, out printer, out kdCustomerReport);

            if (reportsTableAdapter.NeedInsert(kdCustomer, kdKasir, pendaftaran) == null)
            {
                reportsTableAdapter.InsertReport(kdKasir, pendaftaran, kdCustomer, kdCustomerReport, namaSP, printer);
            }
            else
            {
                if (string.IsNullOrEmpty(kdCustomerReport) && string.IsNullOrEmpty(printer))
                    reportsTableAdapter.UpdateStoredProcedure(namaSP, kdCustomer, kdKasir, pendaftaran);

                else if (string.IsNullOrEmpty(kdCustomerReport) && string.IsNullOrEmpty(namaSP))
                    reportsTableAdapter.UpdatePrinter(printer, kdCustomer, kdKasir, pendaftaran);

                else
                    reportsTableAdapter.UpdateCustomerReport(kdCustomerReport, kdKasir, kdCustomer, pendaftaran);
            }
        }

        private static void FillVariables(ListControl combo, out string namaSP, out string printer, out string kdCustomerReport)
        {
            if (combo.ValueMember.Equals("KD_CUSTOMER"))
            {
                kdCustomerReport = combo.SelectedValue.ToString();
                namaSP = null;
                printer = null;
            }
            else if (combo.ValueMember.Equals("Value"))
            {
                kdCustomerReport = null;
                namaSP = null;
                printer = combo.SelectedValue.ToString();
            }
            else
            {
                printer = null;
                kdCustomerReport = null;
                namaSP = combo.SelectedValue.ToString();
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
                reportName = SettingsService.CreateReportName(string.Format("{0:D2}", Settings.Default.Kasir),
                                                              pendaftaran, kdCustomerReport);
                reportTextBox.Text = reportName;
            }
            if (!File.Exists(SettingsService.GetReportFolder() + reportName))
            {
                reportPresentTextBox.Text = reportName + " belum ada di Reports";
            }
            else
            {
                reportPresentTextBox.Text = "";
            }
        }

        private void buttonLaporan_Click(object sender, EventArgs e)
        {
            var service = new DatabaseService(SettingsService.GetConnectionString());
            var dataset = service.ExecuteQuery(string.Format("EXEC {0} ''",
                                               dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[3].Value));

            ttxFolderBrowserDialog.SelectedPath = SettingsService.GetReportFolder();

            DialogResult result = ttxFolderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                var file =
                    new StreamWriter(
                        Path.Combine(ttxFolderBrowserDialog.SelectedPath, reportName.Replace(".rpt", ".ttx")), false);
                file.Write(service.ExtractTtx(dataset.GetSchemaTable()));
                file.Flush();
                file.Close();
            }
            MessageBox.Show(string.Format("Berhasil membuat .ttx datasource untuk report {0}", reportName));
        }

        private void billingRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            pendaftaran = !((RadioButton) sender).Checked;
            SetRadioButtons();
        }

        private void pendaftaranRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            pendaftaran = ((RadioButton)sender).Checked;
            SetRadioButtons();
        }

        private void SetRadioButtons()
        {
            Settings.Default.Pendaftaran = pendaftaran;
            pendaftaranRadioButton.Checked = pendaftaran;
            billingRadioButton.Checked = !pendaftaran;
            UpdateReportsTable();
        }
        private void UpdateReportsTable()
        {
            textBoxKdUnit.Text = kasirComboBox.SelectedValue.ToString();
            kasir = kasirComboBox.SelectedValue.ToString();
            reportsTableAdapter.Fill(rSKUPANGDataSet.ReportsTable, kasir, pendaftaran);            
        }

    }
}

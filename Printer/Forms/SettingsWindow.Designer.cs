namespace Printer.Forms
{
    partial class SettingsWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TabPage tabKelompok;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsWindow));
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.reportPreviewButton = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.reportPresentTextBox = new System.Windows.Forms.TextBox();
            this.buttonLaporan = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.reportTextBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.KdCustomer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cUSTOMERDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.KD_CUSTOMER_REPORT = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.cUSTOMERBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.rSKUPANGDataSet = new Printer.RSKUPANGDataSet();
            this.NAMA_SP = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.proceduresBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.Printer = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.printerListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.reportsTableBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.kasirComboBox = new System.Windows.Forms.ComboBox();
            this.kasirTableBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.rSKUPANGDataSetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.textBoxKdUnit = new System.Windows.Forms.TextBox();
            this.billingRadioButton = new System.Windows.Forms.RadioButton();
            this.pendaftaranRadioButton = new System.Windows.Forms.RadioButton();
            this.reportsTableBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.proceduresBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.modulTabControl = new System.Windows.Forms.TabControl();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.proceduresTableAdapter = new Printer.RSKUPANGDataSetTableAdapters.proceduresTableAdapter();
            this.reportsTableAdapter = new Printer.RSKUPANGDataSetTableAdapters.ReportsTableAdapter();
            this.rSKUPANGDataSetBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.cUSTOMERTableAdapter = new Printer.RSKUPANGDataSetTableAdapters.CUSTOMERTableAdapter();
            this.ttxFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.kasirTableAdapter = new Printer.RSKUPANGDataSetTableAdapters.KasirTableAdapter();
            tabKelompok = new System.Windows.Forms.TabPage();
            tabKelompok.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cUSTOMERBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rSKUPANGDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.proceduresBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.printerListBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportsTableBindingSource)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kasirTableBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rSKUPANGDataSetBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportsTableBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.proceduresBindingSource)).BeginInit();
            this.modulTabControl.SuspendLayout();
            this.tabSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rSKUPANGDataSetBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabKelompok
            // 
            tabKelompok.Controls.Add(this.groupBox5);
            tabKelompok.Controls.Add(this.groupBox4);
            tabKelompok.Controls.Add(this.groupBox3);
            tabKelompok.Controls.Add(this.groupBox2);
            tabKelompok.Controls.Add(this.groupBox1);
            tabKelompok.Location = new System.Drawing.Point(4, 22);
            tabKelompok.Name = "tabKelompok";
            tabKelompok.Padding = new System.Windows.Forms.Padding(3);
            tabKelompok.Size = new System.Drawing.Size(738, 437);
            tabKelompok.TabIndex = 0;
            tabKelompok.Text = "Modul & Kelompok";
            tabKelompok.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.reportPreviewButton);
            this.groupBox5.Location = new System.Drawing.Point(613, 358);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(107, 67);
            this.groupBox5.TabIndex = 7;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Crystal Report";
            // 
            // reportPreviewButton
            // 
            this.reportPreviewButton.Location = new System.Drawing.Point(7, 20);
            this.reportPreviewButton.Name = "reportPreviewButton";
            this.reportPreviewButton.Size = new System.Drawing.Size(94, 37);
            this.reportPreviewButton.TabIndex = 0;
            this.reportPreviewButton.Text = "Preview";
            this.reportPreviewButton.UseVisualStyleBackColor = true;
            this.reportPreviewButton.Click += new System.EventHandler(this.ReportPreviewButtonClick);
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.reportPresentTextBox);
            this.groupBox4.Controls.Add(this.buttonLaporan);
            this.groupBox4.Location = new System.Drawing.Point(252, 358);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(355, 68);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "TTX";
            // 
            // reportPresentTextBox
            // 
            this.reportPresentTextBox.BackColor = System.Drawing.SystemColors.ControlLight;
            this.reportPresentTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.reportPresentTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.reportPresentTextBox.ForeColor = System.Drawing.Color.Red;
            this.reportPresentTextBox.Location = new System.Drawing.Point(6, 19);
            this.reportPresentTextBox.Multiline = true;
            this.reportPresentTextBox.Name = "reportPresentTextBox";
            this.reportPresentTextBox.ReadOnly = true;
            this.reportPresentTextBox.Size = new System.Drawing.Size(261, 38);
            this.reportPresentTextBox.TabIndex = 4;
            // 
            // buttonLaporan
            // 
            this.buttonLaporan.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLaporan.Location = new System.Drawing.Point(273, 19);
            this.buttonLaporan.Name = "buttonLaporan";
            this.buttonLaporan.Size = new System.Drawing.Size(76, 38);
            this.buttonLaporan.TabIndex = 3;
            this.buttonLaporan.Text = "Bikin .ttx";
            this.buttonLaporan.UseVisualStyleBackColor = true;
            this.buttonLaporan.Click += new System.EventHandler(this.ButtonLaporanClick);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox3.Controls.Add(this.reportTextBox);
            this.groupBox3.Location = new System.Drawing.Point(6, 358);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(240, 68);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Crystal Report";
            // 
            // reportTextBox
            // 
            this.reportTextBox.BackColor = System.Drawing.SystemColors.ControlLight;
            this.reportTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.reportTextBox.Location = new System.Drawing.Point(9, 19);
            this.reportTextBox.Multiline = true;
            this.reportTextBox.Name = "reportTextBox";
            this.reportTextBox.ReadOnly = true;
            this.reportTextBox.Size = new System.Drawing.Size(225, 38);
            this.reportTextBox.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.dataGridView1);
            this.groupBox2.Location = new System.Drawing.Point(6, 67);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(724, 285);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Kelompok Pasien";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.KdCustomer,
            this.cUSTOMERDataGridViewTextBoxColumn,
            this.KD_CUSTOMER_REPORT,
            this.NAMA_SP,
            this.Printer});
            this.dataGridView1.DataSource = this.reportsTableBindingSource;
            this.dataGridView1.Location = new System.Drawing.Point(9, 19);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(706, 260);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView1CellContentClick);
            this.dataGridView1.CurrentCellChanged += new System.EventHandler(this.DataGridView1CurrentCellChanged);
            this.dataGridView1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.DataGridView1DataError);
            this.dataGridView1.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.DataGridView1EditingControlShowing1);
            // 
            // KdCustomer
            // 
            this.KdCustomer.DataPropertyName = "KD_CUSTOMER";
            this.KdCustomer.HeaderText = "Kd Customer";
            this.KdCustomer.Name = "KdCustomer";
            this.KdCustomer.Visible = false;
            // 
            // cUSTOMERDataGridViewTextBoxColumn
            // 
            this.cUSTOMERDataGridViewTextBoxColumn.DataPropertyName = "CUSTOMER";
            this.cUSTOMERDataGridViewTextBoxColumn.HeaderText = "Customer";
            this.cUSTOMERDataGridViewTextBoxColumn.Name = "cUSTOMERDataGridViewTextBoxColumn";
            this.cUSTOMERDataGridViewTextBoxColumn.ReadOnly = true;
            this.cUSTOMERDataGridViewTextBoxColumn.Width = 150;
            // 
            // KD_CUSTOMER_REPORT
            // 
            this.KD_CUSTOMER_REPORT.DataPropertyName = "KD_CUSTOMER_REPORT";
            this.KD_CUSTOMER_REPORT.DataSource = this.cUSTOMERBindingSource;
            this.KD_CUSTOMER_REPORT.DisplayMember = "CUSTOMER";
            this.KD_CUSTOMER_REPORT.DropDownWidth = 200;
            this.KD_CUSTOMER_REPORT.HeaderText = "Report";
            this.KD_CUSTOMER_REPORT.Name = "KD_CUSTOMER_REPORT";
            this.KD_CUSTOMER_REPORT.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.KD_CUSTOMER_REPORT.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.KD_CUSTOMER_REPORT.ValueMember = "KD_CUSTOMER";
            this.KD_CUSTOMER_REPORT.Width = 150;
            // 
            // cUSTOMERBindingSource
            // 
            this.cUSTOMERBindingSource.DataMember = "CUSTOMER";
            this.cUSTOMERBindingSource.DataSource = this.rSKUPANGDataSet;
            // 
            // rSKUPANGDataSet
            // 
            this.rSKUPANGDataSet.DataSetName = "RSKUPANGDataSet";
            this.rSKUPANGDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // NAMA_SP
            // 
            this.NAMA_SP.DataPropertyName = "NAMA_SP";
            this.NAMA_SP.DataSource = this.proceduresBindingSource1;
            this.NAMA_SP.DisplayMember = "name";
            this.NAMA_SP.HeaderText = "Nama Stored Procedure";
            this.NAMA_SP.Name = "NAMA_SP";
            this.NAMA_SP.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.NAMA_SP.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.NAMA_SP.ValueMember = "name";
            this.NAMA_SP.Width = 160;
            // 
            // proceduresBindingSource1
            // 
            this.proceduresBindingSource1.DataMember = "procedures";
            this.proceduresBindingSource1.DataSource = this.rSKUPANGDataSet;
            // 
            // Printer
            // 
            this.Printer.DataPropertyName = "PRINTER";
            this.Printer.DataSource = this.printerListBindingSource;
            this.Printer.HeaderText = "Printer";
            this.Printer.Name = "Printer";
            this.Printer.Width = 200;
            // 
            // printerListBindingSource
            // 
            this.printerListBindingSource.DataSource = typeof(Printer.PrinterList);
            // 
            // reportsTableBindingSource
            // 
            this.reportsTableBindingSource.DataMember = "ReportsTable";
            this.reportsTableBindingSource.DataSource = this.rSKUPANGDataSet;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.flowLayoutPanel1);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(724, 55);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Modul";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.kasirComboBox);
            this.flowLayoutPanel1.Controls.Add(this.textBoxKdUnit);
            this.flowLayoutPanel1.Controls.Add(this.billingRadioButton);
            this.flowLayoutPanel1.Controls.Add(this.pendaftaranRadioButton);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(718, 36);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // kasirComboBox
            // 
            this.kasirComboBox.DataSource = this.kasirTableBindingSource;
            this.kasirComboBox.DisplayMember = "DESKRIPSI";
            this.kasirComboBox.FormattingEnabled = true;
            this.kasirComboBox.Location = new System.Drawing.Point(3, 3);
            this.kasirComboBox.Name = "kasirComboBox";
            this.kasirComboBox.Size = new System.Drawing.Size(231, 21);
            this.kasirComboBox.TabIndex = 3;
            this.kasirComboBox.ValueMember = "KD_KASIR";
            this.kasirComboBox.SelectedIndexChanged += new System.EventHandler(this.KasirComboBoxSelectedValueChanged);
            // 
            // kasirTableBindingSource
            // 
            this.kasirTableBindingSource.DataMember = "KasirTable";
            this.kasirTableBindingSource.DataSource = this.rSKUPANGDataSetBindingSource;
            // 
            // rSKUPANGDataSetBindingSource
            // 
            this.rSKUPANGDataSetBindingSource.DataSource = this.rSKUPANGDataSet;
            this.rSKUPANGDataSetBindingSource.Position = 0;
            // 
            // textBoxKdUnit
            // 
            this.textBoxKdUnit.Enabled = false;
            this.textBoxKdUnit.Location = new System.Drawing.Point(240, 3);
            this.textBoxKdUnit.Name = "textBoxKdUnit";
            this.textBoxKdUnit.Size = new System.Drawing.Size(30, 20);
            this.textBoxKdUnit.TabIndex = 4;
            // 
            // billingRadioButton
            // 
            this.billingRadioButton.AutoSize = true;
            this.billingRadioButton.Location = new System.Drawing.Point(276, 3);
            this.billingRadioButton.Name = "billingRadioButton";
            this.billingRadioButton.Size = new System.Drawing.Size(52, 17);
            this.billingRadioButton.TabIndex = 1;
            this.billingRadioButton.TabStop = true;
            this.billingRadioButton.Text = "Billing";
            this.billingRadioButton.UseVisualStyleBackColor = true;
            this.billingRadioButton.CheckedChanged += new System.EventHandler(this.BillingRadioButtonCheckedChanged);
            // 
            // pendaftaranRadioButton
            // 
            this.pendaftaranRadioButton.AutoSize = true;
            this.pendaftaranRadioButton.Location = new System.Drawing.Point(334, 3);
            this.pendaftaranRadioButton.Name = "pendaftaranRadioButton";
            this.pendaftaranRadioButton.Size = new System.Drawing.Size(83, 17);
            this.pendaftaranRadioButton.TabIndex = 2;
            this.pendaftaranRadioButton.TabStop = true;
            this.pendaftaranRadioButton.Text = "Pendaftaran";
            this.pendaftaranRadioButton.UseVisualStyleBackColor = true;
            this.pendaftaranRadioButton.CheckedChanged += new System.EventHandler(this.PendaftaranRadioButtonCheckedChanged);
            // 
            // reportsTableBindingSource1
            // 
            this.reportsTableBindingSource1.DataMember = "ReportsTable";
            this.reportsTableBindingSource1.DataSource = this.rSKUPANGDataSetBindingSource;
            // 
            // proceduresBindingSource
            // 
            this.proceduresBindingSource.DataMember = "procedures";
            this.proceduresBindingSource.DataSource = this.rSKUPANGDataSet;
            // 
            // modulTabControl
            // 
            this.modulTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.modulTabControl.Controls.Add(tabKelompok);
            this.modulTabControl.Controls.Add(this.tabSettings);
            this.modulTabControl.Location = new System.Drawing.Point(12, 0);
            this.modulTabControl.Name = "modulTabControl";
            this.modulTabControl.SelectedIndex = 0;
            this.modulTabControl.Size = new System.Drawing.Size(746, 463);
            this.modulTabControl.TabIndex = 1;
            // 
            // tabSettings
            // 
            this.tabSettings.Controls.Add(this.propertyGrid);
            this.tabSettings.Location = new System.Drawing.Point(4, 22);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabSettings.Size = new System.Drawing.Size(738, 437);
            this.tabSettings.TabIndex = 1;
            this.tabSettings.Text = "Settings";
            this.tabSettings.UseVisualStyleBackColor = true;
            // 
            // propertyGrid
            // 
            this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid.Location = new System.Drawing.Point(6, 6);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(603, 355);
            this.propertyGrid.TabIndex = 0;
            // 
            // proceduresTableAdapter
            // 
            this.proceduresTableAdapter.ClearBeforeFill = true;
            // 
            // reportsTableAdapter
            // 
            this.reportsTableAdapter.ClearBeforeFill = true;
            // 
            // rSKUPANGDataSetBindingSource1
            // 
            this.rSKUPANGDataSetBindingSource1.DataSource = this.rSKUPANGDataSet;
            this.rSKUPANGDataSetBindingSource1.Position = 0;
            // 
            // cUSTOMERTableAdapter
            // 
            this.cUSTOMERTableAdapter.ClearBeforeFill = true;
            // 
            // kasirTableAdapter
            // 
            this.kasirTableAdapter.ClearBeforeFill = true;
            // 
            // SettingsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(763, 467);
            this.Controls.Add(this.modulTabControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsWindow";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "PrintNCI Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsWindowFormClosing);
            this.Load += new System.EventHandler(this.SettingsWindowLoad);
            tabKelompok.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cUSTOMERBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rSKUPANGDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.proceduresBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.printerListBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportsTableBindingSource)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kasirTableBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rSKUPANGDataSetBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportsTableBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.proceduresBindingSource)).EndInit();
            this.modulTabControl.ResumeLayout(false);
            this.tabSettings.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rSKUPANGDataSetBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl modulTabControl;
        private System.Windows.Forms.TabPage tabSettings;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private RSKUPANGDataSet rSKUPANGDataSet;
        private System.Windows.Forms.BindingSource proceduresBindingSource;
        private Printer.RSKUPANGDataSetTableAdapters.proceduresTableAdapter proceduresTableAdapter;
        private Printer.RSKUPANGDataSetTableAdapters.ReportsTableAdapter reportsTableAdapter;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource proceduresBindingSource1;
        private System.Windows.Forms.BindingSource reportsTableBindingSource1;
        private System.Windows.Forms.BindingSource rSKUPANGDataSetBindingSource;
        private System.Windows.Forms.BindingSource rSKUPANGDataSetBindingSource1;
        private System.Windows.Forms.BindingSource reportsTableBindingSource;
        private System.Windows.Forms.BindingSource cUSTOMERBindingSource;
        private Printer.RSKUPANGDataSetTableAdapters.CUSTOMERTableAdapter cUSTOMERTableAdapter;
        private System.Windows.Forms.TextBox reportTextBox;
        private System.Windows.Forms.Button buttonLaporan;
        private System.Windows.Forms.FolderBrowserDialog ttxFolderBrowserDialog;
        private System.Windows.Forms.TextBox reportPresentTextBox;
        private System.Windows.Forms.RadioButton billingRadioButton;
        private System.Windows.Forms.RadioButton pendaftaranRadioButton;
        private System.Windows.Forms.ComboBox kasirComboBox;
        private System.Windows.Forms.BindingSource kasirTableBindingSource;
        private Printer.RSKUPANGDataSetTableAdapters.KasirTableAdapter kasirTableAdapter;
        private System.Windows.Forms.TextBox textBoxKdUnit;
        private System.Windows.Forms.BindingSource printerListBindingSource;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.DataGridViewTextBoxColumn KdCustomer;
        private System.Windows.Forms.DataGridViewTextBoxColumn cUSTOMERDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn KD_CUSTOMER_REPORT;
        private System.Windows.Forms.DataGridViewComboBoxColumn NAMA_SP;
        private System.Windows.Forms.DataGridViewComboBoxColumn Printer;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button reportPreviewButton;
    }
}
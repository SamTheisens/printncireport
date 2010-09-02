﻿namespace PrintNCIService
{
    partial class PrintNCIService
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.serviceController = new System.ServiceProcess.ServiceController();
            this.eventLog = new System.Diagnostics.EventLog();
            this.timer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.eventLog)).BeginInit();
            // 
            // eventLog
            // 
            this.eventLog.EnableRaisingEvents = true;
            this.eventLog.Log = "Application";
            this.eventLog.Source = "PrintNCI";
            // 
            // PrintNCIService
            // 
            this.ServiceName = "PrintNCIService";
            ((System.ComponentModel.ISupportInitialize)(this.eventLog)).EndInit();

        }

        #endregion

        private System.Diagnostics.EventLog eventLog;
        private System.Windows.Forms.Timer timer;
        private System.ServiceProcess.ServiceController serviceController;

    }
}

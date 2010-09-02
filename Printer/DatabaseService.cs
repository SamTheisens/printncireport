using System;
using System.Data;
using System.Data.OleDb;
using System.Globalization;

namespace Printer
{
    public class DatabaseService
    {
        private IDataReader reader;
        private OleDbCommand dbCommand;
        private OleDbConnection dbConnection;
        private readonly string connectionString;

        public DatabaseService(String connectionString)
        {
            this.connectionString = connectionString;
        }

        public DatabaseService(IDataReader reader)
        {
            this.reader = reader;
        }
        public PatientVisitInfo GetVisitInfo(string kdKasir, string noTransaksi)
        {
            return FillVisitInfo(ExecuteQuery("SELECT KUNJUNGAN.KD_PASIEN, KUNJUNGAN.TGL_MASUK, KUNJUNGAN.KD_UNIT, KUNJUNGAN.URUT_MASUK, KUNJUNGAN.BARU, CUSTOMER, UNIT.KD_BAGIAN  FROM  dbo.TRANSAKSI INNER JOIN dbo.KUNJUNGAN ON dbo.TRANSAKSI.KD_PASIEN = dbo.KUNJUNGAN.KD_PASIEN AND dbo.TRANSAKSI.KD_UNIT = dbo.KUNJUNGAN.KD_UNIT AND dbo.TRANSAKSI.TGL_TRANSAKSI = dbo.KUNJUNGAN.TGL_MASUK AND dbo.TRANSAKSI.URUT_MASUK = dbo.KUNJUNGAN.URUT_MASUK  INNER JOIN dbo.CUSTOMER ON dbo.KUNJUNGAN.KD_CUSTOMER = dbo.CUSTOMER.KD_CUSTOMER INNER JOIN dbo.UNIT ON dbo.KUNJUNGAN.KD_UNIT = dbo.UNIT.KD_UNIT WHERE KD_KASIR = '" +
                kdKasir + "' AND NO_TRANSAKSI = '" + noTransaksi + "'"));
        }
        public bool GetPrintStatus(PatientVisitInfo visitInfo, out short status, out short jaminan, out short kartu_berobat, out short tracer)
        {
            status = 0;
            jaminan = 0;
            kartu_berobat = 0;
            tracer = 0;
            reader =
                ExecuteQuery("SELECT TOP 1 STATUS, JAMINAN, KARTU_BEROBAT, TRACER FROM RSUD_CETAKAN" + GetWhereClause(visitInfo));
            if (!reader.Read())
                return false;
            status = (short) reader["STATUS"];
            jaminan = (short) reader["JAMINAN"];
            kartu_berobat = (short) reader["KARTU_BEROBAT"];
            tracer = (short) reader["TRACER"];
            return true;
        }


        public PatientVisitInfo GetVisitInfo(Options options)
        {
            return FillVisitInfo(
                ExecuteQuery(
                    string.Format(
                        "SELECT TOP 1 CUSTOMER, KUNJUNGAN.TGL_MASUK, KUNJUNGAN.BARU, KUNJUNGAN.URUT_MASUK, KUNJUNGAN.KD_UNIT FROM KUNJUNGAN " +
                        "INNER JOIN dbo.CUSTOMER ON dbo.KUNJUNGAN.KD_CUSTOMER = dbo.CUSTOMER.KD_CUSTOMER " +
                        "WHERE KD_PASIEN = '{0}' AND TGL_MASUK = '{1}' AND URUT_MASUK = {2} AND KD_UNIT = '{3}' ORDER BY TGL_MASUK DESC ",
                        options.KdPasien, options.TglMasuk, options.UrutMasuk, options.KdUnit)));
        }


        public PatientVisitInfo GetVisitInfo(string kdPasien)
        {
            return FillVisitInfo(ExecuteQuery(
                                     string.Format(
                                         "SELECT TOP 1 CUSTOMER, KUNJUNGAN.TGL_MASUK, KUNJUNGAN.BARU, KUNJUNGAN.URUT_MASUK, KUNJUNGAN.KD_UNIT FROM KUNJUNGAN INNER JOIN dbo.CUSTOMER ON dbo.KUNJUNGAN.KD_CUSTOMER = dbo.CUSTOMER.KD_CUSTOMER WHERE KD_PASIEN = '{0}' ORDER BY TGL_MASUK DESC ",
                                         kdPasien)));
        }

        public bool GetSjp(PatientVisitInfo visitInfo)
        {
            reader = ExecuteQuery("SELECT TOP 1 * FROM SJP_KUNJUNGAN " + GetWhereClause(visitInfo));
            return reader.Read();
        }

        public void UpdateSjp(PatientVisitInfo visitInfo, string noSjp, bool update)
        {
            string commandText;
            if (update)
                commandText = string.Format("UPDATE SJP_KUNJUNGAN SET NO_SJP = '{0}' {1}", noSjp, GetWhereClause(visitInfo));
            else 
                commandText = string.Format("INSERT INTO SJP_KUNJUNGAN VALUES('{0}', '{1}', '{2}', {3}, '{4}')", visitInfo.KdPasien, visitInfo.KdUnit,
                                            visitInfo.TglMasuk.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), visitInfo.UrutMasuk, noSjp);
            ExecuteQuery(commandText);
        }

        public void UpdatePrintStatus(PatientVisitInfo visitInfo, int status, int jaminan, int kartu_berobat, short tracer, bool hasPrintStatus)
        {
            string commandText;
            if (hasPrintStatus)
            {
                commandText =
                    string.Format(
                        "UPDATE RSUD_CETAKAN SET STATUS = {0}, JAMINAN = {1} , KARTU_BEROBAT = {2}, TRACER = {3} {4}",
                        status, jaminan, kartu_berobat, tracer, GetWhereClause(visitInfo));

            }
            else
            {
                commandText = string.Format("INSERT INTO RSUD_CETAKAN VALUES('{0}', '{1}', '{2}', {3}, {4}, {5}, {6}, {7})",
                                            visitInfo.KdPasien, visitInfo.KdUnit,
                                            visitInfo.TglMasuk.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                                            visitInfo.UrutMasuk, status, jaminan, kartu_berobat, tracer);
            }
            ExecuteQuery(commandText);
        }

        #region Private
        private static string GetWhereClause(PatientVisitInfo visitInfo)
        {
            return
                string.Format(
                    " WHERE KD_PASIEN = '{0}' AND TGL_MASUK = '{1}' AND KD_UNIT = '{2}' AND URUT_MASUK = {3}",
                    visitInfo.KdPasien, visitInfo.TglMasuk.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    visitInfo.KdUnit, visitInfo.UrutMasuk);
        }

        public IDataReader ExecuteQuery(string commandText)
        {
            if (reader != null)
                return reader;

            dbConnection = new OleDbConnection(connectionString);
            dbCommand = new OleDbCommand {Connection = dbConnection, CommandText = commandText};
            dbConnection.Open();
            
            Logger.Logger.WriteLog(commandText);
            return dbCommand.ExecuteReader(CommandBehavior.KeyInfo);
        }

        public static PatientVisitInfo FillVisitInfo(IDataReader reader)
        {
            reader.Read();
            object value;
            var info = new PatientVisitInfo
            {
                KdPasien = TryGetValue(reader, "KD_PASIEN", out value) ? value.ToString() : null,
                KelompokPasien = TryGetValue(reader, "CUSTOMER", out value) ? value.ToString() : null,
                KdBagian = TryGetValue(reader, "KD_BAGIAN", out value) ? (byte)value : new byte?(),
                TglMasuk = TryGetValue(reader, "TGL_MASUK", out value) ? (DateTime)value : new DateTime(),
                Baru = TryGetValue(reader, "BARU", out value) ? (bool)value : new bool?(),
                UrutMasuk = TryGetValue(reader, "URUT_MASUK", out value) ? (short)value : new short?(),
                KdUnit = TryGetValue(reader, "KD_UNIT", out value) ? value.ToString() : null
            };
            return info;
        }
        private static bool TryGetValue(IDataReader reader, string columnName, out object value)
        {
            var table = reader.GetSchemaTable();
            

            if (!Contains(table, columnName))
            {
                value = null;
                return false;
            }
            value = reader[columnName];
            return true;
        }
        private static bool Contains(DataTable table, string columnName)
        {
            foreach (DataRow row in table.Rows)
            {
                if (row[0].ToString() == columnName)
                    return true;
            }
            return false;
        }
        #endregion
   }
}
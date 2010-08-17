using System;
using System.Data;
using System.Data.OleDb;
using System.Globalization;

namespace PrintNCI
{
    public class DatabaseService
    {
        private IDataReader reader;
        private OleDbCommand dbCommand;
        private OleDbConnection dbConnection;

        public DatabaseService(String connectionString)
        {
            dbConnection = new OleDbConnection(connectionString) { ConnectionString = connectionString };
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
        public bool GetPrintStatus(PatientVisitInfo visitInfo, out short status, out short jaminan)
        {
            status = 0;
            jaminan = 0;
            FillVisitInfo(ExecuteQuery(
                              "SELECT TOP 1 STATUS, JAMINAN FROM RSUD_CETAKAN" +
                              " WHERE KD_PASIEN = '" + visitInfo.KdPasien + "'" +
                              " AND TGL_MASUK = '" +
                              visitInfo.TglMasuk.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "'" +
                              " AND KD_UNIT = '" + visitInfo.KdUnit + "'" +
                              " AND URUT_MASUK = " + visitInfo.UrutMasuk));
                    if (!reader.Read()) 
                        return false;
                    status = (short)reader["STATUS"];
                    jaminan = (short)reader["JAMINAN"];
            return true;
        }

        public PatientVisitInfo GetKelompokPasien(string kdPasien)
        {
            return FillVisitInfo(ExecuteQuery(
                                     "SELECT TOP 1 CUSTOMER, KUNJUNGAN.TGL_MASUK, KUNJUNGAN.BARU, KUNJUNGAN.URUT_MASUK, KUNJUNGAN.KD_UNIT FROM KUNJUNGAN " +
                                     "INNER JOIN dbo.CUSTOMER ON dbo.KUNJUNGAN.KD_CUSTOMER = dbo.CUSTOMER.KD_CUSTOMER " +
                                     "WHERE KD_PASIEN = '" + kdPasien + "' ORDER BY TGL_MASUK DESC "));
        }

        public void UpdatePrintStatus(PatientVisitInfo visitInfo, int status, int jaminan, bool hasPrintStatus)
        {
            string commandText;
            if (hasPrintStatus)
            {
                commandText = "UPDATE RSUD_CETAKAN SET STATUS = " + status + ", JAMINAN = " + jaminan +
                              " WHERE KD_PASIEN = '" + visitInfo.KdPasien + "'" +
                              " AND TGL_MASUK = '" + visitInfo.TglMasuk.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "'" +
                              " AND KD_UNIT = '" + visitInfo.KdUnit + "'" +
                              " AND URUT_MASUK = " + visitInfo.UrutMasuk;
            }
            else
            {
                commandText = string.Format("INSERT INTO RSUD_CETAKAN VALUES('{0}', '{1}', '{2}', {3}, {4}, {5})", visitInfo.KdPasien, visitInfo.KdUnit,
                                            visitInfo.TglMasuk.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), visitInfo.UrutMasuk, status, jaminan);
            }
            dbCommand.CommandText = commandText;
            dbCommand.ExecuteNonQuery();
        }

        #region Private
        private IDataReader ExecuteQuery(string commandText)
        {
            dbCommand.CommandText = commandText;
            dbConnection.Open();
            dbCommand = new OleDbCommand { Connection = dbConnection };
            Logger.WriteLog(commandText);
            return dbCommand.ExecuteReader();
        }

        private static PatientVisitInfo FillVisitInfo(IDataReader reader)
        {
            reader.Read();
            var info = new PatientVisitInfo
            {

                KdPasien = TryGetValue(reader, "KD_PASIEN").ToString(),
                KelompokPasien = TryGetValue(reader, "CUSTOMER").ToString(),
                Bagian = (byte)TryGetValue(reader, "KD_BAGIAN"),
                TglMasuk = (DateTime)TryGetValue(reader, "TGL_MASUK"),
                Baru = (bool)TryGetValue(reader, "BARU"),
                UrutMasuk = (short)TryGetValue(reader, "URUT_MASUK"),
                KdUnit = TryGetValue(reader, "KD_UNIT").ToString()
            };
            return info;
        }
        private static object TryGetValue(IDataReader reader, string columnName)
        {
            var table = reader.GetSchemaTable();
            if (!table.Columns.Contains(columnName))
                return null;
            return reader[columnName];
        } 
        #endregion
   }
}
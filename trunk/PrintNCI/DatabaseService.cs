using System;
using System.Data.OleDb;

namespace PrintNCI
{
    public static class DatabaseService
    {
        public static void GetVisitInfo(string connectionString, string kdKasir, string noTransaksi, out string kdPasien, out string kelompokPasien, out int bagian)
        {
            kdPasien = "ERROR";
            kelompokPasien = "ERROR";
            bagian = 0;
            using (var connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.ConnectionString = connectionString;
                    connection.Open();
                    var command = new OleDbCommand
                    {
                        Connection = connection,
                        CommandText =
                            ("SELECT KUNJUNGAN.KD_PASIEN, CUSTOMER, UNIT.KD_BAGIAN " +
                             " FROM  dbo.TRANSAKSI INNER JOIN dbo.KUNJUNGAN ON dbo.TRANSAKSI.KD_PASIEN = dbo.KUNJUNGAN.KD_PASIEN AND dbo.TRANSAKSI.KD_UNIT = dbo.KUNJUNGAN.KD_UNIT AND dbo.TRANSAKSI.TGL_TRANSAKSI = dbo.KUNJUNGAN.TGL_MASUK AND dbo.TRANSAKSI.URUT_MASUK = dbo.KUNJUNGAN.URUT_MASUK " +
                             " INNER JOIN dbo.CUSTOMER ON dbo.KUNJUNGAN.KD_CUSTOMER = dbo.CUSTOMER.KD_CUSTOMER" +
                             " INNER JOIN dbo.UNIT ON dbo.KUNJUNGAN.KD_UNIT = dbo.UNIT.KD_UNIT" +
                             " WHERE KD_KASIR = '" +
                             kdKasir + "' AND NO_TRANSAKSI = '" + noTransaksi + "'")
                    };
                    Logger.WriteLog(command.CommandText);
                    OleDbDataReader reader = command.ExecuteReader();
                    reader.Read();
                    kdPasien = reader["KD_PASIEN"].ToString();
                    kelompokPasien = reader["CUSTOMER"].ToString();
                    bagian = (int)reader["KD_BAGIAN"];
                }
                catch (Exception exception)
                {
                    Logger.WriteLog(exception.Message);
                }
            }
        }

        public static string GetKelompokPasien(string connectionString, string kdPasien, out DateTime tglMasuk, out bool baru)
        {
            tglMasuk = DateTime.Now;
            baru = false;
            using (var connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.ConnectionString = connectionString;
                    connection.Open();
                    var command = new OleDbCommand
                    {
                        Connection = connection,
                        CommandText =
                            "SELECT TOP 1 CUSTOMER, KUNJUNGAN.TGL_MASUK, KUNJUNGAN.BARU FROM KUNJUNGAN " +
                            "INNER JOIN dbo.CUSTOMER ON dbo.KUNJUNGAN.KD_CUSTOMER = dbo.CUSTOMER.KD_CUSTOMER " +
                            "WHERE KD_PASIEN = '" + kdPasien + "' ORDER BY TGL_MASUK DESC "
                    };
                    Logger.WriteLog(command.CommandText);
                    OleDbDataReader reader = command.ExecuteReader();
                    reader.Read();
                    tglMasuk = (DateTime)reader["TGL_MASUK"];
                    baru = (bool)reader["BARU"];

                    return reader["CUSTOMER"].ToString();
                }
                catch (Exception exception)
                {
                    Logger.WriteLog(exception.Message);
                }
            }
            return "";
        }

    }
}

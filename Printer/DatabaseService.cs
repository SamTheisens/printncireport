using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.Linq;

namespace Printer
{
    public struct Column
    {
        public string ColumnName;
        public int ColumnSize;
        public string DataType;
        public bool IsHidden;
    }
    public struct Report
    {
        public string FileName;
        public string Procedure;
    }

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
            return FillVisitInfo(ExecuteQuery("SELECT KUNJUNGAN.KD_PASIEN, KUNJUNGAN.TGL_MASUK, KUNJUNGAN.KD_UNIT, " +
            "KUNJUNGAN.URUT_MASUK, KUNJUNGAN.BARU, CUSTOMER.KD_CUSTOMER, CUSTOMER.CUSTOMER, UNIT.KD_BAGIAN, TRANSAKSI.NO_TRANSAKSI, TRANSAKSI.KD_KASIR, P.NAMA FROM  dbo.TRANSAKSI INNER JOIN dbo.KUNJUNGAN ON " +
            "dbo.TRANSAKSI.KD_PASIEN = dbo.KUNJUNGAN.KD_PASIEN AND dbo.TRANSAKSI.KD_UNIT = dbo.KUNJUNGAN.KD_UNIT AND " +
            "dbo.TRANSAKSI.TGL_TRANSAKSI = dbo.KUNJUNGAN.TGL_MASUK AND dbo.TRANSAKSI.URUT_MASUK = dbo.KUNJUNGAN.URUT_MASUK " + 
            "INNER JOIN dbo.CUSTOMER ON dbo.KUNJUNGAN.KD_CUSTOMER = dbo.CUSTOMER.KD_CUSTOMER INNER JOIN dbo.UNIT ON " +
            "dbo.KUNJUNGAN.KD_UNIT = dbo.UNIT.KD_UNIT INNER JOIN PASIEN P ON P.KD_PASIEN = KUNJUNGAN.KD_PASIEN WHERE KD_KASIR = '" +
                kdKasir + "' AND NO_TRANSAKSI = '" + noTransaksi + "'"));
        }
        public void IncreaseNota(string kdKasir, string noTransaksi, string kdCustomer)
        {
            ExecuteQuery(string.Format("INSERT INTO NOTA_BILL SELECT '{0}', '{1}', MAX(NO_NOTA) + 1, 0, 0, 'TO', GETDATE(), '{2}' " +
                          "FROM NOTA_BILL WHERE KD_KASIR = '{0}'", kdKasir, noTransaksi, kdCustomer));
        }

        public PatientVisitInfo GetVisitInfoKasir(string kdKasir, string kdPasien)
        {
            return FillVisitInfo(ExecuteQuery("SELECT KUNJUNGAN.KD_PASIEN, KUNJUNGAN.TGL_MASUK, KUNJUNGAN.KD_UNIT, " +
"KUNJUNGAN.URUT_MASUK, KUNJUNGAN.BARU, CUSTOMER.KD_CUSTOMER, CUSTOMER.CUSTOMER, UNIT.KD_BAGIAN, TRANSAKSI.NO_TRANSAKSI, TRANSAKSI.KD_KASIR FROM  dbo.TRANSAKSI INNER JOIN dbo.KUNJUNGAN ON " +
"dbo.TRANSAKSI.KD_PASIEN = dbo.KUNJUNGAN.KD_PASIEN AND dbo.TRANSAKSI.KD_UNIT = dbo.KUNJUNGAN.KD_UNIT AND " +
"dbo.TRANSAKSI.TGL_TRANSAKSI = dbo.KUNJUNGAN.TGL_MASUK AND dbo.TRANSAKSI.URUT_MASUK = dbo.KUNJUNGAN.URUT_MASUK " +
"INNER JOIN dbo.CUSTOMER ON dbo.KUNJUNGAN.KD_CUSTOMER = dbo.CUSTOMER.KD_CUSTOMER INNER JOIN dbo.UNIT ON " +
"dbo.KUNJUNGAN.KD_UNIT = dbo.UNIT.KD_UNIT WHERE KD_KASIR = '" + kdKasir + "' AND TRANSAKSI.KD_PASIEN = '" + kdPasien + "'"));
        }


        public void OpenTransaction(string kdKasir, string noTransaksi)
        {
            ExecuteQuery(
                string.Format("EXECUTE RSUD_BUKA_TRANSAKSI '{0}', '{1}'",
                              kdKasir, noTransaksi));
        }

        public Report GetExecutable(string kdKelompokPasien, string kdKasir, bool modulPendaftaran)
        {
            reader = ExecuteQuery(string.Format("SELECT KD_KASIR, PENDAFTARAN, KD_CUSTOMER_REPORT, NAMA_SP FROM RSUD_REPORTS WHERE " +
                                                "KD_KASIR = '{0}' AND KD_CUSTOMER = '{1}' AND PENDAFTARAN = {2}", kdKasir,
                                                kdKelompokPasien, modulPendaftaran ? 1 : 0));
            if (!reader.Read())
                throw new Exception(string.Format("Report belum disetting untuk kdKasir {0} dan kode customer {1}",
                                                  kdKasir, kdKelompokPasien));
            
            var kdCustomer = (string)reader["KD_CUSTOMER_REPORT"];
            var namaStoredProcedure = (string)reader["NAMA_SP"];
            var pendaftaran = (bool) reader["PENDAFTARAN"];
            return new Report
                       {
                           Procedure = namaStoredProcedure,
                           FileName = SettingsService.CreateReportName(kdKasir, pendaftaran, kdCustomer)
                       };
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
                commandText = string.Format("UPDATE SJP_KUNJUNGAN SET NO_SJP = '{0}' {1}", noSjp,
                                            GetWhereClause(visitInfo));
            else
                commandText = string.Format("INSERT INTO SJP_KUNJUNGAN VALUES('{0}', '{1}', '{2}', {3}, '{4}')",
                                            visitInfo.KdPasien, visitInfo.KdUnit,
                                            visitInfo.TglMasuk.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                                            visitInfo.UrutMasuk, noSjp);
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
                KdKelompokPasien = TryGetValue(reader, "KD_CUSTOMER", out value) ? value.ToString() : null,
                KelompokPasien = TryGetValue(reader, "CUSTOMER", out value) ? value.ToString() : null,
                KdBagian = TryGetValue(reader, "KD_BAGIAN", out value) ? (byte)value : new byte?(),
                TglMasuk = TryGetValue(reader, "TGL_MASUK", out value) ? (DateTime)value : new DateTime(),
                Baru = TryGetValue(reader, "BARU", out value) ? (bool)value : new bool?(),
                UrutMasuk = TryGetValue(reader, "URUT_MASUK", out value) ? (short)value : new short?(),
                KdUnit = TryGetValue(reader, "KD_UNIT", out value) ? value.ToString() : null,
                NoTransaksi = TryGetValue(reader, "NO_TRANSAKSI", out value) ? value.ToString() : null,
                KdKasir = TryGetValue(reader, "KD_KASIR", out value) ? value.ToString() : null,
                Nama = TryGetValue(reader, "NAMA", out value) ? value.ToString() : null
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
        public string ExtractTtx(DataTable schemaTable)
        {
            List<Column> tableSchema = ParseTableSchema(schemaTable);
            return printTtxString(tableSchema);
        }

        private static string printTtxString(IEnumerable<Column> tableSchema)
        {
            string ttxFile = "";
            foreach (Column column in tableSchema.Where(c => c.IsHidden == false))
            {
                ttxFile += column.ColumnName + "\t";
                switch ((column.DataType))
                {
                    case "System.String" :
                        ttxFile += "String\t" + column.ColumnSize;
                        break;
                    case "System.DateTime" :
                        ttxFile += "Dates";
                        break;
                    case "System.Int32" :
                        ttxFile += "Long\t1";
                        break;
                    case "System.Int64":
                        ttxFile += "Long\t1";
                        break;
                    case "System.Boolean" :
                        ttxFile += "Boolean\t1";
                        break;
                    case "System.Double" :
                        ttxFile += "Number\t1.00";
                        break;
                    
                    default:
                        ttxFile += "XXX Tidak bisa deteksi type kolom!#t1";
                        break;
                }
                
                ttxFile += "\n";
            }
            return ttxFile;
        }

        private static List<Column> ParseTableSchema(DataTable schemaTable)
        {
            var tableSchema = new List<Column>();
            foreach (DataRow myField in schemaTable.Rows)
            {
                var column = new Column();
                foreach (DataColumn myProperty in schemaTable.Columns)
                {
                    var property = myProperty.ColumnName;
                    if (property.Equals("ColumnName"))
                        column.ColumnName = (string)myField[myProperty];

                    if (property.Equals("ColumnSize"))
                        column.ColumnSize = (int)myField[myProperty];

                    if (property.Equals("DataType"))
                        column.DataType = Convert.ToString(myField[myProperty]);

                    if (property.Equals("IsHidden"))
                        column.IsHidden = (bool)myField[myProperty];

                }
                tableSchema.Add(column);
            }
            return tableSchema;
        }

        #endregion
   }
}
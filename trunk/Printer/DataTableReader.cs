using System;
using System.Data;

namespace Printer
{
    public class DataTableReader : IDataReader
    {
        private const int NotSet = -1;
        private readonly DataTable _table;
        private int _currentRow = NotSet;
        private DataRow _row;
        private bool _closed;

        public DataTableReader(DataTable table)
        {
            _table = table;
        }

        #region IDataReader Members

        public int RecordsAffected
        {
            get { return -1; }
        }

        public bool IsClosed
        {
            get { return _closed; }
        }

        public bool NextResult()
        {
            Close();
            return false;
        }

        public void Close()
        {
            _closed = true;
            _currentRow = NotSet;
            _row = null;
            _table.Dispose();
        }

        public bool Read()
        {
            _currentRow++;
            if (_currentRow < _table.Rows.Count)
            {
                _row = _table.Rows[_currentRow];
                return true;
            }

            //passed the end, clean up 
            Close();
            return false;
        }

        public int Depth
        {
            get
            {
                //we don't have any depth 
                return 0;
            }
        }

        /// <summary> 
        /// Note: this makes no real attempt to get all the functionality in there 
        /// </summary> 
        /// <returns></returns> 
        public DataTable GetSchemaTable()
        {

            var schema = new DataTable("Schema");
            schema.Columns.Add("ColumnName");
            schema.Columns.Add("ColumnOrdinal", typeof(int));
            schema.Columns.Add("ColumnSize", typeof(int));
            schema.Columns.Add("NumericPrecision", typeof(int));
            schema.Columns.Add("NumericScale", typeof(int));
            schema.Columns.Add("DataType", typeof(Type));
            schema.Columns.Add("ProviderType", typeof(Type));
            schema.Columns.Add("IsLong", typeof(bool));
            schema.Columns.Add("AllowDBNull", typeof(bool));
            schema.Columns.Add("IsReadOnly", typeof(bool));
            schema.Columns.Add("IsRowVersion", typeof(bool));
            schema.Columns.Add("IsUnique", typeof(bool));
            schema.Columns.Add("IsKeyColumn", typeof(bool));
            schema.Columns.Add("IsAutoIncrement", typeof(bool));
            schema.Columns.Add("BaseSchemaName");
            schema.Columns.Add("BaseCatalogName");
            schema.Columns.Add("BaseTableName");
            schema.Columns.Add("BaseColumnName");

            foreach (DataColumn col in _table.Columns)
            {
                DataRow colRow = schema.NewRow();
                colRow["ColumnName"] = col.ColumnName;
                colRow["ColumnOrdinal"] = col.Ordinal; //col.Ordinal; 
                colRow["ColumnSize"] = col.MaxLength; //col.ColumnSize; 
                colRow["NumericPrecision"] = null; //col.NumericPrecision; 
                colRow["NumericScale"] = null; //col.NumericScale; 
                colRow["DataType"] = col.DataType;
                colRow["ProviderType"] = col.DataType; //col.ProviderType; 
                colRow["IsLong"] = null; //col.IsLong; 
                colRow["AllowDBNull"] = col.AllowDBNull;
                colRow["IsReadOnly"] = true; //col.IsReadOnly; 
                colRow["IsRowVersion"] = false; //col.IsRowVersion; 
                colRow["IsUnique"] = false; //col.IsUnique; 
                colRow["IsKeyColumn"] = false; //col.IsKeyColumn; 
                colRow["IsAutoIncrement"] = false; //col.IsAutoIncrement; 
                colRow["BaseSchemaName"] = null; //col.BaseSchemaName; 
                colRow["BaseCatalogName"] = null; //col.BaseCatalogName; 
                colRow["BaseTableName"] = null; //col.BaseTableName; 
                colRow["BaseColumnName"] = null; //col.BaseColumnName; 
                schema.Rows.Add(colRow);
            }
            return schema;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Close();
        }

        #endregion

        #region IDataRecord Members

        public int GetInt32(int i)
        {
            return Convert.ToInt32(_row);
        }

        public object this[string name]
        {
            get
            {
                try
                {
                    return _row[name];
                }
                catch (ArgumentException err)
                {
                    throw new IndexOutOfRangeException(err.Message, err);
                }
            }
        }

        object IDataRecord.this[int i]
        {
            get
            {
                return _row[i];
            }
        }

        public object GetValue(int i)
        {
            return _row[i];
        }

        public bool IsDBNull(int i)
        {
            return _row[i] == DBNull.Value;
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new ApplicationException("Not implemented");
        }

        public byte GetByte(int i)
        {
            return Convert.ToByte(_row[i]);
        }

        public Type GetFieldType(int i)
        {
            return _row[i].GetType();
        }

        public decimal GetDecimal(int i)
        {
            return Convert.ToDecimal(_row[i]);
        }

        public int GetValues(object[] values)
        {
            int count = Math.Min(values.Length, _row.ItemArray.Length);
            for (int i = 0; i < count; i++)
                values[i] = _row[i];

            return _row.ItemArray.Length;
        }

        public string GetName(int i)
        {
            return _table.Columns[i].ColumnName;
        }

        public int FieldCount
        {
            get
            {
                return _row.ItemArray.Length;
            }
        }

        public long GetInt64(int i)
        {
            return Convert.ToInt64(_row[i]);
        }

        public double GetDouble(int i)
        {
            return Convert.ToDouble(_row[i]);
        }

        public bool GetBoolean(int i)
        {
            return Convert.ToBoolean(_row[i]);
        }

        public Guid GetGuid(int i)
        {
            return (Guid)_row[i];
        }

        public DateTime GetDateTime(int i)
        {
            return Convert.ToDateTime(_row[i]);
        }

        public int GetOrdinal(string name)
        {
            return _table.Columns[name].Ordinal;
        }

        public string GetDataTypeName(int i)
        {
            return _table.Columns[i].DataType.Name;
        }

        public float GetFloat(int i)
        {
            return (float)Convert.ToDouble(_row[i]);
        }

        public IDataReader GetData(int i)
        {
            return null;
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            char[] chars = _row[i].ToString().ToCharArray(Convert.ToInt32(fieldoffset), length);
            length = Math.Min(chars.Length, length);
            for (int j = 0; j < length; j++)
                buffer[j + bufferoffset] = chars[j];
            return length;
        }

        public string GetString(int i)
        {
            return _row[i].ToString();
        }

        public char GetChar(int i)
        {
            return Convert.ToChar(_row[i]);
        }

        public short GetInt16(int i)
        {
            return Convert.ToInt16(_row[i]);
        }

        #endregion
    }
}


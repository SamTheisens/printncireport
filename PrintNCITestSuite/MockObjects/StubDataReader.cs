using System;
using System.Data;

namespace PrintNCITestSuite.MockObjects
{

    /// <summary>
    /// This class fakes up a data reader.
    /// </summary>
    public class StubDataReader : IDataReader
    {
        readonly StubResultSetCollection stubResultSets;
        private int currentResultsetIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="StubDataReader"/> class. 
        /// Each row in the arraylist is a result set.
        /// </summary>
        /// <param name="stubResultSets">The result sets.</param>
        public StubDataReader(StubResultSetCollection stubResultSets)
        {
            this.stubResultSets = stubResultSets;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StubDataReader"/> class. 
        /// Each row in the arraylist is a result set.
        /// </summary>
        /// <param name="resultSets">The result sets to add.</param>
        public StubDataReader(params StubResultSet[] resultSets)
        {
            stubResultSets = new StubResultSetCollection();
            foreach (StubResultSet resultSet in resultSets)
            {
                stubResultSets.Add(resultSet);
            }
        }

        public void Close()
        {
        }

        public bool NextResult()
        {
            if (currentResultsetIndex >= stubResultSets.Count)
                return false;

            return (++currentResultsetIndex < stubResultSets.Count);
        }

        public bool Read()
        {
            return CurrentResultSet.Read();
        }

        public DataTable GetSchemaTable()
        {
            return new StubDataTable(CurrentResultSet);
        }

        /// <summary>
        /// Gets a value indicating the depth of nesting for the current row.
        /// </summary>
        /// <value></value>
        public int Depth
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsClosed
        {
            get { return false; }
        }

        public int RecordsAffected
        {
            get { return 1; }
        }

        public void Dispose()
        {
        }

        public string GetName(int i)
        {
            return CurrentResultSet.GetFieldNames()[i];
        }

        public string GetDataTypeName(int i)
        {
            return CurrentResultSet.GetFieldNames()[i];
        }

        public Type GetFieldType(int i)
        {
            //KLUDGE: Since we're dynamically creating this, I'll have to 
            //		  look at the actual data to determine this.
            //		  We'll loook at the first row since it's the most likely 
            //			to have data.
            return stubResultSets[0][i].GetType();
        }

        public object GetValue(int i)
        {
            return CurrentResultSet[i];
        }

        public int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public int GetOrdinal(string name)
        {
            return CurrentResultSet.GetIndexFromFieldName(name);
        }

        public bool GetBoolean(int i)
        {
            return (bool)CurrentResultSet[i];
        }

        public byte GetByte(int i)
        {
            return (byte)CurrentResultSet[i];
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            //TODO: Need to test this method.

            var totalBytes = (byte[])CurrentResultSet[i];

            int bytesRead = 0;
            for (int j = 0; j < length; j++)
            {
                long readIndex = fieldOffset + j;
                long writeIndex = bufferoffset + j;
                if (totalBytes.Length <= readIndex)
                    throw new ArgumentOutOfRangeException("fieldOffset", string.Format("Trying to read index '{0}' is out of range. (fieldOffset '{1}' + current position '{2}'", readIndex, fieldOffset, j));

                if (buffer.Length <= writeIndex)
                    throw new ArgumentOutOfRangeException("bufferoffset", string.Format("Trying to write to buffer index '{0}' is out of range. (bufferoffset '{1}' + current position '{2}'", readIndex, bufferoffset, j));

                buffer[writeIndex] = totalBytes[readIndex];
                bytesRead++;
            }
            return bytesRead;
        }

        public char GetChar(int i)
        {
            return (char)CurrentResultSet[i];
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public Guid GetGuid(int i)
        {
            return (Guid)CurrentResultSet[i];
        }

        public short GetInt16(int i)
        {
            return (short)CurrentResultSet[i];
        }

        public int GetInt32(int i)
        {
            return (int)CurrentResultSet[i];
        }

        public long GetInt64(int i)
        {
            return (long)CurrentResultSet[i];
        }

        public float GetFloat(int i)
        {
            return (float)CurrentResultSet[i];
        }

        public double GetDouble(int i)
        {
            return (double)CurrentResultSet[i];
        }

        public string GetString(int i)
        {
            return (string)CurrentResultSet[i];
        }

        public decimal GetDecimal(int i)
        {
            return (decimal)CurrentResultSet[i];
        }

        public DateTime GetDateTime(int i)
        {
            return (DateTime)CurrentResultSet[i];
        }

        public IDataReader GetData(int i)
        {
            var reader = new StubDataReader(stubResultSets) {currentResultsetIndex = i};
            return reader;
        }

        public bool IsDBNull(int i)
        {
            //TODO: Deal with value types.
            return null == CurrentResultSet[i];
        }

        public int FieldCount
        {
            get
            {
                return CurrentResultSet.GetFieldNames().Length;
            }
        }

        public object this[int i]
        {
            get
            {
                return CurrentResultSet[i];
            }
        }

        public object this[string name]
        {
            get
            {
                return CurrentResultSet[name];
            }
        }

        private StubResultSet CurrentResultSet
        {
            get
            {
                return stubResultSets[currentResultsetIndex];
            }
        }
    }

}

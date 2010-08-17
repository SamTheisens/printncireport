using System;

namespace PrintNCITestSuite.MockObjects
{
    /// <summary>
    /// Represents a result set for the StubDataReader.
    /// </summary>
    public class StubResultSet
    {
        int currentRowIndex = -1;
        readonly StubRowCollection rows = new StubRowCollection();
        readonly NameIndexCollection fieldNames = new NameIndexCollection();

        /// <summary>
        /// Initializes a new instance of the <see cref="StubResultSet"/> class with the column names.
        /// </summary>
        /// <param name="fieldNames">The column names.</param>
        public StubResultSet(params string[] fieldNames)
        {
            for (int i = 0; i < fieldNames.Length; i++)
            {
                this.fieldNames.Add(fieldNames[i], i);
            }
        }

        public string[] GetFieldNames()
        {
            return fieldNames.GetAllKeys();
        }

        public string GetFieldName(int i)
        {
            return GetFieldNames()[i];
        }

        /// <summary>
        /// Adds the row.
        /// </summary>
        /// <param name="values">The values.</param>
        public void AddRow(params object[] values)
        {
            if (values.Length != fieldNames.Count)
            {
                throw new ArgumentOutOfRangeException("values", string.Format("The Row must contain '{0}' items", fieldNames.Count));
            }
            rows.Add(new StubRow(values));
        }

        public int GetIndexFromFieldName(string name)
        {
            return fieldNames[name];
        }

        public bool Read()
        {
            return ++currentRowIndex < rows.Count;
        }

        public StubRow CurrentRow
        {
            get
            {
                return rows[currentRowIndex];
            }
        }

        public object this[string key]
        {
            get
            {
                return CurrentRow[GetIndexFromFieldName(key)];
            }
        }

        public object this[int i]
        {
            get
            {
                return CurrentRow[i];
            }
        }
    }
}

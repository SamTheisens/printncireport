using System.Data;

namespace PrintNCITestSuite.MockObjects
{
    public class StubDataTable : DataTable
    {
        public StubDataTable(StubResultSet resultSet)
        {
            Columns.Add("ColumnName");
            foreach (var name in resultSet.GetFieldNames())
            {
                Rows.Add(name);
            }
        }
    }
}
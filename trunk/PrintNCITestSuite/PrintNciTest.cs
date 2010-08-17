using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PrintNCI;
using PrintNCITestSuite.MockObjects;

namespace PrintNCITestSuite
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestFixture]
    public class PrintNciTest
    {
        public PrintNciTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        [Test]
        public void TestFillVisitInfo()
        {

            var resultSet = new StubResultSet("KD_PASIEN", "CUSTOMER", "KD_BAGIAN", "TGL_MASUK", "BARU", "URUT_MASUK", "KD_UNIT");
            resultSet.AddRow("0-15-32-97", "JAMKESMAS", (byte)1, DateTime.Now, true, (short)1, 12);
            var reader = new StubDataReader(resultSet);
            Assert.True(reader.Read());
            //var info = DatabaseService.FillVisitInfo(reader);
            //Assert.True(info.Baru);
        }
    }
}

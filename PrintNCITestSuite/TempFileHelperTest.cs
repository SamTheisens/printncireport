using NUnit.Framework;
using Printer;
using PrintNCITestSuite.MockObjects;
using Printer.Services;


namespace PrintNCITestSuite
{
    [TestFixture]
    class TempFileHelperTest
    {
        [Test]
        public void TestModifyExecutable()
        {
            TempFileHelper.WriteTempFileBill("0177467", "01");

            var resultSet = new StubResultSet("KD_CUSTOMER_REPORT", "NAMA_SP");
            resultSet.AddRow("0000000002", "RSUD_JAMINAN_RJ");
            var reader = new StubDataReader(resultSet);
            var service = new DatabaseService(reader);
            var report = service.GetReportInformation("0000000002", "01", true);
            Assert.AreEqual(report.FileName, "PEN-02");
            Assert.AreEqual(report.Procedure, "RSUD_JAMINAN_RJ");

            NCIPrinter.Print(report);
        }

    }
}

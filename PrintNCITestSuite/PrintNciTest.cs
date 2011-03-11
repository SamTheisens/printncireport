using System;
using System.Globalization;
using NUnit.Framework;
using Printer;
using PrintNCITestSuite.MockObjects;

namespace PrintNCITestSuite
{
    [TestFixture]
    public class PrintNciTest
    {
        [Test]
        public void TestFillVisitInfo()
        {
            var resultSet = new StubResultSet("KD_PASIEN", "CUSTOMER", "KD_BAGIAN", "TGL_MASUK", "BARU", "URUT_MASUK",
                                              "KD_UNIT");
            resultSet.AddRow("0-15-32-97", "JAMKESMAS", (byte)1, DateTime.Now, true, (short)1, 12);
            var reader = new StubDataReader(resultSet);
            var service = new DatabaseService(reader);

            PatientVisitInfo info = service.GetVisitInfo("", "");
            Assert.IsNotNull(info.Baru);
            Assert.IsNotNull(info.Baru.Value);
            Assert.AreEqual("0-15-32-97", info.KdPasien);
            Assert.AreEqual("JAMKESMAS", info.KelompokPasien);
            Assert.AreEqual(1, info.KdBagian);
            Assert.AreEqual(1, info.UrutMasuk);
            Assert.AreEqual("12", info.KdUnit);
            Assert.AreEqual(DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                            info.TglMasuk.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
        }

        [Test]
        public void TestPrintStatus()
        {
            var resultSet = new StubResultSet("KD_PASIEN", "CUSTOMER", "KD_BAGIAN", "TGL_MASUK", "BARU", "URUT_MASUK",
                                  "KD_UNIT");
            resultSet.AddRow("0-15-32-97", "JAMKESMAS", (byte)1, DateTime.Now, true, (short)1, 12);
            var reader = new StubDataReader(resultSet);
            var service = new DatabaseService(reader);
            PatientVisitInfo info = service.GetVisitInfo("", "");

            resultSet = new StubResultSet("STATUS", "JAMINAN", "KARTU_BEROBAT", "TRACER");
            resultSet.AddRow((short)1, (short)0, (short)1, (short)3);
            service = new DatabaseService(new StubDataReader(resultSet));
            short status;
            short jaminan;
            short kartu_berobat;
            short tracer;
            service.GetPrintStatus(info, out status, out jaminan, out kartu_berobat, out tracer);
            Assert.AreEqual(1, status);
            Assert.AreEqual(0, jaminan);
            Assert.AreEqual(3, tracer);
        }

        [Test]
        public void TestGetKelompokPasien()
        {
            var resultSet = new StubResultSet("CUSTOMER", "TGL_MASUK", "BARU", "URUT_MASUK", "KD_UNIT");
            resultSet.AddRow("JAMKESMAS", DateTime.Now, true, (short)1, 12);
            var reader = new StubDataReader(resultSet);
            var service = new DatabaseService(reader);

            PatientVisitInfo info = service.GetVisitInfo("0-15-32-97");

            Assert.IsNull(info.KdPasien);
            Assert.AreEqual("JAMKESMAS", info.KelompokPasien);
            Assert.IsNull(info.KdBagian);
            Assert.AreEqual(1, info.UrutMasuk);
            Assert.AreEqual("12", info.KdUnit);
            Assert.AreEqual(DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                            info.TglMasuk.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));

        }
        [Test]
        public void TestGetExecutable()
        {
            var resultSet = new StubResultSet("KD_KASIR", "PENDAFTARAN", "KD_CUSTOMER_REPORT", "NAMA_SP");
            resultSet.AddRow("02", true, "0000000002", "RSUD_PRINT_BILL");
            var reader = new StubDataReader(resultSet);
            var service = new DatabaseService(reader);
            var form = service.GetExecutable("0000000002", "02", true);
            Assert.AreEqual("CRPEN-02-02.rpt", form.FileName);
            Assert.AreEqual("RSUD_PRINT_BILL", form.Procedure);
        }

    }
}

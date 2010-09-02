using CommandLine;
using CommandLine.Text;

namespace Printer
{
    public sealed class Options
    {
        [Option("c", "configuration", Required = false, HelpText = "Mengubah konfigurasi program ini")]
        public bool Settings;

        [Option("a", "status", Required = false, HelpText = "Cetak Kartu Status Pasien")]
        public bool Status;

        [Option("b", "bill", Required = false, HelpText = "Cetak Bill/Jaminan")]
        public bool Billing;

        [Option("n", "kartuberobat", Required = false, HelpText = "Cetak Kartu Berobat")]
        public bool KartuBerobat;

        [Option("z", "tracer", Required = false, HelpText = "Cetak kertas tracer")]
        public bool Tracer;

        [Option("t", "test", Required = false, HelpText = "Test")]
        public bool Test;

        [Option("k", "kdpasien", Required = false, HelpText = "Nomor Rekam Medis pasien. Untuk mengesampingkan yang diisi di tempfile")]
        public string KdPasien;

        [Option("u", "kdunit", Required = false, HelpText = "Kode Unit untuk kunjungan ini")]
        public string KdUnit;

        [Option("j", "SJP", Required = false, HelpText = "Nomer SJP")]
        public string Sjp;

        [Option("m", "tglmasuk", Required = false, HelpText = "Tanggal masuk untuk kunjungan ini")]
        public string TglMasuk;

        [Option("o", "urut", Required = false, HelpText = "Nomer urut masuk untuk kunjungan ini")]
        public int UrutMasuk;

        [Option("r", "bagian", Required = false, HelpText = "Bagian. RWI = 1, RWJ = 2, UGD = 3")]
        public byte KdBagian;

        [HelpOption(HelpText = "Menimpalkan keterangan ini")]
        public string GetUsage()
        {
            var help = new HelpText("PrintNCI - Cetak laporan Crystal Reports dari command line")
                           {
                               AdditionalNewLineAfterOption = true,
                               Copyright = new CopyrightInfo("Sam Theisens", 2009, 2010)
                           };
            help.AddOptions(this);
            return help;
        }

    }
}

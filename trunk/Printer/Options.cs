using CommandLine;
using CommandLine.Text;

namespace Printer
{
    public sealed class Options
    {
        [Option("h", "help", Required = false, HelpText = "Munjunjukan informasi ini")]
        public bool Help;

        [Option("c", "configuration", Required = false, HelpText = "Mengubah konfigurasi program ini")]
        public bool Settings;

        [Option("i", "kasir", Required = false, HelpText = "Kode Kasir")]
        public string KdKasir;

        [Option("a", "status", Required = false, HelpText = "Cetak Kartu Status Pasien")]
        public bool Status;

        [Option("p", "pendaftaran", Required = false, HelpText = "Cetak dari modul pendaftaran (jaminan)")]
        public bool Pendaftaran;

        [Option("n", "kartuberobat", Required = false, HelpText = "Cetak Kartu Berobat")]
        public bool KartuBerobat;

        [Option("z", "tracer", Required = false, HelpText = "Cetak kertas tracer")]
        public bool Tracer;

        [Option("t", "transaksi", Required = false, HelpText = "No. Transaksi")]
        public string Transaksi;

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

        [Option("v", "skipverify", Required = false, HelpText = "Lompat verifikasi syarat2 sebelum print")]
        public bool SkipVerify;

        [Option("d", "modul", Required = false, HelpText = "Modul")]
        public int Modul;

        [Option("q", "printqueue", Required = false, HelpText = "Cek printqueue")]
        public bool PrintQueue;

        [Option("x", "commandline", Required = false, HelpText = "Cek printqueue")]
        public bool CommandLine;



        [HelpOption(HelpText = "Menimpalkan keterangan ini")]
        public string GetUsage()
        {
            var help = new HelpText("PrintNCI - Cetak laporan Crystal Reports dari command line")
                           {
                               AdditionalNewLineAfterOption = true,
                               Copyright = new CopyrightInfo("Sam Theisens", 2009, 2011)
                           };
            help.AddOptions(this);
            return help;
        }

    }
}

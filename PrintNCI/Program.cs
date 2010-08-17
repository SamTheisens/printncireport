using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using CommandLine;
using PrintNCI.Properties;

namespace PrintNCI
{

    public class Program
    {
        public const string kelompokJamkesmas = "JAMKESMAS";
        public const string kelompokJamkesda = "JAMKESDA";
        public const string kelompokAskes = "ASKES NEGERI";
        public const string kelompokInHealth = "IN HEALTH";

        private static void Main(string[] args)
        {
            var options = new Options();
            ICommandLineParser parser = new CommandLineParser(new CommandLineParserSettings(Console.Error));
            if (!parser.ParseArguments(args, options))
                Environment.Exit(1);
            try
            {
                var printer = new NCIPrinter(options);
                printer.Print();
            }
            catch (Exception exception)
            {
                Logger.WriteLog("Maaf, ada masalah. Tolong panggil Instalasi SIMRS.");
                Logger.WriteLog(exception.Message);
                Console.ReadKey();
            }


        }

   }
}
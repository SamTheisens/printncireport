using System;
using System.Threading;
using CommandLine;
using CommandLine.Parser;
using Printer;

namespace PrintNCI
{
    public class Program
    {

        private static void Main(string[] args)
        {
            bool freeToRun;
            new Mutex(true, "PrintNCI", out freeToRun);
            if (!freeToRun)
            {
                return;
            }

            var options = new Options();
            ICommandLineParser parser = new CommandLineParser(new CommandLineParserSettings(Console.Error));
            if (!parser.ParseArguments(args, options))
                Environment.Exit(1);

            if (options.Settings)
            {
                var window = new SettingsWindow();
                window.ShowDialog();
                return;
            }

            if (!options.Status && !options.Billing && !options.Test && !options.KartuBerobat && !options.Tracer)
            {
                Console.WriteLine(options.GetUsage());
                return;
            }

            try
            {
                var printer = new NCIPrinter(options);
                printer.Print();
            }
            catch (Exception exception)
            {
                Logger.Logger.WriteLog("Maaf, ada masalah. Tolong panggil Instalasi SIMRS.");
                Logger.Logger.WriteLog(exception.Message);
                Console.ReadKey();
            }
        }
   }
}
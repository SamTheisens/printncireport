using System;
using System.IO;
using PrintNCI.Properties;

namespace PrintNCI
{
    public static class Logger
    {
        private static readonly StreamWriter logFile = new StreamWriter(Settings.Default.LogFileName, true);

        public static void WriteLog(string line)
        {
            Console.WriteLine(line);
            if (Settings.Default.Logging)
                logFile.WriteLine(DateTime.Now.ToUniversalTime() + " " + line);
            logFile.Flush();
        }

    }
}

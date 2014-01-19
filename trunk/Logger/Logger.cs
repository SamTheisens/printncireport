using System;
using System.IO;
using Logger.Properties;

namespace Logger
{
    public static class Logger
    {
        private static readonly StreamWriter LogFile = new StreamWriter(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                              Settings.Default.ProgramFolder + @"\"), Settings.Default.LogFileName), true);

        public static void WriteLog(string line)
        {
            Console.WriteLine(line);
            if (Settings.Default.Logging)
                LogFile.WriteLine(DateTime.Now.ToUniversalTime() + " " + line);
            LogFile.Flush();
        }
    }
}

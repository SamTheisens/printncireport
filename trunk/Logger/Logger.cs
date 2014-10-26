using System;
using System.IO;
using Logger.Properties;

namespace Logger
{
    public static class Logger
    {
        private static readonly StreamWriter LogFile = GetLogFile();

        private static StreamWriter GetLogFile()
        {
            var logFile = Path.Combine(GetProgramPath(), Settings.Default.LogFileName);
            return File.Exists(logFile) ? new StreamWriter(logFile, true) : File.CreateText(logFile);
        }
    

        private static string GetProgramPath()
        {
            var programPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), 
                Settings.Default.ProgramFolder + @"\");
            
            if (!Directory.Exists(programPath))
            {
                Directory.CreateDirectory(programPath);
            }
            return programPath;
        }

        public static void WriteLog(string line)
        {
            Console.WriteLine(line);
            if (Settings.Default.Logging)
                LogFile.WriteLine(DateTime.Now.ToUniversalTime() + " " + line);
            LogFile.Flush();
        }
    }
}

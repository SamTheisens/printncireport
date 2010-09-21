using System;
using System.Runtime.InteropServices;
using System.ServiceProcess;

namespace PrintNCIService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            //if (args.Length > 0) new PrintNCIService().Test();
            var ServicesToRun = new ServiceBase[]
                                    {
                                        new PrintNCIService()
                                    };
            ServiceBase.Run(ServicesToRun);
        }
    }
}

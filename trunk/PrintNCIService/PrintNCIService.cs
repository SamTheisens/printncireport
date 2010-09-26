using System;
using System.ServiceProcess;
using System.Threading;
using PrintNCIService.Properties;

namespace PrintNCIService
{
    public partial class PrintNCIService : ServiceBase
    {
        private Timer timer;
        private readonly PrintTimer printTimer;

        public PrintNCIService()
        {
            InitializeComponent();
            printTimer = new PrintTimer();
        }
        public void Test()
        {
            Console.WriteLine("hallo");
            //printTimer = new PrintTimer();
            timer = new Timer(printTimer.Check, null, 3000, Settings.Default.PollingFrequencyInMillis);
            Thread.Sleep(1000000);
        }

        protected override void OnStart(string[] args)
        {
            timer = new Timer(printTimer.Check, null, 3000, Settings.Default.PollingFrequencyInMillis);
        }

        protected override void OnStop()
        {
            timer.Dispose();
        }
    }
}



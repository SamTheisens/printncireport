using System.ServiceModel;

namespace PrintNCIService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class PrintService : IPrintService
    {
        private string statusQueue;
        public string StatusQueue
        {
            set { statusQueue = value; }
        }

        private string tracerQueue;
        public string TracerQueue
        {
            set { tracerQueue = value; }
        }

        public string GetStatusQueue()
        {
            return statusQueue;
        }

        public string GetTracerQueue()
        {
            return tracerQueue;
        }
    }
}
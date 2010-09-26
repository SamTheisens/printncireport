using System.ServiceModel;

namespace PrintNCIService
{
    [ServiceContract]
    public interface IPrintService
    {
        [OperationContract]
        string GetStatusQueue();

        [OperationContract]
        string GetTracerQueue();
    }
}

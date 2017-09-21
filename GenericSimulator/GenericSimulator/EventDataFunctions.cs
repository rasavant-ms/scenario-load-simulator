using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.ServiceBus.Messaging;

namespace GenericSimulator
{
    public static class EventDataFunctions
    {
        [FunctionName("EventDataFunctions")]
        public static void Run([ServiceBusTrigger("eventdataqueue", AccessRights.Listen, Connection = "ServiceBusConnection")]string myQueueItem, TraceWriter log)
        {
            log.Info($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        }
    }
}

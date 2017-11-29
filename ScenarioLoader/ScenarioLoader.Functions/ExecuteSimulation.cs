using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.ServiceBus;

namespace ScenarioLoader.Functions
{
    public static class ExecuteSimulation
    {
        [FunctionName("ExecuteSimulation")]
        public static void Run([EventHubTrigger("execute-simulation", Connection = "EventHubConnection")]string myEventHubMessage, TraceWriter log)
        {
            log.Info($"C# Event Hub trigger function processed a message: {myEventHubMessage}");
        }
    }
}

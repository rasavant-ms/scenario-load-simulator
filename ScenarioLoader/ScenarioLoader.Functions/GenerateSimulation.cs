using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.ServiceBus;
using System;

namespace ScenarioLoader.Functions
{
    public static class GenerateSimulation
    {
        [FunctionName("GenerateSimulation")]
        [return: EventHub("outputEventHubMessage", Connection = "EventHubConnection")]
        public static void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}

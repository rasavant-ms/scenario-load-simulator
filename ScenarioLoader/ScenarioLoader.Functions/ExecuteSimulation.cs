using Autofac;
using Microsoft.Azure.IoTSolutions.DeviceSimulation.Services.Diagnostics;
using Microsoft.Azure.IoTSolutions.DeviceSimulation.Services.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.ServiceBus;
using Newtonsoft.Json;
using ScenarioLoader.Logic.Interfaces;
using ScenarioLoader.Logic.Runtime;
using System.Threading;

namespace ScenarioLoader.Functions
{
    public static class ExecuteSimulation
    {
        private static CancellationToken cancellationToken { get; set; }

        [FunctionName("ExecuteSimulation")]
        public static void Run([EventHubTrigger("execute-simulation", Connection = "EventHubConnection")]string myEventHubMessage, TraceWriter log)
        {
            log.Info($"C# Event Hub trigger function processed a message: {myEventHubMessage}");

            // Temporary workaround to allow twin JSON deserialization
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                CheckAdditionalContent = false
            };

            var container = DependencyResolution.Setup();

            // Print some useful information
            PrintBootstrapInfo(container);

            // TODO: use async/await with C# 7.1
            //container.Resolve<ISimulation>().RunAsync().Wait();
            //var deviceActor = container.Resolve<IDeviceActor>();
            //var deviceModel = new DeviceModel();
            //deviceActor.Setup(deviceModel, 1).Start(cancellationToken);
        }

        private static void PrintBootstrapInfo(IContainer container)
        {
            var logger = container.Resolve<ILogger>();
            var config = container.Resolve<IConfig>();
            logger.Info("Simulation agent started", () => new { Uptime.ProcessId });
            logger.Info("Device Models folder: " + config.ServicesConfig.DeviceModelsFolder, () => { });
            logger.Info("Scripts folder:       " + config.ServicesConfig.DeviceModelsScriptsFolder, () => { });

            logger.Info("Connections per sec:  " + config.ServicesConfig.RateLimiting.ConnectionsPerSecond, () => { });
            logger.Info("Registry ops per sec: " + config.ServicesConfig.RateLimiting.RegistryOperationsPerMinute, () => { });
            logger.Info("Twin reads per sec:   " + config.ServicesConfig.RateLimiting.TwinReadsPerSecond, () => { });
            logger.Info("Twin writes per sec:  " + config.ServicesConfig.RateLimiting.TwinWritesPerSecond, () => { });
            logger.Info("Messages per second:  " + config.ServicesConfig.RateLimiting.DeviceMessagesPerSecond, () => { });
            logger.Info("Messages per day:     " + config.ServicesConfig.RateLimiting.DeviceMessagesPerDay, () => { });
        }
    }
}

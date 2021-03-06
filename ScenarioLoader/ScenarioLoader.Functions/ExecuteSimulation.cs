using Autofac;
using Microsoft.Azure.IoTSolutions.DeviceSimulation.Services;
using Microsoft.Azure.IoTSolutions.DeviceSimulation.Services.Diagnostics;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.ServiceBus;
using Newtonsoft.Json;
using ScenarioLoader.Logic.Interfaces;
using ScenarioLoader.Logic.Models;
using ScenarioLoader.Logic.Runtime;
using System;
using System.Threading;

namespace ScenarioLoader.Functions
{
    public static class ExecuteSimulation
    {
        private static CancellationToken CancellationToken { get; set; }

        [FunctionName("ExecuteSimulation")]
        public static void Run([EventHubTrigger("execute-simulation", Connection = "EventHubConnection")]string simulationDataMessage, TraceWriter log)
        {
            log.Info($"C# Event Hub trigger function started");
            Console.SetOut(new ConsoleTextWriter(log.Trace));

            // Temporary workaround to allow twin JSON deserialization
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                CheckAdditionalContent = false
            };

            var simulationData = JsonConvert.DeserializeObject<SimulationData>(simulationDataMessage);
            var container = DependencyResolution.Setup();

            var deviceFileManager = container.Resolve<IDeviceFileManager>();
            foreach (var kvp in simulationData.DeviceModelFiles)
            {
                if (deviceFileManager.DeviceModelFiles.ContainsKey(kvp.Key))
                {
                    deviceFileManager.DeviceModelFiles[kvp.Key] = kvp.Value;
                }
                else
                {
                    deviceFileManager.DeviceModelFiles.Add(kvp.Key, kvp.Value);
                }
            }

            foreach (var kvp in simulationData.DeviceModelScripts)
            {
                if (deviceFileManager.DeviceScriptFiles.ContainsKey(kvp.Key))
                {
                    deviceFileManager.DeviceScriptFiles[kvp.Key] = kvp.Value;
                }
                else
                {
                    deviceFileManager.DeviceScriptFiles.Add(kvp.Key, kvp.Value);
                }
            }

            // Print some useful information
            PrintBootstrapInfo(container);
            var deviceModels = container.Resolve<IDeviceModels>();
            var deviceModel = deviceModels.Get(simulationData.DeviceType);

            // TODO: use async/await with C# 7.1
            //container.Resolve<ISimulation>().RunAsync().Wait();
            var deviceActor = container.Resolve<IDeviceActor>();
            deviceActor.Setup(deviceModel, simulationData.DeviceName.Replace(' ', '-'), 1).Start(CancellationToken);
            Thread.Sleep(30000);
            deviceActor.Stop();
        }

        private static void PrintBootstrapInfo(IContainer container)
        {
            var logger = container.Resolve<ILogger>();
            var config = container.Resolve<IConfig>();
            logger.Info("Simulation agent started", () => new { Uptime.ProcessId });

            logger.Info("Connections per sec:  " + config.ServicesConfig.RateLimiting.ConnectionsPerSecond, () => { });
            logger.Info("Registry ops per sec: " + config.ServicesConfig.RateLimiting.RegistryOperationsPerMinute, () => { });
            logger.Info("Twin reads per sec:   " + config.ServicesConfig.RateLimiting.TwinReadsPerSecond, () => { });
            logger.Info("Twin writes per sec:  " + config.ServicesConfig.RateLimiting.TwinWritesPerSecond, () => { });
            logger.Info("Messages per second:  " + config.ServicesConfig.RateLimiting.DeviceMessagesPerSecond, () => { });
            logger.Info("Messages per day:     " + config.ServicesConfig.RateLimiting.DeviceMessagesPerDay, () => { });
        }
    }
}

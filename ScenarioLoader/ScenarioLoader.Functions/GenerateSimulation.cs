using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.ServiceBus;
using Newtonsoft.Json;
using ScenarioLoader.Logic.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace ScenarioLoader.Functions
{
    public static class GenerateSimulation
    {
        [FunctionName("GenerateSimulation")]
        public static void Run([TimerTrigger("*/5 * * * * *")]TimerInfo myTimer,
            [Blob("device-scripts/*.json", FileAccess.Read, Connection = "StorageConnectionAppSetting")] Stream inputDeviceScripts,
            [Blob("simulation-devicedata/simulation-devicedata.json", FileAccess.Read, Connection = "StorageConnectionAppSetting")] Stream inputData,
            [EventHub("simulationDataOutput", Connection = "EventHubConnection")] ICollector<SimulationData> simulationDataOutput,
            TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now} with data: {inputDeviceScripts}");

            // Get device list data as Json
            var deviceDataList = GetDataJson(inputData);
            if (deviceDataList.SimulationData != null)
            {
                foreach (var device in deviceDataList.SimulationData)
                {
                    // Get corresponding device scripts
                    // TODO

                    // Get corresponding device models
                    // TODO

                    log.Info(device.DeviceName);
                    simulationDataOutput.Add(new SimulationData()
                    {
                        DeviceType = device.DeviceType,
                        DeviceName = device.DeviceName,
                        DeviceModelFiles = new Dictionary<string, string>(),
                        DeviceModelScripts = new Dictionary<string, string>()
                    });
                }
            }
        }

        public static SimulationDeviceDataList GetDataJson(Stream inputData)
        {
            using (var sr = new StreamReader(inputData))
            {
                string text = sr.ReadToEnd();
                {
                    return JsonConvert.DeserializeObject<SimulationDeviceDataList>(text);
                }
            }
        }

        public static SimulationDeviceDataList GetFilesJson(Stream inputFiles)
        {
            using (var sr = new StreamReader(inputFiles))
            {
                string text = sr.ReadToEnd();
                {
                    return JsonConvert.DeserializeObject<SimulationDeviceDataList>(text);
                }
            }
        }
    }
}
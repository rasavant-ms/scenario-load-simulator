using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.ServiceBus;
using Newtonsoft.Json;
using ScenarioLoader.Logic.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ScenarioLoader.Functions
{
    public static class GenerateSimulation
    {
        [FunctionName("GenerateSimulation")]
        [return: EventHub("outputEventHubMessage", Connection = "EventHubConnection")]
        public static SimulationData Run([TimerTrigger("*/5 * * * * *")]TimerInfo myTimer, 
            //[Blob("device-models/*.js", FileAccess.Read, Connection = "StorageConnectionAppSetting")] Stream inputDeviceModels, 
            [Blob("device-scripts/*.json", FileAccess.Read, Connection = "StorageConnectionAppSetting")] Stream inputDeviceScripts, 
            [Blob("simulation-devicedata/simulation-devicedata.json", FileAccess.Read, Connection = "StorageConnectionAppSetting")] Stream inputData,
            ICollector<SimulationData> outputEventHubMessage,
            TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now} with data: {inputDeviceScripts}");

            // Get device list data as Json
            var deviceDataList = GetDataJson(inputData);

            if (deviceDataList.SimulationData != null)
            {
                foreach(var device in deviceDataList.SimulationData)
                {
                    // Get corresponding device scripts
                    // TODO

                    // Get corresponding device models
                    // TODO

                    log.Info(device.DeviceName);
                    outputEventHubMessage.Add(new SimulationData()
                    {
                        DeviceType = device.DeviceType,
                        DeviceName = device.DeviceName,
                        DeviceModelFiles = new Dictionary<string, string>(),
                        DeviceModelScripts = new Dictionary<string, string>()
                    });
                }
            }

            // Return empty object ?
            return new SimulationData();
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

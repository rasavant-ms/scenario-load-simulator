using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using ScenarioLoader.Logic.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace ScenarioLoader.Functions
{
    public static class GenerateSimulation
    {
        [FunctionName("GenerateSimulation")]
        public static async Task Run([TimerTrigger("*/1 * * * * *")]TimerInfo myTimer,
            [Blob("simulation-devicedata/simulation-devicedata.json", FileAccess.Read, Connection = "StorageConnectionAppSetting")] Stream inputData,
            [EventHub("simulationDataOutput", Connection = "EventHubConnection")] ICollector<SimulationData> simulationDataOutput,
            TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");

            // Get device list data as Json
            var deviceDataList = GetDataJson(inputData);
            if (deviceDataList.SimulationData != null)
            {
                foreach (var device in deviceDataList.SimulationData)
                {
                    var cloudStorageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionAppSetting"]);
                    var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

                    // Get corresponding device models
                    var deviceModelFiles = await DownloadFileContents(cloudBlobClient, "device-models", ".json");

                    // Get corresponding device scripts
                    var deviceModelScripts = await DownloadFileContents(cloudBlobClient, "device-scripts", ".js");

                    log.Info(device.DeviceName);
                    simulationDataOutput.Add(new SimulationData()
                    {
                        DeviceType = device.DeviceType,
                        DeviceName = device.DeviceName,
                        DeviceModelFiles = deviceModelFiles,
                        DeviceModelScripts = deviceModelScripts
                    });
                }
            }
        }

        private static async Task<Dictionary<string, string>> DownloadFileContents(CloudBlobClient cloudBlobClient, string containerReference, string fileExtension)
        {
            var deviceModelFiles = new Dictionary<string, string>();
            var modelContainer = cloudBlobClient.GetContainerReference(containerReference);
            foreach (var listBlobItem in modelContainer.ListBlobs(null, true))
            {
                if (listBlobItem.GetType() == typeof(CloudBlockBlob))
                {
                    var cloudBlockBlob = listBlobItem as CloudBlockBlob;
                    if (cloudBlockBlob.Name.EndsWith(fileExtension))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            cloudBlockBlob.DownloadToStream(memoryStream);
                            memoryStream.Position = 0;
                            using (var streamReader = new StreamReader(memoryStream))
                            {
                                var fileContents = await streamReader.ReadToEndAsync();
                                deviceModelFiles.Add(cloudBlockBlob.Name, fileContents);
                            }
                        }
                    }
                }
            }

            return deviceModelFiles;
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
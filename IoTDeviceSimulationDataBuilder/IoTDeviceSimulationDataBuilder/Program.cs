using System;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;


namespace IoTDeviceSimulationDataBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get simulated data templete information
            var simulationDataTemplate = GetSimulationData();

            // Generate json using the simulation data template
            var deviceJson = GenerateJsonData(simulationDataTemplate.SimulationData);

            // Create json file
            var file = AppDomain.CurrentDomain.BaseDirectory + @"\" + 
                ConfigurationManager.AppSettings["blobName"];

            File.WriteAllText(file, deviceJson);

            // Connect to storage account and upload simulation data file
            UploadSimulationDataToCloud(file);

            Console.WriteLine("simulationDataTemplate = "+ simulationDataTemplate);
        }
         
        private static void UploadSimulationDataToCloud(string file)
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                ConfigurationManager.AppSettings["StorageConnectionString"]);

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference(ConfigurationManager.AppSettings["containerName"]);

            // Retrieve reference to a blob named "myblob".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(ConfigurationManager.AppSettings["blobName"]);

            // Create or overwrite the "myblob" blob with contents from a local file.
            using (var fileStream = System.IO.File.OpenRead(file))
            {
                blockBlob.UploadFromStream(fileStream);
            }

        }

        private static string GenerateJsonData(List<DeviceDetail> simulationData)
        {
            string outJsonData = String.Empty;
            int deviceId = 1;
            SimulationDeviceDataList simulationDeviceDataList = new SimulationDeviceDataList();
            List<SimulationDeviceData> deviceList = new List<SimulationDeviceData>();

            foreach (var device in simulationData)
            {
                int numDevices = device.NumberOfDevices;
                while(numDevices > 0)
                {
                    var simulationDeviceData = new SimulationDeviceData()
                    {
                        DeviceType = device.DeviceType,
                        DeviceName = device.DeviceType + "-" + deviceId.ToString().PadLeft(4, '0')
                    };
                    deviceId++;
                    deviceList.Add(simulationDeviceData);
                    numDevices--;
                }
            }

            simulationDeviceDataList.SimulationData = deviceList;
            return JsonConvert.SerializeObject(simulationDeviceDataList); ;
        }

        private static SimulationDataTemplate GetSimulationData()
        {
            using (var sr = new StreamReader(@"simulation-data-template.json"))
            {
                string json = sr.ReadToEnd();
                {
                    return JsonConvert.DeserializeObject<SimulationDataTemplate>(json);
                }                
            }
        }
    }
}

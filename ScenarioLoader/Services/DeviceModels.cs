using Microsoft.Azure.IoTSolutions.DeviceSimulation.Services.Diagnostics;
using Microsoft.Azure.IoTSolutions.DeviceSimulation.Services.Exceptions;
using Microsoft.Azure.IoTSolutions.DeviceSimulation.Services.Models;
using Microsoft.Azure.IoTSolutions.DeviceSimulation.Services.Runtime;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Azure.IoTSolutions.DeviceSimulation.Services
{
    public interface IDeviceModels
    {
        IEnumerable<DeviceModel> GetList();
        DeviceModel Get(string id);
    }

    public class DeviceModels : IDeviceModels
    {
        private const string EXT = ".json";

        private readonly IDeviceFileManager deviceFileManager;
        private readonly IServicesConfig config;
        private readonly ILogger log;

        private List<string> deviceModelFiles;
        private List<DeviceModel> deviceModels;

        public DeviceModels(
            IDeviceFileManager deviceFileManager,
            IServicesConfig config,
            ILogger logger)
        {
            this.deviceFileManager = deviceFileManager;
            this.config = config;
            this.log = logger;
            this.deviceModelFiles = null;
            this.deviceModels = null;
        }

        public IEnumerable<DeviceModel> GetList()
        {
            if (this.deviceModels != null) return this.deviceModels;

            this.deviceModels = new List<DeviceModel>();

            try
            {
                var files = this.GetDeviceModelFiles();
                foreach (var file in files)
                {
                    //var c = JsonConvert.DeserializeObject<DeviceModel>(File.ReadAllText(f));
                    var deviceModel = JsonConvert.DeserializeObject<DeviceModel>(deviceFileManager.DeviceModelFiles[file]);
                    this.deviceModels.Add(deviceModel);
                }
            }
            catch (Exception e)
            {
                this.log.Error("Unable to load Device Model configuration",
                    () => new { e.Message, Exception = e });

                throw new InvalidConfigurationException("Unable to load Device Model configuration: " + e.Message, e);
            }

            return this.deviceModels;
        }

        public DeviceModel Get(string id)
        {
            var list = this.GetList();
            var item = list.FirstOrDefault(i => i.Id == id);
            if (item != null)
                return item;

            this.log.Warn("Device model not found", () => new { id });

            throw new ResourceNotFoundException();
        }

        private List<string> GetDeviceModelFiles()
        {
            if (this.deviceModelFiles != null) return this.deviceModelFiles;

            //this.log.Debug("Device models folder", () => new { this.config.DeviceModelsFolder });

            //var fileEntries = Directory.GetFiles(this.config.DeviceModelsFolder);

            var fileEntries = deviceFileManager.DeviceModelFiles;
            this.deviceModelFiles = fileEntries.Where(fileName => fileName.Key.EndsWith(EXT)).Select(kvp => kvp.Key).ToList();

            this.log.Debug("Device model files", () => new { this.deviceModelFiles });

            return this.deviceModelFiles;
        }
    }
}

using System.Collections.Generic;

namespace Microsoft.Azure.IoTSolutions.DeviceSimulation.Services
{
    public interface IDeviceFileManager
    {
        Dictionary<string, string> DeviceModelFiles { get; set; }

        Dictionary<string, string> DeviceScriptFiles { get; set; }
    }

    public class DeviceFileManager
        : IDeviceFileManager
    {
        public DeviceFileManager()
        {
            DeviceModelFiles = new Dictionary<string, string>();
            DeviceScriptFiles = new Dictionary<string, string>();
        }

        public Dictionary<string, string> DeviceModelFiles { get; set; }

        public Dictionary<string, string> DeviceScriptFiles { get; set; }
    }
}

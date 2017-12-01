using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTDeviceSimulationDataBuilder
{
    public class SimulationDataTemplate
    {
        public List<DeviceDetail> SimulationData { get; set; }
    }

    public class DeviceDetail
    {
        public string DeviceType { get; set; }
        public int NumberOfDevices { get; set; }
    }
}

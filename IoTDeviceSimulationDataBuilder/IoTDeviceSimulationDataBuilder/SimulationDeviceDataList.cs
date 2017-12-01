using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTDeviceSimulationDataBuilder
{
    public class SimulationDeviceDataList
    {
        public List<SimulationDeviceData> SimulationData { get; set; }
    }

    public class SimulationDeviceData
    {
        public string DeviceType { get; set; }

        public string DeviceName { get; set; }
    }
}

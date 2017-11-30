using System.Collections.Generic;

namespace ScenarioLoader.Logic.Models
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

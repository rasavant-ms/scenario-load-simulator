using System.Collections.Generic;

namespace ScenarioLoader.Logic.Models
{
    public class SimulationData
    {
        public string DeviceType { get; set; }

        public string DeviceName { get; set; }

        public IDictionary<string, string> DeviceModelFiles { get; set; }

        public IDictionary<string, string> DeviceModelScripts { get; set; }
    }
}

using Microsoft.Azure.IoTSolutions.DeviceSimulation.Services.Concurrency;
using Microsoft.Azure.IoTSolutions.DeviceSimulation.Services.Models;
using ScenarioLoader.Logic.Interfaces;
using System;

namespace ScenarioLoader.Logic.Simulation.DeviceStatusLogic.Models
{
    internal class SendTelemetryContext
    {
        public IDeviceActor DeviceActor { get; set; }
        public DeviceModel.DeviceModelMessage Message { get; set; }
        public ITimer MessageTimer { get; set; }
        public TimeSpan Interval { get; set; }
    }
}

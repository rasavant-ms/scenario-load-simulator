using Microsoft.Azure.IoTSolutions.DeviceSimulation.Services.Runtime;

namespace ScenarioLoader.Logic.Interfaces
{
    public interface IConfig
    {
        // Service layer configuration
        IServicesConfig ServicesConfig { get; }
    }
}

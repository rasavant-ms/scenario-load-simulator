using Microsoft.Azure.IoTSolutions.DeviceSimulation.Services.Models;

namespace ScenarioLoader.Logic.Interfaces
{
    public interface IDeviceStatusLogic
    {
        void Setup(string deviceId, DeviceModel deviceModel, IDeviceActor context);
        void Start();
        void Stop();
        void Run(object context);
    }
}

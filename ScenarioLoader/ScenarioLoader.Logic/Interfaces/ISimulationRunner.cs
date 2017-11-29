using Models = Microsoft.Azure.IoTSolutions.DeviceSimulation.Services.Models;

namespace ScenarioLoader.Logic.Interfaces
{
    public interface ISimulationRunner
    {
        void Start(Models.Simulation simulation);
        void Stop();
    }
}

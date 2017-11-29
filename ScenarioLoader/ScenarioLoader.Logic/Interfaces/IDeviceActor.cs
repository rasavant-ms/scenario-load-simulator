using Microsoft.Azure.IoTSolutions.DeviceSimulation.Services;
using Microsoft.Azure.IoTSolutions.DeviceSimulation.Services.Models;
using ScenarioLoader.Logic.Simulation.DeviceStatusLogic.Models;
using System.Collections.Generic;
using System.Threading;

namespace ScenarioLoader.Logic.Interfaces
{
    public interface IDeviceActor
    {
        /// <summary>
        /// Azure IoT Hub client shared by Connect and SendTelemetry
        /// </summary>
        IDeviceClient Client { get; set; }

        /// <summary>
        /// Azure IoT Hub client used by DeviceBootstrap
        /// This extra client is required only because Device Twins require a
        /// MQTT connection. If the main client already uses MQTT, the logic
        /// won't open a new connection, and reuse the existing one instead.
        /// </summary>
        IDeviceClient BootstrapClient { get; set; }

        /// <summary>
        /// The virtual state of the simulated device. The state is
        /// periodically updated using an external script. The value
        /// is shared by UpdateDeviceState and SendTelemetry.
        /// </summary>
        Dictionary<string, object> DeviceState { get; set; }

        /// <summary>
        /// The status of this device simulation actor, e.g. whether
        /// it's connecting, connected, sending, etc. Each state machine
        /// step monitors this value.
        /// </summary>
        Status ActorStatus { get; }

        /// <summary>
        /// Token used to stop the simulation, monitored by the state machine
        /// steps.
        /// </summary>
        CancellationToken CancellationToken { get; }

        /// <summary>
        /// Invoke this method before calling Start(), to initialize the actor
        /// with details like the device model and message type to simulate.
        /// If this method is not called before Start(), the application will
        /// thrown an exception.
        /// Setup() should be called only once, typically after the constructor.
        /// </summary>
        IDeviceActor Setup(DeviceModel deviceModel, int position);

        /// <summary>
        /// Call this method to start the simulated device, e.g. sending
        /// messages and responding to method calls.
        /// Pass a cancellation token, possibly the same to all the actors in
        /// the simulation, so it's easy to stop the entire simulation
        /// cancelling just one common token.
        /// </summary>
        void Start(CancellationToken cancellationToken);

        /// <summary>
        /// An optional method to stop the device actor, instead of using the
        /// cancellation token.
        /// </summary>
        void Stop();

        /// <summary>
        /// State machine flow. Change the internal state and schedule the
        /// execution of the new corresponding logic.
        /// </summary>
        void MoveNext();
    }
}

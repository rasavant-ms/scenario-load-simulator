using System;

namespace ScenarioLoader.Logic.Exceptions
{
    public class DeviceActorAlreadyInitializedException
        : Exception
    {
        public DeviceActorAlreadyInitializedException()
            : base("DeviceActor object already initialized. Call 'Start()'.")
        {
        }
    }
}

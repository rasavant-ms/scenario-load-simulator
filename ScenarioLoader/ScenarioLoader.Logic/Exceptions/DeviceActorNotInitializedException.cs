using System;

namespace ScenarioLoader.Logic.Exceptions
{
    public class DeviceActorNotInitializedException
        : Exception
    {
        public DeviceActorNotInitializedException()
            : base("DeviceActor object not initialized. Call 'Setup()' first.")
        {
        }
    }
}

// Copyright (c) Microsoft. All rights reserved.

using System.IO;

namespace Microsoft.Azure.IoTSolutions.DeviceSimulation.Services.Runtime
{
    public interface IServicesConfig
    {
        string IoTHubConnString { get; set; }
        string StorageAdapterApiUrl { get; set; }
        int StorageAdapterApiTimeout { get; set; }
        IRateLimitingConfiguration RateLimiting { get; set; }
    }

    // TODO: test Windows/Linux folder separator
    //       https://github.com/Azure/device-simulation-dotnet/issues/84
    public class ServicesConfig : IServicesConfig
    {
        private string dtf;
        private string dtbf;

        public ServicesConfig()
        {
            this.dtf = string.Empty;
            this.dtbf = string.Empty;
            this.RateLimiting = new RateLimitingConfiguration()
            {
                ConnectionsPerSecond = 50,
                RegistryOperationsPerMinute = 50,
                DeviceMessagesPerSecond = 50,
                DeviceMessagesPerDay = 8000,
                TwinReadsPerSecond = 5,
                TwinWritesPerSecond = 5,
            };
        }

        public string IoTHubConnString { get; set; }

        public string StorageAdapterApiUrl { get; set; }

        public int StorageAdapterApiTimeout { get; set; }

        public IRateLimitingConfiguration RateLimiting { get; set; }

        private string NormalizePath(string path)
        {
            return path
                       .TrimEnd(Path.DirectorySeparatorChar)
                       .Replace(
                           Path.DirectorySeparatorChar + "." + Path.DirectorySeparatorChar,
                           Path.DirectorySeparatorChar.ToString())
                   + Path.DirectorySeparatorChar;
        }
    }
}

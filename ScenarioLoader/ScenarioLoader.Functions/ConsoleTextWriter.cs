using Microsoft.Azure.WebJobs.Host;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace ScenarioLoader.Functions
{
    public class ConsoleTextWriter
        : TextWriter
    {
        private readonly Action<TraceEvent> handler;

        public ConsoleTextWriter(Action<TraceEvent> handler)
        {
            this.handler = handler;
        }

        public override Encoding Encoding => Encoding.UTF8;

        public override void WriteLine(string value)
        {
            handler(new TraceEvent(TraceLevel.Info, value));
            base.WriteLine(value);
        }
    }
}

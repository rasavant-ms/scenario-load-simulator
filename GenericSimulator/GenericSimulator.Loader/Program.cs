using Microsoft.Azure.ServiceBus;
using System;
using System.Text;
using System.Threading.Tasks;

namespace GenericSimulator.Loader
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var connectionString = "Endpoint=sb://jwdatasimulator.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=ABsRy3NJ7vY3088/qExVpTkrRub18sAU/pa5NzpNkIE=";
            var queueName = "eventdataqueue";

            var client = new QueueClient(connectionString, queueName);
            var message = new Message(Encoding.UTF8.GetBytes("test"));

            await client.SendAsync(message);

            Console.WriteLine("Message successfully sent! Press ENTER to exit program");
            Console.ReadLine();
        }
    }
}

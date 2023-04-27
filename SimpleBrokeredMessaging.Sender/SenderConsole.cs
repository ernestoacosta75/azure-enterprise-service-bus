using Azure.Messaging.ServiceBus;
using System;
using System.Threading.Tasks;

namespace SimpleBrokeredMessaging.Sender;

internal class SenderConsole
{

    //ToDo: Enter a valid Service Bus connection string
    static string ConnectionString = "Endpoint=sb://simplemessagingsystem.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=aGkDbNArHUkXqnyItfz27YeTmxRlQQiOP+ASbOzR9ks=";
    static string QueueName = "demoqueue";

    static string Sentance = "Microsoft Azure Service Bus.";


    static async Task Main(string[] args)
    {
        // Create a service bus client
        var clientOptions = new ServiceBusClientOptions()
        {
            TransportType = ServiceBusTransportType.AmqpWebSockets

        };
        var client = new ServiceBusClient(ConnectionString, clientOptions);

        // Create a service bus sender
        var sender = client.CreateSender(QueueName);


        // Send some messages
        Console.WriteLine("Sending messages...");
        foreach (var character in Sentance)
        {
            // Create a service bus message
            var message = new ServiceBusMessage(character.ToString());

            try
            {
                // Send the message to the service bus
                await sender.SendMessageAsync(message);
            
                Console.WriteLine($"    Sent: {character}");
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.StackTrace); 
            }

        }

        // Close the sender
        await sender.CloseAsync();

        Console.WriteLine("Sent messages.");
        Console.ReadLine();
    }
}

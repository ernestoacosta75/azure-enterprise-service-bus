
using Azure.Messaging.ServiceBus;
using System;
using System.Threading.Tasks;

namespace SimpleBrokeredMessaging.Receiver;

internal class ReceiverConsole
{

    static string ConnectionString = "Endpoint=sb://simplemessagingsystem.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=aGkDbNArHUkXqnyItfz27YeTmxRlQQiOP+ASbOzR9ks=";
    static string QueueName = "demoqueue";


    static async Task Main(string[] args)
    {
        // Create a service bus client
        var clientOptions = new ServiceBusClientOptions()
        {
            TransportType = ServiceBusTransportType.AmqpWebSockets

        };
        var client = new ServiceBusClient(ConnectionString, clientOptions);

        // Create a service bus receiver
        var receiver = client.CreateReceiver(QueueName);


        // Receive the messages
        Console.WriteLine("Receive messages...");

        try
        {
            while (true)
            {
                var message = await receiver.ReceiveMessageAsync();

                if (message != null)
                {
                    await Console.Out.WriteAsync(message.Body.ToString());

                    // Complete the message
                    await receiver.CompleteMessageAsync(message);
                }
                else
                {
                    await Console.Out.WriteLineAsync();
                    await Console.Out.WriteLineAsync("All messages received.");
                }
            }

        }
        finally
        {
            // Close the receiver
            await receiver.CloseAsync();
        }
    }
}

using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using System;
using System.Threading.Tasks;

namespace SimpleBrokeredMessaging.ChatConsole;

    internal class ChatApplication
    {
        //ToDo: Enter a valid Service Bus connection string
    static string ConnectionString = "Endpoint=sb://simplemessagingsystem.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=aGkDbNArHUkXqnyItfz27YeTmxRlQQiOP+ASbOzR9ks=";
        static string TopicName = "chattopic";

        static async Task Main(string[] args)
        {
            Console.WriteLine("Enter name:");
            var userName = Console.ReadLine();


            // Create an administration client to manage artifacts
            var serviceBusAdministrationClient = new ServiceBusAdministrationClient(ConnectionString);

            // Create a topic if it does not exist
            if (!await serviceBusAdministrationClient.TopicExistsAsync(TopicName))
            {
                await serviceBusAdministrationClient.CreateTopicAsync(TopicName);
            }

            // Create a temporary subscription for the user if it does not exist
            if (!await serviceBusAdministrationClient.SubscriptionExistsAsync(TopicName, userName))
            {
                var options = new CreateSubscriptionOptions(TopicName, userName)
                {
                    AutoDeleteOnIdle = TimeSpan.FromMinutes(5)
                };
                await serviceBusAdministrationClient.CreateSubscriptionAsync(options);
            }

            // Create a service bus client
        var clientOptions = new ServiceBusClientOptions()
        {
            TransportType = ServiceBusTransportType.AmqpWebSockets

        };
        var serviceBusClient = new ServiceBusClient(ConnectionString, clientOptions);

            // Create a service bus sender
            var serviceBusSender = serviceBusClient.CreateSender(TopicName);

            // Create a message processor
            var processor = serviceBusClient.CreateProcessor(TopicName, userName);

            // add handler to process messages
            processor.ProcessMessageAsync += MessageHandler;

            // add handler to process any errors
            processor.ProcessErrorAsync += ErrorHandler;

            // Start the message processor
            await processor.StartProcessingAsync();

            // Send a Hello message
            var helloMessage = new ServiceBusMessage($"{ userName } has entered the room.");
            await serviceBusSender.SendMessageAsync(helloMessage);

            while (true)
            {
                var text = Console.ReadLine();

                if (text == "exit")
                {
                    break;
                }

                // Send a chat message
                var message = new ServiceBusMessage($"{ userName }>{ text }");
                await serviceBusSender.SendMessageAsync(message);

            }

            // Send a goodbye message
            var goodbyeMessage = new ServiceBusMessage($"{ userName } has left the room.");
            await serviceBusSender.SendMessageAsync(goodbyeMessage);

            // Stop the message processor
            await processor.StopProcessingAsync();

            // Closee the processor and sender
            await processor.CloseAsync();
            await serviceBusSender.CloseAsync();
        }

        static async Task MessageHandler(ProcessMessageEventArgs args)
        {
            // Retrieve and print the message body.
            var test = args.Message.Body.ToString();
            Console.WriteLine(test);

            // Complete the message
            await args.CompleteMessageAsync(args.Message);
        }

        static async Task ErrorHandler(ProcessErrorEventArgs args)
        {
            throw new NotImplementedException();
        }
    }
}

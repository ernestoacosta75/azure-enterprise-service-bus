# Microsoft Azure Messaging Services

Microsoft Azure currently provides five different messaging services:

1) Service Bus
	- Provides durable borkered messaging: the messages are persisted in a durable datastore whilst they are in transit.
	- We can implement point-to-point messaging using queues.
	- Publish/subscribe messaging using topics and subscriptions.
	- Provides enterprise messaging functionality, such as duplicate detection, sessions, schedule in queue time and message expiration.

2) Event Hubs

3) Event Grid

4) Relay Service

5) Storage Queue

# Asynchronous messaging scenarios

An Enterpriser Service Bus (ESB) is a SW architecture model used for designing and implementing the iteraction and communication between mutually interacting SW applications in Service Oriented Architecture.

# Enterprise Service Bus

The first thing we have to do is to create messaging entities that our application can interact with.

The **Service Bus Namespace** acts as a container for messaging entities, and we can create SB namespaces within our Azure accounts. Once the SB namespace is created, we can start creating **queues**, which will give us
point-to-point messaging.

We can also create topics within a SB namespace, and within those topics we can create **subscriptions**.

Once thse messaging entities have been created, the applications can send and receive messages using these entities.

# Topics and subscriptions

- Publish-subscribe messaging (messages can be brodcast to multiple receivers based on routing rules in the messaging entities).
- Messages are sent to topics
- Messages are received from subscriptions
- Filters can determine message subscription (the subscription will decide which messages they subscribe to).

Other features are:

- **Dead-lettering**: Invalid or poison messages can be moved to a dead-letter queue, for later processing and diagnosis.
- **Message sessions**: Related messages can be grouped together in sessions and processed together.
- **Request-response correlation**: Response messages can be correlated with the appropiate request messages to allow for asynchronous two-way communication.
- **Message deferral**: Messages can be preserved on a messaging entity and retrieved later for processing.
- **Scheduled enqueue time**: Messages can be sent to a messaging entity and then enqueued at a specified time.
- **Duplicate detection**: Duplicate messages can be ignored by a messaging entity.
- **Message expiration**: Messages can be configured to expire after a specified duration.

## ESB Protocols

The ASB can use two protocols for exchanging messages with the messaging entities: **AMQP** and **HTTP**.

# Azure Service Bus NuGet packages

- Microsoft.Azure.ServiceBus (deprecated)
- Azure.Messaging.ServiceBus
	- Latest Service Bus SDK
	- Supports C# 8.0
	- Improved message serialization
	- Message serialization compatibility with legacy SDK
	
## Commonly used classes

**ServiceBusAdministrationClient**: Used to manage messaging entities within a service bus namespace.
		- We can create/delete queues and create/delete topics and subscriptions.
		- Retrievin information about those messaging entities.
		
**ServiceBusClient**: Top-level client through which all Service Bus entities can be interacted with.
	- Provides factory methods to be able to create Service Bus senders, receivers and processes.
	
**ServiceBusSender**: Used to send messages to queues and topics.

**ServiceBusReceiver**: Used to receive messages from queues and subscriptions.

**ServiceBusProcessor**: Provides using an event-based model for receiving and processing messages.

**ServiceBusMessage**: Used to represent a message to be sent to the service bus.
	- It allows us to construct a message, set the message body, and also set various properties on the message.
	
**ServiceBusReceivedMessage**: Used to represent a message that has been received from the service bus.


# Demo Simple Brokered Messaging

- Creating a **resource group** (AzureServiceBusInDepth):
	- This will act as a container for our Service Bus namespace.
	
- Within the resource group, create a resource (a Service Bus namespace. Ex: simplemessaging).
	- This will assign a unique url (ex.: .servicebus.windows.net)
	
- In order to send and receive messages, we're goint to create a queue (Queue button).	

From our console application, to authenticate to the Azure Service Bus, wee need to use both the primary/secondary connection string from the ESB.
 - Azure Portal -> ESB namespace -> Settings -> Shared access policies


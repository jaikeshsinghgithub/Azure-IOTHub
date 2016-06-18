﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
namespace sks_evh
{
    class Program
    {
        static void Main(string[] args)
        {
            string iotHubConnectionString = "HostName={iothub-name}.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey={shared-key}";
            string iotHubD2cEndpoint = "messages/events";
            StoreEventProcessor.StorageConnectionString = "DefaultEndpointsProtocol=https;AccountName={storage-name};AccountKey={storage-key}";
            StoreEventProcessor.ServiceBusConnectionString = "Endpoint=sb://{servicebus-name}.servicebus.windows.net/;SharedAccessKeyName={servicebus-key}";

            string eventProcessorHostName = Guid.NewGuid().ToString();
            EventProcessorHost eventProcessorHost = new EventProcessorHost(eventProcessorHostName, iotHubD2cEndpoint, EventHubConsumerGroup.DefaultGroupName, iotHubConnectionString, StoreEventProcessor.StorageConnectionString, "messages-events");
            Console.WriteLine("Registering EventProcessor...");
            eventProcessorHost.RegisterEventProcessorAsync<StoreEventProcessor>().Wait();

            Console.WriteLine("Receiving. Press enter key to stop worker.");
            Console.ReadLine();
            eventProcessorHost.UnregisterEventProcessorAsync().Wait();
        }
    }
}

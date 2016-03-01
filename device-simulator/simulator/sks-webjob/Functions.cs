using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.NotificationHubs;
using Microsoft.ServiceBus.Messaging;
using System.Diagnostics;

namespace sks_webjob
{
    public class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static void ProcessQueueMessage([QueueTrigger("queue")] string message, TextWriter log)
        {
            log.WriteLine(message);
        }

        private static async void SendNotificationAsync(string msg)
        {
            NotificationHubClient hub = NotificationHubClient
                .CreateClientFromConnectionString("Endpoint=sb://skshub.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=+toF6XlnEcBSPlDTN0WPW2XBNmpIEQnZbQUTfO2y/aY=", "sks-notification");
            var toast = $"<toast><visual><binding template=\"ToastText01\"><text id=\"1\">{msg}</text></binding></visual></toast>";
            await hub.SendWindowsNativeNotificationAsync(toast);
        }

        public static async Task LookupNotification()
        {
            QueueClient qc = QueueClient.CreateFromConnectionString(
                "Endpoint=sb://michiazurecontw.servicebus.windows.net/;SharedAccessKeyName=admin;SharedAccessKey=nY6VrI4Qb8/dNCi6GNpVa1ncLa8ZjcqYakucyaqaMyk=",
                "sksdemo");
            OnMessageOptions options = new OnMessageOptions();
            options.AutoComplete = false;
            options.AutoRenewTimeout = TimeSpan.FromMinutes(1);

            // Callback to handle received messages.
            qc.OnMessage((message) =>
            {
                try
                {
                    // Process message from queue.
                    var msg = message.GetBody<string>();
                    Debug.WriteLine("Body: " + msg );
                    Debug.WriteLine("MessageID: " + message.MessageId);
                    SendNotificationAsync(msg);
                    // Remove message from queue.
                    message.Complete();
                }
                catch (Exception)
                {
                    // Indicates a problem, unlock message in queue.
                    message.Abandon();
                }
            }, options);
        }
    }
}

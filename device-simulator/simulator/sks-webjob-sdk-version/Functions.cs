using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.NotificationHubs;

namespace sks_webjob_sdk_version
{
    public class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static void ProcessQueueMessage([ServiceBusTrigger("sksdemo")] string message,
                TextWriter logger)
        {
            logger.WriteLine(message);
            logger.WriteLine($"{message} received at {DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")}");
            SendNotificationAsync(message);
        }

        private static async void SendNotificationAsync(string msg)
        {
            NotificationHubClient hub = NotificationHubClient
                .CreateClientFromConnectionString("Endpoint=sb://skshub.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=+toF6XlnEcBSPlDTN0WPW2XBNmpIEQnZbQUTfO2y/aY=", "sks-notification");
            var toast = $"<toast><visual><binding template=\"ToastText01\"><text id=\"1\">new message :{msg}</text></binding></visual></toast>";
            await hub.SendWindowsNativeNotificationAsync(toast);
        }
    }
}

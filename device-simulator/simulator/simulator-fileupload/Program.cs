using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simulator_fileupload
{
    class Program
    {
        static void Info(string message)
        {
            Console.WriteLine(message);
        }
        static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        static FileSystemWatcher fileWatcher = null;
        static ServiceClient serviceClient = null;
        static void Main(string[] args)
        {
            if(args.Length < 1)
            {
                Info("execute : simulator-fileupload <folder path> to upload files in specified folder");
                return;
            }
            var path = args[0];
            if (!Directory.Exists(path))
            {
                Error(path);
                return;
            }
            serviceClient = ServiceClient.CreateFromConnectionString(ConfigurationManager.AppSettings["iotHubConnectionString"]);
            fileWatcher = new FileSystemWatcher(args[0]);
            fileWatcher.NotifyFilter = NotifyFilters.LastWrite;
            // Only watch text files.
            fileWatcher.Filter = "*.txt";

            // Add event handlers.
            fileWatcher.Created += new FileSystemEventHandler(OnChanged);
            
            // Begin watching.
            fileWatcher.EnableRaisingEvents = true;

            Info("Press any key to exit...");

            Console.ReadLine();
        }
        private async static Task ReceiveFileUploadNotificationAsync()
        {
            var notificationReceiver = serviceClient.GetFileNotificationReceiver();

            Info("\nReceiving file upload notification from service");
            while (true)
            {
                var fileUploadNotification = await notificationReceiver.ReceiveAsync();
                if (fileUploadNotification == null) continue;

                Info($"Received file upload noticiation:{string.Join(", ", fileUploadNotification.BlobName)}");

                await notificationReceiver.CompleteAsync(fileUploadNotification);
            }
        }
        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            DeviceClient client = DeviceClient.CreateFromConnectionString(ConfigurationManager.AppSettings["iotHubConnectionString"]);
            client.OpenAsync().Wait();
            if(e.ChangeType == WatcherChangeTypes.Created)
            {
                var stream = File.OpenRead(e.FullPath);

                client.UploadToBlobAsync(e.Name, stream).Wait();
            }
            client.CloseAsync().Wait();
        }
    }
}

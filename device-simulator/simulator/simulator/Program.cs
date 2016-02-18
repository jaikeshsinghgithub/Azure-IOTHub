using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;
using Microsoft.Azure.Devices.Client;
using System.Threading;

namespace simulator
{
    class Program
    {
        static RegistryManager registryManager;
        static string connectionString = "HostName=michi-iothub-01.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=evJgIxV2Mwqs9UH1R2dNIwjzM6FxiqOKOFpAjFOL1CI=";
        static string iotHubUri = "michi-iothub-01.azure-devices.net";
        static string deviceId = null;
        static int maxTemperature = 0;
        static int minTemperature = 1;
        static string deviceKey = null;
        static DeviceClient deviceClient = null;
        static Random random = new Random();
        
        /// <summary>
        /// Simulator a device
        /// </summary>
        /// <param name="args">Usage: sumulator {deviceid} {min} {max}</param>
        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Error("Usage: sumulator {deviceid} {min} {max}");
                Wait();
                return;
            }
            deviceId = args[0];
            minTemperature = int.Parse(args[1]);
            maxTemperature = int.Parse(args[2]);

            registryManager = RegistryManager.CreateFromConnectionString(connectionString);
            AddDeviceAsync().Wait();
            //deviceKey = "OTUJwGXWV6mweq/CUSlqaEnackTI6SYXBYM3U75HbKg=";

            var msg = GenerateMessage();
            //AMQP (default)
            deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));
            //HTTPS
            //deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey),
            //                    Microsoft.Azure.Devices.Client.TransportType.Http1);

            SendDeviceToCloudMessagesAsync();

            Wait("Press [ENTER] to exit...");

            RemoveDeviceAsync().Wait();
        }
        static void Wait(string msg = null)
        {
            if (string.IsNullOrEmpty(msg))
            {
                Log("Press [ENTER] to continue...");
            }
            else
            {
                Log(msg);
            }
            Console.ReadLine();
        }
        static string GenerateMessage()
        {
            var sb = new StringBuilder();
            var temp = random.Next(minTemperature, maxTemperature).ToString();
            sb.Append("{");
            sb.Append($"\"deviceId\":\"{deviceId}\",");
            sb.Append($"\"temperature\":{temp},");
            sb.Append($"\"eventId\":\"{Guid.NewGuid().ToString()}\",");
            sb.Append($"\"time\":\"{DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")}\",");
            sb.Append($"\"status\":\"\"");
            sb.Append("}");

            return sb.ToString();
        }
        static void Error(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
        static void Success(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
        static void Log(string msg)
        {
            Console.ResetColor();
            Console.WriteLine(msg);
        }
        private async static Task RemoveDeviceAsync()
        {
            var device = await registryManager.GetDeviceAsync(deviceId);
            await registryManager.RemoveDeviceAsync(device);
        }
        private async static Task AddDeviceAsync()
        {
            Device device;
            try
            {
                device = await registryManager.AddDeviceAsync(new Device(deviceId));
            }
            catch (DeviceAlreadyExistsException)
            {
                device = await registryManager.GetDeviceAsync(deviceId);
            }
            deviceKey = device.Authentication.SymmetricKey.PrimaryKey;
            Log($"device id {deviceId} : {deviceKey}");
        }

        private static async void SendDeviceToCloudMessagesAsync()
        {
            while (true)
            {
                string telemetry = GenerateMessage();

                await deviceClient.SendEventAsync(new Microsoft.Azure.Devices.Client.Message(Encoding.UTF8.GetBytes(telemetry)));
                Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, telemetry);

                Thread.Sleep(3000);
            }
        }
    }
}

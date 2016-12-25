using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace MQTTIoTHubClient
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            var deviceId = ConfigurationManager.AppSettings["deviceId"];
            var iotHubName = ConfigurationManager.AppSettings["iotHubName"];
            var iotHubUrl = $"{iotHubName}.azure-devices.net";

            var clientId = deviceId;
            var userName = $"{iotHubName}.azure-devices.net/{deviceId}";
            var password = ConfigurationManager.AppSettings["deviceSAS"].Replace("[se]","&se").Replace("[sig]","&sig");
            var publishPath = $"devices/{deviceId}/messages/events/";
            var subscribePath = $"devices/{deviceId}/messages/devicebound/#";
            
            var client = new MqttClient(iotHubUrl, 8883, true, null,null,MqttSslProtocols.TLSv1_0);
            //client.CleanSession = true; //DO NOT receive commands before subscrined
            //client.CleanSession = false; //RECEIVE commands before subscribed
            var result = client.Connect(clientId, userName, password);
            var directMethodPath = "$iothub/methods/POST/#";
            client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
            //client.Subscribe(new string[] { subscribePath }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE});
            
            client.Subscribe(new string[] { directMethodPath }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
            while (true)
            {
                var msg = GenerateMessage(deviceId, 0, null);
                var r = client.Publish(publishPath, Encoding.UTF8.GetBytes(msg));
                Console.WriteLine($"[{DateTime.Now}][{r}]Sent {msg}...");
                System.Threading.Thread.Sleep(3000);
            }
            Console.WriteLine("...press any key to exit...");
            Console.ReadLine();
            client.Disconnect();
        }
        static string GenerateMessage(string deviceId, int seq, string message)
        {
            var msg = TelemetryData.Random(deviceId, string.Format("{0}{1}", DateTime.UtcNow.ToString("yyyymmdd"), seq.ToString("0000000")), message, 1, 100);
            return JsonConvert.SerializeObject(msg);
        }
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private static void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{e.Topic}=>{System.Text.Encoding.UTF8.GetString(e.Message)}");
            Console.ResetColor();
        }
    }
}

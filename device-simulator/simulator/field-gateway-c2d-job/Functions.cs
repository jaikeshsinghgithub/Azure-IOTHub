using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices;
using System.Threading;

namespace field_gateway_c2d_job
{
    public class Functions
    {
        /// <summary>
        /// In this sample gateway, device information stored at \APP_DATA\{deviceId}; This function get all device information from the folder
        /// </summary>
        /// <returns></returns>
        public async static Task<string[]> GetAllDevicesAsync()
        {
            var connString = System.Configuration.ConfigurationManager.ConnectionStrings["iotHubOwnerConnectionString"];
            RegistryManager mgr = RegistryManager.CreateFromConnectionString(connString.ConnectionString);
            var hostname = connString.ConnectionString.Split(';')[0].Split('=')[1];
            var devices = await mgr.GetDevicesAsync(1000);
            return devices.Select(d => $"HostName={hostname};DeviceId={d.Id};SharedAccessKey={d.Authentication.SymmetricKey.PrimaryKey}").ToArray();
        }
        public static async Task ReceiveC2DAsync()
        {
            while (true)
            {
                var conns = await GetAllDevicesAsync();

                if (conns != null && conns.Length > 0)
                {
                    foreach (var conn in conns)
                    {
                        try
                        {
                            DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(conn);
                            var cmd = await deviceClient.ReceiveAsync(TimeSpan.FromSeconds(100));
                            if (cmd != null)
                            {
                                using (var sr = new StreamReader(cmd.GetBodyStream()))
                                {
                                    var msg = sr.ReadToEnd();
                                    Console.WriteLine($"*** Received:{msg}");
                                }
                                await deviceClient.CompleteAsync(cmd);
                            }
                        }
                        catch (Exception exp)
                        {
                            Console.WriteLine($"Error handling {conn}");
                            Console.WriteLine($"Exception:{exp.Message}");
                        }
                    }
                }
                Thread.Sleep(1000);
            }
        }
    }
}

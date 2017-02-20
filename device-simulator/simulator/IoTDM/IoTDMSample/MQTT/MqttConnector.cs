using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace IoTDMSample.MQTT
{
    internal class MqttConnector:IDisposable
    {
        //iothubowner:HostName=YFIOPOC.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=Zg8ePUN4p3aV02/Ahtom7TT/eg6bZTHbWs4HEcX9f6o=
        //device001:HostName=YFIOPOC.azure-devices.net;DeviceId=device001;SharedAccessKey=VU20/5r3Pd9cq5DF6IPrVSUdiNRBYildXXXoRuPmLTM=
        internal string DeviceID { get; private set; }
        internal string Password { get; private set; }
        internal string IoTHubName { get; private set; }
        internal event OnC2DReceivedHandler OnC2DReceived;
        private MqttClient mqtt = null;
        private ushort requestId = 0;
        private string deviceTwinRequestId = "";
        internal delegate void OnC2DReceivedHandler(string msg);
        internal void Initialize()
        {
            var iotHubUrl = $"{IoTHubName}.azure-devices.net";
            var userName = $"{IoTHubName}.azure-devices.net/{DeviceID}/api-version=2016-11-14";
            //var d2cTopic = $"devices/{DeviceID}/messages/events/";
            var c2dTopic = $"devices/{DeviceID}/messages/devicebound/#";
            var directMethodSubscribeTopic = "$iothub/methods/POST/#";
            var directMethodResponseTopic = "$iothub/methods/res";
            var twinGetDesiredStateTopic = $"$iothub/twin/res/#"; //"$iothub/twin/res/#";
            var twinNotificationStateTopic = "$iothub/twin/PATCH/properties/desired/#";
            mqtt = new MqttClient(iotHubUrl, 8883, true, null, null, MqttSslProtocols.TLSv1_0);
            mqtt.Subscribe(new string[] { c2dTopic , twinGetDesiredStateTopic, directMethodSubscribeTopic }, 
                                    new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE,
                                                MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE,
                                                MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE});
            mqtt.ConnectionClosed += Mqtt_ConnectionClosed;
            mqtt.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
            mqtt.ProtocolVersion = MqttProtocolVersion.Version_3_1_1;
            var result = mqtt.Connect(DeviceID, userName, Password);

            //Initialize Device Twin communication
            //var deviceTwinRequestId = Guid.NewGuid().ToString();
            //var notifyDeviceTwinTopic = $"$iothub/twin/GET/?$rid={deviceTwinRequestId}";
            //var empty = new
            //{
            //    properties = new
            //    {
            //        reported = new
            //        {
            //        }
            //    }
            //};
            //var emptyText = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(empty));
            //mqtt.Publish(notifyDeviceTwinTopic, null, MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, true);  //Sends an empty message to notify IoT hub of device twins requests
        }

        private void Mqtt_ConnectionClosed(object sender, EventArgs e)
        {
            
        }

        internal ushort SendD2C(string msg)
        {
            var d2cTopic = $"devices/{DeviceID}/messages/events/";
            requestId = mqtt.Publish(d2cTopic, Encoding.UTF8.GetBytes(msg));

            return requestId;
        }
        internal ushort UpdateReportedProperty(string name,string value)
        {
            //var msg = "{\"" + name + "\":\"" + value + "\"}";
            var msg = "{\"" + name + "\":" + value + "}";
            var rid = Guid.NewGuid().ToString();
            //const string twinPatchTopic = "$iothub/twin/PATCH/properties/reported/?$rid={0}";
            //var twinPropertyUpdateTopic = "$iothub/twin/PATCH/properties/reported/?$rid=" + rid;
            var twinPropertyUpdateTopic = "$iothub/twin/PATCH/properties/reported/?$rid=" + rid;

            requestId = mqtt.Publish(twinPropertyUpdateTopic, Encoding.UTF8.GetBytes(msg));

            return requestId;
        }
        private void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            if(OnC2DReceived != null)
            {
                var text = $"{e.Topic}::{System.Text.Encoding.UTF8.GetString(e.Message)}";
                OnC2DReceived(text);
            }
        }

        public void Dispose()
        {
            if (mqtt.IsConnected)
            {
                mqtt.Disconnect();
            }
            mqtt = null;
        }

        internal MqttConnector(string deviceId, string password, string iotHubName)
        {
            DeviceID = deviceId;
            Password = password;
            IoTHubName = iotHubName;

            Initialize();
        }
    }
}

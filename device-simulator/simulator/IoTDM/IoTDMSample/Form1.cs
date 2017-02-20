using IoTDMSample.MQTT;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IoTDMSample
{
    public partial class Form1 : Form
    {
        private MqttConnector mqtt = null;
        private Timer timer = null;
        private Random random = null;
        ushort requestId = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Init()
        {
            mqtt = new MqttConnector("<device id>",
                                        "<password>",
                                        "<iot hub name>");
            mqtt.OnC2DReceived += Mqtt_OnC2DReceived;

            random = new Random((int)DateTime.UtcNow.Ticks);
            timer = new Timer();
            timer.Interval = 3000;
            timer.Tick += Timer_Tick;

        }

        private void Mqtt_OnC2DReceived(string msg)
        {
            WriteLog(msg);
        }

        private decimal GetNewValue(decimal baseValue, int percengate)
        {
            if(random.Next(0,100) >= 49)
            {
                return (decimal)(baseValue - (random.Next(0, percengate)) * baseValue / 100);
            }
            else
            {
                return (decimal)(baseValue + (random.Next(0, percengate) ) * baseValue / 100);
            }
        }
        delegate void WriteLogHandler(string msg);
        private void WriteLog(string msg)
        {
            if (logView.InvokeRequired)
            {
                logView.Invoke(new WriteLogHandler(WriteLog), msg);
            }
            else
            {
                logView.Text += msg + Environment.NewLine;
                logView.SelectionStart = logView.Text.Length - 1;
                logView.SelectionLength = 1;
            }
        }
        private string GenerateMessage(out decimal newFanSpeed, 
                                        out decimal newTemperature, 
                                        out decimal newPressure,
                                        out decimal newWaterlevel,
                                        bool isPlainText = true)
        {
            var baseFanSpeed = fanSpeed.Value;
            var baseTemperature = temperature.Value;
            var basePressure = pressure.Value;
            var baseWaterLevel = waterLevel.Value;

            newFanSpeed = GetNewValue(baseFanSpeed, 3);
            newTemperature = GetNewValue(baseTemperature, 20);
            newPressure = GetNewValue(basePressure, 3);
            newWaterlevel = GetNewValue(baseWaterLevel, 3);

            var telemetry = TelemetryData.Random();
            telemetry.Temperature = newTemperature;
            telemetry.FanSpeed = newFanSpeed;
            telemetry.Pressure = newPressure;
            telemetry.WaterLevel = newWaterlevel; ;
            var msg = isPlainText ?
                            $"{newFanSpeed}|{newTemperature}|{newPressure}|{newWaterlevel}" :
                            JsonConvert.SerializeObject(
                                    new
                                    {
                                        Timestamp = DateTime.Now,
                                        FanSpeed = newFanSpeed,
                                        Temperature = newTemperature,
                                        Pressure = newPressure,
                                        WaterLevel = newWaterlevel
                                    }
                                );
            return msg;
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            decimal newFanSpeed, newTemperature, newPressure, newWaterlevel;
            var msg = GenerateMessage(out newFanSpeed, out newTemperature, out newPressure, out newWaterlevel, false);
            requestId = mqtt.SendD2C(msg);
            WriteLog($"[{requestId}]sent message:{msg}");

            requestId = mqtt.UpdateReportedProperty("temperature", newTemperature.ToString());
            WriteLog($"[{requestId}]updated twin property");
        }

        private void startD2C_Click(object sender, EventArgs e)
        {
            if(timer == null)
            {
                Init();
                timer.Start();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if(timer != null)
            {
                timer.Stop();
                mqtt.Dispose();
                mqtt = null;
                timer = null;
            }
        }
    }
}

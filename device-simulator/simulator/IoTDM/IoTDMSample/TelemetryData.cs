using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTDMSample
{
    //流水號,timestamp,類別,主機號,UID,DC/AC,ADSL/3G,Msg
    public class TelemetryData
    {
        public decimal Temperature { get; set; }
        public decimal WaterLevel { get; set; }
        static Random random = new System.Random();
        public decimal Pressure { get; set; }
        public DateTime Timestamp { get; set; }


        public decimal FanSpeed { get; set; }
        public static TelemetryData Random()
        {
            var rnd = random.Next(10, 15);

            var ret = new TelemetryData()
            {
                Temperature = rnd,
                Timestamp = DateTime.Now,
                Pressure = random.Next(900, 950),
                WaterLevel = random.Next(90, 95),
                FanSpeed = random.Next(1000, 1200)
            };
            return ret;
        }
    }

}

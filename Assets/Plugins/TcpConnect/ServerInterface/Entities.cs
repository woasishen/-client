using System;
using Newtonsoft.Json;

namespace TcpConnect.ServerInterface
{
    public class Eat
    {
        [JsonProperty(@"time")]
        public long Time { get; private set; }
        [JsonProperty(@"drinktype")]
        public string DrinkType { get; private set; }
        [JsonProperty(@"ml")]
        public int Ml { get; private set; }

        public override string ToString()
        {
            var time = new DateTime(1970, 1, 1, 8, 0, 0).AddMilliseconds(Time);
            return time.ToString(time.Date == DateTime.Now.Date ? "HH:mm:ss" : "yyyy/MM/dd\nHH:mm:ss");
        }
    }

    public class Diaper
    {
        [JsonProperty(@"time")]
        public long Time { get; private set; }
        [JsonProperty(@"excreteType")]
        public string ExcreteType { get; private set; }
        [JsonProperty(@"mg")]
        public int Mg { get; private set; }

        public override string ToString()
        {
            var time = new DateTime(1970, 1, 1, 8, 0, 0).AddMilliseconds(Time);
            return time.ToString(time.Date == DateTime.Now.Date ? "HH:mm:ss" : "yyyy/MM/dd\nHH:mm:ss");
        }
    }
}

using Newtonsoft.Json;

namespace TcpConnect.ServerInterface
{
    public class Eat
    {
        [JsonProperty(@"time")]
        public int Time { get; private set; }
        [JsonProperty(@"drinktype")]
        public string DrinkType { get; private set; }
        [JsonProperty(@"ml")]
        public int Ml { get; private set; }
    }

    public class Diaper
    {
        [JsonProperty(@"time")]
        public int Time { get; private set; }
        [JsonProperty(@"excreteType")]
        public string ExcreteType { get; private set; }
        [JsonProperty(@"mg")]
        public int Mg { get; private set; }
    }
}

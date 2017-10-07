using Newtonsoft.Json;

namespace TcpConnect.ServerInterface
{
    public class Packet
    {
        public Packet(string id, object msg)
        {
            Id = id;
            Msg = msg;
        }

        [JsonProperty(@"id")]
        public string Id { get; private set; }

        [JsonProperty(@"msg")]
        public object Msg { get; private set; }
    }

}

﻿using Newtonsoft.Json;
using TcpConnect.Socket;

// ReSharper disable InconsistentNaming

namespace TcpConnect.ServerInterface
{
    public enum ClientMsgId
    {
        get_eats,
        get_diapers,
        add_eat,
        add_diaper,
        del_eat,
        del_diaper,
    }

    public abstract class ClientMsgBase
    {
        [JsonIgnore]
        public abstract ClientMsgId ClientMsgId { get; }

        public Packet ToPacket()
        {
            return new Packet(ClientMsgId.ToString(), this);
        }
    }

    public abstract class ClientMsgType
    {
        public class GetEats : ClientMsgBase
        {
            public override ClientMsgId ClientMsgId => ClientMsgId.get_eats;
            public GetEats(int start, int stop)
            {
                Start = start;
                Stop = stop;
            }

            public GetEats()
            {
            }

            [JsonProperty(@"start")]
            public int Start { get; private set; }

            [JsonProperty(@"stop")]
            public int Stop { get; private set; } = 9;
        }

        public class GetDiapers : ClientMsgBase
        {
            public override ClientMsgId ClientMsgId => ClientMsgId.get_diapers;
            public GetDiapers(int start, int stop)
            {
                Start = start;
                Stop = stop;
            }

            public GetDiapers()
            {
            }

            [JsonProperty(@"start")]
            public int Start { get; private set; }

            [JsonProperty(@"stop")]
            public int Stop { get; private set; } = 9;
        }

        public class AddEat : ClientMsgBase
        {
            public override ClientMsgId ClientMsgId => ClientMsgId.add_eat;

            public AddEat(string drinkType, int ml)
            {
                DrinkType = drinkType;
                Ml = ml;
            }

            [JsonProperty(@"drinktype")]
            public string DrinkType { get; private set; }
            [JsonProperty(@"ml")]
            public int Ml { get; private set; }
        }

        public class AddDiaper : ClientMsgBase
        {
            public override ClientMsgId ClientMsgId => ClientMsgId.add_diaper;

            public AddDiaper(bool shit, int mg)
            {
                Shit = shit;
                Mg = mg;
            }

            [JsonProperty(@"shit")]
            public bool Shit { get; private set; }
            [JsonProperty(@"mg")]
            public int Mg { get; private set; }
        }

        public class DelEat : ClientMsgBase
        {
            public override ClientMsgId ClientMsgId => ClientMsgId.del_eat;
            public DelEat(int curLength)
            {
                CurLength = curLength;
            }

            [JsonProperty(@"length")]
            public int CurLength { get; private set; }
        }

        public class DelDiaper : ClientMsgBase
        {
            public override ClientMsgId ClientMsgId => ClientMsgId.del_diaper;
            public DelDiaper(int curLength)
            {
                CurLength = curLength;
            }

            [JsonProperty(@"length")]
            public int CurLength { get; private set; }
        }
    }

    public class SendMethod
    {
        private readonly SyncQueue<ClientMsgBase> _sendQueue;

        public SendMethod(SyncQueue<ClientMsgBase> sendQueue)
        {
            _sendQueue = sendQueue;
        }

        public void GetEat()
        {
            var msg = new ClientMsgType.GetEats();
            _sendQueue.Enqueue(msg);
        }

        public void GetEat(int start, int stop)
        {
            var msg = new ClientMsgType.GetEats(start, stop);
            _sendQueue.Enqueue(msg);
        }

        public void GetDiaper()
        {
            var msg = new ClientMsgType.GetDiapers();
            _sendQueue.Enqueue(msg);
        }

        public void GetDiaper(int start, int stop)
        {
            var msg = new ClientMsgType.GetDiapers(start, stop);
            _sendQueue.Enqueue(msg);
        }

        public void AddEat(string drinkType, int ml)
        {
            var msg = new ClientMsgType.AddEat(drinkType, ml);
            _sendQueue.Enqueue(msg);
        }
        public void AddDiaper(bool shit, int mg)
        {
            var msg = new ClientMsgType.AddDiaper(shit, mg);
            _sendQueue.Enqueue(msg);
        }

        public void DelEat()
        {
            var msg = new ClientMsgType.DelEat(StaticData.Eats.Count);
            _sendQueue.Enqueue(msg);
        }

        public void DelDiaper()
        {
            var msg = new ClientMsgType.DelDiaper(StaticData.Diapers.Count);
            _sendQueue.Enqueue(msg);
        }
    }
}

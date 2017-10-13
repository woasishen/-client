using System;
using System.Collections.Generic;
using Newtonsoft.Json;

// ReSharper disable InconsistentNaming

namespace TcpConnect.ServerInterface
{
    public enum ServerMsgId
    {
        none,
        get_eats,
        get_diapers,

        b_add_eatc,
        b_add_diaperc,
        b_del_eatc,
        b_del_diaperc,
    }

    public class ServerMsgBase
    {
        /// <summary>
        /// server error
        /// </summary>
        [JsonProperty(@"err")]
        internal object Err { get; private set; }

        /// <summary>
        /// normal error
        /// </summary>
        [JsonProperty(@"error")]
        internal object Error { get; private set; }

        /// <summary>
        /// sucess
        /// </summary>
        [JsonProperty(@"succeed")]
        internal bool Succeed { get; private set; }
    }

    public class ServerIdAttribute : Attribute
    {
        public ServerMsgId Id { get; private set; }
        public ServerIdAttribute(ServerMsgId id)
        {
            Id = id;
        }
    }

    public abstract class ServerMsgType
    {
        [ServerId(ServerMsgId.get_eats)]
        public class GetEats : ServerMsgBase
        {
            [JsonProperty(@"start")]
            public int Start { get; private set; }

            [JsonProperty(@"data")]
            public Cache<Eat> Eats { get; private set; }
        }

        [ServerId(ServerMsgId.get_diapers)]
        public class GetDiapers : ServerMsgBase
        {
            [JsonProperty(@"start")]
            public int Start { get; private set; }

            [JsonProperty(@"data")]
            public Cache<Diaper> Diapers { get; private set; }
        }
    }

    public abstract class BroadcastMsgType
    {
        [ServerId(ServerMsgId.b_add_eatc)]
        public class AddEat : ServerMsgBase
        {
            [JsonProperty(@"stop")]
            public int Stop { get; private set; }
            [JsonProperty(@"data")]
            public Eat Eat { get; private set; }
        }

        [ServerId(ServerMsgId.b_add_diaperc)]
        public class AddDiaper : ServerMsgBase
        {
            [JsonProperty(@"stop")]
            public int Stop { get; private set; }
            [JsonProperty(@"data")]
            public Diaper Diaper { get; private set; }
        }

        [ServerId(ServerMsgId.b_del_eatc)]
        public class DelEat
        {
            [JsonProperty(@"stop")]
            public int Stop { get; private set; }
        }

        [ServerId(ServerMsgId.b_del_diaperc)]
        public class DelDiaper
        {
            [JsonProperty(@"stop")]
            public int Stop { get; private set; }
        }
    }

    public class ServerMsgAction
    {
        [ServerId(ServerMsgId.get_eats)]
        public Action<ServerMsgType.GetEats> GetEats;

        [ServerId(ServerMsgId.get_diapers)]
        public Action<ServerMsgType.GetDiapers> GetDiapers;

        //broadcast
        [ServerId(ServerMsgId.b_add_eatc)]
        public Action<BroadcastMsgType.AddEat> B_AddEat;

        [ServerId(ServerMsgId.b_add_diaperc)]
        public Action<BroadcastMsgType.AddDiaper> B_AddDiaper;

        [ServerId(ServerMsgId.b_del_eatc)]
        public Action<BroadcastMsgType.DelEat> B_DelEat;

        [ServerId(ServerMsgId.b_del_diaperc)]
        public Action<BroadcastMsgType.DelDiaper> B_DelDiaper;
    }
}

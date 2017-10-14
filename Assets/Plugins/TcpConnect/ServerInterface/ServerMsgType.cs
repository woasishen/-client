using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local

namespace TcpConnect.ServerInterface
{
    public enum ServerMsgId
    {
        none,
        get_eatsc,
        get_diapersc,

        add_eatc,
        add_diaperc,
        del_eatc,
        del_diaperc,

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
        [ServerId(ServerMsgId.get_eatsc)]
        public class GetEats : ServerMsgBase
        {
            [JsonProperty(@"start")]
            public int Start { get; private set; }

            [JsonProperty(@"data")]
            public List<Eat> Eats { get; private set; }
        }

        [ServerId(ServerMsgId.get_diapersc)]
        public class GetDiapers : ServerMsgBase
        {
            [JsonProperty(@"start")]
            public int Start { get; private set; }

            [JsonProperty(@"data")]
            public List<Diaper> Diapers { get; private set; }
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
        private readonly Dictionary<ServerMsgId, bool> _isDirty = new Dictionary<ServerMsgId, bool>();

        private static readonly Dictionary<ServerMsgId, MethodInfo> MsgAction = 
            new Dictionary<ServerMsgId, MethodInfo>();

        static ServerMsgAction()
        {
            BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;
            foreach (var methodInfo in typeof(ServerMsgAction).GetMethods(bindingFlags))
            {
                var serverIdAttr = (ServerIdAttribute)Attribute.GetCustomAttribute(methodInfo, typeof(ServerIdAttribute));
                if (serverIdAttr == null)
                {
                    continue;
                }
                MsgAction[serverIdAttr.Id] = methodInfo;
            }
        }

        public void HandleMsg(ServerMsgId id, object[] args)
        {
            if (MsgAction.ContainsKey(id))
            {
                MsgAction[id].Invoke(this, args);
            }
            _isDirty[id] = true;
        }

        public bool IsDirty(ServerMsgId id)
        {
            if (_isDirty.ContainsKey(id))
            {
                return _isDirty[id];
            }
            return false;
        }

        public void ClearDirty(ServerMsgId id)
        {
            _isDirty[id] = false;
        }

        [ServerId(ServerMsgId.get_eatsc)]
        protected void OnGetEats(ServerMsgType.GetEats obj)
        {
            StaticData.Eats = new Cache<Eat>(obj.Eats); ;
        }
        [ServerId(ServerMsgId.get_diapersc)]
        protected void OnGetDiapers(ServerMsgType.GetDiapers obj)
        {
            StaticData.Diapers = new Cache<Diaper>(obj.Diapers);
        }
        [ServerId(ServerMsgId.b_add_eatc)]
        protected void OnBAddEat(BroadcastMsgType.AddEat obj)
        {
            if (obj.Stop == StaticData.Eats.Stop + 1)
            {
                StaticData.Eats.Push(obj.Eat);
            }
        }
        [ServerId(ServerMsgId.b_add_diaperc)]
        protected void OnBAddDiaper(BroadcastMsgType.AddDiaper obj)
        {
            if (obj.Stop == StaticData.Diapers.Stop + 1)
            {
                StaticData.Diapers.Push(obj.Diaper);
            }
        }
        [ServerId(ServerMsgId.b_del_eatc)]
        protected void OnBDelEat(BroadcastMsgType.DelEat obj)
        {
            if (obj.Stop == StaticData.Eats.Stop)
            {
                StaticData.Eats.Pop();
            }
        }
        [ServerId(ServerMsgId.b_del_diaperc)]
        protected void OnBDelDiaper(BroadcastMsgType.DelDiaper obj)
        {
            if (obj.Stop == StaticData.Diapers.Stop)
            {
                StaticData.Diapers.Pop();
            }
        }

    }
}

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TcpConnect.ServerInterface;

namespace TcpConnect.ServerManager
{
    internal class GetMsgManager
    {
        private const string BROADER_CAST_START = @"b_";

        private readonly ServerMsgAction _serverMsgAction;

        private readonly Dictionary<ServerMsgId, Type> _msgTypeDict =
            new Dictionary<ServerMsgId, Type>();
        private readonly Dictionary<ServerMsgId, Type> _broadcastMsgTypeDict =
            new Dictionary<ServerMsgId, Type>();

        public Action<string> ErrAction;
        public Action<string> ErrorAction;
        public Action NotSucceedAction;

        public GetMsgManager(ServerMsgAction serverMsgAction)
        {
            _serverMsgAction = serverMsgAction;
            foreach (var typeInfo in typeof(ServerMsgType).GetNestedTypes())
            {
                var serverIdAttr = (ServerIdAttribute)Attribute.GetCustomAttribute(typeInfo, typeof(ServerIdAttribute));
                if (serverIdAttr == null)
                {
                    continue;
                }
                _msgTypeDict[serverIdAttr.Id] = typeInfo;
            }

            foreach (var typeInfo in typeof(BroadcastMsgType).GetNestedTypes())
            {
                var serverIdAttr = (ServerIdAttribute)Attribute.GetCustomAttribute(typeInfo, typeof(ServerIdAttribute));
                if (serverIdAttr == null)
                {
                    continue;
                }
                _broadcastMsgTypeDict[serverIdAttr.Id] = typeInfo;
            }
        }

        public void HandleMsg(Packet packet)
        {
            if (!packet.Id.StartsWith(BROADER_CAST_START))
            {
                HandleNormalMsg(packet);
            }
            else
            {
                HandleBroaderCastMsg(packet);
            }
        }

        private void HandleNormalMsg(Packet packet)
        {
            ServerMsgId id;
            EnumHelper.TryParse(packet.Id, out id);

            var serverMsgBase = (ServerMsgBase)JsonConvert.DeserializeObject(packet.Msg.ToString(), 
                _msgTypeDict.ContainsKey(id) ? _msgTypeDict[id] : typeof(ServerMsgBase));
            _serverMsgAction.HandleNormalMsg(id, new object[] {serverMsgBase});
            if (serverMsgBase.Err != null)
            {
                //数据库内部错误
                if (ErrAction != null)
                {
                    ErrAction.Invoke(serverMsgBase.Err);
                }
                return;
            }
            if (serverMsgBase.Error != null)
            {
                if (ErrorAction != null)
                {
                    ErrorAction.Invoke(serverMsgBase.Error);
                }
                return;
            }
            if (!serverMsgBase.Succeed)
            {
                if (NotSucceedAction != null)
                {
                    NotSucceedAction.Invoke();
                }
                return;
            }
        }

        private void HandleBroaderCastMsg(Packet packet)
        {
            var id = (ServerMsgId)Enum.Parse(typeof(ServerMsgId), packet.Id);

            var msg = _broadcastMsgTypeDict.ContainsKey(id)
                ? JsonConvert.DeserializeObject(packet.Msg.ToString(), _broadcastMsgTypeDict[id])
                : packet.Msg.ToString();

            _serverMsgAction.HandleBMsg(id, new[] { msg });
        }
    }
}

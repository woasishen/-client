using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using TcpConnect.ServerInterface;

namespace TcpConnect.ServerManager
{
    internal class GetMsgManager
    {
        private const string BROADER_CAST_START = @"b_";

        private readonly ServerMsgAction _serverMsgAction;

        private readonly Dictionary<ServerMsgId, FieldInfo> _msgActionDict =
            new Dictionary<ServerMsgId, FieldInfo>();
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
            foreach (var fieldInfo in typeof(ServerMsgAction).GetFields())
            {
                var serverIdAttr = (ServerIdAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(ServerIdAttribute));
                if (serverIdAttr == null)
                {
                    continue;
                }
                _msgActionDict[serverIdAttr.Id] = fieldInfo;
            }

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
            Enum.TryParse(packet.Id, out id);
            var serverMsgBase = (ServerMsgBase)JsonConvert.DeserializeObject(packet.Msg.ToString(), 
                _msgTypeDict.ContainsKey(id) ? _msgTypeDict[id] : typeof(ServerMsgBase));
            if (serverMsgBase.Err != null)
            {
                //数据库内部错误
                ErrAction?.Invoke(serverMsgBase.Err.ToString());
                return;
            }
            if (serverMsgBase.Error != null)
            {
                ErrorAction?.Invoke(serverMsgBase.Error.ToString());
                return;
            }
            if (!serverMsgBase.Succeed)
            {
                NotSucceedAction.Invoke();
                return;
            }

            var action = _msgActionDict[id].GetValue(_serverMsgAction);
            if (action == null)
            {
                return;
            }
            var methond = action.GetType().GetMethod("Invoke");
            methond.Invoke(action, new object[] { serverMsgBase });
        }

        private void HandleBroaderCastMsg(Packet packet)
        {
            var id = (ServerMsgId)Enum.Parse(typeof(ServerMsgId), packet.Id);

            var msg = _broadcastMsgTypeDict.ContainsKey(id)
                ? JsonConvert.DeserializeObject(packet.Msg.ToString(), _broadcastMsgTypeDict[id])
                : packet.Msg?.ToString();

            var action = _msgActionDict[id].GetValue(_serverMsgAction);
            if (action == null)
            {
                return;
            }
            var methond = action.GetType().GetMethod("Invoke");
            methond.Invoke(action, new[] { msg });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Reflection;
using TcpConnect;
using TcpConnect.ServerInterface;

// ReSharper disable InconsistentNaming

namespace BabySchedule
{
    public class MsgController : MonoBehaviourBase
    {
        private static readonly Dictionary<ServerMsgId, MethodInfo> _msgAction =
            new Dictionary<ServerMsgId, MethodInfo>();

        static MsgController()
        {
            BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;
            foreach (var methodInfo in typeof(MsgController).GetMethods(bindingFlags))
            {
                var serverIdAttr = (ServerIdAttribute)Attribute.GetCustomAttribute(methodInfo, typeof(ServerIdAttribute));
                if (serverIdAttr == null)
                {
                    continue;
                }
                _msgAction[serverIdAttr.Id] = methodInfo;
            }
        }

        public event Action GetEats;
        public event Action GetDiapers;
        public event Action AddEats;
        public event Action AddDiapers;
        public event Action DelEats;
        public event Action DelDiapers;

        //broadcast
        public event Action B_AddEat;
        public event Action B_AddDiaper;
        public event Action B_DelEat;
        public event Action B_DelDiaper;

        public static MsgController Instance { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            Instance = this;
        }

        protected override void Update()
        {
            base.Update();
            foreach (var keyValue in _msgAction)
            {
                if (TcpInstance.Socket.MsgActions.IsDirty(keyValue.Key))
                {
                    keyValue.Value.Invoke(this, new object[] { });
                    TcpInstance.Socket.MsgActions.ClearDirty(keyValue.Key);
                }
            }
        }
        [ServerId(ServerMsgId.get_eatsc)]
        protected void OnGetEats()
        {
            var handler = GetEats;
            if (handler != null) handler();
        }
        [ServerId(ServerMsgId.get_diapersc)]
        protected void OnGetDiapers()
        {
            var handler = GetDiapers;
            if (handler != null) handler();
        }
        [ServerId(ServerMsgId.add_eatc)]
        protected virtual void OnAddEats()
        {
            var handler = AddEats;
            if (handler != null) handler();
        }
        [ServerId(ServerMsgId.add_diaperc)]
        protected virtual void OnAddDiapers()
        {
            var handler = AddDiapers;
            if (handler != null) handler();
        }
        [ServerId(ServerMsgId.del_eatc)]
        protected virtual void OnDelEats()
        {
            var handler = DelEats;
            if (handler != null) handler();
        }
        [ServerId(ServerMsgId.del_diaperc)]
        protected virtual void OnDelDiapers()
        {
            var handler = DelDiapers;
            if (handler != null) handler();
        }
        [ServerId(ServerMsgId.b_add_eatc)]
        protected virtual void OnBAddEat()
        {
            var handler = B_AddEat;
            if (handler != null) handler();
        }
        [ServerId(ServerMsgId.b_add_diaperc)]
        protected virtual void OnBAddDiaper()
        {
            var handler = B_AddDiaper;
            if (handler != null) handler();
        }
        [ServerId(ServerMsgId.b_del_eatc)]
        protected virtual void OnBDelEat()
        {
            var handler = B_DelEat;
            if (handler != null) handler();
        }
        [ServerId(ServerMsgId.b_del_diaperc)]
        protected virtual void OnBDelDiaper()
        {
            var handler = B_DelDiaper;
            if (handler != null) handler();
        }
    }
}

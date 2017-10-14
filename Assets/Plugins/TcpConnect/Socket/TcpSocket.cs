using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using TcpConnect.ServerInterface;
using TcpConnect.ServerManager;

namespace TcpConnect.Socket
{
    public class TcpSocket
    {
        private static int HeadLength;

        private readonly TcpClient _client;
        private readonly int _recieveBuffSize;

        private readonly Thread _sendThread;
        private readonly Thread _getThread;
        private readonly Thread _handleMsgThread;

        private readonly SyncQueue<ClientMsgBase> _sendQueue = new SyncQueue<ClientMsgBase>();
        private readonly SyncQueue<Packet> _getQueue = new SyncQueue<Packet>();

        private readonly GetMsgManager _getMsgManager;
        private readonly ServerMsgAction _msgActions = new ServerMsgAction();

        public Action<string> ErrAction;
        public Action<string> ErrorAction;
        public Action NotSucceedAction;

        public ServerMsgAction MsgActions
        {
            get { return _msgActions; }
        }

        public SendMethod SendMethod { get; private set; }

        public TcpSocket(string address, int port)
        {
            _getMsgManager = new GetMsgManager(MsgActions)
            {
                ErrAction = s => ErrAction.Invoke(s),
                ErrorAction = s => ErrorAction.Invoke(s),
                NotSucceedAction = () => NotSucceedAction.Invoke(),
            };
            SendMethod = new SendMethod(_sendQueue);

            _client = new TcpClient(address, port);
            _recieveBuffSize = _client.ReceiveBufferSize;

            //client开始必须和server约定Head长度
            var buffer = new byte[4];
            _client.GetStream().Read(buffer, 0, 4);
            HeadLength = Convert.ToInt32(Encoding.UTF8.GetString(buffer, 0, 4));

            _sendThread = new Thread(SendThread);
            _getThread = new Thread(GetThread);
            _handleMsgThread = new Thread(HandleMsgFromServer);

            _sendThread.Start();
            _getThread.Start();
            _handleMsgThread.Start();
        }

        public void Abort()
        {
            _sendThread.Abort();
            _getThread.Abort();
            _handleMsgThread.Abort();
            _client.Client.Disconnect(false);
            _client.Close();
        }

        private void HandleMsgFromServer()
        {
            while (true)
            {
                var msg = _getQueue.Dequeue();
                _getMsgManager.HandleMsg(msg);
            }
            // ReSharper disable once FunctionNeverReturns
        }

        private void SendThread()
        {
            while (true)
            {
                var msg = _sendQueue.Dequeue();
                using (var ms = new MemoryStream())
                {
                    var json = JsonConvert.SerializeObject(msg.ToPacket());
                    using (var sw = new StreamWriter(ms))
                    {
                        sw.Write(json);
                        sw.Flush();
                        var title = BitConverter.GetBytes(ms.Length);
                        _client.GetStream().Write(title, 0, HeadLength);
                        _client.GetStream().Write(ms.GetBuffer(), 0, (int)ms.Length);
                    }
                }
            }
            // ReSharper disable once FunctionNeverReturns
        }

        private void GetThread()
        {
            var bodyBuffer = new byte[_recieveBuffSize];
            var headBuffer = new byte[HeadLength];
            var bodyStream = new MemoryStream();
            var sr = new StreamReader(bodyStream);

            while (true)
            {
                _client.GetStream().Read(headBuffer, 0, HeadLength);
                var leftLength = BitConverter.ToInt32(headBuffer, 0);
                while (leftLength > 0)
                {
                    if (leftLength <= _recieveBuffSize)
                    {
                        _client.GetStream().Read(bodyBuffer, 0, leftLength);
                        bodyStream.Write(bodyBuffer, 0, leftLength);
                        leftLength = 0;
                    }
                    else
                    {
                        _client.GetStream().Read(bodyBuffer, 0, _recieveBuffSize);
                        bodyStream.Write(bodyBuffer, 0, _recieveBuffSize);
                        leftLength = leftLength - _recieveBuffSize;
                    }

                }
                bodyStream.Position = 0;
                var str = sr.ReadToEnd();
                var packet = JsonConvert.DeserializeObject<Packet>(str);
                bodyStream.SetLength(0);
                _getQueue.Enqueue(packet);

            }
            // ReSharper disable once FunctionNeverReturns
        }
    }
}

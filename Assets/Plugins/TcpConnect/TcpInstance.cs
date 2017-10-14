using System;
using System.Collections.Generic;
using UnityEngine;
using TcpConnect.ServerInterface;
using TcpConnect.Socket;

namespace TcpConnect
{
    public class TcpInstance
    {
        //private readonly TcpSocket _socket = new TcpSocket("192.168.0.252", 18080);

        public static TcpSocket Socket { get; private set; }

        static TcpInstance()
        {
            //Socket = new TcpSocket("111.206.45.12", 30021);
            Socket = new TcpSocket("192.168.1.3", 18080);

            Socket.ErrAction += s => Debug.LogError(@"Err:" + s);
            Socket.ErrorAction += s => Debug.LogError(@"Error:" + s);
            Socket.NotSucceedAction += () => Debug.LogError(@"请求未成功");
        }
        
    }
}

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

        public static TcpSocket Socket { get; }

        static TcpInstance()
        {
            //Socket = new TcpSocket("111.206.45.12", 30021);
            Socket = new TcpSocket("192.168.1.3", 18080);

            Socket.ErrAction += s => Debug.LogError(@"Err:" + s);
            Socket.ErrorAction += s => Debug.LogError(@"Error:" + s);
            Socket.NotSucceedAction += () => Debug.LogError(@"请求未成功");

            Socket.MsgActions.GetEats = GetEats;
            Socket.MsgActions.GetDiapers = GetDiapers;

            Socket.MsgActions.B_AddEat = BAddEat;
            Socket.MsgActions.B_AddDiaper = BAddDiaper;
            Socket.MsgActions.B_DelEat = BDelEat;
            Socket.MsgActions.B_DelDiaper = BDelDiaper;
        }

        private static void BDelDiaper(BroadcastMsgType.DelDiaper delDiaper)
        {
            if (delDiaper.Stop == StaticData.Diapers.Stop)
            {
                StaticData.Diapers.Pop();
            }
        }

        private static void BDelEat(BroadcastMsgType.DelEat delEat)
        {
            if (delEat.Stop == StaticData.Eats.Stop)
            {
                StaticData.Eats.Pop();
            }
        }

        private static void BAddEat(BroadcastMsgType.AddEat addEat)
        {
            if (addEat.Stop == StaticData.Eats.Stop + 1)
            {
                StaticData.Eats.Push(addEat.Eat);
            }
        }

        private static void BAddDiaper(BroadcastMsgType.AddDiaper addDiaper)
        {
            if (addDiaper.Stop == StaticData.Diapers.Stop + 1)
            {
                StaticData.Diapers.Push(addDiaper.Diaper);
            }
        }

        private static void GetDiapers(ServerMsgType.GetDiapers getDiapers)
        {
            StaticData.Diapers = getDiapers.Diapers;
        }

        private static void GetEats(ServerMsgType.GetEats getEats)
        {
            StaticData.Eats = getEats.Eats;
        }
    }
}

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
            Socket = new TcpSocket("192.168.0.252", 18080);

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

        private static void BDelDiaper(string delDiaper)
        {
            if (delDiaper == (StaticData.Diapers.Count - 1).ToString())
            {
                StaticData.Diapers.Pop();
                return;
            }
        }

        private static void BDelEat(string delEat)
        {
            if (delEat == (StaticData.Eats.Count - 1).ToString())
            {
                StaticData.Eats.Pop();
                return;
            }

        }

        private static void BAddEat(BroadcastMsgType.AddEat addEat)
        {
            if (addEat.CurLength + 1 == StaticData.Eats.Count)
            {
                StaticData.Eats.Push(addEat.Eat);
            }
        }

        private static void BAddDiaper(BroadcastMsgType.AddDiaper addDiaper)
        {
            if (addDiaper.CurLength + 1 == StaticData.Diapers.Count)
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

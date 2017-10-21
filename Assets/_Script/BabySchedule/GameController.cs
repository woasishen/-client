using TcpConnect;
using UnityEngine;

namespace BabySchedule
{
    public class GameController : MonoBehaviourBase
    {
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
            TcpInstance.Socket.SendMethod.GetEat();
            TcpInstance.Socket.SendMethod.GetDiaper();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Debug.Log("gameover");
            TcpInstance.Socket.Abort();
        }
    }
}

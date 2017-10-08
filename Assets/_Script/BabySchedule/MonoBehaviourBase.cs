using UnityEngine;

namespace BabySchedule
{
    public class MonoBehaviourBase : MonoBehaviour
    {
        protected virtual void Awake() { }
        protected virtual void Start() { }
        protected virtual void OnDestroy() { }
    }
}
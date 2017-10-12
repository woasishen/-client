using UnityEngine;

namespace BabySchedule
{
    public class MonoBehaviourBase : MonoBehaviour
    {
        protected virtual void Awake() { }
        protected virtual void Start() { }

        protected virtual void Update() { }

        protected virtual void OnDestroy() { }

        protected virtual void OnRectTransformDimensionsChange() { }
    }
}
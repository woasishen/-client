using UnityEngine;

namespace BabySchedule
{
    public class MonoBehaviourBase : MonoBehaviour
    {
        protected virtual void Awake() { }
        protected virtual void Start() { }

        protected virtual void Update() { }

        protected virtual void OnEnable() { }

        protected virtual void OnDisable() { }

        protected virtual void OnDestroy() { }
    }

    public abstract class MonoBehaviourDateChange : MonoBehaviourBase
    {

        protected abstract void DataChanged();

        protected override void Awake()
        {
            base.Awake();
            MsgController.Instance.GetEats += DataChanged;
            MsgController.Instance.GetDiapers += DataChanged;
            MsgController.Instance.B_AddEat += DataChanged;
            MsgController.Instance.B_DelEat += DataChanged;
            MsgController.Instance.B_AddDiaper += DataChanged;
            MsgController.Instance.B_DelDiaper += DataChanged;
        }


        protected override void OnDestroy()
        {
            base.OnDestroy();
            MsgController.Instance.GetEats -= DataChanged;
            MsgController.Instance.GetDiapers -= DataChanged;
            MsgController.Instance.B_AddEat -= DataChanged;
            MsgController.Instance.B_DelEat -= DataChanged;
            MsgController.Instance.B_AddDiaper -= DataChanged;
            MsgController.Instance.B_DelDiaper -= DataChanged;
        }
    }
}
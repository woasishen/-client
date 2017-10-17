using UnityEngine;

namespace BabySchedule.Panels
{
    public class CanvasInstance : MonoBehaviourBase
    {
        private GameObject _waitting;

        public static CanvasInstance Instance;

        public void ShowWaitting()
        {
            _waitting.transform.SetAsLastSibling();
            _waitting.SetActive(true);
        }

        public void HideWaitting()
        {
            _waitting.transform.SetAsFirstSibling();
            _waitting.SetActive(false);
        }

        protected override void Awake()
        {
            base.Awake();
            Instance = this;
            gameObject.AddComponent<UILayers>();
            gameObject.AddComponent<UIViews>();
            _waitting = Instantiate(Resources.Load<GameObject>("Prefabs/Panels/Waitting"));
            _waitting.transform.SetParent(transform);
            _waitting.transform.localPosition = Vector3.zero;
            _waitting.transform.localScale = Vector3.one;
            _waitting.SetActive(false);
        }
    }
}

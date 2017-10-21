using BabySchedule.Panels.Views.Base;
using UnityEngine;

namespace BabySchedule.Panels
{
    public class UIViews : MonoBehaviourBase
    {
        public static UIViews Instance { get; private set; }

        public GameObject Views { get; private set; }

        public T ShowView<T>() where T : BaseView
        {
            var viewRes = Resources.Load<GameObject>("Prefabs/Panels/Views/" + typeof(T).Name);
            if (!viewRes)
            {
                Debug.LogError("LayerRes is null :" + typeof(T).Name);
                return null;
            }
            Views.SetActive(true);
            Views.transform.SetAsLastSibling();

            var viewObj = Instantiate(viewRes);
            var viewComponent = viewObj.AddComponent<T>();
            viewComponent.OnExit = () =>
            {
                Views.SetActive(false);
            };
            viewObj.transform.SetParent(Views.transform);
            viewObj.transform.localScale = Vector3.one;
            viewObj.transform.localPosition = Vector3.zero;
            return viewComponent;
        }

        protected override void Awake()
        {
            base.Awake();
            Instance = this;

            Views = new GameObject("Views");
            Views.transform.SetParent(transform);
            Views.transform.localScale = Vector3.one;
            Views.transform.localPosition = Vector3.zero;
            Views.SetActive(false);
        }
    }
}

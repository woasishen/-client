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
            viewRes.transform.SetAsLastSibling();

            var viewObj = Instantiate(viewRes);
            var viewComponent = viewObj.AddComponent<T>();
            viewComponent.OnExit = () =>
            {
                Views.SetActive(false);
            };
            viewObj.transform.SetParent(Views.transform);
            viewObj.UIFill();
            return viewComponent;
        }

        protected override void Awake()
        {
            base.Awake();
            Instance = this;

            Views = new GameObject("Views");
            Views.transform.SetParent(transform);
            Views.AddComponent<RectTransform>().UIFill();
            Views.SetActive(false);
        }
    }
}

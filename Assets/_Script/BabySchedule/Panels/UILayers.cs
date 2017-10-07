using BabySchedule.Panels.Layers.Base;
using UnityEngine;

namespace BabySchedule.Panels
{
    public class UILayers : MonoBehaviourBase
    {
        public static UILayers Instance { get; private set; }

        public LayerBg LayerBg;
        public GameObject Layers { get; private set; }

        public T ShowLayer<T>(bool useBgClose = true) where T : BaseLayer
        {
            var layerRes = Resources.Load<GameObject>("Prefabs/Panels/Layers/" + typeof(T).Name);
            if (!layerRes)
            {
                Debug.LogError("LayerRes is null :" + typeof(T).Name);
                return null;
            }
            Layers.SetActive(true);
            Layers.transform.SetAsLastSibling();

            var layerObj = Instantiate(layerRes);
            var layerComponent = layerObj.AddComponent<T>();
            layerComponent.OnExit += () =>
            {
                Layers.SetActive(false);
            };
            layerObj.transform.SetParent(Layers.transform);
            layerObj.transform.localPosition = Vector3.zero;
            LayerBg.Show(useBgClose);
            return layerComponent;
        }

        protected override void Awake()
        {
            base.Awake();
            Instance = this;

            Layers = new GameObject("Layers");
            Layers.transform.SetParent(transform);
            Layers.AddComponent<RectTransform>().UIFill();
            Layers.SetActive(false);

            var layerBgObj = Instantiate(Resources.Load<GameObject>("Prefabs/Panels/Layers/Base/LayerBg"));
            layerBgObj.transform.SetParent(Layers.transform);
            layerBgObj.UIFill();
            layerBgObj.SetActive(false);
            LayerBg = layerBgObj.GetComponent<LayerBg>();
        }
    }
}

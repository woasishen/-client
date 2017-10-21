using BabySchedule.Panels.Layers.Base;
using UnityEngine;

namespace BabySchedule.Panels
{
    public class UiLayers : MonoBehaviourBase
    {
        public static UiLayers Instance { get; private set; }

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
            layerObj.transform.localScale = Vector3.one;
            LayerBg.Show(useBgClose);
            return layerComponent;
        }

        protected override void Awake()
        {
            base.Awake();
            Instance = this;

            Layers = new GameObject("Layers");
            Layers.transform.SetParent(transform);
            Layers.transform.localScale = Vector3.one;
            Layers.transform.localPosition = Vector3.zero;
            Layers.SetActive(false);

            var layerBgObj = Instantiate(Resources.Load<GameObject>("Prefabs/Panels/Layers/Base/LayerBg"));
            layerBgObj.transform.SetParent(Layers.transform);
            layerBgObj.transform.localScale = Vector3.one;
            layerBgObj.transform.localPosition = Vector3.zero;
            layerBgObj.SetActive(false);
            LayerBg = layerBgObj.GetComponent<LayerBg>();
        }
    }
}

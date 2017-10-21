using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BabySchedule.Panels.Layers.Base
{
    public class LayerBg : MonoBehaviourBase, IPointerClickHandler
    {
        private Image _image;

        public void Show(bool useBgClose = true)
        {
            gameObject.SetActive(true);
            Enable = useBgClose;

            _image.color = !useBgClose ? new Color(0, 0, 0, 0.3f) : new Color(0, 0, 0, 0);
        }

        public bool Enable { get; set; }

        protected override void Awake()
        {
            base.Awake();
            _image = gameObject.GetComponent<Image>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (UILayers.Instance.Layers.transform.childCount == 0)
            {
                Debug.LogError("LayerBg has no child");
                gameObject.SetActive(false);
                return;
            }
            if (!Enable)
            {
                return;
            }
            Enable = false;
            UILayers.Instance.Layers.GetComponentInChildren<Animator>().SetTrigger("Exit");
        }
    }
}

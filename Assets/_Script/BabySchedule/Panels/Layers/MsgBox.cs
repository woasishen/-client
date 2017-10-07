using BabySchedule.Panels.Layers.Base;
using UnityEngine.UI;

namespace BabySchedule.Panels.Layers
{
    public class MsgBox : BaseLayer
    {
        public static void Show(string msg)
        {
            var msgBox = UILayers.Instance.ShowLayer<MsgBox>(false);
            msgBox._text.text = msg;
        }

        private Text _text;

        protected override void Awake()
        {
            base.Awake();
            var panel = transform.GetChild(0);
            _text = panel.GetChild(0).GetComponent<Text>();
            panel.GetChild(1).GetComponent<Button>().onClick.AddListener(() =>
            {
                UILayers.Instance.LayerBg.gameObject.SetActive(false);
                Destroy(gameObject);
            });
        }
    }
}

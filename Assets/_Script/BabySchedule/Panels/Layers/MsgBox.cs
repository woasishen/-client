using BabySchedule.Panels.Layers.Base;
using UnityEngine;
using UnityEngine.UI;

namespace BabySchedule.Panels.Layers
{
    public enum MsgBoxEnum
    {
        Confirm,
        ConfirmCancel,
    }

    public enum MsgBoxResult
    {
        Confirm,
        Cancel,
    }

    public class MsgBox : BaseLayer
    {
        public static void Show(string msg, MsgBoxEnum boxEnum = MsgBoxEnum.Confirm)
        {
            var msgBox = UILayers.Instance.ShowLayer<MsgBox>(false);
            var panel = msgBox.transform.GetChild(0);
            panel.GetChild(0).GetComponent<Text>().text = msg;
            var confirm = panel.GetChild(1).GetComponent<Text>();
            var cancel = panel.GetChild(2).GetComponent<Text>();

            switch (boxEnum)
            {
                case MsgBoxEnum.Confirm:
                    cancel.gameObject.SetActive(false);
                    confirm.transform.localPosition = new 
                        Vector3(0, confirm.transform.localPosition.y);
                    break;
                case MsgBoxEnum.ConfirmCancel:
                    break;
            }
        }


        protected override void Awake()
        {
            base.Awake();
            
            panel.GetChild(1).GetComponent<Button>().onClick.AddListener(() =>
            {
                UILayers.Instance.Layers.GetComponentInChildren<Animator>().SetTrigger("Exit");
            });
        }
    }
}

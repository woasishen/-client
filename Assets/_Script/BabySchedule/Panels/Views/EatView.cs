using System.Collections.Generic;
using System.Linq;
using BabySchedule.Panels.Layers;
using BabySchedule.Panels.Views.Base;
using TcpConnect;
using UnityEngine.UI;

namespace BabySchedule.Panels.Views
{
    public class EatView : BaseView
    {
        private ToggleGroup _milkOrWater;
        private InputField _ml;
        protected override void Awake()
        {
            base.Awake();
            transform.Find("ConfirmButton").GetComponent<Button>().onClick.AddListener(ConfirmBtnClicked);
            _milkOrWater = transform.Find("MilkOrWater").GetComponent<ToggleGroup>();
            _ml = transform.Find("Ml").GetComponentInChildren<InputField>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            MsgController.Instance.AddEats += AddEats;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            MsgController.Instance.AddEats -= AddEats;
        }

        private void AddEats()
        {
            CanvasInstance.Instance.HideWaitting();
            Exit();
        }

        private void ConfirmBtnClicked()
        {
            if (!_milkOrWater.AnyTogglesOn())
            {
                MsgBox.Show("请勾选饮品");
                return;
            }

            int ml;
            int.TryParse(_ml.text, out ml);

            if (ml <= 0)
            {
                MsgBox.Show("请填写ML");
                return;
            }

            TcpInstance.Socket.SendMethod.AddEat(
                _milkOrWater.ActiveToggles().First().name,
                ml
            );
            CanvasInstance.Instance.ShowWaitting();
        }
    }
}

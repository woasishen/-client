using System;
using System.Collections.Generic;
using System.Linq;
using BabySchedule.Panels.Layers;
using BabySchedule.Panels.Views.Base;
using TcpConnect;
using TcpConnect.Socket;
using UnityEngine.UI;

namespace BabySchedule.Panels.Views
{
    public class DiaperView : BaseView
    {
        private ToggleGroup _shitOrPee;
        private InputField _mg;
        protected override void Awake()
        {
            base.Awake();
            transform.Find("ConfirmButton").GetComponent<Button>().onClick.AddListener(ConfirmBtnClicked);
            _shitOrPee = transform.Find("ShitOrPee").GetComponent<ToggleGroup>();
            _mg = transform.Find("Mg").GetComponentInChildren<InputField>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            MsgController.Instance.AddDiapers += AddDiapers;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            MsgController.Instance.AddDiapers -= AddDiapers;
        }

        private void AddDiapers()
        {
            CanvasInstance.Instance.HideWaitting();
            Exit();
        }

        private void ConfirmBtnClicked()
        {
            if (!_shitOrPee.AnyTogglesOn())
            {
                MsgBox.Show("请勾选是否拉屎");
                return;
            }

            int mg;
            int.TryParse(_mg.text, out mg);

            if (mg <= 0)
            {
                MsgBox.Show("请填写尿不湿质量");
                return;
            }

            TcpInstance.Socket.SendMethod.AddDiaper(
                _shitOrPee.ActiveToggles().First().name,
                mg
            );
            CanvasInstance.Instance.ShowWaitting();
        }
    }
}

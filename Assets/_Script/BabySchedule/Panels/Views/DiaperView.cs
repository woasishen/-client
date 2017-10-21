using System.Linq;
using BabySchedule.Panels.Layers;
using BabySchedule.Panels.Views.Base;
using TcpConnect;
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

        private void ConfirmBtnClicked()
        {
            if (!_shitOrPee.AnyTogglesOn())
            {
                MsgBox.Instance.Show("请勾选是否拉屎");
                return;
            }

            int mg;
            int.TryParse(_mg.text, out mg);

            if (mg <= 0)
            {
                MsgBox.Instance.Show("请填写尿不湿质量");
                return;
            }

            TcpInstance.Socket.SendMethod.AddDiaper(
                _shitOrPee.ActiveToggles().First().name,
                mg
            );
            CanvasInstance.Instance.ShowWaitting();
            Exit();
        }
    }
}

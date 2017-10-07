using BabySchedule.Panels.Layers;
using BabySchedule.Panels.Views.Base;
using TcpConnect;
using UnityEngine.UI;

namespace BabySchedule.Panels.Views
{
    public class EatView : BaseView
    {
        private ToggleGroup _milkOrWater;
        protected override void Awake()
        {
            base.Awake();
            transform.Find("ConfirmButton").GetComponent<Button>().onClick.AddListener(ConfirmBtnClicked);
            _milkOrWater = transform.Find("MilkOrWater").GetComponent<ToggleGroup>();
        }

        private void ConfirmBtnClicked()
        {
            if (!_milkOrWater.AnyTogglesOn())
            {
                MsgBox.Show("请勾选饮品");
                return;
            }
            TcpInstance.Socket.SendMethod.AddEat("milk", 1);
        }


    }
}

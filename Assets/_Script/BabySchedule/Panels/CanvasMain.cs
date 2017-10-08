using BabySchedule.Panels.Views;
using TcpConnect;
using UnityEngine;
using UnityEngine.UI;

namespace BabySchedule.Panels
{
    public class CanvasMain : MonoBehaviourBase
    {
        private Button _diaperBtn;
        private Button _eatBtn;

        protected override void Awake()
        {
            base.Awake();
            _diaperBtn = transform.Find("Bottom/DiaperButton").GetComponent<Button>();
            _eatBtn = transform.Find("Bottom/EatButton").GetComponent<Button>();

            _diaperBtn.onClick.AddListener(DiaperBtnClicked);
            _eatBtn.onClick.AddListener(EatBtnClicked);
        }

        private void DiaperBtnClicked()
        {
            UIViews.Instance.ShowView<DiaperView>();
        }

        private void EatBtnClicked()
        {
            UIViews.Instance.ShowView<EatView>();
        }
    }   
}

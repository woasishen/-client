using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BabySchedule.Panels.Views;
using TcpConnect;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace BabySchedule.Panels.Main
{
    public class CanvasMain : MonoBehaviourDateChange
    {
        private static readonly Dictionary<int, Dictionary<int, List<string>>> Options =
            new Dictionary<int, Dictionary<int, List<string>>>
            {
                {
                    1,
                    new Dictionary<int, List<string>>
                    {
                        {
                            1,
                            new List<string>{
                                "Milk",
                                "Water"
                            }
                        },
                        {
                            2,
                            new List<string>{
                                "Shit",
                                "Pee"
                            }
                        },
                    }
                }
            };

        private const int DropDownCount = 2;
        private static readonly TimeSpan CanDelTime = new TimeSpan(0, 0, 10, 0);

        private Button _diaperBtn;
        private Button _eatBtn;
        private Button _delBtn;
        private Coroutine _disableDelBtn;
        private MainVerticalScrollView _verticalScrollView;

        private List<Dropdown> _dropdowns;

        protected override void DataChanged()
        {
            UpdateDelBtn();
        }

        private void UpdateDelBtn()
        {
            var lastAddTime = CommonMethod.GetLastAddItemTime();
            if (lastAddTime == null)
            {
                _delBtn.gameObject.SetActive(false);
                return;
            }
            var passedTime = DateTime.Now - lastAddTime.Value;
            if (passedTime > CanDelTime)
            {
                _delBtn.gameObject.SetActive(false);
                return;
            }
            _delBtn.gameObject.SetActive(true);
            if (_disableDelBtn != null)
            {
                StopCoroutine(_disableDelBtn);
            }
            _disableDelBtn = StartCoroutine(DisableDelBtn(CanDelTime - passedTime));
        }

        private IEnumerator DisableDelBtn(TimeSpan span)
        {
            yield return new WaitForSeconds((float)span.TotalSeconds);
            _delBtn.gameObject.SetActive(false);
        }

        protected override void Awake()
        {
            base.Awake();

            _diaperBtn = transform.Find("Bottom/DiaperButton").GetComponent<Button>();
            _eatBtn = transform.Find("Bottom/EatButton").GetComponent<Button>();

            _verticalScrollView = transform.Find("Content/ScrollView").GetComponent<MainVerticalScrollView>();

            _diaperBtn.onClick.AddListener(DiaperBtnClicked);
            _eatBtn.onClick.AddListener(EatBtnClicked);

            _dropdowns = new List<Dropdown>();
            for (int i = 0; i < DropDownCount; i++)
            {
                _dropdowns.Add(transform.Find("Top/ChoicePanel/Dropdown" + i).GetComponent<Dropdown>());
                var index = i;
                _dropdowns[i].onValueChanged.AddListener(arg => DropDownChange(index, arg));
                if (i != 0)
                {
                    _dropdowns[i].gameObject.SetActive(false);
                }
            }
            _delBtn = transform.Find("Top/ChoicePanel/DelFirstBtn").GetComponent<Button>();
            UpdateDelBtn();
            _delBtn.onClick.AddListener(() => StartCoroutine(DelLatelyAdd()));
        }

        private IEnumerator DelLatelyAdd()
        {
            var msgBox = MsgBox.Instance.ShowWarn("确认要删除吗？", MsgBoxStyle.YesNo);
            yield return msgBox;
            if (msgBox.Result != MsgBoxResult.Yes)
            {
                yield break;
            }

            CanvasInstance.Instance.ShowWaitting();
            if (!StaticData.Eats.Any())
            {
                TcpInstance.Socket.SendMethod.DelDiaper();
                yield break;
            }
            if (!StaticData.Diapers.Any())
            {
                TcpInstance.Socket.SendMethod.DelEat();
                yield break;
            }
            if (StaticData.Diapers.Peek().Time > StaticData.Eats.Peek().Time)
            {
                TcpInstance.Socket.SendMethod.DelDiaper();
            }
            else
            {
                TcpInstance.Socket.SendMethod.DelEat();
            }
        }

        private void DropDownChange(int index, int arg)
        {
            if (index == 0)
            {
                for (int i = 1; i < DropDownCount; i++)
                {
                    _dropdowns[i].value = 0;
                    _dropdowns[i].gameObject.SetActive(arg != 0);
                    if (arg <= 0)
                    {
                        continue;
                    }
                    _dropdowns[i].options.RemoveRange(1, _dropdowns[i].options.Count - 1);
                    _dropdowns[i].AddOptions(Options[i][arg]);
                }
            }

            _verticalScrollView.SetData(_dropdowns[0].value, _dropdowns[1].captionText.text);
            _verticalScrollView.ReloadData();
        }

        private void EatBtnClicked()
        {
            UIViews.Instance.ShowView<EatView>();
        }

        private void DiaperBtnClicked()
        {
            UIViews.Instance.ShowView<DiaperView>();
        }
    }
}

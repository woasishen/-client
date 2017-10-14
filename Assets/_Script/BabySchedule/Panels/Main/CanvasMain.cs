using System;
using System.Collections.Generic;
using BabySchedule.Panels.Views;
using PathologicalGames;
using TcpConnect;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace BabySchedule.Panels.Main
{
    public class CanvasMain : MonoBehaviourBase
    {
        private static readonly Dictionary<int, Dictionary<int, List<string>>> OPTIONS =
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

        private const int DROP_DOWN_COUNT = 2;

        private Button _diaperBtn;
        private Button _eatBtn;
        private MainVerticalScrollView _verticalScrollView;

        private List<Dropdown> _dropdowns;

        protected override void Awake()
        {
            base.Awake();
            _diaperBtn = transform.Find("Bottom/DiaperButton").GetComponent<Button>();
            _eatBtn = transform.Find("Bottom/EatButton").GetComponent<Button>();

            _verticalScrollView = transform.Find("Content/ScrollView").GetComponent<MainVerticalScrollView>();

            _diaperBtn.onClick.AddListener(DiaperBtnClicked);
            _eatBtn.onClick.AddListener(EatBtnClicked);

            _dropdowns = new List<Dropdown>();
            for (int i = 0; i < DROP_DOWN_COUNT; i++)
            {
                _dropdowns.Add(transform.Find("Top/ChoicePanel/Dropdown" + i).GetComponent<Dropdown>());
                var index = i;
                _dropdowns[i].onValueChanged.AddListener(arg => DropDownChange(index, arg));
                if (i != 0)
                {
                    _dropdowns[i].gameObject.SetActive(false);
                }
            }
        }

        private void DropDownChange(int index, int arg)
        {
            if (index == 0)
            {
                for (int i = 1; i < DROP_DOWN_COUNT; i++)
                {
                    _dropdowns[i].value = 0;
                    _dropdowns[i].gameObject.SetActive(arg != 0);
                    if (arg <= 0)
                    {
                        continue;
                    }
                    _dropdowns[i].options.RemoveRange(1, _dropdowns[i].options.Count - 1);
                    _dropdowns[i].AddOptions(OPTIONS[i][arg]);
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

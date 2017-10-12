using System;
using System.Collections.Generic;
using BabySchedule.Panels.Views;
using TcpConnect;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace BabySchedule.Panels
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
        private ScrollRect _scrollRect;
        private GameObject _scrollCell;
        private float _scrollCellHeight;

        private List<Dropdown> _dropdowns;

        protected override void Awake()
        {
            base.Awake();
            _diaperBtn = transform.Find("Bottom/DiaperButton").GetComponent<Button>();
            _eatBtn = transform.Find("Bottom/EatButton").GetComponent<Button>();

            _scrollRect = transform.Find("Content/ScrollView").GetComponent<ScrollRect>();
            _scrollCell = Resources.Load<GameObject>("Prefabs/Panels/VIews/DiaperViewItems/Cell");
            _scrollCellHeight = _scrollCell.GetComponent<RectTransform>().rect.height;

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

            _scrollRect.verticalScrollbar.onValueChanged.AddListener(OnScrollRectScrolled);
        }

        private void OnScrollRectScrolled(float arg)
        {
            
        }

        private void DropDownChange(int index, int arg)
        {
            if (index > 0)
            {
                UpdateContent();
                return;
            }
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

            UpdateContent();
        }

        private void UpdateContent()
        {
            int totalCount = 0;
            switch (_dropdowns[0].value)
            {
                case 0:
                    totalCount = StaticData.Eats.Count + StaticData.Diapers.Count;
                    break;
                case 1:
                    totalCount = StaticData.Eats.Count;
                    break;
                case 2:
                    totalCount = StaticData.Diapers.Count;
                    break;
            }
            totalCount = 20;
            if (totalCount == 0)
            {
                return;
            }
            _scrollRect.content.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, totalCount * _scrollCellHeight);

            var needCount = Math.Ceiling(_scrollRect.viewport.rect.height / _scrollCellHeight) + 1;
            for (int i = 0; i < needCount; i++)
            {
                var tempItem = Instantiate(_scrollCell);
                tempItem.transform.SetParent(_scrollRect.content.transform);
                tempItem.transform.localPosition = new Vector3(0, -100 * i, 0);
                tempItem.UIFillWidth();
            }
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

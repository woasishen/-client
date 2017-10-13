using System;
using PathologicalGames;
using UnityEngine;
using UnityEngine.UI;

namespace BabySchedule.Panels
{
    public abstract class BaseVerticalScrollView : MonoBehaviourBase
    {
        protected abstract string CellPath { get; }

        protected abstract int CellCount { get; }
        protected abstract void UpdateItem(Transform item, int index);

        protected ScrollRect ScrollRect;

        private float _cellHight;
        private int _usingCellCount;

        protected override void Awake()
        {
            base.Awake();
            Spawns.Instance.CheckAndPrepareSpawnPool(Spawns.Instance.ViewCellPool, CellPath);

            ScrollRect = GetComponent<ScrollRect>();
            ScrollRect.verticalScrollbar.onValueChanged.AddListener(OnScrollRectScrolled);

            _cellHight = ScrollRect.content.rect.height;

            _usingCellCount = (int)Math.Ceiling(GetComponent<RectTransform>().rect.height / _cellHight) + 1;
        }

        public void ReloadData(bool keepPosition = false)
        {
            if (!keepPosition)
            {
                ScrollRect.content.anchoredPosition = new Vector2(0, 0);
            }

            ScrollRect.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
                CellCount * _cellHight);

            var startIndex = (int)(ScrollRect.content.anchoredPosition.y / _cellHight);

            var endIndex = Math.Min(startIndex + _usingCellCount, CellCount);
            for (var i = startIndex; i < endIndex; i++)
            {
                var tempItem = Spawns.Instance.ViewCellPool.Spawn(CellPath);
                tempItem.SetParent(ScrollRect.content);
                tempItem.localPosition = new Vector3(0, - i * _cellHight);
                UpdateItem(tempItem, i);
            }
        }

        private void OnScrollRectScrolled(float arg)
        {
            //if (_cellHight - _usingCells.First.Value.localPosition.y < ScrollRect.content.anchoredPosition.y)
            //{
            //    Spawns.Instance.ViewCellPool.Despawn(_usingCells.First.Value.transform);

            //    _usingCells.RemoveFirst();
            //}
        }
    }
}

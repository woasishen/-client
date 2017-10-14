using System;
using System.Collections.Generic;
using System.Linq;
using PathologicalGames;
using UnityEngine;
using UnityEngine.UI;

namespace BabySchedule.Panels
{
    class CellList<T> : LinkedList<T>
    {
        public int StartIndex { set; get; }

        public int EndIndex
        {
            get { return StartIndex + Count; }
        }
    }

    public abstract class BaseVerticalScrollView : MonoBehaviourBase
    {
        enum FirstOrLast
        {
            First,
            Last
        }

        protected abstract string CellPath { get; }

        protected abstract int CellCount { get; }
        protected abstract void UpdateItem(Transform item, int index);

        protected ScrollRect ScrollRect;

        private float _oldScrollF;
        private float _cellHeight;
        private float _viewHeight;
        private int _showCellMaxCount;
        private readonly CellList<Transform> _usingCells = new CellList<Transform>();

        protected override void Awake()
        {
            base.Awake();
            Spawns.Instance.CheckAndPrepareSpawnPool(Spawns.Instance.ViewCellPool, CellPath);

            ScrollRect = GetComponent<ScrollRect>();
            ScrollRect.verticalScrollbar.onValueChanged.AddListener(OnScrollRectScrolled);

            _cellHeight = ScrollRect.content.rect.height;
            _viewHeight = GetComponent<RectTransform>().rect.height;

            _showCellMaxCount = (int)Math.Ceiling(_viewHeight / _cellHeight) + 2;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            ReloadData(true);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            ClearItems();
        }

        public void ReloadData(bool keepPosition = false)
        {
            ClearItems();
            if (!keepPosition)
            {
                ScrollRect.content.anchoredPosition = new Vector2(0, 0);
            }

            ScrollRect.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
                CellCount * _cellHeight);

            var startIndex = (int)(ScrollRect.content.anchoredPosition.y / _cellHeight);

            var endIndex = Math.Min(startIndex + _showCellMaxCount, CellCount);

            _usingCells.StartIndex = startIndex;

            for (var i = startIndex; i < endIndex; i++)
            {
                AddItem(FirstOrLast.Last);
            }
            
            _oldScrollF = ScrollRect.verticalScrollbar.value;
        }

        private void ClearItems()
        {
            while (_usingCells.Count > 0)
            {
                RemoveItem(FirstOrLast.First);
            }
            _usingCells.StartIndex = 0;
        }

        private void AddItem(FirstOrLast firstOrLast)
        {
            var tempItem = Spawns.Instance.ViewCellPool.Spawn(CellPath);
            int index = 0;
            switch (firstOrLast)
            {
                case FirstOrLast.First:
                    _usingCells.AddFirst(tempItem);
                    _usingCells.StartIndex--;
                    index = _usingCells.StartIndex;
                    break;
                case FirstOrLast.Last:
                    _usingCells.AddLast(tempItem);
                    index = _usingCells.EndIndex - 1;
                    break;
            }
            tempItem.SetParent(ScrollRect.content);
            tempItem.localPosition = new Vector3(0, -index * _cellHeight);
            tempItem.localScale = Vector3.one;
            UpdateItem(tempItem, index);
        }

        private void RemoveItem(FirstOrLast firstOrLast)
        {
            switch (firstOrLast)
            {
                case FirstOrLast.First:
                    Spawns.Instance.ViewCellPool.Despawn(_usingCells.First.Value.transform);
                    _usingCells.RemoveFirst();
                    _usingCells.StartIndex++;
                    break;
                case FirstOrLast.Last:
                    Spawns.Instance.ViewCellPool.Despawn(_usingCells.Last.Value.transform);
                    _usingCells.RemoveLast();
                    break;
            }
        }

        private bool TryMoveFirstToLast()
        {
            if (_usingCells.EndIndex == CellCount)
            {
                return false;
            }
            RemoveItem(FirstOrLast.First);
            AddItem(FirstOrLast.Last);
            return true;
        }

        private bool TryMoveLastToFirst()
        {
            if (_usingCells.StartIndex == 0)
            {
                return false;
            }
            RemoveItem(FirstOrLast.Last);
            AddItem(FirstOrLast.First);
            return true;
        }

        private void OnScrollRectScrolled(float arg)
        {
            if (_usingCells.Count == 0)
            {
                return;
            }
            //move up
            if (_oldScrollF > arg)
            {
                while (-_usingCells.Last.Value.localPosition.y <
                    ScrollRect.content.anchoredPosition.y + _viewHeight)
                {
                    if (!TryMoveFirstToLast())
                    {
                        break;
                    }
                }
            }
            //move down
            else
            {
                while (_cellHeight - _usingCells.First.Value.localPosition.y >
                    ScrollRect.content.anchoredPosition.y)
                {
                    if (!TryMoveLastToFirst())
                    {
                        break;
                    }
                }
            }
            _oldScrollF = arg;

        }
    }
}

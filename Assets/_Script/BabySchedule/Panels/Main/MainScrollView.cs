using System;
using PathologicalGames;
using TcpConnect;
using UnityEngine;

namespace BabySchedule.Panels.Main
{
    public class MainScrollView : BaseScrollView
    {
        protected override string CellPath => @"Prefabs/Panels/VIews/DiaperViewItems/Cell";


        private void UpdateItem(GameObject item, int index)
        {
            
        }

        public void UpdateContent(int mainDropDownValue)
        {
            int totalCount = 0;
            switch (mainDropDownValue)
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
            ScrollRect.content.GetComponent<RectTransform>()
                .SetSizeWithCurrentAnchors(
                    RectTransform.Axis.Vertical, totalCount * _cellHight);

            var needCount = Math.Ceiling(ScrollRect.viewport.rect.height / _cellHight) + 1;

            _usingCells.Clear();
            for (var i = 0; i < needCount; i++)
            {
                var tempItem = Spawns.Instance.ViewCellPool.Spawn(CELL_PATH);
                tempItem.SetParent(ScrollRect.content.transform);
                tempItem.localPosition = new Vector3(0, -100 * i, 0);
                tempItem.gameObject.UIFillWidth();
                _usingCells.AddLast(tempItem.GetComponent<RectTransform>());
            }
        }
    }
}

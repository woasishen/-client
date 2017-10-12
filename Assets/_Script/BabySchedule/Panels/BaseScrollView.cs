using PathologicalGames;
using UnityEngine;
using UnityEngine.UI;

namespace BabySchedule.Panels
{
    public abstract class BaseScrollView : MonoBehaviourBase
    {
        protected abstract string CellPath { get; }

        protected abstract void UpdateItem(GameObject item, int index);



        private float _cellHight;
        protected ScrollRect ScrollRect;

        protected override void Awake()
        {
            base.Awake();
            Spawns.Instance.CheckAndPrepareSpawnPool(Spawns.Instance.ViewCellPool, CellPath);
            var tempObj = Spawns.Instance.ViewCellPool.Spawn(CellPath);
            _cellHight = tempObj.GetComponent<RectTransform>().rect.height;
            Spawns.Instance.ViewCellPool.Despawn(tempObj);

            ScrollRect = GetComponent<ScrollRect>();
            ScrollRect.verticalScrollbar.onValueChanged.AddListener(OnScrollRectScrolled);
        }

        protected void UpdateCells()
        {
            
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

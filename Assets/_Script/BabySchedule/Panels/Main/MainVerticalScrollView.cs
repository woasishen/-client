using System;
using PathologicalGames;
using TcpConnect;
using UnityEngine;

namespace BabySchedule.Panels.Main
{
    public class MainVerticalScrollView : BaseVerticalScrollView
    {
        protected override string CellPath => @"Prefabs/Panels/VIews/DiaperViewItems/Cell";

        protected override int CellCount
        {
            get { return 20; }
        }

        protected override void UpdateItem(Transform item, int index)
        {
            
        }

        public void SetData(int dropDownValue)
        {
            
        }
    }
}

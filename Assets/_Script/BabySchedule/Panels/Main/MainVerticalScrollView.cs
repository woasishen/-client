using System;
using PathologicalGames;
using TcpConnect;
using UnityEngine;
using UnityEngine.UI;

namespace BabySchedule.Panels.Main
{
    public class MainVerticalScrollView : BaseVerticalScrollView
    {
        protected override string CellPath
        {
            get { return @"Prefabs/Panels/VIews/DiaperViewItems/Cell"; }
        } 

        protected override int CellCount
        {
            get { return 20; }
        }

        protected override void UpdateItem(Transform item, int index)
        {
            item.GetChild(1).GetComponent<Text>().text = index.ToString();
        }

        public void SetData(int dropDownValue)
        {
            
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using TcpConnect;
using UnityEngine;
using UnityEngine.UI;

namespace BabySchedule.Panels.Main
{
    public class MainVerticalScrollView : BaseVerticalScrollView
    {
        private class MainVerticalScrollViewCell
        {
            private readonly List<string> _lines = new List<string>(4);

            public MainVerticalScrollViewCell(
                string line1,
                string line2, 
                string line3, 
                string line4)
            {
                _lines.Add(line1);
                _lines.Add(line2);
                _lines.Add(line3);
                _lines.Add(line4);
            }

            public string this[int index]
            {
                get { return _lines[index]; }
            }
        }

        private List<MainVerticalScrollViewCell> _cells = new List<MainVerticalScrollViewCell>();

        protected override string CellPath
        {
            get { return @"Prefabs/Panels/VIews/DiaperViewItems/Cell"; }
        } 

        protected override int CellCount
        {
            get { return _cells.Count; }
        }

        protected override void UpdateItem(Transform item, int index)
        {
            for (int i = 0; i < 4; i++)
            {
                item.GetChild(i).GetComponent<Text>().text = _cells[index][i];
            }
        }

        public void SetData(int dropDownValue)
        {
            _cells.Clear();

            if (dropDownValue == 0 || dropDownValue == 1)
            {
                _cells.AddRange(StaticData.Eats.Select(s=>
                    new MainVerticalScrollViewCell(
                        "Eat", 
                        s.DrinkType, 
                        s.Ml.ToString(), 
                        s.Time.ToString())));
            }
            if (dropDownValue == 0 || dropDownValue == 2)
            {
                _cells.AddRange(StaticData.Diapers.Select(s =>
                    new MainVerticalScrollViewCell(
                        "Eat",
                        s.ExcreteType,
                        s.Mg.ToString(),
                        s.Time.ToString())));
            }
        }
    }
}

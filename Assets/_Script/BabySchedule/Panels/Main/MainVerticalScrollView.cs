using System.Collections.Generic;
using System.Linq;
using TcpConnect;
using TcpConnect.ServerInterface;
using UnityEngine;
using UnityEngine.UI;

namespace BabySchedule.Panels.Main
{
    public class MainVerticalScrollView : BaseVerticalScrollView
    {
        private const string All = "All";
        private int _dataKind;
        private string _filter = All;

        private class MainVerticalScrollViewCell
        {
            private readonly List<string> _lines = new List<string>(4);

            public long Time { get; private set; }

            public MainVerticalScrollViewCell(Eat eat)
            {
                _lines.Add("吃");
                _lines.Add(eat.DrinkType);
                _lines.Add(eat.Ml + "(ml)");
                Time = eat.Time;
                _lines.Add(CommonMethod.TickToTimeStr(Time));
            }

            public MainVerticalScrollViewCell(Diaper diaper)
            {
                _lines.Add("排泄");
                _lines.Add(diaper.ExcreteType);
                _lines.Add(diaper.Mg + "(mg)");
                Time = diaper.Time;
                _lines.Add(CommonMethod.TickToTimeStr(Time));
            }

            public string this[int index]
            {
                get { return _lines[index]; }
            }
        }

        private readonly List<MainVerticalScrollViewCell> _cells = new List<MainVerticalScrollViewCell>();

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

        protected override void Awake()
        {
            SetData(_dataKind, _filter);
            base.Awake();
        }

        protected override void DataChanged()
        {
            SetData(_dataKind, _filter);
            ReloadData(true);
        }

        public void SetData(int dataKind, string filter)
        {
            _dataKind = dataKind;
            _filter = filter;
            _cells.Clear();

            if (dataKind == 0 || dataKind == 1)
            {
                _cells.AddRange(StaticData.Eats
                    .Where(s => filter == All || filter == s.DrinkType)
                    .Select(s => new MainVerticalScrollViewCell(s)));
            }
            if (dataKind == 0 || dataKind == 2)
            {
                _cells.AddRange(StaticData.Diapers
                    .Where(s => filter == All || filter == s.ExcreteType)
                    .Select(s => new MainVerticalScrollViewCell(s)));
            }

            _cells.Sort((s1, s2) => (int)(s2.Time / 1000 - s1.Time / 1000));
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using BabySchedule;
using UnityEngine;
using UnityEngine.UI;

public enum MsgBoxResult
{
    Cancel,
    Ok,
    Yes,
    No
}

public enum MsgBoxStyle
{
    Ok,
    OkCancel,
    YesNo,
    YesNoCancel,
}

public class MsgBox : MonoBehaviourBase
{
    private const string PerfabPath = "MsgBox/MsgBox";
    private const string BtnPerfabPath = "MsgBox/MsgBtn";

    private const string BtnRootPath = "Panel/BtnRoot";
    private const string TitlePath = "Panel/Title";
    private const string ContentPath = "Panel/Content";

    private GameObject _perfab;
    private GameObject _btnPerfab;

    public static MsgBox Instance { get; private set; }

    protected override void Awake()
    {
        Instance = this;
        _perfab = Resources.Load<GameObject>(PerfabPath);
        _btnPerfab = Resources.Load<GameObject>(BtnPerfabPath);
    }

    public BoxAsyn Show(string msg, MsgBoxStyle style = MsgBoxStyle.Ok)
    {
        return Show("提示", msg, style);
    }

    public BoxAsyn Show(string title, string content, MsgBoxStyle style = MsgBoxStyle.Ok)
    {
        var boxObj = Instantiate(_perfab);
        boxObj.transform.SetParent(transform);
        boxObj.transform.localPosition = Vector3.zero;
        boxObj.transform.localScale = Vector3.one;
        boxObj.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        boxObj.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        boxObj.transform.SetAsLastSibling();

        boxObj.transform.Find(TitlePath).GetComponent<Text>().text = title;
        boxObj.transform.Find(ContentPath).GetComponent<Text>().text = content;

        var btnRoot = boxObj.transform.Find(BtnRootPath);

        List<GameObject> btns = new List<GameObject>();
        switch (style)
        {
            case MsgBoxStyle.Ok:
                var okBtn1 = Instantiate(_btnPerfab);
                okBtn1.transform.SetParent(btnRoot);
                okBtn1.transform.localScale = Vector3.one;
                okBtn1.name = MsgBoxResult.Ok.ToString();

                btns.Add(okBtn1);
                break;
            case MsgBoxStyle.OkCancel:
                var okBtn2 = Instantiate(_btnPerfab);
                okBtn2.transform.SetParent(btnRoot);
                okBtn2.transform.localScale = Vector3.one;
                okBtn2.name = MsgBoxResult.Ok.ToString();

                var cancelBtn2 = Instantiate(_btnPerfab);
                cancelBtn2.transform.SetParent(btnRoot);
                cancelBtn2.transform.localScale = Vector3.one;
                cancelBtn2.name = MsgBoxResult.Cancel.ToString();

                btns.Add(okBtn2);
                btns.Add(cancelBtn2);
                break;
            case MsgBoxStyle.YesNo:
                var yesBtn3 = Instantiate(_btnPerfab);
                yesBtn3.transform.SetParent(btnRoot);
                yesBtn3.transform.localScale = Vector3.one;
                yesBtn3.name = MsgBoxResult.Yes.ToString();

                var noBtn3 = Instantiate(_btnPerfab);
                noBtn3.transform.SetParent(btnRoot);
                noBtn3.transform.localScale = Vector3.one;
                noBtn3.name = MsgBoxResult.No.ToString();

                btns.Add(yesBtn3);
                btns.Add(noBtn3);
                break;
            case MsgBoxStyle.YesNoCancel:
                var yesBtn4 = Instantiate(_btnPerfab);
                yesBtn4.transform.SetParent(btnRoot);
                yesBtn4.transform.localScale = Vector3.one;
                yesBtn4.name = MsgBoxResult.Yes.ToString();

                var noBtn4 = Instantiate(_btnPerfab);
                noBtn4.transform.SetParent(btnRoot);
                noBtn4.transform.localScale = Vector3.one;
                noBtn4.name = MsgBoxResult.No.ToString();

                var cancelBtn4 = Instantiate(_btnPerfab);
                cancelBtn4.transform.SetParent(btnRoot);
                cancelBtn4.transform.localScale = Vector3.one;
                cancelBtn4.name = MsgBoxResult.Cancel.ToString();

                btns.Add(yesBtn4);
                btns.Add(noBtn4);
                btns.Add(cancelBtn4);
                break;
        }
        switch (btns.Count)
        {
            case 1:
                btns[0].transform.localPosition = Vector3.zero;
                break;
            case 2:
                btns[0].transform.localPosition = new Vector3(-150, 0);
                btns[1].transform.localPosition = new Vector3(150, 0);
                break;
            case 3:
                btns[0].transform.localPosition = new Vector3(-250, 0);
                btns[1].transform.localPosition = new Vector3(0, 0);
                btns[2].transform.localPosition = new Vector3(250, 0);
                break;
        }

        var msgBox = new BoxAsyn(boxObj, btns);
        return msgBox;
    }

    public class BoxAsyn : IEnumerator
    {
        private const string CloseBtnPath = "Panel/CloseBtn";

        private readonly GameObject _boxObj;
        public MsgBoxResult Result { get; private set; }

        public BoxAsyn(GameObject boxObj, List<GameObject> btns)
        {
            _boxObj = boxObj;
            _boxObj.transform.Find(CloseBtnPath).GetComponent<Button>().onClick.AddListener(() =>
            {
                Result = MsgBoxResult.Cancel;
                _boxObj.SetActive(false);
            });

            for (int i = 0; i < btns.Count; i++)
            {
                var str = btns[i].name;
                btns[i].GetComponentInChildren<Text>().text = str;
                btns[i].GetComponent<Button>().onClick.AddListener(() =>
                {
                    Result = (MsgBoxResult) Enum.Parse(typeof(MsgBoxResult), str);
                    _boxObj.SetActive(false);
                });
            }
        }

        public bool MoveNext()
        {
            return _boxObj.activeSelf;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public object Current
        {
            get { return null; }
        }
    }
}
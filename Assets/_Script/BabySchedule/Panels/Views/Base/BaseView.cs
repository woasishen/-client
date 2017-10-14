using System;
using UnityEngine.UI;

namespace BabySchedule.Panels.Views.Base
{
    public abstract class BaseView : MonoBehaviourBase
    {
        public Action OnExit;

        protected override void Awake()
        {
            base.Awake();
            var exitBtn = transform.Find("ExitButton");
            if (exitBtn)
            {
                exitBtn.GetComponent<Button>().onClick.AddListener(Exit);
            }
            var cancelBtn = transform.Find("CancelButton");
            if (cancelBtn)
            {
                cancelBtn.GetComponent<Button>().onClick.AddListener(Exit);
            }
        }
        protected void Exit()
        {
            Destroy(gameObject);
            OnExit.Invoke();
        }
    }
}

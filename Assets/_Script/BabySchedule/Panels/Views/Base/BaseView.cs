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

            transform.Find("ExitButton")?.GetComponent<Button>().onClick.AddListener(Exit);
            transform.Find("CancelButton").GetComponent<Button>().onClick.AddListener(Exit);
        }
        private void Exit()
        {
            Destroy(gameObject);
            OnExit.Invoke();
        }
    }
}

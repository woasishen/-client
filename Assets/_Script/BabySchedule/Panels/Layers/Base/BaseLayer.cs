using System;
using UnityEngine.UI;

namespace BabySchedule.Panels.Layers.Base
{
    public abstract class BaseLayer : MonoBehaviourBase
    {
        public Action OnExit;

        protected override void Awake()
        {
            base.Awake();

            var cancelBtn = transform.Find("CancelButton");
            if (cancelBtn)
            {
                cancelBtn.GetComponent<Button>().onClick.AddListener(Exit);
            }
        }
        private void Exit()
        {
            Destroy(gameObject);
            OnExit.Invoke();
        }
    }
}

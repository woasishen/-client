namespace BabySchedule.Panels
{
    public class CanvasInstance : MonoBehaviourBase
    {
        public static CanvasInstance Instance;

        protected override void Awake()
        {
            base.Awake();
            Instance = this;
            gameObject.AddComponent<UILayers>();
            gameObject.AddComponent<UIViews>();
        }
    }
}

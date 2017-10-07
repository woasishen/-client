using UnityEngine;

namespace BabySchedule.Panels
{
    public static class UIHelper
    {
        public static void UIFill(this RectTransform rect)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMax = Vector2.zero;
            rect.offsetMin = Vector2.zero;
        }

        public static void UIFill(this GameObject obj)
        {
            obj.GetComponent<RectTransform>().UIFill();
        }
    }
}

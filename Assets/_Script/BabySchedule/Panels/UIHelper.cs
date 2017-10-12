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

        public static void UIFillWidth(this RectTransform rect)
        {
            rect.offsetMax = new Vector2(0, rect.offsetMax.y);
            rect.offsetMin = new Vector2(0, rect.offsetMin.y);
        }

        public static void UIFillWidth(this GameObject obj)
        {
            obj.GetComponent<RectTransform>().UIFillWidth();
        }
    }
}

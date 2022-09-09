using UnityEngine.UIElements;

namespace DeviceControlSystem
{
    public static class QoLExtentions
    {
        //Requires .hidden uss selector
        public static void Hide(this VisualElement e)
        {
            e.AddToClassList("hidden");
            e.MarkDirtyRepaint();
        }
        //Requires .hidden uss selector
        public static void Show(this VisualElement e)
        {
            e.RemoveFromClassList("hidden");
            e.MarkDirtyRepaint();
        }
    }
}

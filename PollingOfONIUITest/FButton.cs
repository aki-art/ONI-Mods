using UnityEngine.EventSystems;

namespace PollingOfONIUITest
{
    public class FButton : KMonoBehaviour, IEventSystemHandler, IPointerDownHandler, IPointerEnterHandler
    {
        public event System.Action OnClick;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (KInputManager.isFocused)
            {
                KInputManager.SetUserActive();
                PlaySound("event:/UI/Mouse/HUD_Click_Open");
                OnClick?.Invoke();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (KInputManager.isFocused)
            {
                KInputManager.SetUserActive();
                PlaySound("event:/UI/Mouse/HUD_Mouseover");
            }
        }
    }
}

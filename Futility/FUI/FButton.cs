using UnityEngine.EventSystems;

namespace FUtility.FUI
{
    public class FButton : KMonoBehaviour, IEventSystemHandler, IPointerDownHandler, IPointerEnterHandler
    {
        public event System.Action OnClick;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (KInputManager.isFocused)
            {
                KInputManager.SetUserActive();
                PlaySound(UISoundHelper.ClickOpen);
                OnClick?.Invoke();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (KInputManager.isFocused)
            {
                KInputManager.SetUserActive();
                PlaySound(UISoundHelper.MouseOver);
            }
        }
    }
}

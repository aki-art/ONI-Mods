using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FUtility.FUI
{
    public class FButton : KMonoBehaviour, IEventSystemHandler, IPointerDownHandler, IPointerEnterHandler
    {
        public event System.Action OnClick;

        public bool isInteractable = true;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (isInteractable && KInputManager.isFocused)
            {
                KInputManager.SetUserActive();
                PlaySound(UISoundHelper.ClickOpen);
                OnClick?.Invoke();
            }
        }

        public void SetInteractable(bool value)
        {
            isInteractable = value;

            // TODO: there is no need to use Unity Button here, fix this some day
            if(GetComponent<Button>() is Button button)
            {
                button.interactable = value;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (isInteractable && KInputManager.isFocused)
            {
                KInputManager.SetUserActive();
                PlaySound(UISoundHelper.MouseOver);
            }
        }
    }
}

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FUtility.FUI
{
    public class FToggle2 : KMonoBehaviour, IEventSystemHandler, IPointerDownHandler, IPointerEnterHandler
    {
        [SerializeField]
        public Image mark;

        public event System.Action OnClick;
        public event System.Action OnChange;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            mark = gameObject.GetComponentInChildren<Image>();
        }

        private bool on;

        public bool On
        {
            get => on;
            set
            {
                on = value;
                mark.enabled = value;
                OnChange?.Invoke();
            }
        }

        public void Toggle() => On = !On;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (KInputManager.isFocused)
            {
                KInputManager.SetUserActive();
                PlaySound(UISoundHelper.Click);
                Toggle();
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

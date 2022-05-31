using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FUtility.FUI
{
    public class FButton : KMonoBehaviour, IEventSystemHandler, IPointerDownHandler, IPointerEnterHandler
    {
        public event System.Action OnClick;

        private bool interactable;
        private Material material;

        [MyCmpReq]
        private Image image;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            material = image.material;
            interactable = true;
        }

        public void SetInteractable(bool interactable)
        {
            if(interactable == this.interactable)
            {
                return;
            }

            this.interactable = interactable;
            image.material = interactable ? material : global::Assets.instance.UIPrefabAssets.TableScreenWidgets.DesaturatedUIMaterial;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if(!interactable)
            {
                return;
            }

            if (KInputManager.isFocused)
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
            if (!interactable)
            {
                return;
            }

            if (KInputManager.isFocused)
            {
                KInputManager.SetUserActive();
                PlaySound(UISoundHelper.MouseOver);
            }
        }
    }
}

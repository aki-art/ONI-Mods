using UnityEngine.EventSystems;

namespace Twitchery.Content.Scripts
{
    public class PipHoverable : KMonoBehaviour, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        [MyCmpGet]
        private DesktopPip pip;

        [MyCmpGet]
        private KBatchedAnimController kbac;

        public void OnPointerDown(PointerEventData eventData)
        {
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
        }

        public void OnPointerExit(PointerEventData eventData)
        {
        }
    }
}

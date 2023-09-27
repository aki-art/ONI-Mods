
using UnityEngine;
using UnityEngine.EventSystems;

namespace Moonlet.Scripts.UI
{
	public class DraggableDialog : MonoBehaviour, IDragHandler, IBeginDragHandler
	{
		public RectTransform targetTransform;
		public Vector2 dragOffset;

		public void OnBeginDrag(PointerEventData eventData)
		{
			dragOffset = eventData.position - (Vector2)targetTransform.position;
		}

		public void OnDrag(PointerEventData eventData)
		{
			if (targetTransform == null) return;
			targetTransform.position = eventData.position - dragOffset;
		}
	}
}

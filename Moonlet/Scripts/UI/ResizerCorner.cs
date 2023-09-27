using UnityEngine;
using UnityEngine.EventSystems;

namespace Moonlet.Scripts.UI
{
	public class ResizerCorner : KMonoBehaviour, IDragHandler, IBeginDragHandler
	{
		[SerializeField] public RectTransform targetTransform;

		public Vector2 minSize = new (256, 128);
		public Vector2 maxSize = new (2048, 1024);

		private Vector2 startOffset;
		private Vector2 startSize;
		private Vector2 offset;

		public void OnBeginDrag(PointerEventData eventData)
		{
			startOffset = eventData.position;
			if (targetTransform != null)
				startSize = targetTransform.sizeDelta;
		}

		public void OnDrag(PointerEventData eventData)
		{
			if (targetTransform != null)
			{
				offset = startOffset - eventData.position;

				var x = startSize.x - offset.x;
				x = Mathf.Clamp(x, minSize.x, maxSize.x);

				var y = startSize.y + offset.y;
				y = Mathf.Clamp(y, minSize.y, maxSize.y);

				targetTransform.sizeDelta = new Vector2(x, y);
			}
		}
	}
}

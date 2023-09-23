using UnityEngine;
using UnityEngine.EventSystems;

namespace DecorPackA.Buildings.MoodLamp
{
	public class CategoryHeader : KMonoBehaviour, IPointerClickHandler
	{
		public LocText label;
		public bool open;
		public Transform body;

		public void Toggle()
		{
			open = !open;
			body.gameObject.SetActive(open);
		}

		public void OnPointerClick(PointerEventData eventData) => Toggle();
	}
}

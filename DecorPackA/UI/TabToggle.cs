using UnityEngine;
using UnityEngine.UI;

namespace DecorPackA.UI
{
	public class TabToggle : Toggle
	{
		[SerializeField] public GameObject target;

		private Image icon;
		private static Color offColor = new(0.7f, 0.7f, 0.7f);
		private static Color onColor = Color.black;

		public void Setup(GameObject target)
		{
			icon = GetComponent<Image>();
			this.target = target;
		}

		public void OnToggle(bool on)
		{
			icon.color = on ? onColor : offColor;
			target.SetActive(on);
		}
	}
}

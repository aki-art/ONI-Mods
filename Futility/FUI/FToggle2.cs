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

		private bool on;

		public bool On
		{
			get => on;
			set
			{
				on = value;
				if (mark != null)
					mark.enabled = value;

				OnChange?.Invoke();
			}
		}

		public void SetOnWithoutTrigger(bool value)
		{
			on = value;
			if (mark != null)
				mark.enabled = value;
		}

		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();

			// mark = gameObject.GetComponentInChildren<Image>();
		}

		public void SetCheckmark(string path)
		{
			mark = transform.Find(path).GetComponent<Image>();
		}

		protected override void OnSpawn()
		{
			base.OnSpawn();
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

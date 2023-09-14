using DecorPackA.Buildings.MoodLamp;
using DecorPackA.Scripts;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace DecorPackA.UI
{
	public class DecorPackA_TintableSideScreen : SideScreenContent
	{
		[SerializeField] private HSVColorSelector hsvColorSelector;
		[SerializeField] private SwatchSelector swatchSelector;
		[SerializeField] private TabToggle showHSVToggle;
		[SerializeField] private TabToggle showSwatchToggle;

		private TintableLamp target;

		public override int GetSideScreenSortOrder() => 0;

		public override bool IsValidForTarget(GameObject target) => target.TryGetComponent(out TintableLamp tintable) && tintable.IsActive;

		public override void SetTarget(GameObject target)
		{
			base.SetTarget(target);

			if (target.TryGetComponent(out TintableLamp newTarget))
			{
				this.target = newTarget;

				swatchSelector.SetSwatch(newTarget.swatchIdx, false);
				hsvColorSelector.SetColor(newTarget.Color, false);
			}
		}

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();

			FUtility.FUI.Helper.ListChildren(transform);

			titleKey = "Backwalls.STRINGS.UI.WALLSIDESCREEN.TITLE";

			hsvColorSelector = transform.Find("Contents/ColorSelector").gameObject.AddOrGet<HSVColorSelector>();
			swatchSelector = transform.Find("Contents/ColorGrid").gameObject.AddOrGet<SwatchSelector>();

			showHSVToggle = transform.Find("Contents/ColorTabs/Sliders").gameObject.AddOrGet<TabToggle>();
			showSwatchToggle = transform.Find("Contents/ColorTabs/Swatches").gameObject.AddOrGet<TabToggle>();
		}

		public override void OnSpawn()
		{
			base.OnSpawn();

			gameObject.SetActive(true);
			RefreshUI();
		}

		private void RefreshUI()
		{
			swatchSelector.Setup();

			showHSVToggle.Setup(hsvColorSelector.gameObject);
			showSwatchToggle.Setup(swatchSelector.gameObject);

			showHSVToggle.onValueChanged.AddListener(on =>
			{
				Scripts.DecorPackA_Mod.Instance.showHSV = on;

				showHSVToggle.OnToggle(on);
			});

			showHSVToggle.isOn = Scripts.DecorPackA_Mod.Instance.showHSV;

			showSwatchToggle.onValueChanged.AddListener(on =>
			{
				Scripts.DecorPackA_Mod.Instance.showSwatches = on;

				showSwatchToggle.OnToggle(on);
			});

			showSwatchToggle.isOn = Scripts.DecorPackA_Mod.Instance.showSwatches;

			hsvColorSelector.OnChange += OnHSVColorChange;
			swatchSelector.OnChange += OnSwatchChange;
		}

		private void OnHSVColorChange(Color color)
		{
			Log.Debuglog("on hsv change " + color.ToString());

			if (target == null)
				return;

			target.SetColor(color);
			target.swatchIdx = SwatchSelector.Invalid;

			if (swatchSelector.isActiveAndEnabled)
				swatchSelector.SetSwatch(SwatchSelector.Invalid, false);
		}

		private void OnSwatchChange(Color color, int index)
		{
			Log.Debuglog("on swatch change " + index);

			if (target == null || (target.TryGetComponent(out KSelectable kSelectable) && !kSelectable.IsSelected))
				return;

			target.SetColor(index);

			if (hsvColorSelector.isActiveAndEnabled)
				hsvColorSelector.SetColor(color, false);
		}
	}
}

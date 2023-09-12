using DecorPackA.Buildings.MoodLamp;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DecorPackA.UI
{
	public class DecorPackA_ParticleSelectorSideScreen : SideScreenContent
	{
		private Dictionary<string, Toggle> toggles;
		private Toggle togglePrefab;

		private ScatterLightLamp target;

		public override int GetSideScreenSortOrder() => 15;

		public override bool IsValidForTarget(GameObject target) => target.TryGetComponent(out ScatterLightLamp scatterLight) && scatterLight.enabled;

		public override void SetTarget(GameObject target)
		{
			base.SetTarget(target);

			if (target.TryGetComponent(out ScatterLightLamp newTarget))
			{
				this.target = newTarget;
				if (toggles != null && toggles.TryGetValue(newTarget.particleType, out var toggle))
					toggle.SetIsOnWithoutNotify(true);
			}
		}

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();

			FUtility.FUI.Helper.ListChildren(transform);

			titleKey = "Backwalls.STRINGS.UI.WALLSIDESCREEN.TITLE";

			togglePrefab = transform.Find("Contents/OptionsGrid/ButtonPrefab").gameObject.GetComponent<Toggle>();
		}

		public override void OnSpawn()
		{
			base.OnSpawn();

			gameObject.SetActive(true);
			RefreshUI();
		}

		private void RefreshUI()
		{
			if (toggles != null)
				return;

			toggles = new();

			foreach(var option in ModAssets.Textures.particles)
			{
				var toggle = Instantiate(togglePrefab, togglePrefab.transform.parent);

				toggle.onValueChanged.AddListener(val => OnToggled(option.Key, val));
				toggles[option.Key] = toggle;

				var tex = option.Value;

				toggle.gameObject.SetActive(true);

				toggle.transform.Find("Image").GetComponent<Image>().sprite = 
					Sprite.Create(
						tex, 
						new Rect(0, 0, tex.width / 3f, tex.height), 
						Vector3.zero);
			}
		}

		private void OnToggled(string key, bool isOn)
		{
			if (target == null)
				return;

			if (isOn)
				target.SetParticles(key);
		}
	}
}

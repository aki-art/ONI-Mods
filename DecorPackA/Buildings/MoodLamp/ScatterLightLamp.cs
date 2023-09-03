using KSerialization;
using System.Collections;
using UnityEngine;

namespace DecorPackA.Buildings.MoodLamp
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class ScatterLightLamp : KMonoBehaviour
	{
		public float BURNOUT = 5f;

		[MyCmpReq] private Operational operational;
		[MyCmpReq] private MoodLamp moodLamp;
		[MyCmpReq] private KSelectable kSelectable;

		[Serialize] private bool visibleParticles;

		private GameObject lightOverlay;

		private bool ShowParticles => visibleParticles && operational.IsOperational && ModAssets.Prefabs.scatterLampPrefabs.ContainsKey(moodLamp.currentVariantID);

		public override void OnSpawn()
		{
			base.OnSpawn();
			Subscribe((int)GameHashes.OperationalChanged, OnOperationalChanged);
			Subscribe(ModEvents.OnMoodlampChanged, OnMoodlampChanged);
			Subscribe((int)GameHashes.RefreshUserMenu, OnRefreshUserMenu);
			Subscribe((int)GameHashes.CopySettings, OnCopySettings);

			OnMoodlampChanged(moodLamp.currentVariantID);
		}

		private void OnCopySettings(object obj)
		{
			if (((GameObject)obj).TryGetComponent(out ScatterLightLamp other))
			{
				visibleParticles = other.visibleParticles;
				moodLamp.RefreshAnimation();
			}
		}

		private void OnRefreshUserMenu(object obj)
		{
			if (enabled)
			{
				var text = visibleParticles
					? STRINGS.UI.USERMENUACTIONS.SCATTER_LIGHT.DISABLED.NAME
					: STRINGS.UI.USERMENUACTIONS.SCATTER_LIGHT.ENABLED.NAME;

				var toolTip = visibleParticles
					? STRINGS.UI.USERMENUACTIONS.SCATTER_LIGHT.DISABLED.TOOLTIP
					: STRINGS.UI.USERMENUACTIONS.SCATTER_LIGHT.ENABLED.TOOLTIP;

				var button = new KIconButtonMenu.ButtonInfo("action_switch_toggle", text, RefreshParticles, tooltipText: toolTip);

				Game.Instance.userMenu.AddButton(gameObject, button);
			}
		}

		private void RefreshParticles()
		{
			visibleParticles = !visibleParticles;

			if (visibleParticles && lightOverlay == null)
				CreateParticles(moodLamp.currentVariantID);

			lightOverlay?.SetActive(ShowParticles);
		}

		private void OnMoodlampChanged(object data)
		{
			Log.Debuglog("moodlamp changed");

			if (data is string id && ModAssets.Prefabs.scatterLampPrefabs.TryGetValue(id, out var particles))
				CreateParticles(id);

			if (kSelectable.IsSelected)
				DetailsScreen.Instance.Refresh(gameObject);

			RefreshParticles();
		}

		private void DestroyParticles()
		{
			if (lightOverlay != null)
				Util.KDestroyGameObject(lightOverlay);

			lightOverlay = null;
		}

		private void CreateParticles(string id)
		{
			DestroyParticles();

			if (ModAssets.Prefabs.scatterLampPrefabs.TryGetValue(id, out var particles))
			{
				lightOverlay = Instantiate(particles);
				lightOverlay.transform.SetParent(transform);
				lightOverlay.transform.position = transform.position with { z = Grid.GetLayerZ(Grid.SceneLayer.FXFront2) };
				lightOverlay.transform.position += Vector3.up;
			}

			lightOverlay?.SetActive(enabled && ShowParticles);
		}

		private void OnOperationalChanged(object obj)
		{
			lightOverlay?.SetActive(enabled && ShowParticles);
		}

		public override void OnCmpEnable()
		{
			base.OnCmpEnable();
			lightOverlay?.SetActive(ShowParticles);
		}

		public override void OnCmpDisable()
		{
			Log.Debuglog("Disabled component");
			base.OnCmpDisable();
			lightOverlay?.SetActive(false);
		}
	}
}

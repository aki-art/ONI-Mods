using KSerialization;
using System.Collections.Generic;
using UnityEngine;

namespace DecorPackA.Buildings.MoodLamp
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class SimpleParticleLamp : KMonoBehaviour
	{
		[MyCmpReq] private Operational operational;
		[MyCmpReq] private MoodLamp moodLamp;

		[Serialize][SerializeField] public bool visibleParticles;

		private GameObject lightOverlay;

		public bool IsActive { get; private set; }

		private bool ShowParticles => enabled && visibleParticles && operational.IsOperational;

		public SimpleParticleLamp()
		{
			visibleParticles = true;
		}

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
			if (((GameObject)obj).TryGetComponent(out SimpleParticleLamp other))
			{
				visibleParticles = other.visibleParticles;
				moodLamp.RefreshAnimation();
			}
		}

		private void OnRefreshUserMenu(object obj)
		{
			if (IsActive)
			{
				var text = visibleParticles
					? STRINGS.UI.USERMENUACTIONS.SCATTER_LIGHT.DISABLED.NAME
					: STRINGS.UI.USERMENUACTIONS.SCATTER_LIGHT.ENABLED.NAME;

				var toolTip = visibleParticles
					? STRINGS.UI.USERMENUACTIONS.SCATTER_LIGHT.DISABLED.TOOLTIP
					: STRINGS.UI.USERMENUACTIONS.SCATTER_LIGHT.ENABLED.TOOLTIP;

				var button = new KIconButtonMenu.ButtonInfo("action_switch_toggle", text, ToggleParticles, tooltipText: toolTip);

				Game.Instance.userMenu.AddButton(gameObject, button);
			}
		}

		private void ToggleParticles()
		{
			visibleParticles = !visibleParticles;

			if (visibleParticles && lightOverlay == null)
				CreateParticles(moodLamp.currentVariantID);

			lightOverlay?.SetActive(ShowParticles);
		}

		private void OnMoodlampChanged(object data)
		{
			DestroyParticles();

			if (LampVariant.TryGetData<Dictionary<HashedString, object>>(data, "Data", out var dict))
			{
				if (dict.TryGetValue("ParticleType", out var particleType))
				{
					CreateParticles((string)particleType);
					RefreshParticles();
					IsActive = true;

					return;
				}
			}

			IsActive = false;
		}

		private void DestroyParticles()
		{
			if (lightOverlay != null)
				Util.KDestroyGameObject(lightOverlay);

			lightOverlay = null;
		}

		private bool CreateParticles(string id)
		{
			if (ModAssets.Prefabs.scatterLampPrefabs.TryGetValue(id, out var particles))
			{
				lightOverlay = Instantiate(particles);
				lightOverlay.transform.SetParent(transform);
				lightOverlay.transform.position = transform.position with { z = Grid.GetLayerZ(Grid.SceneLayer.FXFront2) };
				lightOverlay.transform.position += Vector3.up;

				return true;
			}

			return false;
		}

		private void RefreshParticles() => lightOverlay?.SetActive(ShowParticles);

		private void OnOperationalChanged(object obj) => RefreshParticles();
	}
}

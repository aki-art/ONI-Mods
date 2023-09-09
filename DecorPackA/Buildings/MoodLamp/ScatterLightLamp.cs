﻿using KSerialization;
using UnityEngine;

namespace DecorPackA.Buildings.MoodLamp
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class ScatterLightLamp : KMonoBehaviour
	{
		[MyCmpReq] private Operational operational;
		[MyCmpReq] private MoodLamp moodLamp;
		[MyCmpReq] private TintableLamp tintable;

		[Serialize][SerializeField] public bool visibleParticles;
		[Serialize] public string particleType;

		private GameObject lightOverlay;
		private ParticleSystem particles;
		private ParticleSystemRenderer renderer;

		private Color previousColor;

		private const float HUE_SHIFT = 0.1f;
		private const float DARKENING = 0.2f;

		private static GradientAlphaKey[] alphas = new[]
		{
			new GradientAlphaKey(0, 0),
			new GradientAlphaKey(1, 0.05f),
			new GradientAlphaKey(1, 0.95f),
			new GradientAlphaKey(0, 1f),
		};

		private bool ShowParticles => enabled && visibleParticles && operational.IsOperational;

		public override void OnSpawn()
		{
			base.OnSpawn();
			Subscribe((int)GameHashes.OperationalChanged, OnOperationalChanged);
			Subscribe(ModEvents.OnMoodlampChanged, OnMoodlampChanged);
			Subscribe((int)GameHashes.RefreshUserMenu, OnRefreshUserMenu);
			Subscribe((int)GameHashes.CopySettings, OnCopySettings);
			Subscribe(ModEvents.OnLampTinted, TintParticles);

			OnMoodlampChanged(moodLamp.currentVariantID);
		}

		private void TintParticles(object obj)
		{
			if (!enabled || particles == null)
				return;

			if (obj is Color color && !color.Equals(previousColor))
				SetColor(color);
		}

		private void SetColor(Color color)
		{
			previousColor = color;

			particles.Stop();
			renderer.enabled = false;

			var colorOverLifetime = particles.colorOverLifetime;
			colorOverLifetime.enabled = true;

			var grad = new Gradient();
			grad.SetKeys(new[]
			{
				new GradientColorKey(color, 0f),
				new GradientColorKey(HueShiftWarmer(color), 1f),
			},
			alphas);

			colorOverLifetime.color = grad;

			renderer.enabled = true;
			particles.Play();
		}

		private Color HueShiftWarmer(Color color)
		{
			Color.RGBToHSV(color, out var h, out var s, out var v);

			if (h > 0.25f && h < 0.75f)
				h += HUE_SHIFT;
			else
				h -= HUE_SHIFT;

			h = (h % 1 + 1) % 1; // wrap

			v -= DARKENING;
			v = Mathf.Clamp01(v);

			return Color.HSVToRGB(h, s, v);
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

			if (data is string id)
				CreateParticles(id);

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
			if (ModAssets.Prefabs.scatterLampPrefabs.TryGetValue(id, out var particles))
			{
				lightOverlay = Instantiate(particles);
				lightOverlay.transform.SetParent(transform);
				lightOverlay.transform.position = transform.position with { z = Grid.GetLayerZ(Grid.SceneLayer.FXFront2) };
				lightOverlay.transform.position += Vector3.up;

				this.particles = lightOverlay.GetComponent<ParticleSystem>();
				renderer = lightOverlay.GetComponent<ParticleSystemRenderer>();

				SetColor(tintable.Color);
			}
		}

		private void RefreshParticles()
		{
			lightOverlay?.SetActive(ShowParticles);
		}

		private void OnOperationalChanged(object obj)
		{
			RefreshParticles();
		}

		public override void OnCmpEnable()
		{
			base.OnCmpEnable();
		}

		public override void OnCmpDisable()
		{
			base.OnCmpDisable();
			lightOverlay?.SetActive(false);
		}
	}
}

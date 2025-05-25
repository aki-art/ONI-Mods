using UnityEngine;

namespace Twitchery.Content.Scripts.WorldEvents
{
	public class AETE_SolarStorm : AETE_WorldEvent, ISim33ms, ISim200ms
	{
		[SerializeField] public bool isAggressive;

		private AnimationCurve solarActivity;
		public GameObject solarStormverlay;
		private Material solarStormMaterial;

		private const float SOLAR_DARKEN = 0.55f;
		private const float SOLAR_STORM_DAMAGE_CHANCE_MOD = 0.01f;
		public static readonly Color SOLAR_COLOR = Util.ColorFromHex("BDECA2");
		private static readonly int DARKEN = Shader.PropertyToID("_Darken");

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();

			var tangent = 1f / 3f;

			solarActivity = new AnimationCurve([
				new Keyframe(0, 0, 0, 0, tangent, tangent),
				new Keyframe(0.25f, 1f, -0.02795937f, -0.02795937f,  0.508772f, 0.143034f),
				new Keyframe(1f, 0, 0, 0, tangent, tangent)
			]);

			for (int i = 0; i < solarActivity.length; i++)
				solarActivity.SmoothTangents(i, 0);
		}

		public override void OnSpawn()
		{
			base.OnSpawn();
			if (Stage == WorldEventStage.Active)
			{
				ShowOverlay();

				AkisTwitchEvents.Instance.NotifyAllBionics(ModEvents.SolarStormBegan);
				AkisTwitchEvents.Instance.ToggleOverlay(this.GetMyWorldId(), OverlayRenderer.SOLAR, true, true);
				AETE_WorldEventsManager.Instance.OnEventBegin(this);
			}
		}

		public override void Begin()
		{
			base.Begin();
			ShowOverlay();

			AkisTwitchEvents.Instance.NotifyAllBionics(ModEvents.SolarStormBegan);
			AkisTwitchEvents.Instance.ToggleOverlay(this.GetMyWorldId(), OverlayRenderer.SOLAR, true, false);
		}

		private void ShowOverlay()
		{
			if (solarStormverlay != null)
				return;

			solarStormverlay = Instantiate(ModAssets.Prefabs.solarStormQuad);

			var world = this.GetMyWorld();

			solarStormverlay.transform.localScale = new Vector3(world.Width, world.Height, 1);
			solarStormverlay.transform.position = new Vector3(world.minimumBounds.x + world.Width / 2f, world.minimumBounds.y + world.Height / 2f, Grid.GetLayerZ(Grid.SceneLayer.FXFront2) - 3f);
			solarStormMaterial = solarStormverlay.GetComponent<MeshRenderer>().materials[0];
			solarStormMaterial.SetFloat(DARKEN, 1f);

			solarStormverlay.SetActive(true);
		}

		public override void End()
		{
			base.End();

			AkisTwitchEvents.Instance.NotifyAllBionics(ModEvents.SolarStormEnded);

			if (solarStormverlay != null)
				Destroy(solarStormverlay);

			AkisTwitchEvents.Instance.ToggleOverlay(this.GetMyWorldId(), OverlayRenderer.SOLAR, false, false);

			Util.KDestroyGameObject(gameObject);
		}

		private void DoElectricDamageFx(Vector3 position)
		{
			Game.Instance.SpawnFX(ModAssets.Fx.bigZap, Grid.PosToCell(position), Random.Range(0, 360));
			AudioUtil.PlaySound(ModAssets.Sounds.ELECTRIC_SHOCK, position, ModAssets.GetSFXVolume(), Random.Range(0.9f, 1.1f));
		}

		private float GetSolarActivity() => Mathf.Clamp01(solarActivity.Evaluate(GetProgress01()));

		private void UpdateSolarStormDamage()
		{
			var triggerChance = solarActivity == null ? 0.01f : GetSolarActivity() * SOLAR_STORM_DAMAGE_CHANCE_MOD;

			var batteries = Components.Batteries.GetWorldItems(this.GetMyWorldId());

			if (batteries.Count > 0 && Random.value < triggerChance)
			{
				var battery = batteries.GetRandom();

				if (battery.joulesAvailable > 0)
				{
					DoElectricDamageFx(battery.transform.position);

					var amount = battery.joulesAvailable;

					if (!isAggressive)
						amount = Mathf.Clamp(amount, 0, battery.capacity * 0.1f);

					battery.AddEnergy(-amount);
				}
			}

			if (isAggressive)
			{
				var energyConsumer = Components.EnergyConsumers.GetWorldItems(this.GetMyWorldId());
				if (energyConsumer.Count > 0 && Random.value < triggerChance)
				{
					var item = energyConsumer.GetRandom();

					if (item.IsPowered && item.TryGetComponent(out BuildingHP hp))
					{
						DoElectricDamageFx(item.transform.position);
						hp.DoDamage(Mathf.CeilToInt(hp.MaxHitPoints * (1f / 10f)));
					}
				}
			}

			/// also <see cref="AETE_SolarStormBionicReactionMonitor"/>
		}

		public void Sim33ms(float dt)
		{
			elapsedTime += dt;

			if (Stage == WorldEventStage.Active && solarStormMaterial != null)
			{
				var activity = GetSolarActivity();
				var darken = Mathf.Lerp(1f, SOLAR_DARKEN, activity);
				solarStormMaterial.SetFloat(DARKEN, darken);

				if (elapsedTime > durationInSeconds)
					End();
			}
		}

		public void Sim200ms(float dt)
		{
			if (Stage == WorldEventStage.Active)
				UpdateSolarStormDamage();
		}
	}
}

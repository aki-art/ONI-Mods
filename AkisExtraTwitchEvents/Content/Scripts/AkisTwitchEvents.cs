using ImGuiNET;
using KSerialization;
using ONITwitchLib;
using PeterHan.PLib.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using Twitchery.Content.Events;
using Twitchery.Utils;
using UnityEngine;
using static ProcGen.SubWorld;
using Random = UnityEngine.Random;

namespace Twitchery.Content.Scripts
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class AkisTwitchEvents : KMonoBehaviour, IRender200ms, ISim200ms
	{
		public static AkisTwitchEvents Instance;

		[Serialize] public float lastLongBoiSpawn;
		[Serialize] public float lastRadishSpawn;
		[Serialize] internal bool hasRaddishSpawnedBefore;
		[Serialize] public bool hasUnlockedPizzaRecipe;

		[Serialize] public Dictionary<int, ZoneType> zoneTypeOverrides = [];
		[Serialize] public Dictionary<int, ZoneType> pendingZoneTypeOverrides = [];
		[Serialize] public float solarStormRemaining;
		[Serialize] public bool solarStormAggressive;

		public Dictionary<int, List<ZoneTile>> zoneTiles = [];

		public System.Action onSim200ms;

		private bool zoneTypesDirty;

		public static System.Action OnDrawFn;

		private bool isHotTubActive;
		public GameObject solarStormverlay;
		private Material solarStormMaterial;
		private float solarFadeElapsed;
		private const float SOLAR_FADE_IN = 5f;
		private const float SOLAR_DARKEN = 0.58f;
		public float solarBatteryDamageChance = 0.0015f;
		public static readonly Color SOLAR_COLOR = Util.ColorFromHex("BDECA2");
		public static readonly Color WHITE = Color.white;

		private GameObject overlay;
		private Color targetOverlayColor;
		private Color overlayColor;
		private float overlayElapsed;
		private const float OVERLAY_FADE = 5f;
		private Coroutine currentOverlayFadeCoroutine;
		private Material overlayMaterial;

		private float solarZ = Grid.GetLayerZ(Grid.SceneLayer.FXFront2);
		private int solarRenderQueue = 3501;

		// Moonlet compat
		public delegate void AddBiomeOverrideDelegate(int cell, ZoneType zoneType);
		public AddBiomeOverrideDelegate addBiomeOverrideFn;

		public bool IsSolarStormActive() => solarStormRemaining > 0;

		public bool HotTubActive
		{
			get => isHotTubActive;
			set
			{
				if (value)
					CreateOrEnableFireOverlay();

				if (fireOverlay != null)
					fireOverlay.OnHotTubToggled(value);

				isHotTubActive = value;
			}
		}

		public float originalLiquidTransparency;
		public bool hideLiquids;
		public bool eggActive;
		public AETE_EggPostFx eggFx;

		public static ONITwitchLib.EventInfo polymorphEvent;
		//public static MinionIdentity polymorphTarget;
		//public static string polyTargetName;

		public static ONITwitchLib.EventInfo encouragePipEvent;
		public static RegularPip regularPipTarget;
		public static string regularPipTargetName;

		private FireOverlay fireOverlay;

		public class TargetingEvent<T>
		{
			public EventInfo eventInfo;
			public T target;
			public string minionName;
		}

		public static string pizzaRecipeID;
		public static string radDishRecipeID;
		public static string frozenHoneyRecipeID;

		public static Danger MaxDanger
		{
			get
			{
				if (!TwitchModInfo.TwitchIsPresent)
					return Danger.None;

				var dict = ONITwitchLib.Core.TwitchSettings.GetSettingsDictionary();

				if (dict != null && ONITwitchLib.Core.TwitchSettings.GetSettingsDictionary().TryGetValue("MaxDanger", out var result))
				{
					//return (Danger)System.Convert.ToInt32(result);
					return (Danger)(long)result;
				}

				return Danger.High;
			}
		}

		public static int VotesPerTurn
		{
			get
			{
				if (!TwitchModInfo.TwitchIsPresent)
					return 0;

				var dict = ONITwitchLib.Core.TwitchSettings.GetSettingsDictionary();

				return dict != null && ONITwitchLib.Core.TwitchSettings.GetSettingsDictionary().TryGetValue("VoteCount", out var result)
					? System.Convert.ToInt32(result)
					: 0;
			}
		}

		public void AddZoneTile(ZoneTile zoneTile)
		{
			foreach (var cell in zoneTile.building.PlacementCells)
			{
				if (!zoneTiles.ContainsKey(cell))
					zoneTiles[cell] = [zoneTile];
				else
					zoneTiles[cell].Add(zoneTile);
			}
		}

		public void RemoveZoneTile(ZoneTile zoneTile)
		{
			foreach (var cell in zoneTile.building.PlacementCells)
			{
				zoneTiles[cell].Remove(zoneTile);
				if (zoneTiles[cell].Count == 0)
					zoneTiles.Remove(cell);
			}
		}

		public void Sim200ms(float dt)
		{
			if (solarStormRemaining > 0)
			{
				solarStormRemaining -= dt;
				if (solarStormRemaining <= 0)
					EndSolarStorm(false);
				else
					UpdateSolarStormDamage();
			}

			onSim200ms?.Invoke();
		}

		public void SetOverlayColor(Color color, bool instant)
		{
			targetOverlayColor = color;

			if (currentOverlayFadeCoroutine != null)
				StopCoroutine(currentOverlayFadeCoroutine);

			currentOverlayFadeCoroutine = StartCoroutine(FadeInOverlay(instant));
		}

		public IEnumerator FadeInOverlay(bool instant)
		{
			overlayElapsed = instant ? OVERLAY_FADE : 0;

			if (overlay == null)
			{
				overlay = Instantiate(ModAssets.Prefabs.overlayQuad);
				overlay.transform.localScale = new Vector3(Grid.WidthInMeters, Grid.HeightInMeters, 1);
				overlay.transform.position = new Vector3(Grid.WidthInMeters / 2f, Grid.HeightInMeters / 2f, Grid.GetLayerZ(Grid.SceneLayer.FXFront2) - 3f);

				overlayMaterial = overlay.GetComponent<MeshRenderer>().materials[0];

				overlayColor = WHITE;
				overlayMaterial.color = WHITE;
			}

			overlay.SetActive(true);

			do
			{
				overlayElapsed += Time.deltaTime;
				overlayMaterial.SetColor("_Color", Color.Lerp(overlayColor, targetOverlayColor, overlayElapsed / OVERLAY_FADE));

				yield return SequenceUtil.WaitForSeconds(0.033f);
			}
			while (overlayElapsed < OVERLAY_FADE);

			overlayColor = targetOverlayColor;
			yield return null;
		}

		public IEnumerator FadeOutOverlay()
		{
			if (overlay != null && overlayMaterial != null)
			{
				overlayElapsed = 0;
				targetOverlayColor = WHITE;

				while (overlayElapsed < OVERLAY_FADE)
				{
					overlayElapsed += Time.deltaTime;
					overlayMaterial.color = Color.Lerp(overlayColor, targetOverlayColor, overlayElapsed / OVERLAY_FADE);

					yield return SequenceUtil.WaitForSeconds(0.033f);
				}

				overlay.SetActive(false);
			}

			overlayColor = targetOverlayColor;
			yield return null;
		}

		public IEnumerator FadeInSolarStorm()
		{
			solarFadeElapsed = 0;
			SetOverlayColor(SOLAR_COLOR, false);

			while (solarFadeElapsed < SOLAR_FADE_IN)
			{
				solarFadeElapsed += Time.deltaTime;
				solarStormMaterial.SetFloat("_Darken", Mathf.Lerp(1f, SOLAR_DARKEN, solarFadeElapsed / SOLAR_FADE_IN));

				yield return SequenceUtil.WaitForSeconds(0.033f);
			}

			yield return null;
		}

		public IEnumerator FadeOutSolarStorm()
		{
			solarFadeElapsed = 0;

			while (solarFadeElapsed < SOLAR_FADE_IN)
			{
				solarFadeElapsed += Time.deltaTime;

				if (solarStormMaterial != null)
					solarStormMaterial.SetFloat("_Darken", Mathf.Lerp(SOLAR_DARKEN, 1.0f, solarFadeElapsed / SOLAR_FADE_IN));

				yield return SequenceUtil.WaitForSeconds(0.033f);
			}

			solarStormMaterial = null;
			Destroy(solarStormverlay);
			yield return null;
		}

		public IEnumerator UpdateZoneTypes()
		{
			yield return SequenceUtil.waitForEndOfFrame;

			pendingZoneTypeOverrides = new(zoneTypeOverrides);
			zoneTypesDirty = zoneTypeOverrides.Count > 0 || pendingZoneTypeOverrides.Count > 0;
		}

		private void UpdateSolarStormDamage()
		{
			if (Random.value < 0.01f && Components.Batteries.Count > 0)
			{
				var battery = Components.Batteries.Items.GetRandom();

				if (battery.joulesAvailable > 0)
				{
					DoElectricDamageFx(battery.transform.position);

					var amount = battery.joulesAvailable;
					if (!solarStormAggressive)
						amount = Mathf.Clamp(amount, 0, battery.capacity * 0.1f);

					battery.AddEnergy(-amount);
				}
			}

			if (solarStormAggressive)
			{

				if (Random.value < 0.01f && Components.EnergyConsumers.Count > 0)
				{
					var item = Components.EnergyConsumers.Items.GetRandom();

					if (item.IsPowered && item.TryGetComponent(out BuildingHP hp))
					{
						DoElectricDamageFx(item.transform.position);
						hp.DoDamage(Mathf.CeilToInt(hp.MaxHitPoints * (1f / 10f)));
					}
				}
			}

			/// also <see cref="AETE_SolarStormBionicReactionMonitor"/>
		}

		private void DoElectricDamageFx(Vector3 position)
		{
			Game.Instance.SpawnFX(ModAssets.Fx.bigZap, Grid.PosToCell(position), Random.Range(0, 360));
			AudioUtil.PlaySound(ModAssets.Sounds.ELECTRIC_SHOCK, position, ModAssets.GetSFXVolume(), Random.Range(0.9f, 1.1f));
		}

		public void AddZoneTypeOverride(int cell, ZoneType zoneType)
		{
			if (addBiomeOverrideFn != null)
			{
				addBiomeOverrideFn(cell, zoneType);
				zoneTypeOverrides[cell] = zoneType; // saving for reload if Moonlet is gone;
			}
			else
			{
				pendingZoneTypeOverrides[cell] = zoneType;
				zoneTypesDirty = true;
			}

		}

		public void BeginSolarStorm(float duration, bool instant, bool solarStormAggressive)
		{
			if (IsSolarStormActive())
				EndSolarStorm(true);

			this.solarStormAggressive = solarStormAggressive;

			solarStormverlay = Instantiate(ModAssets.Prefabs.solarStormQuad);
			solarStormverlay.transform.localScale = new Vector3(Grid.WidthInMeters, Grid.HeightInMeters, 1);
			solarStormverlay.transform.position = new Vector3(Grid.WidthInMeters / 2f, Grid.HeightInMeters / 2f, Grid.GetLayerZ(Grid.SceneLayer.FXFront2) - 3f);

			solarStormMaterial = solarStormverlay.GetComponent<MeshRenderer>().materials[0];

			if (instant)
			{
				solarStormMaterial.SetFloat("_Darken", SOLAR_DARKEN);
				SetOverlayColor(SOLAR_COLOR, true);
				solarFadeElapsed = SOLAR_FADE_IN;
			}
			else
			{
				StartCoroutine(FadeInSolarStorm());
			}

			solarStormRemaining = duration;

			solarStormverlay.SetActive(true);
			NotifyAllBionics(ModEvents.SolarStormBegan);
		}

		public void EndSolarStorm(bool instant)
		{
			if (instant)
			{
				solarStormRemaining = 0;
				NotifyAllBionics(ModEvents.SolarStormEnded);
				if (solarStormverlay != null)
					Destroy(solarStormverlay);
				if (overlay != null)
					overlay.SetActive(false);

				return;
			}

			if (solarStormverlay != null)
				StartCoroutine(FadeOutSolarStorm());

			StartCoroutine(FadeOutOverlay());
			solarStormRemaining = 0;
			NotifyAllBionics(ModEvents.SolarStormEnded);
		}

		private void NotifyAllBionics(ModHashes ev)
		{
			if (DlcManager.IsContentSubscribed(DlcManager.DLC3_ID))
			{
				if (Components.MinionIdentitiesByModel.TryGetValue(GameTags.Minions.Models.Bionic, out var bionics))
				{
					foreach (var bionic in bionics.Items)
					{
						bionic.Trigger(ev);
					}
				}
			}
		}

		public void ApplyLiquidTransparency(WaterCubes waterCubes)
		{
			if (originalLiquidTransparency == 0)
				originalLiquidTransparency = waterCubes.material.GetFloat("_BlendScreen");

			waterCubes.material.SetFloat("_BlendScreen", hideLiquids ? 0 : originalLiquidTransparency);
		}

		public AkisTwitchEvents()
		{
			lastLongBoiSpawn = float.NegativeInfinity;
			lastRadishSpawn = 0;
		}

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			Instance = this;

			// untested
			var moonletType = Type.GetType("Moonlet, Moonlet.ModAPI");
			if (moonletType != null)
			{
				var m_AddBiomeOverride = moonletType.GetMethod("AddBiomeOverride", [typeof(int), typeof(ZoneType)]);
				addBiomeOverrideFn = (AddBiomeOverrideDelegate)Delegate.CreateDelegate(typeof(AddBiomeOverrideDelegate), m_AddBiomeOverride);
			}
		}

		public override void OnSpawn()
		{
			base.OnSpawn();
			OnDraw();

			if (IsSolarStormActive())
			{
				BeginSolarStorm(solarStormRemaining, true, solarStormAggressive);
			}

			if (addBiomeOverrideFn == null)
				StartCoroutine(UpdateZoneTypes());
		}

		// run even when paused
		public void Render200ms(float dt)
		{
			if (zoneTypesDirty)
			{
				foreach (var pending in pendingZoneTypeOverrides)
				{
					if (!zoneTiles.ContainsKey(pending.Key))
						SimMessages.ModifyCellWorldZone(pending.Key, pending.Value == ZoneType.Space ? byte.MaxValue : (byte)pending.Value);
				}

				RegenerateBackwallTexture(pendingZoneTypeOverrides);

				foreach (var item in pendingZoneTypeOverrides)
					zoneTypeOverrides[item.Key] = item.Value;

				pendingZoneTypeOverrides.Clear();

				World.Instance.zoneRenderData.OnActiveWorldChanged();

				zoneTypesDirty = false;
			}
		}

		public static void OnWorldLoaded()
		{
			RegularPip.regularPipCache.Clear();
			AGridUtil.OnWorldLoad();
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();
			Instance = null;
		}

		public void OnDraw()
		{
			UpdatePolymorphTarget();
			UpdateEncouragePipTarget();
			TwitchEvents.OnDraw();
		}

		private static void UpdateEncouragePipTarget()
		{
			regularPipTarget = GetUpgradeablePip();
		}

		private static void UpdatePolymorphTarget()
		{
			/*			polymorphTarget = Components.LiveMinionIdentities.GetRandom();

						if (polymorphTarget != null)
							polyTargetName = polymorphEvent.FriendlyName = STRINGS.AETE_EVENTS.POLYMOPRH.TOAST.Replace("{Name}", Util.StripTextFormatting(polymorphTarget.GetProperName()));*/
		}

		public static bool HasUpgradeablePip() => regularPipTarget != null;

		public static RegularPip GetUpgradeablePip()
		{
			if (Mod.regularPips.Count > 0)
			{
				var potentialPips = new List<RegularPip>();

				foreach (var pip in Mod.regularPips.items)
				{
					if (pip.potentialNextSkills != null && pip.potentialNextSkills.Count > 0)
						potentialPips.Add(pip);
				}

				if (potentialPips.Count > 0)
				{
					regularPipTarget = potentialPips.GetRandom();

					if (regularPipTarget != null)
						regularPipTargetName = encouragePipEvent.FriendlyName = STRINGS.AETE_EVENTS.ENCOURAGEREGULARPIP.TOAST.Replace("{Name}", Util.StripTextFormatting(regularPipTarget.GetProperName()));

					return regularPipTarget;
				}
			}

			return null;
		}

		public static void UpdateRegularPipWeight()
		{
			/*			encouragePipEvent?.Group.SetWeight(encouragePipEvent, Mod.regularPips.Count > 0
							? TwitchEvents.Weights.RARE
							: TwitchEvents.Weights.COMMON);*/
		}

		public void CreateOrEnableFireOverlay()
		{
			/*			if (!hotTubActive)
							return;*/
			if (fireOverlay == null)
			{
				var ui = Instantiate(ModAssets.Prefabs.fireOverlay);
				ui.SetParent(GameScreenManager.Instance.ssOverlayCanvas);
				ui.gameObject.SetActive(false);

				fireOverlay = ui.AddComponent<FireOverlay>();
			}

			if (!isHotTubActive)
				return;

			fireOverlay.gameObject.SetActive(true);
			fireOverlay.Play();
		}

		public void HideFireOverlay()
		{
			if (fireOverlay != null && fireOverlay.isActiveAndEnabled)
			{
				fireOverlay.Stop();
				fireOverlay.gameObject.SetActive(false);
			}
		}

		public void RegenerateBackwallTexture() => RegenerateBackwallTexture(zoneTypeOverrides);

		public void RegenerateBackwallTexture(Dictionary<int, ZoneType> overrides)
		{
			if (World.Instance.zoneRenderData == null)
			{
				Debug.Log("Subworld zone render data is not yet initialized.");
				return;
			}

			var zoneRenderData = World.Instance.zoneRenderData;

			var colorData = zoneRenderData.colourTex.GetRawTextureData();
			var indexData = zoneRenderData.indexTex.GetRawTextureData();

			foreach (var tile in overrides)
			{
				var cell = tile.Key;
				var zoneType = (byte)tile.Value;

				var color = World.Instance.zoneRenderData.zoneColours[zoneType];

				var index = (tile.Value == ZoneType.Space)
					? byte.MaxValue
					: (byte)World.Instance.zoneRenderData.zoneTextureArrayIndices[zoneType];

				indexData[cell] = index;

				colorData[cell * 3] = color.r;
				colorData[cell * 3 + 1] = color.g;
				colorData[cell * 3 + 2] = color.b;

				World.Instance.zoneRenderData.worldZoneTypes[cell] = tile.Value;
			}

			zoneRenderData.colourTex.LoadRawTextureData(colorData);
			zoneRenderData.indexTex.LoadRawTextureData(indexData);
			zoneRenderData.colourTex.Apply();
			zoneRenderData.indexTex.Apply();

			zoneRenderData.OnShadersReloaded();
		}

		public void ImGuiDraw()
		{
			if (ImGui.CollapsingHeader("Solar Storm"))
			{
				if (IsSolarStormActive() && ImGui.Button("End Solar Storm"))
				{
					EndSolarStorm(false);
					solarStormRemaining = 0;
				}

				ImGui.DragFloat("Battery Damage Chance##solarBatteryDamageChance", ref solarBatteryDamageChance);
				if (ImGui.DragFloat("Z#solarZ", ref solarZ, 1f, 0, 100))
				{
					if (solarStormverlay != null)
						solarStormverlay.transform.position = solarStormverlay.transform.position with { z = solarZ };
				}
				if (ImGui.DragInt("Render Qeueue#solarQueue", ref solarRenderQueue, 1, 2500, 4000))
				{
					if (solarStormMaterial != null)
						solarStormMaterial.renderQueue = solarRenderQueue;
				}

			}


		}
	}
}

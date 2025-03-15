using ImGuiNET;
using KSerialization;
using ONITwitchLib;
using ONITwitchLib.Core;
using PeterHan.PLib.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Twitchery.Content.Events;
using Twitchery.Content.Scripts.WorldEvents;
using Twitchery.Utils;
using UnityEngine;
using static ProcGen.SubWorld;
using Random = UnityEngine.Random;

namespace Twitchery.Content.Scripts
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class AkisTwitchEvents : KMonoBehaviour, IRender200ms, ISim200ms, ISim33ms
	{
		public static AkisTwitchEvents Instance;

		public Dictionary<int, OverlayRenderer> overlayRendererPerWorld;

		[Serialize] public float lastLongBoiSpawn;
		[Serialize] public float lastRadishSpawn;
		[Serialize] internal bool hasRaddishSpawnedBefore;
		[Serialize] public bool hasUnlockedPizzaRecipe;

		[Serialize] public Dictionary<int, ZoneType> zoneTypeOverrides = [];
		[Serialize] public Dictionary<int, ZoneType> pendingZoneTypeOverrides = [];
		[Serialize] public float solarStormRemaining;
		[Serialize] public float solarStormTotalDuration;
		[Serialize] public bool solarStormAggressive;

		public List<AETE_WorldEvent> onGoingEvents;

		[Serialize] public float lastSuperDupeEventCycle;
		public System.Action onDupeSuperEnded;

		public AETE_CursorDurationMarker durationMarker;

		public static HashSet<Tag> dangerousElementIds = [
			SimHashes.ViscoGel.CreateTag(),
			"ITCE_Inverse_Water_Placeholder",
			"ITCE_CreepyLiquid",
			"ITCE_VoidLiquid",
			"Beached_SulfurousWater"
			];

		/*		public void BeginSandStorm(int worldIdx, float duration, int aggression, bool instant)
				{
					sandStorm ??= new();
					sandStormConfig = sandStorm.Configure(worldIdx, duration, aggression);
					sandStorm.Begin(instant);
				}

				public void EndSandStorm(bool instant)
				{
					sandStorm?.End(instant);
				}

				[OnSerializing]
				private void OnSerializing()
				{
					sandStorm?.OnSerialize();
				}
		

		public bool IsSandStormActive()
		{
			return sandStorm != null && sandStorm.IsRunning();
		}
		*/
		public List<SimHashes> GetGenerallySafeLiquids()
		{
			var minTemp = GameUtil.GetTemperatureConvertedToKelvin(20, GameUtil.TemperatureUnit.Celsius);
			var maxTemp = GameUtil.GetTemperatureConvertedToKelvin(50, GameUtil.TemperatureUnit.Celsius);

			return GetLiquids(minTemp, maxTemp, dangerousElementIds);
		}

		public List<SimHashes> GetLiquids(float minTemp, float maxTemp, HashSet<Tag> ignoredElements = null)
		{
			var potentialElements = new List<SimHashes>();

			foreach (var element in ElementLoader.elements)
			{
				if (element.disabled
				|| !element.IsLiquid
					|| element.highTemp < minTemp
					|| element.lowTemp > maxTemp
					|| element.HasTag(TTags.useless)
					|| (ignoredElements != null && ignoredElements.Contains(element.tag)))
					continue;

				var debris = Assets.GetPrefab(element.tag);
				if (debris == null || debris.HasTag(ExtraTags.OniTwitchSurpriseBoxForceDisabled))
					continue;

				potentialElements.Add(element.id);
			}

			return potentialElements;
		}

		public Dictionary<int, List<ZoneTile>> zoneTiles = [];

		public System.Action onSim200ms;

		private bool zoneTypesDirty;

		private readonly Dictionary<int, float> cameraShakers = new()
		{
			{ 0, 0f }
		};

		private bool shakeCamera;
		PerlinNoise cameraPerlin;

		public static System.Action OnDrawFn;

		private bool isHotTubActive;
		private bool photoSensitiveModeOn = false;

		private AnimationCurve solarActivity;
		public GameObject solarStormverlay;
		private Material solarStormMaterial;
		private const float SOLAR_DARKEN = 0.55f;
		private const float SOLAR_STORM_DAMAGE_CHANCE_MOD = 0.01f;
		public static readonly Color SOLAR_COLOR = Util.ColorFromHex("BDECA2");

		private static readonly int
			DARKEN = Shader.PropertyToID("_Darken"),
			BLEND_SCREEN = Shader.PropertyToID("_BlendScreen");

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

		public void SetCameraShake(int key, float amount)
		{
			if (photoSensitiveModeOn)
				return;

			if (amount == 0 && cameraShakers.ContainsKey(key))
				cameraShakers.Remove(key);
			else
				cameraShakers[key] = amount;

			if (amount == 0 || !shakeCamera)
				shakeCamera = cameraShakers.Values.Max() > 0;
		}

		private void ShakeCamera(PerlinNoise perlin)
		{
			if (Game.Instance.IsPaused)
				return;

			Vector3 currentPos = CameraController.Instance.transform.GetPosition();

			float noiseScale = cameraShakers.Values.Max();
			float frequency = cameraShakeFrequency;
			float time = Time.time * frequency;

			float noiseX = (float)perlin.Noise(time, 0, 0);
			float noiseY = (float)perlin.Noise(0, time, 0);

			var offset = new Vector3(noiseX * noiseScale, noiseY * noiseScale);

			CameraController.Instance.SetPosition(currentPos + offset);
		}

		public float originalLiquidTransparency;
		public bool hideLiquids;
		public bool eggActive;
		public AETE_EggPostFx eggFx;

		public static EventInfo polymorphEvent;
		//public static MinionIdentity polymorphTarget;
		//public static string polyTargetName;

		public static EventInfo encouragePipEvent;
		public static RegularPip regularPipTarget;
		public static string regularPipTargetName;

		private FireOverlay fireOverlay;
		private float cameraShakeFrequency = 3f;

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
					EndSolarStorm(0, false);
				else
					UpdateSolarStormDamage();
			}

			onSim200ms?.Invoke();
		}


		public IEnumerator UpdateZoneTypes()
		{
			yield return SequenceUtil.waitForEndOfFrame;

			pendingZoneTypeOverrides = new(zoneTypeOverrides);
			zoneTypesDirty = zoneTypeOverrides.Count > 0 || pendingZoneTypeOverrides.Count > 0;
		}

		private void UpdateSolarStormDamage()
		{
			var triggerChance = solarActivity == null ? 0.01f : GetSolarActivity() * SOLAR_STORM_DAMAGE_CHANCE_MOD;

			if (Random.value < triggerChance && Components.Batteries.Count > 0)
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
				if (Random.value < triggerChance && Components.EnergyConsumers.Count > 0)
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

		private float GetSolarActivity() => Mathf.Clamp01(solarActivity.Evaluate(1f - (solarStormRemaining / solarStormTotalDuration)));

		public int GetTargetableWorld(bool preferActiveWorld, bool allowNoPopulation)
		{
			if (preferActiveWorld)
			{
				var activeWorld = ClusterManager.Instance.activeWorld;
				if (IsWorldTargetable(activeWorld, allowNoPopulation))
					return activeWorld.id;
			}

			foreach (var world in ClusterManager.Instance.WorldContainers)
			{
				if (world.IsDupeVisited && IsWorldTargetable(world, allowNoPopulation))
					return world.id;
			}

			return -1;
		}

		private bool IsWorldTargetable(WorldContainer worldContainer, bool allowNoPopulation)
		{
			if (worldContainer.IsModuleInterior)
				return false;

			if (!allowNoPopulation && Components.LiveMinionIdentities.GetWorldItems(worldContainer.id, true).Count == 0)
				return false;

			if (worldContainer.Width <= 32 || worldContainer.Height <= 32)
				return false;

			return true;
		}

		private OverlayRenderer GetOverlayForWorld(int worldIdx)
		{
			if (!overlayRendererPerWorld.ContainsKey(worldIdx))
			{
				var go = new GameObject("AETE_OverlayRenderer_" + worldIdx);
				var renderer = go.AddComponent<OverlayRenderer>();

				overlayRendererPerWorld[worldIdx] = renderer;
			}

			return overlayRendererPerWorld[worldIdx];
		}

		public void BeginSolarStorm(int world, float duration, bool instant, bool solarStormAggressive)
		{
			if (IsSolarStormActive())
				EndSolarStorm(0, true);

			this.solarStormAggressive = solarStormAggressive;

			solarStormverlay = Instantiate(ModAssets.Prefabs.solarStormQuad);
			solarStormverlay.transform.localScale = new Vector3(Grid.WidthInMeters, Grid.HeightInMeters, 1);
			solarStormverlay.transform.position = new Vector3(Grid.WidthInMeters / 2f, Grid.HeightInMeters / 2f, Grid.GetLayerZ(Grid.SceneLayer.FXFront2) - 3f);

			solarStormMaterial = solarStormverlay.GetComponent<MeshRenderer>().materials[0];

			if (instant)
			{
				Sim33ms(0);
			}

			//overlayRenderer.SetOverlayColor(SOLAR_COLOR, instant);

			overlayRendererPerWorld[world].ToggleOverlay(OverlayRenderer.SOLAR, true, instant);

			solarStormRemaining = duration;
			solarStormTotalDuration = duration;

			solarStormverlay.SetActive(true);
			NotifyAllBionics(ModEvents.SolarStormBegan);
		}

		public void EndSolarStorm(int worldIndex, bool instant)
		{
			if (instant)
			{
				solarStormRemaining = 0;
				NotifyAllBionics(ModEvents.SolarStormEnded);

				if (solarStormverlay != null)
					Destroy(solarStormverlay);

				GetOverlayForWorld(worldIndex).Disable();

				return;
			}

			if (solarStormverlay != null)
			{
				solarStormMaterial = null;
				Destroy(solarStormverlay);
			}

			overlayRendererPerWorld[0].ToggleOverlay(OverlayRenderer.SOLAR, false, instant);

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
				originalLiquidTransparency = waterCubes.material.GetFloat(BLEND_SCREEN);

			waterCubes.material.SetFloat(BLEND_SCREEN, hideLiquids ? 0 : originalLiquidTransparency);
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

			var go = new GameObject("AETE_Components");
			overlayRendererPerWorld = [];
			overlayRendererPerWorld[0] = go.AddComponent<OverlayRenderer>();

			go.transform.parent = transform;
			go.SetActive(true);

			var tangent = 1f / 3f;
			solarActivity = new AnimationCurve([
				new Keyframe(0, 0, 0, 0, tangent, tangent),
				new Keyframe(0.25f, 1f, -0.02795937f, -0.02795937f,  0.508772f, 0.143034f),
				new Keyframe(1f, 0, 0, 0, tangent, tangent)
			]);

			for (int i = 0; i < solarActivity.length; i++)
				solarActivity.SmoothTangents(i, 0);

			// untested
			var moonletType = Type.GetType("Moonlet, Moonlet.ModAPI");
			if (moonletType != null)
			{
				var m_AddBiomeOverride = moonletType.GetMethod("AddBiomeOverride", [typeof(int), typeof(ZoneType)]);
				addBiomeOverrideFn = (AddBiomeOverrideDelegate)Delegate.CreateDelegate(typeof(AddBiomeOverrideDelegate), m_AddBiomeOverride);
			}

		}

		void Update()
		{
			if (Game.Instance.IsPaused)
				return;

			if (shakeCamera)
			{
				ShakeCamera(cameraPerlin);
			}

			switch (SpeedControlScreen.Instance.speed)
			{
				// slow
				case 0:
					cameraShakeFrequency = 7f;
					break;
				// medium
				case 1:
					cameraShakeFrequency = 5f;
					break;
				// high - ultra
				case 2:
					cameraShakeFrequency = 3f;
					break;
			}
		}

		public void Sim33ms(float dt)
		{
			if (IsSolarStormActive())
			{
				var activity = GetSolarActivity();
				var darken = Mathf.Lerp(1f, SOLAR_DARKEN, activity);
				solarStormMaterial.SetFloat(DARKEN, darken);
			}
		}

		public override void OnSpawn()
		{
			onGoingEvents = [];
			cameraPerlin = new PerlinNoise(Random.Range(1, 999));
			durationMarker = new GameObject("AETE_DurationMarker").AddComponent<AETE_CursorDurationMarker>();
			durationMarker.transform.parent = GameScreenManager.Instance.ssOverlayCanvas.transform;

			base.OnSpawn();
			OnDraw();

			if (IsSolarStormActive())
			{
				BeginSolarStorm(0, solarStormRemaining, true, solarStormAggressive);
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
			Instance.photoSensitiveModeOn = (bool)TwitchSettings.GetSettingsDictionary()["PhotosensitiveMode"];
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

		private float test;
		private float progress;
		private float debugCameraShakePower;

		public void ImGuiDraw()
		{
			if (overlayRendererPerWorld != null)
				overlayRendererPerWorld[0].OnImguiRender();

			ImGui.Text("Events");
			ImGui.Separator();

			foreach (var ev in onGoingEvents)
			{
				ImGui.Text(ev.name);
				ImGui.Text($"Remaining: {ev.durationInSeconds - ev.elapsedTime}");
				ImGui.Separator();
				ev.OnImguiDraw();
				ImGui.Separator();
			}
			if (ImGui.CollapsingHeader("Charring"))
			{
				if (SelectTool.Instance.selected != null && SelectTool.Instance.selected.TryGetComponent(out MinionIdentity minion))
				{
					if (ImGui.Button("Char"))
						CharredEntity.CreateAndChar(SelectTool.Instance.selected.gameObject);
				}
			}

			if (ImGui.CollapsingHeader("Camera Shake"))
			{
				if (ImGui.DragFloat("Shake##camerashake", ref debugCameraShakePower, 0.01f, 0, 1))
				{
					SetCameraShake(0, debugCameraShakePower);
				}
				ImGui.DragFloat("Frequency##cameraFrequency", ref cameraShakeFrequency, 0.1f, 1, 5f);
				ImGui.Text($"shaking: {shakeCamera}");
			}

			if (ImGui.CollapsingHeader("Solar Storm"))
			{
				if (IsSolarStormActive())
				{
					progress = 1f - (solarStormRemaining / solarStormTotalDuration);
					ImGui.SliderFloat("Solar Storm Progress", ref progress, 0, 1);
					ImGui.Text("current progress: " + GetSolarActivity());
					ImGui.Text("darken: " + GetSolarActivity());
					ImGui.DragFloat("Curve Test", ref test, 0.01f, 0, 1);

					if (solarActivity != null)
					{
						ImGui.SameLine();
						ImGui.Text($"curve: " + solarActivity.Evaluate(test));
					}

					if (ImGui.Button("End Solar Storm"))
					{
						EndSolarStorm(0, false);
						solarStormRemaining = 0;
					}

				}
			}
		}

		public void ToggleOverlay(int worldIdx, int overlayId, bool enabled, bool instant)
		{
			if (!overlayRendererPerWorld.ContainsKey(worldIdx))
				overlayRendererPerWorld.Add(worldIdx, new OverlayRenderer());

			overlayRendererPerWorld[worldIdx].ToggleOverlay(overlayId, enabled, instant);
		}

		public bool CanGlobalEventStart()
		{
			return !onGoingEvents.Any(e => e.bigEvent);
		}
	}
}

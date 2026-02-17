using ImGuiNET;
using Klei.AI;
using KSerialization;
using ONITwitchLib;
using ONITwitchLib.Core;
using PeterHan.PLib.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Twitchery.Content.Defs;
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

		public const float SUBNAUTICA_OVERLAY_DURATION = 120;
		private float _subnauticaElapsed;

		public Dictionary<int, OverlayRenderer> overlayRendererPerWorld;

		[Serialize] public float lastLongBoiSpawn;
		[Serialize] public float lastRadishSpawn;
		[Serialize] internal bool hasRaddishSpawnedBefore;
		[Serialize] public bool hasUnlockedPizzaRecipe;
		[Serialize] public float harvestMoonRemaining;
		[Serialize] public bool hasFixedBottomBorder;

		[Serialize] public Dictionary<int, ZoneType> zoneTypeOverrides = [];
		[Serialize] public Dictionary<int, ZoneType> pendingZoneTypeOverrides = [];
		[Serialize] public Dictionary<int, HarvestMoonVisualizer> harvestMoonTracker = [];

		public List<AETE_WorldEvent> onGoingEvents;

		[Serialize] public float lastSuperDupeEventCycle;
		public System.Action onDupeSuperEnded;

		public AETE_CursorDurationMarker durationMarker;
		private GameObject extraCompsGo;

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

		private bool IsCorrectState(Element.State state, Element.State flag)
		{
			return flag.HasFlag(state);
			if (state == Element.State.Solid)
				return flag.HasFlag(Element.State.Solid);
		}

		public void FixBottomBorder()
		{
			if (hasFixedBottomBorder)
				return;

			foreach (var world in ClusterManager.Instance.WorldContainers)
			{
				if (world == null || world.IsModuleInterior)
					continue;

				if (!world.IsDiscovered)
					continue;

				var neutroniumCount = 0;

				var holes = new HashSet<int>();

				for (var x = 0; x < world.WorldSize.X; x++)
				{
					var cell = Grid.XYToCell((int)world.minimumBounds.x + x, (int)world.minimumBounds.y);

					if (!Grid.IsValidCell(cell))
						continue;

					var element = Grid.Element[cell];

					if (Grid.Element[cell].id == SimHashes.Unobtanium)
						neutroniumCount++;

					else if (!element.IsSolid)
						holes.Add(cell);
				}

				if (neutroniumCount > 5 && holes.Count > 0)
				{
					foreach (var hole in holes)
					{
						SimMessages.ReplaceElement(
							hole,
							SimHashes.Katairite,
							AGridUtil.cellEvent,
							200f,
							300f,
							byte.MaxValue,
							0);
					}


					Log.Info($"Patched up world bottom with {holes.Count} cells.");
				}
			}

			hasFixedBottomBorder = true;
		}

		public List<SimHashes> GetDangerElements(float hotterThan, float colderThan, HashSet<Element.State> states, HashSet<Tag> ignoredElements = null)
		{
			var potentialElements = new List<SimHashes>();

			foreach (var element in ElementLoader.elements)
			{
				Log.Debug($"checking: {element.name} {element.state}");

				if (element.disabled
					|| element.HasTag(TTags.useless)
					|| (ignoredElements != null && ignoredElements.Contains(element.tag)))
					continue;

				if (states != null)
				{
					var isCorrectState = false;
					foreach (var state in states)
					{
						if (element.state.HasFlag(state))
						{
							isCorrectState = true;
							break;
						}
					}

					if (!isCorrectState)
						continue;
				}

				var veryHot = element.lowTemp > GameUtil.GetTemperatureConvertedToKelvin(hotterThan, GameUtil.TemperatureUnit.Celsius);
				var veryCold = element.highTemp < GameUtil.GetTemperatureConvertedToKelvin(colderThan, GameUtil.TemperatureUnit.Celsius);

				Log.Debug($"\n\tcold enough? {veryCold} " +
					$"\n\thot enough? {veryHot}");

				if (!veryCold && !veryHot)
					continue;

				var debris = Assets.GetPrefab(element.tag);
				if (debris == null || debris.HasTag(ExtraTags.OniTwitchSurpriseBoxForceDisabled))
					continue;

				potentialElements.Add(element.id);
			}

			return potentialElements;
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
		private PerlinNoise cameraPerlin;

		public static System.Action OnDrawFn;

		private bool isHotTubActive;
		private bool isWaterOverlayActive;
		private bool photoSensitiveModeOn = false;

		private static readonly int BLEND_SCREEN = Shader.PropertyToID("_BlendScreen");

		// Moonlet compat
		public delegate void AddBiomeOverrideDelegate(int cell, ZoneType zoneType);
		public AddBiomeOverrideDelegate addBiomeOverrideFn;

		public bool IsFakeFloodActive => HotTubActive || WaterOverlayActive;

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

		public bool WaterOverlayActive
		{
			get => isWaterOverlayActive;
			set
			{
				isWaterOverlayActive = value;
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

			var currentPos = CameraController.Instance.transform.GetPosition();

			var noiseScale = cameraShakers.Values.Max();
			var frequency = cameraShakeFrequency;
			var time = Time.time * frequency;

			var noiseX = (float)perlin.Noise(time, 0, 0);
			var noiseY = (float)perlin.Noise(0, time, 0);

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
			onSim200ms?.Invoke();
		}


		public IEnumerator UpdateZoneTypes()
		{
			yield return SequenceUtil.waitForEndOfFrame;

			pendingZoneTypeOverrides = new(zoneTypeOverrides);
			zoneTypesDirty = zoneTypeOverrides.Count > 0 || pendingZoneTypeOverrides.Count > 0;
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

		public void NotifyAllBionics(ModHashes ev)
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

			go.AddComponent<AETE_WorldEventsManager>();

			go.transform.parent = transform;
			go.SetActive(true);

			extraCompsGo = go;

			var moonletType = Type.GetType("Moonlet, Moonlet.ModAPI");
			if (moonletType != null)
			{
				var m_AddBiomeOverride = moonletType.GetMethod("AddBiomeOverride", [typeof(int), typeof(ZoneType)]);
				addBiomeOverrideFn = (AddBiomeOverrideDelegate)Delegate.CreateDelegate(typeof(AddBiomeOverrideDelegate), m_AddBiomeOverride);
			}

		}

		private void Update()
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
			if (WaterOverlayActive)
			{
				_subnauticaElapsed += dt;
				if (_subnauticaElapsed > SUBNAUTICA_OVERLAY_DURATION)
				{
					WaterOverlayActive = false;
					_subnauticaElapsed = 0;
				}
			}
		}

		public override void OnSpawn()
		{
			onGoingEvents = [];
			cameraPerlin = new PerlinNoise(Random.Range(1, 999));
			durationMarker = new GameObject("AETE_DurationMarker").AddComponent<AETE_CursorDurationMarker>();
			durationMarker.transform.parent = GameScreenManager.Instance.ssOverlayCanvas.transform;

			Subscribe(ModEvents.HarvestMoonSet, OnHarvestMoonSet);

			base.OnSpawn();
			OnDraw();

			if (addBiomeOverrideFn == null)
				StartCoroutine(UpdateZoneTypes());

			//FixBottomBorder();
		}

		private void OnHarvestMoonSet(object data)
		{
			if (data is int worldIdx)
				EndHarvestMoon(worldIdx);
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
		}

		public void ToggleOverlay(int worldIdx, int overlayId, bool enabled, bool instant)
		{
			if (!overlayRendererPerWorld.ContainsKey(worldIdx))
			{
				var renderer = extraCompsGo.AddComponent<OverlayRenderer>();
				renderer.worldIdx = worldIdx;

				overlayRendererPerWorld.Add(worldIdx, renderer);
			}

			overlayRendererPerWorld[worldIdx].ToggleOverlay(overlayId, enabled, instant);
		}

		public static IEnumerator<int> GetWorldIds()
		{
			if (ClusterManager.Instance.activeWorld != null && IsWorldTargatable(ClusterManager.Instance.activeWorld.id))
				yield return ClusterManager.Instance.activeWorld.id;

			var startWorld = ClusterManager.Instance.GetStartWorld();
			if (startWorld != null && startWorld.id != ClusterManager.Instance.activeWorld?.id)
				yield return startWorld.id;

		}

		private static bool IsWorldTargatable(int worldIdx)
		{
			var world = ClusterManager.Instance.GetWorld(worldIdx);

			if (world == null)
				return false;

			if (world.IsModuleInterior)
				return false;

			if (!world.IsDupeVisited)
				return false;

			if (Components.LiveMinionIdentities.GetWorldItems(worldIdx).Count <= 0 && !DebugHandler.InstantBuildMode)
				return false;

			return true;
		}

		public bool CanGlobalEventStart()
		{
			// not initialized yet
			if (onGoingEvents == null)
				return false;

			return !onGoingEvents.Any(e => e.bigEvent);
		}

		public bool IsHarvestMoonActive() => harvestMoonRemaining > 0f;

		public void BeginHarvestMoon(int worldId)
		{
			harvestMoonTracker ??= [];

			foreach (var plant in Components.Crops.GetWorldItems(worldId))
				AddHarvestMoonBoon(plant);

			var world = ClusterManager.Instance.GetWorld(worldId);

			HarvestMoonVisualizer hmv;

			if (harvestMoonTracker.TryGetValue(worldId, out var vis))
				hmv = vis;
			else
			{
				var go = FUtility.Utils.Spawn(HarvestMoonVisualizerConfig.ID, world.worldOffset + ((Vector2)world.WorldSize / 2.0f));
				hmv = go.GetComponent<HarvestMoonVisualizer>();
			}

			if (hmv == null)
				Log.Warning("Something went wrong initializing harvest moon visualizer :(");
			else
				hmv.Begin(CONSTS.CYCLE_LENGTH * 3f);

			ToastManager.InstantiateToast(STRINGS.AETE_EVENTS.HARVESTMOON.TOAST, string.Format(STRINGS.AETE_EVENTS.HARVESTMOON.DESC, world.GetProperName()));
		}

		public void EndHarvestMoon(int worldId)
		{
			var world = ClusterManager.Instance.GetWorld(worldId);
			if (world == null)
				return;

			ToastManager.InstantiateToast(STRINGS.AETE_EVENTS.HARVESTMOON.TOAST, string.Format(STRINGS.AETE_EVENTS.HARVESTMOON.OVER, world.GetProperName()));
		}

		public void AddHarvestMoonBoon(Crop plant)
		{
			if (plant == null || harvestMoonRemaining <= 0)
				return;

			var effectInstance = plant.GetComponent<Effects>().Add(TEffects.HARVESTMOON, false);
			effectInstance.timeRemaining = harvestMoonRemaining;
		}

	}
}

using ONITwitchLib;
using ONITwitchLib.Utils;
using System.Collections.Generic;
using System.Linq;
using Twitchery.Content.Defs;
using Twitchery.Content.Scripts;
using Twitchery.Utils;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	public class PlaceGeyserEvent : TwitchEventBase
	{
		public const string ID = "PlaceGeyser";

		public static List<string> specialGeysers =
		[
			"GeyserGeneric_" + TGeyserConfigs.MOLTEN_GLASS_VOLCANO,
			"GeyserGeneric_" + TGeyserConfigs.GOOP_GEYSER
		];

		public static HashSet<string> templates =
		[
			"akis_extra_twitch_events/generic_geyser_open"
		];

		public static HashSet<string> disabledGeysers =
		[
			OilWellConfig.ID
		];

		private List<string> geyserPrefabs;

		public PlaceGeyserEvent() : base(ID)
		{
			//if (DlcManager.IsExpansion1Active())
			//	specialGeysers.Add("GeyserGeneric_" + TGeyserConfigs.NUCLEAR_WASTE_GEYSER);
		}

		private void InitGeysers()
		{
			geyserPrefabs ??= Assets.GetPrefabsWithComponent<Geyser>()
					.Where(IsGeyserValid)
					.Select(p => p.PrefabID().ToString())
					.ToList();
		}

		private bool IsGeyserValid(GameObject go)
		{
			if (go == null)
				return false;

			if (!go.TryGetComponent(out Geyser geyser))
				return false;

			var id = geyser.PrefabID();
			if (disabledGeysers.Contains(id.name))
				return false;

			return true;
		}

		public override Danger GetDanger() => Danger.High;

		public override int GetWeight() => Consts.EventWeight.Rare;

		public override void Run()
		{
			InitGeysers();

			var go = new GameObject("geyser spawner");

			var cursor = go.AddComponent<WarningCursor>();

			cursor.OnTimerDoneFn += SpawnGeyser;
			cursor.startDelaySeconds = 1.5f;
			cursor.endDelaySeconds = 0.1f;
			cursor.timer = 12f;
			cursor.disallowRocketInteriors = true;
			cursor.disallowProtectedCells = true;
			cursor.overTimer = 120f;
			cursor.animationFile = "aete_warning_kanim";

			go.SetActive(true);

			cursor.StartTimer();

			ToastManager.InstantiateToast(
				STRINGS.AETE_EVENTS.PLACEGEYSER.TOAST,
				STRINGS.AETE_EVENTS.PLACEGEYSER.DESC);
		}

		private void SpawnGeyser(Transform _)
		{
			var position = PosUtil.ClampedMouseWorldPos();
			var template = TemplateCache.GetTemplate(templates.GetRandom());

			string prefabId = null;

			if (Random.value < 0.5f)
			{
				specialGeysers.Shuffle();
				foreach (var specialGeyser in specialGeysers)
				{
					if (!DoesGeyserExist(specialGeyser))
						prefabId = specialGeyser;
				}
			}

			if (prefabId.IsNullOrWhiteSpace())
				prefabId = geyserPrefabs.GetRandom();

			template.otherEntities[0].id = prefabId;

			if (AkisTwitchEvents.MaxDanger < Danger.Deadly)
			{
				foreach (var cell in template.cells)
				{
					if (cell.element == SimHashes.Unobtanium)
						cell.element = SimHashes.Katairite;
				}
			}

			var offsetPos = position + Vector3.left; // the template is off center
													 //TemplateLoader.Stamp(template, offsetPos, () => OnTemplatePlaced(offsetPos, template, prefabId));
			AGridUtil.PlaceStampSavePickupables(template, offsetPos, new Vector2(0f, 1f), () => OnTemplatePlaced(offsetPos, template, prefabId));
			AudioUtil.PlaySound(ModAssets.Sounds.ROCK, ModAssets.GetSFXVolume() * 0.3f);
			Game.Instance.SpawnFX(SpawnFXHashes.MeteorImpactDust, offsetPos, 0f);
		}

		private static bool DoesGeyserExist(string g)
		{
			if (Components.Geysers.m_CmpsByWorld == null)
				return false;

			foreach (var worldId in Components.Geysers.GetWorldsIds())
			{
				foreach (var geyser in Components.Geysers.GetItems(worldId))
				{
					if (geyser.PrefabID() == g)
						return true;
				}
			}

			return false;
		}

		private void OnTemplatePlaced(Vector3 position, TemplateContainer template, string prefabId)
		{
			if (template.otherEntities == null || template.otherEntities.Count == 0)
				return;

			var center = Grid.PosToCell(position);

			var geyser = template.otherEntities[0];

			var prefab = Assets.GetPrefab(prefabId);
			if (prefab == null)
				return;

			if (geyser != null)
			{
				var expectedCell = Grid.OffsetCell(center, geyser.location_x, geyser.location_y);
				Grid.ObjectLayers[(int)ObjectLayer.Building].TryGetValue(expectedCell, out var geyserGo);
				{
					if (geyserGo != null && geyserGo.IsPrefabID(prefabId))
					{
						geyserGo.GetComponent<KPrefabID>().AddTag(TTags.aeteSpawnedGeyser, true);
						geyserGo.AddOrGet<Demolishable>();
					}
				}
			}
		}
	}
}

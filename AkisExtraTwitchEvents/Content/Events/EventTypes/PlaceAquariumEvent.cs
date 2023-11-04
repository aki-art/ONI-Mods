using HarmonyLib;
using ONITwitchLib;
using ONITwitchLib.Utils;
using System.Collections.Generic;
using TemplateClasses;
using Twitchery.Utils;

namespace Twitchery.Content.Events.EventTypes
{
	public class PlaceAquariumEvent() : TwitchEventBase("PlaceAquarium")
	{
		public const string ARTIFACT = "AETE_ArtifactPlaceholder";
		public const string WALL = "AETE_WallPlaceholder";
		public const string GLASSTILE = "AETE_GlassTilePlaceholder";

		public static HashSet<string> templates = new()
		{
			"akis_extra_twitch_events/aquariums/simple"
			// tropical
			// cold
			// erny
		};

		public static HashSet<string> glassTiles = new()
		{
			GlassTileConfig.ID,
			"DecorPackA_WaterStainedGlassTile",
			"DecorPackA_SaltWaterStainedGlassTile",
			"DecorPackA_EthanolStainedGlassTile",
			"DecorPackA_CobaltStainedGlassTile",
			"DecorPackA_Beached_AquamarineStainedGlassTile",
			"DecorPackA_Tundra_LapisLazuliStainedGlassTile",
		};

		public static HashSet<string> artifacts = new()
		{
			"BioluminescentRock",
			"HatchFossil",
			"Moldavite",
			"Sandstone"
		};

		private const string BACKWALL = "Backwall_DecorativeBackwall";

		private bool usesBackwall;

		public override Danger GetDanger() => Danger.Medium;

		public override int GetWeight() => WEIGHTS.COMMON;

		private static void ConfigureTemplate()
		{
			templates.RemoveWhere(t => !TemplateCache.TemplateExists(t));
			foreach (var templateId in templates)
			{
				if (TemplateCache.TemplateExists(templateId))
				{
					var template = TemplateCache.GetTemplate(templateId);
					foreach (var building in template.buildings)
					{
						switch (building.id)
						{
							case GlassTileConfig.ID:
								AddTag(building, GLASSTILE);
								break;
							case ExteriorWallConfig.ID:
								AddTag(building, WALL);
								break;
						}
					}

					foreach (var pickupable in template.pickupables)
					{
						if (pickupable.id == HeatCubeConfig.ID)
							AddTag(pickupable, ARTIFACT);
					}
				}
			}
		}

		private static void AddTag(Prefab prefab, string tag)
		{
			var values = new List<Prefab.template_amount_value>()
			{
				new Prefab.template_amount_value(tag, 1)
			};

			if (prefab.other_values != null)
				values.AddRange(prefab.other_values);

			prefab.other_values = values.ToArray();
		}

		public override void OnGameLoad()
		{
			base.OnGameLoad();

			ConfigureTemplate();

			glassTiles.RemoveWhere(x => Assets.GetBuildingDef(x) == null);

			var template = TemplateCache.GetTemplate(templates.GetRandom());

			var backwall = Assets.GetBuildingDef(BACKWALL);
			if (backwall != null)
			{
				foreach (var building in template.buildings)
				{
					if (HasTag(building, WALL))
						building.id = BACKWALL;
				}

				usesBackwall = true;
			}
		}

		private bool HasTag(Prefab prefab, string tag)
		{
			if (prefab == null || prefab.other_values == null)
				return false;

			foreach (var data in prefab.other_values)
				if (data.id == tag && data.value > 0) return true;

			return false;
		}

		private void RefreshArtifacts(TemplateContainer template)
		{
			if (template.pickupables == null)
				return;

			foreach (var pickupable in template.pickupables)
			{
				if (HasTag(pickupable, ARTIFACT))
					pickupable.id = artifacts.GetRandom();
			}
		}

		public override void Run()
		{
			var tile = glassTiles.GetRandom();

			var position = PosUtil.ClampedMouseWorldPos();
			var originCell = Grid.PosToCell(position);
			var glassTile = Assets.GetBuildingDef(tile);

			var template = TemplateCache.GetTemplate(templates.GetRandom());
			RefreshArtifacts(template);

			foreach (var cell in template.cells)
			{
				var buildingCell = Grid.OffsetCell(originCell, cell.location_x, cell.location_y);
				TileUtil.ClearTile(buildingCell);
			}

			TemplateLoader.Stamp(template, position, () => OnTemplatePlaced(position, template, glassTile.PrefabID));
			// TODO: bubble particles and sound fx
			// TODO. dupe get oxygen mask

			ToastManager.InstantiateToast(STRINGS.AETE_EVENTS.PLACEAQUARIUM.TOAST, STRINGS.AETE_EVENTS.PLACEAQUARIUM.DESC);
		}

		private void OnTemplatePlaced(UnityEngine.Vector3 position, TemplateContainer template, string glassTile)
		{
			if (usesBackwall)
			{
				var cell = Grid.PosToCell(position);
				foreach (var building in template.buildings)
				{
					var buildingCell = Grid.OffsetCell(cell, building.location_x, building.location_y);

					var go = Grid.Objects[buildingCell, (int)ObjectLayer.Backwall];

					if (go == null)
						continue;

					if (building.id == BACKWALL)
					{
						var backwall = go.GetComponent("Backwall");

						if (backwall != null)
							Traverse.Create(backwall).Method("TrySetPattern", glassTile).GetValue();
					}

					go.GetComponent<KPrefabID>().AddTag(GameTags.NoRocketRefund, true);
				}
			}
			/*
			var bounds = template.info.GetBounds(Grid.CellToXY(cell), 0);
			var extents = new Extents(bounds.position.x, bounds.position.y, bounds.width, bounds.height);

						var entries = ListPool<ScenePartitionerEntry, Comet>.Allocate();

						GameScenePartitioner.Instance.GatherEntries(extents, GameScenePartitioner.Instance.pickupablesLayer, entries);

						entries.Recycle();*/
		}
	}
}

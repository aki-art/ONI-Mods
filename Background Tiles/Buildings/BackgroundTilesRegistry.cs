using FUtility;
using HarmonyLib;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace BackgroundTiles.Buildings
{
    class BackgroundTilesRegistry
    {
		static GameObject baseTemplate;
		public static Dictionary<BuildingDef, BuildingDef> tiles = new Dictionary<BuildingDef, BuildingDef>();

		public static void SetBaseTemplate()
		{
			baseTemplate = Traverse.Create(BuildingConfigManager.Instance).Field<GameObject>("baseTemplate").Value;
		}

        public static void RegisterTile(IBuildingConfig original, BuildingDef originalDef)
        {
			string ID = Mod.ID + "_" + originalDef.Tag + "Wall";
			RegisterBuilding(original, originalDef, ID);

            //ModUtil.AddBuildingToPlanScreen(Mod.BackwallCategory, ID);
        }

		static void RegisterStrings(string original, string newTag)
		{
			string key = $"STRINGS.BUILDINGS.PREFABS.{newTag.ToUpperInvariant()}";
			string originalKey = $"STRINGS.BUILDINGS.PREFABS.{original.ToUpperInvariant()}";

			Log.Debuglog(key, originalKey);

			Strings.Add(key + ".NAME", $"Backwall ({Strings.Get(originalKey + ".NAME")})"); // todo: also translatable
			Strings.Add(key + ".DESC", Strings.Get(originalKey + ".DESC"));
			Strings.Add(key + ".EFFECT", Strings.Get(originalKey + ".EFFECT"));
		}

		// manually registering
		public static void RegisterBuilding(IBuildingConfig original, BuildingDef originalDef, string ID)
		{
			if (!DlcManager.IsDlcListValidForCurrentContent(original.GetDlcIds()))
			{
				return;
			}

			BuildingDef def = CreateBuildingDef(originalDef, ID);

			def.RequiredDlcIds = original.GetDlcIds();
			GameObject gameObject = Object.Instantiate(baseTemplate);
            Object.DontDestroyOnLoad(gameObject);

			gameObject.GetComponent<KPrefabID>().PrefabTag = def.Tag;

			gameObject.name = def.PrefabID + "Template";
			gameObject.GetComponent<Building>().Def = def;
			gameObject.GetComponent<OccupyArea>().OccupiedCellsOffsets = def.PlacementOffsets;

			ConfigureBuildingTemplate(gameObject, def.Tag);

			def.BuildingComplete = BuildingLoader.Instance.CreateBuildingComplete(gameObject, def);

			def.BuildingUnderConstruction = BuildingLoader.Instance.CreateBuildingUnderConstruction(def);
			def.BuildingUnderConstruction.name = BuildingConfigManager.GetUnderConstructionName(def.BuildingUnderConstruction.name);

			def.BuildingPreview = BuildingLoader.Instance.CreateBuildingPreview(def);
			def.BuildingPreview.name += "Preview";

			def.PostProcess();

			DoPostConfigureComplete(def.BuildingComplete);
			DoPostConfigureUnderConstruction(def.BuildingUnderConstruction);

			Assets.AddBuildingDef(def);
			tiles.Add(def, originalDef);
			RegisterStrings(originalDef.PrefabID, def.PrefabID);
		}

		public static BuildingDef CreateBuildingDef(BuildingDef original, string ID)
		{
			BuildingDef def = BuildingTemplates.CreateBuildingDef(
				ID,
				1,
				1,
				original.AnimFiles[0].name,
				original.HitPoints,
				original.ConstructionTime,
				original.Mass,
				MATERIALS.ANY_BUILDABLE,
				original.BaseMeltingPoint,
				BuildLocationRule.NotInTiles,
				DECOR.NONE,
				NOISE_POLLUTION.NONE
			);

			def.IsFoundation = false;
			def.TileLayer = ObjectLayer.Backwall;
			def.ReplacementLayer = ObjectLayer.Backwall;

			def.ReplacementTags = new List<Tag>
			{
				// self
			};

			def.Floodable = false;
			def.Overheatable = false;
			def.Entombable = false;
			def.UseStructureTemperature = false;
			def.AudioCategory = original.AudioCategory;
			def.AudioSize = "small";
			def.BaseTimeUntilRepair = -1f;
			def.ObjectLayer = ObjectLayer.Backwall;
			def.SceneLayer = Grid.SceneLayer.Backwall;
			def.isKAnimTile = true;
			def.BlockTileIsTransparent = true; // otherwise it does not want to render solid tiles
			def.ShowInBuildMenu = true; // this makes the Blueprints mod happy

			def.BlockTileMaterial = original.BlockTileMaterial;

			def.BlockTileAtlas = original.BlockTileAtlas;
			def.BlockTilePlaceAtlas = original.BlockTilePlaceAtlas; // todo: replace with custom
			def.BlockTileShineAtlas = original.BlockTileShineAtlas;

			// leaving these null so they are not rendered
			// def.DecorBlockTileInfo = null;
			// def.DecorPlaceBlockTileInfo = null;

			return def;
		}

		static void ConfigureBuildingTemplate(GameObject go, Tag tag)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);
			go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
			go.AddComponent<ZoneTile>();
			go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = Hash.SDBMLower("tiles_" +  tag + "_tops");
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), tag);
		}

		static void DoPostConfigureUnderConstruction(GameObject go)
		{
			go.AddOrGet<KAnimGridTileVisualizer>();
		}

		static void DoPostConfigureComplete(GameObject go)
		{
			GeneratedBuildings.RemoveLoopingSounds(go);
		}
	}
}

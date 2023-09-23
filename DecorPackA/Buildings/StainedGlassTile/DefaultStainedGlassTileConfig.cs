using FUtility;
using TUNING;
using UnityEngine;

namespace DecorPackA.Buildings.StainedGlassTile
{
	// This one exists as a fall back / base for others. should not be buildable in game
	public class DefaultStainedGlassTileConfig : IBuildingConfig
	{
		public string name = "Default";
		public const string DEFAULT_ID = Mod.PREFIX + "DefaultStainedGlassTile";

		public string ID => Mod.PREFIX + name + "StainedGlassTile";

		public static EffectorValues decor;

		public override BuildingDef CreateBuildingDef()
		{
			var ratio = Mathf.Clamp01(Mod.Settings.GlassTile.DyeRatio);

			var materials = new[] { MATERIALS.TRANSPARENT, ModAssets.Tags.stainedGlassDye.ToString() };
			var mass = new[]
			{
				(1f - ratio) * 100f,
				ratio * 100f
			};

			var anim = ID == DEFAULT_ID ? "floor_stained_glass" : name.ToLowerInvariant() + "_glass_tiles";

			var def = BuildingUtil.CreateTileDef(ID, anim, mass, materials, decor, true);

			def.ShowInBuildMenu = true;

			TextureLoader.AddCustomTileAtlas(def, name.ToLowerInvariant() + "_glass", true);
			TextureLoader.AddCustomTileTops(def, name.ToLowerInvariant() + "_glass", existingPlaceID: "tiles_glass_tops_decor_place_info");

			if (ID != DEFAULT_ID)
			{
				var element = ElementLoader.FindElementByName(name);
				var elementName = element != null ? element.tag.ProperNameStripLink() : "N/A";

				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{ID.ToUpperInvariant()}.NAME", StainedGlassTiles.GetFormattedName(elementName));
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{ID.ToUpperInvariant()}.DESC", STRINGS.BUILDINGS.PREFABS.DECORPACKA_DEFAULTSTAINEDGLASSTILE.DESC);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{ID.ToUpperInvariant()}.EFFECT", STRINGS.BUILDINGS.PREFABS.DECORPACKA_DEFAULTSTAINEDGLASSTILE.EFFECT);
			}

			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag tag)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), tag);

			var simCellOccupier = go.AddOrGet<SimCellOccupier>();
			simCellOccupier.notifyOnMelt = true;
			simCellOccupier.setTransparent = true;
			simCellOccupier.movementSpeedMultiplier = Mod.Settings.GlassTile.SpeedBonus;

			if (Mod.Settings.GlassTile.UseDyeTC)
			{
				go.AddComponent<DyeInsulator>();
			}

			go.AddOrGet<TileTemperature>();
			go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = TileConfig.BlockTileConnectorID;
			go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
		}


		public override void DoPostConfigureComplete(GameObject go)
		{
			GeneratedBuildings.RemoveLoopingSounds(go);


			if(go.TryGetComponent(out KPrefabID prefabID))
			{
				prefabID.AddTag(GameTags.FloorTiles);
				prefabID.AddTag(GameTags.Window);
				prefabID.AddTag(ModAssets.Tags.stainedGlass);
				prefabID.AddTag(ModAssets.Tags.noPaint);
			}
		}

		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			go.AddOrGet<KAnimGridTileVisualizer>();

			go.AddTag(GameTags.CorrosionProof);

			// insulate storage
			if (Mod.Settings.GlassTile.InsulateConstructionStorage)
			{
				go.GetComponent<Storage>().SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
			}
		}
	}
}

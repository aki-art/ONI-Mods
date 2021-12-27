using FUtility;
using FUtility.BuildingHelper;
using TUNING;
using UnityEngine;

namespace DecorPackA.Buildings.StainedGlassTile
{
    // This one exists as a fall back / base for others. should not be buildable in game
    public class DefaultStainedGlassTileConfig : IBuildingConfig, IModdedBuilding
    {
        public string name = "Default";
        public const string DEFAULT_ID = Mod.PREFIX + "DefaultStainedGlassTile";

        public string ID => Mod.PREFIX + name + "StainedGlassTile";

        public MBInfo Info => new MBInfo(ID, Consts.BUILD_CATEGORY.BASE, "GlassFurnishings", GlassTileConfig.ID);

        public static EffectorValues decor;

        public override BuildingDef CreateBuildingDef()
        {
            float ratio = Mathf.Clamp01(Mod.Settings.GlassTile.DyeRatio);

            string[] materials = new string[] { MATERIALS.TRANSPARENT, ModAssets.Tags.stainedGlassDye.ToString() };
            float[] mass = new float[]
            {
                (1f - ratio) * 100f,
                ratio * 100f
            };

            BuildingDef def = FUtility.Buildings.CreateTileDef(ID, "floor_stained_glass", mass, materials, decor, true);

            Tiles.AddCustomTileAtlas(def, name.ToLowerInvariant() + "_glass", true);
            Tiles.AddCustomTileTops(def, name.ToLowerInvariant() + "_glass", existingPlaceID: "tiles_glass_tops_decor_place_info");

            return def;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag tag)
        {
            GeneratedBuildings.MakeBuildingAlwaysOperational(go);
            BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), tag);

            SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
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
            go.AddTag(GameTags.FloorTiles);
            go.AddTag(GameTags.Window);
            go.AddTag(ModAssets.Tags.stainedGlass);
            go.AddTag(ModAssets.Tags.noPaint);
        }

        public override void DoPostConfigureUnderConstruction(GameObject go)
        {
            go.AddOrGet<KAnimGridTileVisualizer>();
        }
    }
}

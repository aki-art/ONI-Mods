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

            Log.Debuglog("MASS ");
            Log.Debuglog((1f - ratio) * 100f);
            Log.Debuglog(ratio * 100f);


            var anim = ID == DEFAULT_ID ? "floor_stained_glass" : name.ToLowerInvariant() + "_glass_tiles";

            var def = BuildingUtil.CreateTileDef(ID, anim, mass, materials, decor, true);

            def.ShowInBuildMenu = true;

            Tiles.AddCustomTileAtlas(def, name.ToLowerInvariant() + "_glass", true);
            Tiles.AddCustomTileTops(def, name.ToLowerInvariant() + "_glass", existingPlaceID: "tiles_glass_tops_decor_place_info");

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
            go.AddTag(GameTags.FloorTiles);
            go.AddTag(GameTags.Window);
            go.AddTag(ModAssets.Tags.stainedGlass);
            go.AddTag(ModAssets.Tags.noPaint);
        }

        public override void DoPostConfigureUnderConstruction(GameObject go)
        {
            go.AddOrGet<KAnimGridTileVisualizer>();
            go.AddOrGet<DyeData>();

            // insulate storage
            if (Mod.Settings.GlassTile.InsulateConstructionStorage)
            {
                go.GetComponent<Storage>().SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
            }
        }
    }
}

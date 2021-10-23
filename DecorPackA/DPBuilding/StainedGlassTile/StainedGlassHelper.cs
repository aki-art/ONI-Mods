using FUtility;
using FUtility.BuildingHelper;
using TUNING;
using UnityEngine;

namespace DecorPackA.DPBuilding.StainedGlassTile
{
    class StainedGlassHelper
    {
        public static string GetID(string material)
        {
            return Mod.PREFIX + material + "StainedGlassTile";
        }

        public static BuildingDef GetDef(string name)
        {
            string[] materials = new string[] { ModAssets.Tags.stainedGlassDye.ToString(), MATERIALS.GLASS };
            float[] mass = new float[] { 50f, 50f };

            var def = Buildings.CreateTileDef(
                GetID(name),
                "floor_glass",
                mass,
                materials,
                BUILDINGS.DECOR.BONUS.TIER2,
                true);

            def.ShowInBuildMenu = true;

            Tiles.AddCustomTileAtlas(def, name + "_glass", true);
            Tiles.AddCustomTileTops(def, name + "_glass", existingPlaceID: "tiles_glass_tops_decor_place_info");

            return def;
        }
    }
}

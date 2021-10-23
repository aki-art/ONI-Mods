using FUtility;
using FUtility.BuildingHelper;
using TUNING;

namespace DecorPackA.DPBuilding.StainedGlassTile
{
    class StainedGlassHelper
    {
        public static readonly EffectorValues DECOR = new EffectorValues
        {
            amount = 15,
            radius = 2
        };

        public static string GetID(string material)
        {
            return Mod.PREFIX + material + "StainedGlassTile";
        }

        public static BuildingDef GetDef(string name)
        {
            string[] materials = new string[] { ModAssets.Tags.stainedGlassDye.ToString(), MATERIALS.TRANSPARENT };
            float[] mass = new float[] { 50f, 50f };

            var def = Buildings.CreateTileDef(
                GetID(name),
                "floor_stained_glass",
                mass,
                materials,
                DECOR,
                true);

            def.ShowInBuildMenu = true; // this makes the Blueprints mod happy

            Tiles.AddCustomTileAtlas(def, name + "_glass", true);
            Tiles.AddCustomTileTops(def, name + "_glass", existingPlaceID: "tiles_glass_tops_decor_place_info");

            return def;
        }
    }
}

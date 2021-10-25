using FUtility;
using FUtility.BuildingHelper;
using System;
using TUNING;

namespace DecorPackA.DPBuilding.StainedGlassTile
{
    class StainedGlassHelper
    {
        public static readonly EffectorValues DECOR = new EffectorValues
        {
            amount = 10,
            radius = 3
        };

        public static string GetID(string material)
        {
            return Mod.PREFIX + material + "StainedGlassTile";
        }

        public static BuildingDef GetDef(string name, float thermalConductivity = 1f)
        {
            string[] materials = new string[] { MATERIALS.TRANSPARENT, ModAssets.Tags.stainedGlassDye.ToString() };
            float[] mass = new float[] { 50f, 50f };

            var def = Buildings.CreateTileDef(
                GetID(name),
                "floor_stained_glass",
                mass,
                materials,
                DECOR,
                true);

            Element element = ElementLoader.FindElementByName(name);
            //if(element is object)
            //{
            //    def.ThermalConductivity = 2; // Math.Min((element.thermalConductivity + 1f) / 2f, 220);
            //}

            def.ShowInBuildMenu = true; // this makes the Blueprints mod happy

            Tiles.AddCustomTileAtlas(def, name + "_glass", true);
            Tiles.AddCustomTileTops(def, name + "_glass", existingPlaceID: "tiles_glass_tops_decor_place_info");

            return def;
        }
    }
}

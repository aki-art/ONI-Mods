using FUtility;
using UnityEngine;

namespace DecorPackA.DPBuilding.StainedGlassTile
{
	class SuperInsulatorSGTConfig : DefaultStainedGlassTileConfig
	{
		private static readonly string name = "SuperInsulator";
		new public static string ID = Mod.PREFIX + name + "StainedGlassTile";

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef def = StainedGlassHelper.GetDef(name);
            def.BlockTileMaterial = new Material(def.BlockTileMaterial);
            def.BlockTileMaterial.SetColor("_ShineColour", ModAssets.Colors.extraPink);
            return def;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            base.ConfigureBuildingTemplate(go, prefab_tag);
            var shifter = go.AddComponent<ColorShifter>();
            shifter.a = ModAssets.Colors.palePink;
            shifter.b = ModAssets.Colors.lavender;
            shifter.frequency = 1f;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            base.DoPostConfigureComplete(go);
            //go.AddOrGet<Insulator>();
            go.AddTag(ModAssets.Tags.colorShifty);
        }
    }
} 
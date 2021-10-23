using UnityEngine;

namespace DecorPackA.DPBuilding.StainedGlassTile
{
	class LeadSGTConfig : DefaultStainedGlassTileConfig
	{
		private static readonly string name = "Lead";
		new public static string ID = Mod.PREFIX + name + "StainedGlassTile";

		public override BuildingDef CreateBuildingDef() => StainedGlassHelper.GetDef(name);

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            base.ConfigureBuildingTemplate(go, prefab_tag);
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            base.DoPostConfigureComplete(go);

            if(go.TryGetComponent(out KBatchedAnimController kbac))
            {
                kbac.GetBatch().matProperties.SetColor("_ShineColour", Color.red);
            }
        }
    }
}
using UnityEngine;

namespace DuctTape.Buildings
{
    internal class LiquidInputDuctTape : IBuildingConfig
    {
        public const string ID = "DuctTape_LiquidIn";

        public override BuildingDef CreateBuildingDef()
        {
            var def = DuctTapeTemplate.GetDef(ID, "walls_kanim");

            def.InputConduitType = ConduitType.Liquid;
            def.UtilityInputOffset = new CellOffset(0, 0);

            return def;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            //go.AddTag(GameTags.ModularConduitPort);
            go.AddTag(ModAssets.Tags.ductTape);

            go.AddOrGetDef<ModularConduitPortController.Def>().mode = ModularConduitPortController.Mode.Load;

            var chainedBuilding = go.AddOrGetDef<ChainedBuilding.Def>();
            chainedBuilding.headBuildingTag = ModAssets.Tags.ductTapeable;
            chainedBuilding.linkBuildingTag = ID;
            chainedBuilding.objectLayer = ObjectLayer.Building;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
        }
    }
}

using FUtility;
using UnityEngine;
using static FUtility.Consts;

namespace DuctTapePipes.Buildings
{
    public class LiquidOutputLeechConfig : IBuildingConfig, IModdedBuilding
    {
        public static string ID = Mod.ID + "LiquidOutput";
        public MBInfo Info => new MBInfo(ID, BUILD_CATEGORY.PLUMBING, TECH.LIQUIDS.FLOW_REDIRECTION);

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef def = LeechDefHelper.GetDef(ID, "walls_kanim");
            def.UtilityOutputOffset = CellOffset.none;
            def.OutputConduitType = ConduitType.Liquid;
            def.ObjectLayer = ObjectLayer.LiquidConduitConnection;
            GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, ID);

            return def;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            Storage storage = go.AddComponent<Storage>();
            storage.capacityKg = 10;

            go.AddComponent<LiquidOutputLeech>().storage = storage;

            ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
            conduitDispenser.conduitType = ConduitType.Liquid;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddTag(GameTags.OverlayBehindConduits);
        }
    }
}

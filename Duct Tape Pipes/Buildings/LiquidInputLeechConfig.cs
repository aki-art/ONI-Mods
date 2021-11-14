using FUtility;
using TUNING;
using UnityEngine;
using static FUtility.Consts;

namespace DuctTapePipes.Buildings
{
    public class LiquidInputLeechConfig : IBuildingConfig, IModdedBuilding
    {
        public static string ID = Mod.ID + "LiquidInput";
        public MBInfo Info => new MBInfo(ID, BUILD_CATEGORY.PLUMBING, TECH.LIQUIDS.FLOW_REDIRECTION);

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef def = LeechDefHelper.GetDef(ID, "walls_kanim");
            def.UtilityInputOffset = CellOffset.none;
            def.InputConduitType = ConduitType.Liquid;
            def.ObjectLayer = ObjectLayer.LiquidConduitConnection;
            GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, ID);

            return def;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            Storage storage = go.AddComponent<Storage>();
            storage.capacityKg = 10;

            go.AddComponent<LiquidInputLeech>().storage = storage;

            ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
            conduitConsumer.conduitType = ConduitType.Liquid;
            conduitConsumer.ignoreMinMassCheck = true;
            conduitConsumer.forceAlwaysSatisfied = true;
            conduitConsumer.storage = storage;
            conduitConsumer.capacityKG = storage.capacityKg;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddTag(GameTags.OverlayBehindConduits);
        }
    }
}

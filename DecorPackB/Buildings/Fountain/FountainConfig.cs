using FUtility.BuildingUtil;
using TUNING;
using UnityEngine;
using static DecorPackB.STRINGS.BUILDINGS.PREFABS.DECORPACKB_FOUNTAIN;
using static FUtility.Consts;

namespace DecorPackB.Buildings.Fountain
{
    internal class FountainConfig : IBuildingConfig, IModdedBuilding
    {
        public static string ID = Mod.PREFIX + "Fountain";

        public MBInfo Info => new MBInfo(ID, BUILD_CATEGORY.FURNITURE, TECH.DECOR.FINEART, MarbleSculptureConfig.ID);

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef def = BuildingTemplates.CreateBuildingDef(
               ID,
               5,
               3,
               "decor_pack_b_fountain_kanim",
               BUILDINGS.HITPOINTS.TIER2,
               BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER4,
               new float[2] { 1200, 1200 },
               ModAssets.Materials.FOSSIL,
               BUILDINGS.MELTING_POINT_KELVIN.TIER1,
               BuildLocationRule.OnFloor,
               new EffectorValues(Mod.Settings.FossilDisplay.BaseDecor.Amount, Mod.Settings.FossilDisplay.BaseDecor.Range),
               NOISE_POLLUTION.NONE
           );

            def.Floodable = false;
            def.Overheatable = false;
            def.AudioCategory = AUDIO_CATEGORY.GLASS;
            def.BaseTimeUntilRepair = -1f;
            def.ViewMode = OverlayModes.Decor.ID;
            def.DefaultAnimState = "base";
            def.PermittedRotations = PermittedRotations.FlipH;

            def.InputConduitType = ConduitType.Liquid;
            def.UtilityInputOffset = new CellOffset(1, 0);

            def.OutputConduitType = ConduitType.Liquid;
            def.UtilityInputOffset = new CellOffset(-1, 0);

            return def;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            go.AddTag(GameTags.Decoration);
            go.AddOrGet<BuildingComplete>().isArtable = true;

            Storage storageIn = go.AddComponent<Storage>();
            storageIn.capacityKg = 10f;
            storageIn.storageFilters = STORAGEFILTERS.LIQUIDS;
            storageIn.allowItemRemoval = false;

            ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
            conduitConsumer.conduitType = ConduitType.Liquid;
            conduitConsumer.capacityKG = 10f;
            conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
            conduitConsumer.storage = storageIn;
            conduitConsumer.forceAlwaysSatisfied = true;

            Storage storageOut = go.AddComponent<Storage>();
            storageOut.capacityKg = 10f;
            storageOut.storageFilters = STORAGEFILTERS.LIQUIDS;
            storageOut.allowItemRemoval = false;

            ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
            conduitDispenser.conduitType = ConduitType.Liquid;
            conduitDispenser.storage = storageIn;

            Fountain fountain = go.AddOrGet<Fountain>();
            fountain.storageIn = storageIn;
            fountain.storageOut = storageOut;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            Settings.Config.FossilDisplayConfig config = Mod.Settings.FossilDisplay;


            FountainArtable fountain = go.AddComponent<FountainArtable>();
            fountain.stages.Add(new Artable.Stage("Default", NAME, "base", -5, false, Artable.Status.Ready));

            fountain.stages.Add(new Artable.Stage("Fish", GENIUSQUALITYNAME, "fish_off", 15, true, Artable.Status.Great));
        }
    }
}

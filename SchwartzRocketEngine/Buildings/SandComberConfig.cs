using FUtility;
using System.Collections.Generic;
using TUNING;
using UnityEngine;
using static ComplexRecipe;
using static FUtility.Consts;

namespace SchwartzRocketEngine.Buildings
{
    public class SandComberConfig : IBuildingConfig, IModdedBuilding
    {
        public static readonly string ID = Mod.Prefix("SandComber");
        public MBInfo Info => new MBInfo(ID, BUILD_CATEGORY.FURNITURE, null, SculptureConfig.ID);

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef def = BuildingTemplates.CreateBuildingDef(
               ID,
               4,
               4,
               "rockrefinery_kanim",
               BUILDINGS.HITPOINTS.TIER2,
               BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER4,
               BUILDINGS.CONSTRUCTION_MASS_KG.TIER2,
               MATERIALS.ALL_METALS,
               BUILDINGS.MELTING_POINT_KELVIN.TIER1,
               BuildLocationRule.OnFloor,
               DECOR.PENALTY.TIER2,
               NOISE_POLLUTION.NONE
           );

            def.Floodable = false;
            def.Overheatable = false;

            def.AudioCategory = AUDIO_CATEGORY.PLASTIC;
            def.AudioSize = AUDIO.SIZE.SMALL;

            def.BaseTimeUntilRepair = -1f;
            def.PermittedRotations = PermittedRotations.FlipH;

            def.RequiresPowerInput = true;
            def.EnergyConsumptionWhenActive = 240f;
            def.SelfHeatKilowattsWhenActive = 16f;

            def.ViewMode = OverlayModes.Power.ID;

            return def;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddOrGet<DropAllWorkable>();
            go.AddOrGet<BuildingComplete>().isManuallyOperated = true;

            Storage inStorage = go.AddComponent<Storage>();
            inStorage.capacityKg = 20000f;
            inStorage.showInUI = true;
            inStorage.SetDefaultStoredItemModifiers(Storage.StandardFabricatorStorage);

            Utils.AddRecipe(ID, new RecipeElement(SimHashes.Sand.CreateTag(), 5f), new RecipeElement(SlagWoolConfig.ID, 5f), "Desc", 1);

            Storage outStorage = go.AddComponent<Storage>();
            outStorage.capacityKg = 20000f;
            outStorage.showInUI = true;
            outStorage.SetDefaultStoredItemModifiers(Storage.StandardFabricatorStorage);

            go.AddOrGet<CopyBuildingSettings>();
        }
    }
}

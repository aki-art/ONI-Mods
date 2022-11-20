using HarmonyLib;
using System;
using TUNING;
using UnityEngine;

namespace SolidWaterPump.Buildings
{
    public class SolidWaterPumpConfig : IBuildingConfig
    {
        public const string ID = "SolidWaterPump";

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef def = BuildingTemplates.CreateBuildingDef(
               ID,
               2,
               4,
               "solid_water_pump_kanim",
               BUILDINGS.HITPOINTS.TIER2,
               Mod.Settings.ConstructionTime, //BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER2,
               Mod.Settings.ConstructionMass, //BUILDINGS.CONSTRUCTION_MASS_KG.TIER5,
               Mod.Settings.ConstructionMaterial, //MATERIALS.RAW_MINERALS,
               BUILDINGS.MELTING_POINT_KELVIN.TIER1,
               BuildLocationRule.Anywhere,
               new EffectorValues(
                   Mod.Settings.Decor.Amount,
                   Mod.Settings.Decor.Range), //DECOR.NONE,
               NOISE_POLLUTION.NONE
            );

            def.Floodable = false;
            def.Entombable = true;

            def.AudioCategory = AUDIO.CATEGORY.METAL;
            def.AudioSize = AUDIO.SIZE.LARGE;

            def.UtilityInputOffset = new CellOffset(0, 0);
            def.UtilityOutputOffset = new CellOffset(0, 0);

            def.DefaultAnimState = "on";
            def.ForegroundLayer = Grid.SceneLayer.TileFront;

            def.ShowInBuildMenu = true;

            //def.ReplacementTags = new List<Tag> { LiquidPumpingStationConfig.ID };

            return def;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag tag)
        {
            go.AddOrGet<LoopingSounds>();

            go.AddOrGet<BuildingComplete>().isManuallyOperated = true;

            go.AddOrGet<LiquidPumpingStation>().overrideAnims = new KAnimFile[]
            {
                Assets.GetAnim("anim_interacts_waterpump_kanim")
            };

            var storage = go.AddOrGet<Storage>();
            storage.showInUI = false;
            storage.allowItemRemoval = true;
            storage.showDescriptor = true;
            storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);

            go.AddTag(GameTags.CorrosionProof);
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            var buildingDef = go.GetComponent<Building>().Def;
            var AddGuide = Traverse.Create(typeof(LiquidPumpingStationConfig)).Method("AddGuide", new Type[] { typeof(GameObject), typeof(bool) });

            AddGuide.GetValue(buildingDef.BuildingPreview, false);
            AddGuide.GetValue(buildingDef.BuildingUnderConstruction, true);

            // uses MakeBaseSolid like Solar Panels instead of FakeFloor like original Pumps, this is solid tiles
            MakeBaseSolid.Def solidBase = go.AddOrGetDef<MakeBaseSolid.Def>();
            solidBase.occupyFoundationLayer = true;
            solidBase.solidOffsets = new CellOffset[]
            {
                new CellOffset(0, 0),
                new CellOffset(1, 0)
            };
        }
    }
}

using FUtility;
using System.Collections.Generic;
using TUNING;
using UnityEngine;
using static FUtility.Consts;

namespace DecorPackB.Buildings.OilLantern
{
    internal class OilLanternConfig : IBuildingConfig, IModdedBuilding
    {
        public static string ID = Mod.PREFIX + "OilLantern";

        public MBInfo Info => new MBInfo(ID, BUILD_CATEGORY.FURNITURE, null, FloorLampConfig.ID);

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef def = BuildingTemplates.CreateBuildingDef(
               ID,
               1,
               1,
               "decor_pack_b_lantern_kanim",
               BUILDINGS.HITPOINTS.TIER2,
               BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER4,
               BUILDINGS.CONSTRUCTION_MASS_KG.TIER2,
               MATERIALS.ALL_METALS,
               BUILDINGS.MELTING_POINT_KELVIN.TIER1,
               BuildLocationRule.OnFoundationRotatable,
               new EffectorValues(Mod.Settings.FossilDisplay.BaseDecor.Amount, Mod.Settings.FossilDisplay.BaseDecor.Range),
               NOISE_POLLUTION.NONE
            );

            def.ExhaustKilowattsWhenActive = 0.5f;
            def.SelfHeatKilowattsWhenActive = 0.5f;

            def.UtilityInputOffset = new CellOffset(0, 0);
            def.InputConduitType = ConduitType.Liquid;

            def.BaseTimeUntilRepair = -1f;

            def.AudioCategory = AUDIO_CATEGORY.GLASS;
            def.ViewMode = OverlayModes.Decor.ID;
            def.PermittedRotations = PermittedRotations.R360;

            return def;
        }

        public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
        {
            LightShapePreview lightShapePreview = go.AddComponent<LightShapePreview>();
            lightShapePreview.lux = 1000;
            lightShapePreview.radius = 4f;
            lightShapePreview.shape = LightShape.Circle;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            go.AddTag(RoomConstraints.ConstraintTags.LightSource);
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddOrGet<LoopingSounds>();

            Light2D light2D = go.AddOrGet<Light2D>();
            light2D.overlayColour = LIGHT2D.FLOORLAMP_OVERLAYCOLOR;
            light2D.Color = new Color(2f, 1.8f, 0);
            light2D.Range = 4f;
            light2D.Angle = 0.0f;
            light2D.Direction = LIGHT2D.FLOORLAMP_DIRECTION;
            light2D.Offset = new Vector2(0f, 0.5f);
            light2D.shape = LightShape.Circle;
            light2D.drawOverlay = true;

            go.AddOrGet<OilLantern>();

            Storage storage = go.AddOrGet<Storage>();

            Tag oil = SimHashes.CrudeOil.CreateTag();

            storage.capacityKg = 10f;
            storage.showInUI = true;
            storage.allowItemRemoval = false;
            storage.storageFilters = new List<Tag> { oil };

            ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
            manualDeliveryKG.SetStorage(storage);
            manualDeliveryKG.requestedItemTag = oil;
            manualDeliveryKG.capacity = 10f;
            manualDeliveryKG.refillMass = 1.5f;
            manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
            manualDeliveryKG.operationalRequirement = FetchOrder2.OperationalRequirement.Functional;
            manualDeliveryKG.allowPause = true;

            ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
            conduitConsumer.conduitType = ConduitType.Liquid;
            conduitConsumer.consumptionRate = 0.01f;
            conduitConsumer.capacityTag = oil;
            conduitConsumer.capacityKG = 10f;
            conduitConsumer.forceAlwaysSatisfied = true;
            conduitConsumer.ignoreMinMassCheck = true;
            conduitConsumer.alwaysConsume = true;
            conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;

            ElementConverter elementConverter = go.AddComponent<ElementConverter>();
            elementConverter.consumedElements = new ElementConverter.ConsumedElement[1]
            {
                new ElementConverter.ConsumedElement(oil, 0.01f)
            };
            elementConverter.outputElements = new ElementConverter.OutputElement[]
            {
                new ElementConverter.OutputElement(0.01f, SimHashes.CarbonDioxide, 303.15f, outputElementOffsety: 1f)
            };

            go.AddOrGetDef<LightController.Def>();
            go.GetComponent<RequireInputs>().SetRequirements(false, false);

            Prioritizable.AddRef(go);
        }
    }
}

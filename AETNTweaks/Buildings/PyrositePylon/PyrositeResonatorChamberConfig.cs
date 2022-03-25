using System;
using TUNING;
using UnityEngine;

namespace AETNTweaks.Buildings.PyrositePylon
{
    // copy pasta of AETN for now
    public class PyrositeResonatorChamberConfig : IBuildingConfig
    {
        public const string ID = "AETNT_PyrositeResonatorChamber";

        public override BuildingDef CreateBuildingDef()
        {
            var def = BuildingTemplates.CreateBuildingDef(
                ID,
                1,
                1,
                "farmtile_kanim",
                BUILDINGS.HITPOINTS.TIER0,
                BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER3,
                BUILDINGS.CONSTRUCTION_MASS_KG.TIER2,
                MATERIALS.ALL_METALS,
                BUILDINGS.MELTING_POINT_KELVIN.TIER3,
                BuildLocationRule.Anywhere,
                DECOR.BONUS.TIER0,
                NOISE_POLLUTION.NONE);

            // TODO: temporary values
            def.ExhaustKilowattsWhenActive = -16f;
            def.SelfHeatKilowattsWhenActive = -64f;

            def.Floodable = true;
            def.Entombable = false;

            def.AudioCategory = AUDIO.CATEGORY.METAL;

            def.UtilityInputOffset = CellOffset.none;
            def.InputConduitType = ConduitType.Gas;

            return def;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);

            go.AddOrGet<MassiveHeatSink>(); // TODO: I probably want custom
            go.AddOrGet<Pyrosite>().activeDuration = Mod.Settings.PyrositeActivityDuration;

            // TODO: should this be a child gameObject instead?
            var lineRenderer = go.AddOrGet<LineRenderer>();
            lineRenderer.startWidth = lineRenderer.endWidth = 0.05f;

            go.AddOrGet<Tether>();

            go.AddOrGet<MinimumOperatingTemperature>().minimumTemperature = 100f;

            go.AddOrGet<LoopingSounds>();

            go.AddOrGet<Storage>().capacityKg = 0.099999994f; 

            var consumer = go.AddOrGet<ConduitConsumer>();
            consumer.conduitType = ConduitType.Gas;
            consumer.consumptionRate = 1f;
            consumer.capacityTag = GameTagExtensions.Create(SimHashes.Hydrogen);
            consumer.capacityKG = 0.099999994f;
            consumer.forceAlwaysSatisfied = true;
            consumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;

            go.AddOrGet<ElementConverter>().consumedElements = new ElementConverter.ConsumedElement[]
            {
                new ElementConverter.ConsumedElement(ElementLoader.FindElementByHash(SimHashes.Hydrogen).tag, 0.01f)
            };

            go.AddOrGetDef<PoweredActiveController.Def>();
        }
    }
}

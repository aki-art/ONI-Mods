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
            BuildingDef def = BuildingTemplates.CreateBuildingDef(
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

            return def;
        }

        public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
        {
            base.DoPostConfigurePreview(def, go);

            LineRenderer lineRenderer = go.AddOrGet<LineRenderer>();
            lineRenderer.startWidth = lineRenderer.endWidth = 0.15f;
            lineRenderer.material = ModAssets.tetherPlaceMaterial;

            Tether tether = go.AddOrGet<Tether>();
            tether.subDivisionCount = 15;
            tether.segmentLength = 0.25f;

            go.AddComponent<TetherPlaceVisualizer>().centerOffset = new Vector3(0.5f, 3.5f);
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);

            go.AddOrGet<NotSoMassiveHeatSink>();
            go.AddOrGet<Pyrosite>().activeDuration = Mod.Settings.PyrositeActivityDuration;

            // TODO: should this be a child gameObject instead?
            LineRenderer lineRenderer = go.AddOrGet<LineRenderer>();
            lineRenderer.startWidth = lineRenderer.endWidth = 0.15f;
            lineRenderer.material = ModAssets.tetherMaterial;

            Tether tether = go.AddOrGet<Tether>();
            tether.subDivisionCount = 25;
            tether.segmentLength = 0.15f;

            go.AddOrGet<MinimumOperatingTemperature>().minimumTemperature = 100f;

            go.AddOrGet<LoopingSounds>();

            go.AddOrGet<Storage>().capacityKg = 0.1f;

            go.AddOrGet<ElementConverter>().consumedElements = new ElementConverter.ConsumedElement[]
            {
                new ElementConverter.ConsumedElement(ElementLoader.FindElementByHash(SimHashes.Hydrogen).tag, 0.01f)
            };

            go.AddOrGetDef<PoweredActiveController.Def>();
        }
    }
}

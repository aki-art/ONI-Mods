using FUtility;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace TransparentAluminum.Content.Buildings.SolarRoad
{
    public class SolarRoadConfig : IBuildingConfig
    {
        public const string ID = "TransparentAluminum_SolarRoad";

        public override BuildingDef CreateBuildingDef()
        {
            var def = BuildingTemplates.CreateBuildingDef(
                ID,
                1,
                1,
                "farmtile_kanim",
                BUILDINGS.HITPOINTS.TIER2,
                BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER3,
                new[] { 400f, 200f },
                new[] { MATERIALS.REFINED_METAL, Elements.TransparentAluminum.ToString() },
                BUILDINGS.MELTING_POINT_KELVIN.TIER3,
                BuildLocationRule.Tile,
                DECOR.NONE,
                NOISE_POLLUTION.NONE);

            BuildingTemplates.CreateFoundationTileDef(def);

            def.Floodable = false;
            def.Entombable = false;
            def.Overheatable = false;

            def.GeneratorWattageRating = SolarRoad.BASE_WATTS * 8f;
            def.GeneratorBaseCapacity = def.GeneratorWattageRating;

            def.RequiresPowerOutput = true;
            def.PowerOutputOffset = new CellOffset(0, 0);

            def.ViewMode = OverlayModes.Power.ID;

            def.ExhaustKilowattsWhenActive = 0f;
            def.SelfHeatKilowattsWhenActive = 0f;

            def.BaseTimeUntilRepair = -1f;

            def.SceneLayer = Grid.SceneLayer.TileMain;

            def.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;

            def.DragBuild = true;

            BuildingTemplates.CreateFoundationTileDef(def);

            return def;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddOrGet<SolarRoad>();
            go.AddOrGet<Upgradeable>().maxLevel = 3;
        }
    }
}

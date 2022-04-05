using AsphaltStairs.Cmps;
using Stairs;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace AsphaltStairs.Buildings
{
    public class AsphaltStairsConfig : IBuildingConfig
    {
        public static string ID = "AsphaltStairs";

        public override BuildingDef CreateBuildingDef()
        {
            var def = BuildingTemplates.CreateBuildingDef(
                ID,
                1,
                1,
                "asphalt_stairs_kanim",
                BUILDINGS.HITPOINTS.TIER0,
                BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER1,
                new float[] { BUILDINGS.CONSTRUCTION_MASS_KG.TIER2[0], BUILDINGS.CONSTRUCTION_MASS_KG.TIER0[0] },
                new string[] { "Bitumen", "Metal" },
                BUILDINGS.MELTING_POINT_KELVIN.TIER2,
                BuildLocationRule.Anywhere,
                BUILDINGS.DECOR.PENALTY.TIER1,
                NOISE_POLLUTION.NONE);

            def.Floodable = false;
            def.Entombable = false;
            def.Overheatable = false;
            def.AudioCategory = AUDIO.CATEGORY.HOLLOW_METAL;
            def.AudioSize = AUDIO.SIZE.SMALL;
            def.BaseTimeUntilRepair = -1f;
            def.DragBuild = true;
            def.IsFoundation = false;
            def.BaseDecor = -2f;
            def.TileLayer = ObjectLayer.LadderTile;
            def.PermittedRotations = PermittedRotations.FlipH;
            def.ReplacementLayer = ObjectLayer.ReplacementLadder;
            def.ReplacementTags = new List<Tag> { Mod.stairsTag };
            def.EquivalentReplacementLayers = new List<ObjectLayer> { ObjectLayer.ReplacementTile };

            return def;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            GeneratedBuildings.MakeBuildingAlwaysOperational(go);

            go.AddOrGet<Stair>();

            var tileable = go.AddOrGet<ComplexAnimTileable>();
            tileable.objectLayer = ObjectLayer.LadderTile;
            tileable.tag = Mod.stairsTag;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
        }
    }
}

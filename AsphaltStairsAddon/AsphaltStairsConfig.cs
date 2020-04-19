using Stairs;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace AsphaltStairsAddon
{
    public class AsphaltStairsConfig : IBuildingConfig
    {
        public static string ID = "AsphaltStairs";
        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(
                id: ID,
                width: 1,
                height: 1,
                anim: "asphalt_stairs_kanim",
                hitpoints: 10,
                construction_time: 10f,
                construction_mass: new float[] { BUILDINGS.CONSTRUCTION_MASS_KG.TIER2[0], BUILDINGS.CONSTRUCTION_MASS_KG.TIER0[0] },
                construction_materials: new string[] { "Bitumen", "Metal" },
                melting_point: 2400f,
                build_location_rule: BuildLocationRule.Anywhere,
                decor: BUILDINGS.DECOR.PENALTY.TIER1,
                noise: NOISE_POLLUTION.NONE );

            buildingDef.Floodable = false;
            buildingDef.Entombable = false;
            buildingDef.Overheatable = false;
            buildingDef.AudioCategory = "Metal";
            buildingDef.AudioSize = "small";
            buildingDef.BaseTimeUntilRepair = -1f;
            buildingDef.DragBuild = true;
            buildingDef.IsFoundation = false;
            buildingDef.BaseDecor = -2f;
            buildingDef.TileLayer = ObjectLayer.LadderTile;
            buildingDef.PermittedRotations = PermittedRotations.FlipH;
            buildingDef.ReplacementLayer = ObjectLayer.ReplacementLadder;
            buildingDef.ReplacementTags = new List<Tag> { Patches.tag_Stairs };
            buildingDef.EquivalentReplacementLayers = new List<ObjectLayer> { ObjectLayer.ReplacementTile };

            return buildingDef;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            GeneratedBuildings.MakeBuildingAlwaysOperational(go);
            go.AddOrGet<Stair>();
            go.AddOrGet<ComplexAnimTileable>();
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
        }

        public override void DoPostConfigureUnderConstruction(GameObject go)
        {
            base.DoPostConfigureUnderConstruction(go);
        }
    }
}

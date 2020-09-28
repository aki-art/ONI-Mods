using FUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TUNING;
using UnityEngine;

namespace Bomb
{
    class FuseConfig : IBuildingConfig, IModdedBuilding
    {
        public static readonly string ID = "AB_Fuse";
        public MBInfo Info => new MBInfo(ID, "Base");

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef def = BuildingTemplates.CreateBuildingDef(
               id: ID,
               width: 1,
               height: 1,
               anim: "ladder_kanim",
               hitpoints: 100,
               construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER4,
               construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER4,
               construction_materials: MATERIALS.RAW_METALS,
               melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER1,
               build_location_rule: BuildLocationRule.Anywhere,
               decor: new EffectorValues(20, 8),
               noise: NOISE_POLLUTION.NONE
           );

            def.Floodable = false;
            def.Overheatable = false;
            def.Entombable = false;
            def.AudioCategory = "Metal";
            def.AudioSize = "small";
            def.BaseTimeUntilRepair = -1f;
            def.DragBuild = true;
            def.TileLayer = ObjectLayer.LogicWireTile;
            def.ReplacementLayer = ObjectLayer.ReplacementLogicWire;
            def.SceneLayer = Grid.SceneLayer.LogicWires;

            return def;
        }
        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            go.AddOrGet<AnimTileable>();
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddComponent<Fuse>();
        }
    }

}

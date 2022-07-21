﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUNING;
using UnityEngine;

namespace Backwalls.Buildings
{
    public class BackwallConfig : IBuildingConfig
    {
        public const string ID = "Backwall_Backwall";

        public override BuildingDef CreateBuildingDef()
        {
            var def = BuildingTemplates.CreateBuildingDef(
                ID,
                1,
                1,
                "floor_glass_kanim",
                BUILDINGS.HITPOINTS.TIER1,
                BUILDINGS.WORK_TIME_SECONDS.SHORT_WORK_TIME,
                BUILDINGS.CONSTRUCTION_MASS_KG.TIER1,
                MATERIALS.GLASSES,
                BUILDINGS.MELTING_POINT_KELVIN.TIER1,
                BuildLocationRule.Anywhere,
                DECOR.BONUS.TIER1,
                NOISE_POLLUTION.NONE);

            def.DefaultAnimState = "jfgifgfg";
            def.ObjectLayer = ObjectLayer.Backwall;
            def.Floodable = false;
            def.Breakable = false;
            def.Entombable = false;

            return def;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            //go.AddOrGet<SimCellOccupier>().doReplaceElement = false;
            go.AddOrGet<CopyBuildingSettings>().copyGroupTag = ID;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddComponent<BackwallLink>();
            go.AddComponent<Backwall>();
        }
    }
}

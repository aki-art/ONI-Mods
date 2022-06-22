using FUtility;
using System;
using UnityEngine;

namespace ZiplineTest
{
    public class ZiplineAnchorConfig : IBuildingConfig
    {
        public const string ID = "TestZiplineAnchor";

        public override BuildingDef CreateBuildingDef()
        {
            var def = FBuildingTemplates.CreateTestingDef(ID, 1, 2, BuildLocationRule.OnFloor);

            return def;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            LineRenderer lineRenderer = go.AddOrGet<LineRenderer>();
            lineRenderer.startWidth = lineRenderer.endWidth = 0.15f;
            //lineRenderer.material = ModAssets.tetherMaterial;

            Tether tether = go.AddOrGet<Tether>();
            tether.subDivisionCount = 25;
            tether.segmentLength = 0.15f;

            go.AddOrGet<NavTeleporter>();
            go.AddOrGet<ZiplineAnchor>();

        }
        public override void DoPostConfigureComplete(GameObject go)
        {
        }
    }
}

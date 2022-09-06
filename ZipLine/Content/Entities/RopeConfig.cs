using System.Collections.Generic;
using UnityEngine;
using ZipLine.Content.Cmps;

namespace ZipLine.Content.Entities
{
    internal class RopeConfig : IEntityConfig
    {
        public const string ID = "Zipline_Rope";

        public GameObject CreatePrefab()
        {
            // KSelectable, KPrefabID
            var template = EntityTemplates.CreateEntity(ID, "Rope");

            template.AddOrGet<KPolygonCollider2D>();
            template.AddOrGet<Prioritizable>();
            template.AddOrGet<OccupyArea>();
            template.AddOrGet<EntombVulnerable>();
            template.AddOrGet<Rope>();
            template.AddOrGet<SelectableLineRenderer>();

            var fiberStorage = template.AddOrGet<Storage>();
            fiberStorage.capacityKg = 100f;
            fiberStorage.storageFilters = new List<Tag>
            {
                BasicFabricConfig.ID
            };

            var lineRenderer = template.AddOrGet<LineRenderer>();

            var linePrefab = ModAssets.linePrefab.GetComponent<LineRenderer>();
            lineRenderer.material = linePrefab.material;
            lineRenderer.startWidth = lineRenderer.endWidth = linePrefab.startWidth;
            lineRenderer.textureMode = linePrefab.textureMode;
            lineRenderer.material.mainTexture = ModAssets.ghostTex;

            return template;
        }

        public string[] GetDlcIds()
        {
            return DlcManager.AVAILABLE_ALL_VERSIONS;
        }

        public void OnPrefabInit(GameObject inst)
        {
        }

        public void OnSpawn(GameObject inst)
        {
        }
    }
}

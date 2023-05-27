using Bestagons.Content.ModDb;
using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bestagons.Content.Map
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class PurchasableHex : KMonoBehaviour
    {
        [Serialize] public List<HashedString> availableOptions;
        [Serialize] public HexCoord location;
        [Serialize] public bool purchased;
        [Serialize] public bool isAccessible; // assume once accessible, does not become inaccessible

        private GameObject visualizer;

        public override void OnSpawn()
        {
            base.OnSpawn();
            visualizer = MakeMeshData(1f);
            visualizer.transform.position = transform.position with { z = Grid.GetLayerZ(Grid.SceneLayer.SceneMAX) };
            visualizer.SetActive(true);

            Mod.hexes.Add(this);

            //Purchase(BDb.hexTiles.resources.GetRandom().IdHash);

            Subscribe((int)GameHashes.NewGameSpawn, OnNewGame);
        }

        private void OnNewGame(object obj)
        {

        }

        public override void OnCleanUp()
        {
            base.OnCleanUp();
            Destroy(visualizer);
        }

        public void AddOption(HashedString hexTileId)
        {
            availableOptions ??= new List<HashedString>();
            availableOptions.Add(hexTileId);
        }

        public void Purchase(HashedString hexTileId)
        {
            var hexTile = BDb.hexTiles.TryGet(hexTileId);

            if (hexTile == null)
            {
                Log.Warning("Tried to purchase a hextile that does not exist!");
                return;
            }

            var template = TemplateCache.GetTemplate(hexTile.templateId);
            if (template == null)
            {
                Log.Warning($"Cannot place non-existing template: {hexTile.templateId}");
                return;
            }

            var bounds = template.GetTemplateBounds(transform.position);
            var min = Grid.XYToCell(bounds.min.x, bounds.min.y);
            var max = Grid.XYToCell(bounds.max.x, bounds.max.y);

            if (!Grid.IsValidBuildingCell(min) || !Grid.IsValidBuildingCell(max))
            {
                Log.Warning("template outside of bounds");
                return;
            }

            TemplateLoader.Stamp(template, transform.position, OnHexTemplatePlaced);
        }

        private void OnHexTemplatePlaced()
        {
            // reveal grid
        }

        private GameObject MakeMeshData(float dia)
        {
            var vertices = new Vector3[]
            {
                new Vector3(dia / 2, dia / 20),
                new Vector3(dia / 2, 1, 0),
                new Vector3(1, 3 * (dia / 4)),
                new Vector3(1, dia / 4),
                new Vector3(dia / 2, 0),
                new Vector3(0, dia / 4),
                new Vector3(0, 3 * (dia / 4)),
            };

            var triangles = new int[]
            {
                0, 1, 2,
                0, 2, 3,
                0, 3, 4,
                0, 4, 5,
                0, 5, 6,
                0, 6, 1
            };

            var mesh = new Mesh
            {
                vertices = vertices,
                triangles = triangles
            };

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            var go = new GameObject();
            go.AddComponent<MeshFilter>().mesh = mesh;

            var material = new Material(Shader.Find("Sprites/Default"))
            {
                renderQueue = 3501
            };

            material.SetColor("_Color", UnityEngine.Random.ColorHSV() with { a = 0.3f });

            go.AddComponent<MeshRenderer>().material = material;

            return go;
        }

        [Serializable]
        public struct Option
        {
            public List<Price> price;
            public HashedString hexTileId;
        }

        [Serializable]
        public struct Price
        {
            public string currencyId;
            public int amount;
        }
    }
}

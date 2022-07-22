using Backwalls.Buildings;
using FUtility;
using Rendering;
using System;
using System.Collections.Generic;
using UnityEngine;
using Bits = Rendering.BlockTileRenderer.Bits;

namespace Backwalls
{
    public class BackwallRenderer : MonoBehaviour
    {
        [SerializeField]
        public bool forceRebuild;

        private readonly Dictionary<BackwallVariant, RenderInfo> renderInfos = new Dictionary<BackwallVariant, RenderInfo>();

        public readonly Dictionary<int, Color> colorInfos = new Dictionary<int, Color>();

        private int selectedCell = -1;
        private int highlightCell = -1;

        private Color highlightColour = new Color(1.25f, 1.25f, 1.25f, 1f);
        private Color selectColour = new Color(1.5f, 1.5f, 1.5f, 1f);

        public void LateUpdate()
        {
            Render();
        }

        private void Render()
        {
            Vector2I bottomLeft;
            Vector2I topRight;

            if (GameUtil.IsCapturingTimeLapse())
            {
                bottomLeft = new Vector2I(0, 0);
                topRight = new Vector2I(Grid.WidthInCells / 16, Grid.HeightInCells / 16);
            }
            else
            {
                var visibleArea = GridVisibleArea.GetVisibleArea();
                bottomLeft = new Vector2I(visibleArea.Min.x / 16, visibleArea.Min.y / 16);
                topRight = new Vector2I((visibleArea.Max.x + 16 - 1) / 16, (visibleArea.Max.y + 16 - 1) / 16);
            }

            foreach (var renderInfo in renderInfos)
            {
                var info = renderInfo.Value;
                for (var y = bottomLeft.y; y < topRight.y; y++)
                {
                    for (var x = bottomLeft.x; x < topRight.x; x++)
                    {
                        info.Rebuild(this, x, y, MeshUtil.vertices, MeshUtil.uvs, MeshUtil.indices, MeshUtil.colours);
                        info.Render(x, y);
                    }
                }
            }
        }

        public void AddBlock(int renderLayer, BackwallVariant def, int cell)
        {
            if (!renderInfos.TryGetValue(def, out var renderInfo))
            {
                renderInfo = new RenderInfo(this, renderLayer, def);
                renderInfos[def] = renderInfo;
            }

            renderInfo.AddCell(cell);
        }

        public void RemoveBlock(BackwallVariant key, int cell)
        {
            if (renderInfos.TryGetValue(key, out var renderInfo))
            {
                renderInfo.RemoveCell(cell);
            }
        }

        private static bool MatchesDef(GameObject go, BackwallVariant variant, Color color)
        {
            if(go == null)
            {
                return false;
            }

            if(go.GetComponent<Backwall>()?.variant != variant)
            {
                return false;
            }

            if(go.TryGetComponent(out Dyeable dyeable))
            {
                return dyeable.color == color;
            }

            return false;
        }

        public virtual Bits GetConnectionBits(int x, int y, int query_layer)
        {
            Bits bits = 0;
            var gameObject = Grid.Objects[y * Grid.WidthInCells + x, query_layer];
            var variant = gameObject?.GetComponent<Backwall>().variant;

            var color = Color.white;

            if(gameObject != gameObject.TryGetComponent(out Dyeable dyeable))
            {
                color = dyeable.color;
            }

            if (y > 0)
            {
                var cell = (y - 1) * Grid.WidthInCells + x;
                if (x > 0 && MatchesDef(Grid.Objects[cell - 1, query_layer], variant, color))
                {
                    bits |= Bits.DownLeft;
                }
                if (MatchesDef(Grid.Objects[cell, query_layer], variant, color))
                {
                    bits |= Bits.Down;
                }
                if (x < Grid.WidthInCells - 1 && MatchesDef(Grid.Objects[cell + 1, query_layer], variant, color))
                {
                    bits |= Bits.DownRight;
                }
            }

            var num2 = y * Grid.WidthInCells + x;

            if (x > 0 && MatchesDef(Grid.Objects[num2 - 1, query_layer], variant, color))
            {
                bits |= Bits.Left;
            }
            if (x < Grid.WidthInCells - 1 && MatchesDef(Grid.Objects[num2 + 1, query_layer], variant, color))
            {
                bits |= Bits.Right;
            }
            if (y < Grid.HeightInCells - 1)
            {
                var num3 = (y + 1) * Grid.WidthInCells + x;
                if (x > 0 && MatchesDef(Grid.Objects[num3 - 1, query_layer], variant, color))
                {
                    bits |= Bits.UpLeft;
                }
                if (MatchesDef(Grid.Objects[num3, query_layer], variant, color))
                {
                    bits |= Bits.Up;
                }
                if (x < Grid.WidthInCells + 1 && MatchesDef(Grid.Objects[num3 + 1, query_layer], variant, color))
                {
                    bits |= Bits.UpRight;
                }
            }

            return bits;
        }

        public void SelectCell(int cell, bool enabled)
        {
            UpdateCellStatus(ref selectedCell, cell, enabled);
        }

        public void HighlightCell(int cell, bool enabled)
        {
            UpdateCellStatus(ref highlightCell, cell, enabled);
        }

        private void UpdateCellStatus(ref int cellStatus, int cell, bool enabled)
        {
            if (enabled)
            {
                if (cell == cellStatus)
                {
                    return;
                }

                if (cellStatus != -1)
                {
                    foreach (var info in renderInfos)
                    {
                        info.Value.MarkDirtyIfOccupied(cellStatus);
                    }
                }

                cellStatus = cell;

                foreach (var info in renderInfos)
                {
                    info.Value.MarkDirtyIfOccupied(cellStatus);
                }

                return;
            }

            if (cellStatus == cell)
            {
                foreach (var keyValuePair3 in renderInfos)
                {
                    keyValuePair3.Value.MarkDirty(cellStatus);
                }

                cellStatus = -1;
            }

            forceRebuild = true;
        }

        public void Rebuild(int cell)
        {
            foreach (var info in renderInfos)
            {
                info.Value.MarkDirty(cell);
            }
        }

        private Color GetCellColour(int num)
        {
            var zoneType = World.Instance.zoneRenderData.GetSubWorldZoneType(num);
            var zoneColor = World.Instance.zoneRenderData.zoneColours[(int)zoneType];
            var color = new Color32(zoneColor.r, zoneColor.g, zoneColor.b, 255);

            var baseColor = Color.white;

            if(colorInfos.ContainsKey(num))
            {
                baseColor = colorInfos[num];
            }

            color = Color.Lerp(color, baseColor, 0.8f);

            var selectionColor = num == selectedCell ? selectColour : num == highlightCell ? highlightColour : Color.white;
            return selectionColor * color;
        }

        private class RenderInfo
        {
            private int queryLayer = (int)ObjectLayer.Backwall;
            private int renderLayer;

            private Vector3 rootPosition;
            private Material material;

            private float zOffset;

            private Mesh[,] meshChunks;
            private bool[,] dirtyChunks;

            private Vector2 trimUVSize = new Vector2(0.03125f, 0.03125f);

            private AtlasInfo[] atlasInfo;

            private Dictionary<int, int> occupiedCells = new Dictionary<int, int>();

            public RenderInfo(BackwallRenderer backwallRenderer, int renderLayer, BackwallVariant variant)
            {
                this.renderLayer = renderLayer;

                zOffset = Grid.GetLayerZ(Grid.SceneLayer.TileFront) - Grid.GetLayerZ(Grid.SceneLayer.Liquid) - 2f;
                rootPosition = new Vector3(0f, 0f, zOffset);

                material = new Material(variant.material)
                {
                    renderQueue = RenderQueues.Liquid,
                    name = variant.atlas.name + "Mat",
                    mainTexture = variant.atlas.texture
                };

                var x = Grid.WidthInCells / 16 + 1;
                var y = Grid.HeightInCells / 16 + 1;

                meshChunks = new Mesh[x, y];
                dirtyChunks = new bool[x, y];

                for (var i = 0; i < y; i++)
                {
                    for (var j = 0; j < x; j++)
                    {
                        dirtyChunks[j, i] = true;
                    }
                }

                var num3 = variant.atlas.items[0].name.Length - 4 - 8;
                var startIndex = num3 - 1 - 8;
                atlasInfo = new AtlasInfo[variant.atlas.items.Length];

                for (var k = 0; k < atlasInfo.Length; k++)
                {
                    var item = variant.atlas.items[k];

                    var value = item.name.Substring(startIndex, 8);
                    var value2 = item.name.Substring(num3, 8);

                    var requiredConnections = Convert.ToInt32(value, 2);
                    var forbiddenConnections = Convert.ToInt32(value2, 2);

                    atlasInfo[k].requiredConnections = (Bits)requiredConnections;
                    atlasInfo[k].forbiddenConnections = (Bits)forbiddenConnections;

                    atlasInfo[k].uvBox = item.uvBox;
                    atlasInfo[k].name = item.name;
                }
            }

            public void MarkDirtyIfOccupied(int cell)
            {
                if (occupiedCells.ContainsKey(cell))
                {
                    MarkDirty(cell);
                }
            }

            public void AddCell(int cell)
            {
                occupiedCells.TryGetValue(cell, out var num);
                occupiedCells[cell] = num + 1;
                MarkDirty(cell);
            }

            public void MarkDirty(int cell)
            {
                var chunkIdx = BlockTileRenderer.GetChunkIdx(cell);
                dirtyChunks[chunkIdx.x, chunkIdx.y] = true;
            }

            public void RemoveCell(int cell)
            {
                occupiedCells.TryGetValue(cell, out var num);

                if (num > 1)
                {
                    occupiedCells[cell] = num - 1;
                }
                else
                {
                    occupiedCells.Remove(cell);
                }

                MarkDirty(cell);
            }

            internal void Rebuild(BackwallRenderer renderer, int chunkX, int chunkY, List<Vector3> vertices, List<Vector2> uvs, List<int> indices, List<Color> colors)
            {
                if (!dirtyChunks[chunkX, chunkY] && !renderer.forceRebuild)
                {
                    return;
                }

                dirtyChunks[chunkX, chunkY] = false;

                vertices.Clear();
                uvs.Clear();
                indices.Clear();
                colors.Clear();

                for (var x = chunkY * 16; x < chunkY * 16 + 16; x++)
                {
                    for (var y = chunkX * 16; y < chunkX * 16 + 16; y++)
                    {
                        var cell = x * Grid.WidthInCells + y;
                        if (occupiedCells.ContainsKey(cell))
                        {
                            var connectionBits = renderer.GetConnectionBits(y, x, queryLayer);
                            for (var k = 0; k < atlasInfo.Length; k++)
                            {
                                var flag = (atlasInfo[k].requiredConnections & connectionBits) == atlasInfo[k].requiredConnections;
                                var flag2 = (atlasInfo[k].forbiddenConnections & connectionBits) > 0;

                                if (flag && !flag2)
                                {
                                    var cellColour = renderer.GetCellColour(cell);
                                    AddVertexInfo(atlasInfo[k], trimUVSize, y, x, connectionBits, cellColour, vertices, uvs, indices, colors);

                                    break;
                                }
                            }
                        }
                    }
                }

                var mesh = meshChunks[chunkX, chunkY];

                if (vertices.Count > 0)
                {
                    if (mesh == null)
                    {
                        mesh = new Mesh
                        {
                            name = "BackWall"
                        };

                        meshChunks[chunkX, chunkY] = mesh;
                    }

                    mesh.Clear();
                    mesh.SetVertices(vertices);
                    mesh.SetUVs(0, uvs);
                    mesh.SetColors(colors);
                    mesh.SetTriangles(indices, 0);
                }
                else if (mesh != null)
                {
                    meshChunks[chunkX, chunkY] = null;
                }
            }

            private void AddVertexInfo(AtlasInfo atlas_info, Vector2 uv_trim_size, int x, int y, Bits connection_bits, Color color, List<Vector3> vertices, List<Vector2> uvs, List<int> indices, List<Color> colours)
            {
                var vector = new Vector2(x, y);
                var vector2 = vector + new Vector2(1f, 1f);
                var vector3 = new Vector2(atlas_info.uvBox.x, atlas_info.uvBox.w);
                var vector4 = new Vector2(atlas_info.uvBox.z, atlas_info.uvBox.y);

                if ((connection_bits & Bits.Left) == 0)
                {
                    vector.x -= 0.25f;
                }
                else
                {
                    vector3.x += uv_trim_size.x;
                }
                if ((connection_bits & Bits.Right) == 0)
                {
                    vector2.x += 0.25f;
                }
                else
                {
                    vector4.x -= uv_trim_size.x;
                }
                if ((connection_bits & Bits.Up) == 0)
                {
                    vector2.y += 0.25f;
                }
                else
                {
                    vector4.y -= uv_trim_size.y;
                }
                if ((connection_bits & Bits.Down) == 0)
                {
                    vector.y -= 0.25f;
                }
                else
                {
                    vector3.y += uv_trim_size.y;
                }

                var count = vertices.Count;

                vertices.Add(vector);
                vertices.Add(new Vector2(vector2.x, vector.y));
                vertices.Add(vector2);
                vertices.Add(new Vector2(vector.x, vector2.y));

                uvs.Add(vector3);
                uvs.Add(new Vector2(vector4.x, vector3.y));
                uvs.Add(vector4);
                uvs.Add(new Vector2(vector3.x, vector4.y));

                indices.Add(count);
                indices.Add(count + 1);
                indices.Add(count + 2);
                indices.Add(count);
                indices.Add(count + 2);
                indices.Add(count + 3);

                colours.Add(color);
                colours.Add(color);
                colours.Add(color);
                colours.Add(color);
            }
            internal void Render(int x, int y)
            {
                if (meshChunks[x, y] != null)
                {
                    Graphics.DrawMesh(meshChunks[x, y], rootPosition, Quaternion.identity, material, renderLayer);
                }
            }

            private struct AtlasInfo
            {
                public Bits requiredConnections;
                public Bits forbiddenConnections;
                public Vector4 uvBox;
                public string name;
            }
        }
    }
}
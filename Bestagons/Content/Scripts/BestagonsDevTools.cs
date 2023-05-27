using Bestagons.Content.Map;
using Delaunay.Geo;
using ImGuiNET;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

namespace Bestagons.Content.Scripts
{
    public class BestagonsDevTools : DevTool
    {
        public BestagonsDevTools() 
        {
            RequiresGameRunning = true;
        }

        public List<GameObject> debugMeshes;
        public int radius = 1;
        public int density = 15;
        public float vscale = 10f;
        public float hscale = 10.3923045f;
        public float vOffset = 0;
        public float hOffset = 0;
        private bool infiniteMoneyDebug;

        public override void RenderTo(DevPanel panel)
        {
            if(ImGui.Button("Force generate active world"))
            {
                var test = new GameObject();
                var world = ClusterManager.Instance.activeWorld;
                test.transform.position = world.PosMin() + new Vector2(1, 1);
                test.SetActive(true);

                test.AddComponent<Bestagons_HexagonGrid>();
            }

            if(ImGui.Checkbox("Infinite currency", ref infiniteMoneyDebug))
                Bestagons_Mod.Instance.ToggleInfiniteMoney(infiniteMoneyDebug);

            ImGui.DragInt("Radius", ref radius);
            ImGui.DragInt("Density", ref density);

            if(ImGui.Button("DrawHexGrid"))
            {
                var controlGo = MakeMeshData(radius);
                controlGo.transform.position = Vector3.zero with { z = Grid.GetLayerZ(Grid.SceneLayer.SceneMAX) };
                controlGo.SetActive(true);

                if (debugMeshes != null)
                {
                    for (int i = 0; i < debugMeshes.Count; i++) 
                    {
                        Object.Destroy(debugMeshes[i]);
                    }
                }

                debugMeshes = new ();
                
                var world = ClusterManager.Instance.activeWorld;
                int xCount = Mathf.FloorToInt(world.Width / Bestagons_HexagonGrid.HEX_BOUNDS_WIDTH);
                int yCount = Mathf.FloorToInt(world.Height / Bestagons_HexagonGrid.HEX_BOUNDS_HEIGHT);
                var hexes = new List<AxialI>();

                for (int r = 0; r <= yCount; r++)
                {
                    int rOffset = Mathf.FloorToInt(r / 2);
                    for (int q = 0 - rOffset; q <= xCount - rOffset; q++)
                    {
                        hexes.Add(new AxialI(r, q));
                    }
                }

                foreach (var point in hexes) 
                {
                    var go = MakeMeshData(radius);
                    go.transform.position = (Vector3)point.ToWorld2D() with { z = Grid.GetLayerZ(Grid.SceneLayer.SceneMAX) };
                    go.SetActive(true);

                    debugMeshes.Add(go);
                }
            }

            ImGui.DragFloat("hori scale", ref hscale);
            ImGui.DragFloat("vert scale", ref vscale);
            ImGui.DragFloat("hori Offset", ref hOffset);
            ImGui.DragFloat("vert Offset", ref vOffset);

            if (ImGui.Button("DrawHexGrid2"))
            {
                if (debugMeshes != null)
                {
                    for (int i = 0; i < debugMeshes.Count; i++)
                    {
                        Object.Destroy(debugMeshes[i]);
                    }
                }

                debugMeshes = new();

                var world = ClusterManager.Instance.activeWorld;
                int xCount = Mathf.FloorToInt(world.Width / Bestagons_HexagonGrid.HEX_BOUNDS_WIDTH);
                int yCount = Mathf.FloorToInt(world.Height / Bestagons_HexagonGrid.HEX_BOUNDS_HEIGHT);
                var hexes = new List<HexCoord>();

                for (int r = 0; r <= yCount; r++)
                {
                    int rOffset = Mathf.FloorToInt(r / 2f);
                    for (int q = 0 - rOffset; q <= xCount - rOffset; q++)
                    {
                        HexCoord item = new HexCoord(q, r);
                        hexes.Add(item);
                    }
                }

                foreach (var point in hexes)
                {
                    var go = DrawDebugHexCenter(point , UnityEngine.Random.ColorHSV());
                    debugMeshes.Add(go);
                }
            }

        }
        public GameObject DrawDebugHexCenter(AxialI hex, UnityEngine.Color color)
        {
            var gameObject = new GameObject("Beached_DebugLineRenderer");

            gameObject.SetActive(true);

            var debugLineRenderer = gameObject.AddComponent<LineRenderer>();

            debugLineRenderer.material = new Material(Shader.Find("Sprites/Default"))
            {
                renderQueue = 3501
            };

            debugLineRenderer.startColor = debugLineRenderer.endColor = color;
            debugLineRenderer.startWidth = debugLineRenderer.endWidth = 0.05f;

            debugLineRenderer.GetComponent<LineRenderer>().material.renderQueue = RenderQueues.Liquid;

            debugLineRenderer.positionCount = 4;
            debugLineRenderer.loop = true;
            var position = hex.ToWorld();
            position *= new Vector2(hscale, vscale);

            var z = Grid.GetLayerZ(Grid.SceneLayer.SceneMAX);
            var radius = 0.5f;

            debugLineRenderer.SetPositions(new[] {
                new Vector3(position.x - radius, position.y - radius, z),
                new Vector3(position.x + radius, position.y - radius, z),
                new Vector3(position.x + radius, position.y + radius, z),
                new Vector3(position.x - radius, position.y + radius, z)
            });

            return gameObject;
        }

        public GameObject DrawDebugHexCenter(HexCoord hex, UnityEngine.Color color)
        {
            var gameObject = new GameObject("Beached_DebugLineRenderer");

            gameObject.SetActive(true);

            var debugLineRenderer = gameObject.AddComponent<LineRenderer>();

            debugLineRenderer.material = new Material(Shader.Find("Sprites/Default"))
            {
                renderQueue = 3501
            };

            debugLineRenderer.startColor = debugLineRenderer.endColor = color;
            debugLineRenderer.startWidth = debugLineRenderer.endWidth = 0.05f;

            debugLineRenderer.GetComponent<LineRenderer>().material.renderQueue = RenderQueues.Liquid;

            debugLineRenderer.positionCount = 4;
            debugLineRenderer.loop = true;
            var position = hex.Position();
            position *= new Vector2(hscale, vscale);

            var z = Grid.GetLayerZ(Grid.SceneLayer.SceneMAX);
            var radius = 0.5f;

            debugLineRenderer.SetPositions(new[] {
                new Vector3(position.x - radius, position.y - radius, z),
                new Vector3(position.x + radius, position.y - radius, z),
                new Vector3(position.x + radius, position.y + radius, z),
                new Vector3(position.x - radius, position.y + radius, z)
            });

            return gameObject;
        }

        public GameObject DrawDebugHex(HexCoord hex, UnityEngine.Color color)
        {
            var gameObject = new GameObject("Beached_DebugLineRenderer");

            gameObject.SetActive(true);

            var debugLineRenderer = gameObject.AddComponent<LineRenderer>();

            debugLineRenderer.material = new Material(Shader.Find("Sprites/Default"))
            {
                renderQueue = 3501
            };

            debugLineRenderer.startColor = debugLineRenderer.endColor = color;
            debugLineRenderer.startWidth = debugLineRenderer.endWidth = 0.05f;
            debugLineRenderer.positionCount = 6;
            debugLineRenderer.loop = true;
            debugLineRenderer.GetComponent<LineRenderer>().material.renderQueue = RenderQueues.Liquid;
            debugLineRenderer.SetPositions(hex.Corners3Offset().ToArray());

            return gameObject;
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

            material.SetColor("_Color", Random.ColorHSV() with { a = 0.3f });

            go.AddComponent<MeshRenderer>().material = material;

            return go;
        }
    }
}

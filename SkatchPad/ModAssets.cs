using SketchPad.Tools.Pencil;
using UnityEngine;

namespace SketchPad
{
    public class ModAssets
    {
        public static GameObject linePrefab;

        public static void CreateLinePrefab()
        {
            linePrefab = new GameObject("line");

            LineRenderer renderer = linePrefab.AddComponent<LineRenderer>();

            renderer.useWorldSpace = false;
            renderer.material = new Material(Shader.Find("Klei/Biome/Unlit Transparent"));
            renderer.startColor = renderer.endColor = Color.yellow;
            renderer.startWidth = renderer.endWidth = 0.07f;

            renderer.SetPositions(new Vector3[]
            {
                new Vector3(0, 0, 0),
                new Vector3(1, 0, 0)
            });

            linePrefab.AddComponent<SketchBookLine>();

            linePrefab.SetLayerRecursively(LayerMask.NameToLayer(nameof(SimDebugView)));
            linePrefab.SetActive(false);
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FUtility
{
    public class DebugHelp
    {
        // MeshRenderer componentInChildren = this.visualizer.GetComponentInChildren<MeshRenderer>();
        private static Canvas canvas;
        private static List<KBatchedAnimController> squares = new List<KBatchedAnimController>();

        public GameObject visualizer;
        public Grid.SceneLayer visualizerLayer = Grid.SceneLayer.Move;
        // Permanent for now
        public static void HighlightCell(int cell, Color color)
        {
#if DEBUG
            var square = FXHelpers.CreateEffect(
                "transferarmgrid_kanim", Grid.CellToPosCCC(cell, Grid.SceneLayer.FXFront), null, false, Grid.SceneLayer.FXFront);
            square.destroyOnAnimComplete = false;
            square.Play("grid_loop");
            square.TintColour = color;

            squares.Add(square);
#endif
        }

        public static void HighlightCell(Extents extents, Color color)
        {
#if DEBUG
            int attempts = 10;
            for (int x = 0; x < extents.x; x++)
            {
                for (int y = 0; x < extents.y; y++)
                {
                    HighlightCell(Grid.XYToCell(x, y), color);
                    if(attempts-- <= 0) break;
                }
            }
#endif
        }
        public static void DrawText(object msg, Vector3 position, Color color)
        {
            if (canvas == null)
                MakeCanvas();

            position.z = Grid.GetLayerZ(Grid.SceneLayer.FXFront);
            var label = new GameObject
            {
                layer = LayerMask.NameToLayer("UI")
            };

            RectTransform rectTransform = label.AddComponent<RectTransform>();
            rectTransform.SetParent(canvas.FindOrAddComponent<RectTransform>());
            rectTransform.localScale = new Vector3(0.02f, 0.02f, 1f);

            label.transform.SetPosition(position);

            var text = label.AddComponent<Text>();
            text.text = msg.ToString();
            text.font = global::Assets.DebugFont;
            text.color = color;
            text.fontSize = 10;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.alignment = TextAnchor.MiddleCenter;

            label.gameObject.SetActive(true);
            canvas.gameObject.SetActive(true);
            var cg = label.gameObject.AddComponent<CanvasGroup>();
            cg.blocksRaycasts = false;
            cg.interactable = false;

        }

        private static void MakeCanvas()
        {
            GameObject parent;
            parent = new GameObject
            {
                name = "FUDebugCanvas"
            };

            Object.DontDestroyOnLoad(parent);
            canvas = parent.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1;
            canvas.sortingOrder = 3000;
            parent.AddComponent<GraphicRaycaster>().enabled = false;
            var cg = parent.AddComponent<CanvasGroup>();
            cg.blocksRaycasts = false;
            //cg.interactable = false;
        }

        public static void DrawText(object msg, int cell, Color color)
        {
            var pos = Grid.CellToPos(cell) + new Vector3(0.5f, 0.5f, 0f);
            DrawText(msg, pos, color);
        }
    }
}

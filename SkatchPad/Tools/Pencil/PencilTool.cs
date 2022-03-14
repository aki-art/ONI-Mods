using FUtility;
using SketchPad.History;
using UnityEngine;

namespace SketchPad.Tools.Pencil
{
    public class PencilTool : InterfaceTool
    {
        public static PencilTool Instance;

        public SketchBookLine activeLine;

        public Vector3 initialState;
        public float smoothingLength = 2f;
        public int smoothingSections = 10;

        private Color color = Color.white;
        private float strokeWidth = 0.05f;

        private string debugColor = "FFFFFF";
        private float debugWidth = 0.05f;
        private Rect GUIRect = new Rect(10, 400, 200, 500);

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            viewMode = SketchOverlayMode.ID;
            Instance = this;
        }

        public static void DestroyInstance()
        {
            Instance = null;
        }

        public override void OnMouseMove(Vector3 cursor_pos)
        {
            if (activeLine != null)
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos());
                activeLine.AddPoint(pos);
                activeLine.Smooth(smoothingSections);
            }
        }

        protected override void OnActivateTool()
        {
            activeLine = null;
            PlayerController.Instance.ActivateTool(this);

            base.OnActivateTool();
        }

        public override void OnLeftClickDown(Vector3 cursor_pos)
        {
#if DEBUG
            // ignore the GUI box
            var mouse = KInputManager.GetMousePos();
            if (mouse.x > GUIRect.xMin && mouse.x < 400 && mouse.y > GUIRect.yMin && mouse.y < GUIRect.yMax) return;
#endif

            var drawCommand = new DrawCommand(strokeWidth, color);
            EditHistory.Instance.AddCommand(drawCommand);

            activeLine = drawCommand.Stroke;
        }

        public override void OnLeftClickUp(Vector3 cursor_pos)
        {
            activeLine = null;
        }

#if DEBUG
#pragma warning disable IDE0051 // Remove unused private members
        private void OnGUI()
        {
            GUILayout.BeginArea(GUIRect);

            GUILayout.Label("Color");
            debugColor = GUILayout.TextField(debugColor, 25);

            GUILayout.Label($"Brush Size {debugWidth}");
            debugWidth = GUILayout.HorizontalSlider(debugWidth, 0, 0.1f);

            if (GUILayout.Button("Change Brush"))
            {
                color = Util.ColorFromHex(debugColor);
                strokeWidth = debugWidth;
            }

            GUILayout.Space(20);

            GUILayout.Label("Bezier Settings");

            GUILayout.Label($"Subdivision Count {smoothingSections}");
            smoothingSections = (int)GUILayout.HorizontalSlider(smoothingSections, 1, 20);

            GUILayout.EndArea();
        }
#pragma warning restore IDE0051 // Remove unused private members
#endif
    }
}

using System.Collections.Generic;
using UnityEngine;
using ZipLine.Content.Tools;

namespace ZipLine
{
    public class Helper
    {
        public static void DrawLine(Vector3 position1, Vector3 position2, Color color, float scale = 1f)
        {
            Color color2 = Gizmos.color;
            Gizmos.color = color;
            Gizmos.DrawLine(position1, position2);
            Gizmos.color = color2;
        }

        public static void SetPriority(GameObject gameObject)
        {
            if (gameObject == null)
            {
                return;
            }

            if (gameObject.TryGetComponent(out Prioritizable prioritizable))
            {
                if (BuildMenu.Instance != null)
                {
                    prioritizable.SetMasterPriority(BuildMenu.Instance.GetBuildingPriority());
                }

                if (PlanScreen.Instance != null)
                {
                    prioritizable.SetMasterPriority(PlanScreen.Instance.GetBuildingPriority());
                }
            }
        }

        // very basic desaturation, that just lerps between the color and it's greyscale counterpart
        public static Color Desaturate(Color color, float power)
        {
            var L = 0.3f * color.r + 0.6f * color.g + 0.1f * color.b;

            return new Color(
                color.r + power * (L - color.r),
                color.g + power * (L - color.g),
                color.b + power * (L - color.b));
        }

        public static bool IsInstantBuilding()
        {
            return DebugHandler.InstantBuildMode || (Game.Instance.SandboxModeActive && SandboxToolParameterMenu.instance.settings.InstantBuild);
        }

        public static void DigCells(List<ZipConnectorTool.OffsetInfo> cells)
        {
            if (IsInstantBuilding())
            {
                foreach (var cell in cells)
                {
                    var offset_cell = Grid.PosToCell(cell.position);

                    if (Grid.IsValidCell(offset_cell) && Grid.Solid[offset_cell] && !Grid.Foundation[offset_cell])
                    {
                        WorldDamage.Instance.DestroyCell(offset_cell);
                        continue;
                    }
                }

                return;
            }

            foreach (var offsetInfo in cells)
            {
                var cell = Grid.PosToCell(offsetInfo.position);

                var digGo = DigTool.PlaceDig(cell, Mathf.FloorToInt(offsetInfo.distance));
                if (digGo != null)
                {
                    if (digGo.TryGetComponent(out Prioritizable prioritizable))
                    {
                        prioritizable.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
                    }
                }
            }
        }
    }
}

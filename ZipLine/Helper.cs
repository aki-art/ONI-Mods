using System;
using System.Collections.Generic;
using UnityEngine;
using ZipLine.Tools;

namespace ZipLine
{
    public class Helper
    {
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

            foreach(var offsetInfo in cells)
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

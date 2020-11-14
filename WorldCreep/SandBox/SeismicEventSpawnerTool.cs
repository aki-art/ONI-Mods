using System.Collections.Generic;
using UnityEngine;
using WorldCreep.WorldEvents;

namespace WorldCreep.SandBox
{
    public class SeismicEventSpawnerTool : BrushTool
    {
        public static SeismicEventSpawnerTool instance;
        protected HashSet<int> recentlyAffectedCells = new HashSet<int>();
        private Dictionary<int, Color> recentAffectedCellColor = new Dictionary<int, Color>(); 

        private SandboxSettings Settings => SandboxToolParameterMenu.instance.settings;

        public static void DestroyInstance() => instance = null;

        protected override void OnPrefabInit()
        {
            name = nameof(SeismicEventSpawnerTool);
            base.OnPrefabInit();
            instance = this;
            Debug.Log("PREFABNAME" + name);
        }

        public void Activate() => PlayerController.Instance.ActivateTool(this);

        protected override void OnActivateTool()
        {
            base.OnActivateTool();
            SandboxToolParameterMenu.instance.gameObject.SetActive(true);
            SandboxToolParameterMenu.instance.DisableParameters();
            SandboxToolParameterMenu.instance.brushRadiusSlider.row.SetActive(true);
            SandboxToolParameterMenu.instance.massSlider.row.SetActive(true);
            SandboxToolParameterMenu.instance.entitySelector.row.SetActive(true);
        }

        protected override void OnDeactivateTool(InterfaceTool new_tool)
        {
            base.OnDeactivateTool(new_tool);
            SandboxToolParameterMenu.instance.gameObject.SetActive(false);
        }

        public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
        {
            colors = new HashSet<ToolMenu.CellColorData>();
            foreach (int recentlyAffectedCell in recentlyAffectedCells)
            {
                Color color = new Color(recentAffectedCellColor[recentlyAffectedCell].r, recentAffectedCellColor[recentlyAffectedCell].g, recentAffectedCellColor[recentlyAffectedCell].b, MathUtil.ReRange(Mathf.Sin(Time.realtimeSinceStartup * 10f), -1f, 1f, 0.1f, 0.2f));
                colors.Add(new ToolMenu.CellColorData(recentlyAffectedCell, color));
            }
            foreach (int cellsInRadiu in cellsInRadius)
                colors.Add(new ToolMenu.CellColorData(cellsInRadiu, radiusIndicatorColor));
        }

        public override void SetBrushSize(int radius)
        {
            brushRadius = radius;
            brushOffsets.Clear();
            for (int index1 = 0; index1 < brushRadius * 2; ++index1)
            {
                for (int index2 = 0; index2 < brushRadius * 2; ++index2)
                {
                    if (Vector2.Distance(new Vector2(index1, index2), new Vector2(brushRadius, brushRadius)) < brushRadius - 0.800000011920929)
                        brushOffsets.Add(new Vector2(index1 - brushRadius, index2 - brushRadius));
                }
            }
        }

        public override void OnLeftClickDown(Vector3 cursor_pos)
        {
            base.OnLeftClickDown(cursor_pos);
            WorldEvent earthquake = FUtility.Utils.Spawn(EarthQuakeConfig.ID, cursor_pos).GetComponent<EarthQuake>(); //WorldEventScheduler.Instance.CreateEvent(EarthQuakeConfig.ID, Grid.CellToPos(cell), Settings.GetFloatSetting("SandboxTools.Mass"));

            earthquake.randomize = false; 
            earthquake.Power = Settings.GetFloatSetting("SandboxTools.Mass");
            earthquake.radius = brushRadius;
            //earthquake.affectedCells = recentlyAffectedCells;
        }
/*        protected override void OnPaintCell(int cell, int distFromOrigin)
        {
            base.OnPaintCell(cell, distFromOrigin);

            recentlyAffectedCells.Add(cell);
            if (!recentAffectedCellColor.ContainsKey(cell))
                recentAffectedCellColor.Add(cell, Color.red);
            else
                recentAffectedCellColor[cell] = Color.red;

            int index1 = Game.Instance.callbackManager.Add(new Game.CallbackInfo(() =>
            {
                recentlyAffectedCells.Remove(cell);
                recentAffectedCellColor.Remove(cell);
            })).index;
            int callbackIdx = index1;

            //SimMessages.ReplaceElement(cell, (SimHashes)id, sandBoxTool, (float)floatSetting1, (float)floatSetting2, (byte)index2, intSetting, callbackIdx);
        }*/
    }
}

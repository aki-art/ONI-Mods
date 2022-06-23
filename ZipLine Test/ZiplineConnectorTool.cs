using FUtility;
using HarmonyLib;
using UnityEngine;

namespace ZiplineTest
{
    public class ZiplineConnectorTool : InterfaceTool
    {
        public static ZiplineConnectorTool Instance;
        public ZiplineAnchor selected;

        [HarmonyPatch(typeof(Game), "OnPrefabInit")]
        public class Game_OnPrefabInit_Patch
        {
            public static void Postfix()
            {
                Game.Instance.gameObject.AddComponent<ZiplineConnectorTool>();
            }
        }

        public static void DestroyInstance()
        {
            Instance = null;
        }

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Instance = this;

            var defaultLayerMask = 1 | LayerMask.GetMask(new[]
            {
                        "World",
                        "Pickupable",
                        "Place",
                        "PlaceWithDepth",
                        "BlockSelection",
                        "Construction",
                        "Selection"
            });

            populateHitsList = true;
            layerMask = defaultLayerMask;
        }

        public void Activate()
        {
            PlayerController.Instance.ActivateTool(this);
            ToolMenu.Instance.PriorityScreen.ResetPriority();
        }

        public void Select(ZiplineAnchor obj)
        {
            Log.Debuglog("SELECTED:" + obj?.name);
            selected = obj;

            DeactivateTool(null);
        }

        public override void OnLeftClickDown(Vector3 cursor_pos)
        {
            var objectUnderCursor = GetObjectUnderCursor<ZiplineAnchor>(true, s => s.GetComponent<KSelectable>().IsSelectable, selected);
            //this.selectedCell = Grid.PosToCell(cursor_pos);
            Select(objectUnderCursor);
        }

        protected override void OnDeactivateTool(InterfaceTool new_tool)
        {
            //UnityEngine.Object.Destroy(this.visualizer);
            base.OnDeactivateTool(new_tool);
        }

        public void Deactivate()
        {
            SelectTool.Instance.Activate();
        }
    }
}

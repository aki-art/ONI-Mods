using ExtensionCords.Buildings;
using FUtility;
using HarmonyLib;
using KSerialization;
using System.Collections.Generic;

namespace ExtensionCords
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class ReelTool : BuildTool
    {
        public static new ReelTool Instance;

        // cell of the sender reel block
        public PlaceableOutlet parent;

        protected override void OnPrefabInit()
        {
            var t = Traverse.Create(this);

            t.Field("tooltip").SetValue(GetComponent<ToolTip>());
            t.Field("buildingCount").SetValue(UnityEngine.Random.Range(1, 14));

            canChangeDragAxis = false;
            Instance = this;
        }

        public void Activate(BuildingDef def, IList<Tag> selected_elements, PlaceableOutlet parent)
        {
            this.parent = parent;
            Activate(def, selected_elements);
        }

        protected override void OnDeactivateTool(InterfaceTool new_tool)
        {
            base.OnDeactivateTool(new_tool);
            parent = null;
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
        }

        public static new void DestroyInstance()
        {
            Instance = null;
        }
    }
}

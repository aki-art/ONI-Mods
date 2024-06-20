using Backwalls.Cmps;
using HarmonyLib;
using UnityEngine;

namespace Backwalls.Patches
{
	public class BuildToolPatch
	{
		[HarmonyPatch(typeof(BuildTool), "OnDeactivateTool")]
		public class BuildTool_OnDeactivateTool_Patch
		{
			public static void Postfix() => Backwalls_Mod.Instance.ClearCopyBuilding();
		}

		[HarmonyPatch(typeof(BuildTool), "UpdateVis")]
		public class BuildTool_UpdateVis_Patch
		{
			public static void Prefix(ref Vector3 pos, BuildingDef ___def)
			{
				if (___def.ObjectLayer == ObjectLayer.Backwall)
					Backwalls_SmartBuildTool.Instance.OnUpdateVis(ref pos);
			}

			public static void Postfix(BuildingDef ___def, GameObject ___visualizer)
			{
				Backwalls_SmartBuildTool.Instance.UpdateVisColor(___def, ___visualizer);
			}
		}

		[HarmonyPatch(typeof(BuildTool), "OnDragTool")]
		public class BuildTool_OnDragTool_Patch
		{
			public static void Prefix(BuildingDef ___def, ref int cell, ref int ___lastDragCell)
			{
				Log.Debug("DRAG");
				Backwalls_SmartBuildTool.Instance.OnDragTool(___def, ref cell, ref ___lastDragCell);
			}
		}


		[HarmonyPatch(typeof(BuildTool), "OnLeftClickUp")]
		public class BuildTool_OnLeftClickUp_Patch
		{
			public static void Pretfix(BuildingDef ___def, ref int ___lastDragCell)
			{
				Backwalls_SmartBuildTool.Instance.OnLeftClickUp(___def, ref ___lastDragCell);
			}
		}
	}
}

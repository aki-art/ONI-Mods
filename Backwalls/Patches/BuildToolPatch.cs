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
			public static void Prefix(BuildTool __instance, ref Vector3 pos)
			{
				// the game crashes if this is null, and patching the method seems to increase the chance of it happening
				if (__instance.visualizer == null)
				{
					Log.Debug("vis is null");
					return;
				}

				if (__instance.def.ObjectLayer == ObjectLayer.Backwall)
					Backwalls_SmartBuildTool.Instance.OnUpdateVis(ref pos);
			}

			public static void Postfix(BuildTool __instance)
			{
				Backwalls_SmartBuildTool.Instance.UpdateVisColor(__instance.def, __instance.visualizer);
			}
		}

		[HarmonyPatch(typeof(BuildTool), "OnDragTool")]
		public class BuildTool_OnDragTool_Patch
		{
			public static void Prefix(BuildingDef ___def, ref int cell, ref int ___lastDragCell)
			{
				Backwalls_SmartBuildTool.Instance.OnDragTool(___def, ref cell, ref ___lastDragCell);
			}
		}


		[HarmonyPatch(typeof(BuildTool), "OnKeyDown")]
		public class BuildTool_OnKeyDown_Patch
		{
			public static void Prefix(KButtonEvent e)
			{
				if (e.TryConsume(BWActions.SmartBuildAction.GetKAction()))
					Backwalls_SmartBuildTool.Instance.Toggle();
			}
		}

		[HarmonyPatch(typeof(BuildTool), "OnLeftClickUp")]
		public class BuildTool_OnLeftClickUp_Patch
		{
			public static void Prefix(BuildTool __instance)
			{
				//if (__instance is BuildTool buildTool)
				//{
				Backwalls_SmartBuildTool.Instance.OnLeftClickUp(__instance.def, ref __instance.lastDragCell);
				//}
			}
		}
	}
}

using Backwalls.Cmps;
using HarmonyLib;

namespace Backwalls.Patches
{
	public class BuildToolPatch
	{
		[HarmonyPatch(typeof(BuildTool), "OnDeactivateTool")]
		public class BuildTool_OnDeactivateTool_Patch
		{
			public static void Postfix() => Backwalls_Mod.Instance.ClearCopyBuilding();
		}
	}
}

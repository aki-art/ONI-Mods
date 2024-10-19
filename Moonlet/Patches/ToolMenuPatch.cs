using HarmonyLib;
using Moonlet.Scripts.Tools;

namespace Moonlet.Patches
{
	public class ToolMenuPatch
	{
		[HarmonyPatch(typeof(ToolMenu), "CreateSandBoxTools")]
		public class ToolMenu_CreateSandBoxTools_Patch
		{
			public static void Postfix(ToolMenu __instance)
			{
				__instance.sandboxTools.Add(ToolMenu.CreateToolCollection(
					"Zone Types",
					"brush",
					Action.SandboxStressTool,
					nameof(Moonlet_ZonetypePainterTool), // must match
					"",
					false));
			}
		}
	}
}

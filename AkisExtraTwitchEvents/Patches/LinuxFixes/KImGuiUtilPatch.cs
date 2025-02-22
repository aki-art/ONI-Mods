using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Twitchery.Patches.LinuxFixes
{
	public class KImGuiUtilPatch
	{
		// https://github.com/asquared31415/ONITwitch/blob/main/ONITwitchCore/Patches/DevToolPatches.cs#L76C1-L85C3
		[HarmonyPatch(typeof(KImGuiUtil), nameof(KImGuiUtil.SetKAssertCB))]
		public static class ImGui_Patch
		{
			private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> orig)
			{
				return [new CodeInstruction(OpCodes.Ret)];
			}
		}
	}
}

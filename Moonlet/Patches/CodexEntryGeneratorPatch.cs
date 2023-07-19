using HarmonyLib;
using System.Collections.Generic;

namespace Moonlet.Patches
{
	public class CodexEntryGeneratorPatch
	{
		[HarmonyPatch(typeof(CodexEntryGenerator), "GenerateFoodEntries")]
		public class CodexEntryGenerator_GenerateFoodEntries_Patch
		{
			public static void Postfix(ref Dictionary<string, CodexEntry> __result)
			{
				if (Mod.sharedElementsLoader.edibleElements == null)
					return;

				foreach (var element in Mod.sharedElementsLoader.edibleElements)
					__result.Remove(element);
			}
		}
	}
}

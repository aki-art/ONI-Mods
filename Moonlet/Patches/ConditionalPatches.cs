using HarmonyLib;

namespace Moonlet.Patches
{
	public class ConditionalPatches
	{
		public static void PatchElements(Harmony harmony)
		{
			ElementLoaderPatch.ElementLoader_CollectElementsFromYAML_Patch.Patch(harmony);
			ElementLoaderPatch.ElementLoader_Load_Patch.Patch(harmony);
		}

		public static void PatchEnums(Harmony harmony)
		{
			EnumPatch.SimHashes_Parse_Patch.Patch(harmony);
			EnumPatch.SimHashes_ToString_Patch.Patch(harmony);
		}
	}
}

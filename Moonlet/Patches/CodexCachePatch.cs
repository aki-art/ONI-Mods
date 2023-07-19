using HarmonyLib;

namespace Moonlet.Patches
{
	public class CodexCachePatch
	{
		[HarmonyPatch(typeof(CodexCache), "AddEntry")]
		public class CodexCache_AddEntry_Patch
		{
			public static bool Prefix(string id, CodexEntry entry)
			{
				if (Mod.sharedElementsLoader.edibleElements == null)
					return true;

				// skip adding to foods it would be a duplicate entry with element)
				return entry.parentId != CodexEntryGenerator.FOOD_CATEGORY_ID
					|| !Mod.sharedElementsLoader.edibleElements.Contains(id);
			}
		}
	}
}

using HarmonyLib;

namespace Twitchery.Patches
{
	public class CodexEntryGenerator_Creatures_Patch
	{
		[HarmonyPatch(typeof(CodexEntryGenerator_Creatures), "GenerateCritterEntry")]
		public class CodexEntryGenerator_Creatures_GenerateCritterEntry_Patch
		{
			// need to remove the _-s and move the same entries under the fixed ID
			// https://forums.kleientertainment.com/klei-bug-tracker/oni/codex-critter-entry-generators-ignore-_-s-but-sidescreen-links-dont-r46250/

			public static void Postfix(ref CodexEntry __result)
			{
				foreach (var item in __result.subEntries)
				{
					var oldId = item.id;

					if (!oldId.StartsWith("AkisExtraTwitchEvents_"))
						continue;

					var correctId = CodexCache.FormatLinkID(item.id);
					CodexCache.AddSubEntry(correctId, item);
					item.id = correctId;
					CodexCache.subEntries.Remove(oldId);
				}
			}
		}
	}
}

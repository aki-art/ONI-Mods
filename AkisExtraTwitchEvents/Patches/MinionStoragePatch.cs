namespace Twitchery.Patches
{
	public class MinionStoragePatch
	{
		/*		[HarmonyPatch(typeof(MinionStorage), "GetStoredMinionInfo")]
				public class MinionStorage_GetStoredMinionInfo_Patch
				{
					public static bool Prefix(MinionStorage __instance, ref List<MinionStorage.Info> __result)
					{
						if (__instance.HasTag(TTags.hideDeadDupesWithin) && __instance.serializedMinions != null)
						{
							__result = __instance.serializedMinions
								.Where(minion => !AETE_GraveStoneMinionStorage.storedMinionGUIDs.Contains(minion.id))
								.ToList();

							return false;
						}

						return true;
					}
				}*/
	}
}

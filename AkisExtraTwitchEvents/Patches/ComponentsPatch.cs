namespace Twitchery.Patches
{
	public class ComponentsPatch
	{
		/*		[HarmonyPatch(typeof(Components.Cmps<MinionAssignablesProxy>))]
				[HarmonyPatch("Items", MethodType.Getter)]
				public class Components_Items_Patch
				{
					public static void Postfix(List<MinionAssignablesProxy> __result)
					{
						if (__result == null || __result.Count == 0)
						{
							return;
						}

						__result = __result.Where(item => !item.HasTag(GameTags.Dead)).ToList();
					}
				}*/
	}
}

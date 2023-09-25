using HarmonyLib;

namespace Twitchery.Patches
{
	public class MinionStoragePatch
	{
		// hacky temporary fix for https://forums.kleientertainment.com/klei-bug-tracker/oni/false-atmosuit-worn-by-dupes-who-were-stored-in-a-rocket-module-r42327/
		[HarmonyPatch(typeof(MinionIdentity), "OnSpawn")]
		public class MinionIdentity_OnSpawn_Patch
		{
			public static void Postfix(MinionIdentity __instance)
			{
				if (__instance.assignableProxy == null)
					MinionAssignablesProxy.InitAssignableProxy(__instance.assignableProxy, __instance);

				var suit = __instance.GetEquipment()?.GetSlot(Db.Get().AssignableSlots.Suit);

				var hasSuit = suit != null && suit.assignable != null;

				// if this dupe has no suit, disable all suit related skins
				if (!hasSuit && __instance.TryGetComponent(out WearableAccessorizer wearableAccessorizer))
				{
					var kbac = __instance.GetComponent<KBatchedAnimController>();
					kbac.SetSymbolVisiblity("snapTo_neck", false);

					var component = __instance.GetComponent<SymbolOverrideController>();

					if (wearableAccessorizer.wearables.TryGetValue(WearableAccessorizer.WearableType.CustomSuit, out var suits))
					{
						foreach (var buildAnim in suits.BuildAnims)
						{
							var build = buildAnim.GetData().build;
							if (build != null)
							{
								foreach (var symbol in build.symbols)
								{
									var symbolName = HashCache.Get().Get(symbol.hash);
									component.RemoveSymbolOverride((HashedString)symbolName, suits.buildOverridePriority);
								}
							}
						}
					}
				}
			}
		}
	}
}

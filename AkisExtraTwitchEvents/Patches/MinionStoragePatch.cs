using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using Twitchery.Content;
using Twitchery.Content.Scripts;

namespace Twitchery.Patches
{
	public class MinionStoragePatch
	{

		[HarmonyPatch(typeof(MinionStorage), "GetStoredMinionInfo")]
		public class MinionStorage_GetStoredMinionInfo_Patch
		{
			public static bool Prefix(MinionStorage __instance, ref List<MinionStorage.Info> __result)
			{
				__result = __instance.serializedMinions
					//.Where(minion => !minion.serializedMinion.Get().HasTag(GameTags.Dead))
					.Where(minion => !AETE_GraveStoneMinionStorage.storedMinionGUIDs.Contains(minion.id))
					.ToList();

				return false;

				if (__instance.HasTag(TTags.hideDeadDupesWithin) && __instance.serializedMinions != null)
				{
					__result = __instance.serializedMinions
						.Where(minion => !(bool)minion.serializedMinion?.Get()?.HasTag(GameTags.Dead))
						.ToList();

					return false;
				}

				return true;
			}
		}

		// hacky temporary fix for https://forums.kleientertainment.com/klei-bug-tracker/oni/false-atmosuit-worn-by-dupes-who-were-stored-in-a-rocket-module-r42327/
		[HarmonyPatch(typeof(MinionIdentity), "OnSpawn")]
		public class MinionIdentity_OnSpawn_Patch
		{
			public static void Postfix(MinionIdentity __instance)
			{
				if (__instance.assignableProxy == null)
					return;

				var equipment = __instance.GetEquipment();

				if (equipment == null) return;

				var suit = equipment.GetSlot(Db.Get().AssignableSlots.Suit);

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

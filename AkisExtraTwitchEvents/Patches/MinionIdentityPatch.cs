using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Twitchery.Content;

namespace Twitchery.Patches
{
	public class MinionIdentityPatch
	{

		[HarmonyPatch(typeof(StoredMinionIdentity), "CleanupLimboMinions")]
		public class StoredMinionIdentity_CleanupLimboMinions_Patch
		{
			public static bool Prefix(StoredMinionIdentity __instance)
			{
				if (__instance == null)
				{
					return false;
				}

				return true;
			}
		}

		[HarmonyPatch(typeof(MinionIdentity), "OnSpawn")]
		public class MinionIdentity_OnSpawn_Patch
		{
			public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> orig)
			{
				var codes = orig.ToList();
				var index = codes.FindIndex(c => c.opcode == OpCodes.Ldstr && c.operand is string str && str == "head_swap_kanim");

				if (index == -1)
					return codes;

				var m_RemapSymbolName = AccessTools.Method(
					typeof(MinionIdentity_OnSpawn_Patch),
					nameof(RemapAnimFileName),
					new[]
					{
						typeof(string),
						typeof(MinionIdentity)
					});

				codes.InsertRange(index + 1, new[]
				{
					// string on stack
					new CodeInstruction(OpCodes.Ldarg_0),
					new CodeInstruction(OpCodes.Call, m_RemapSymbolName)
				});

				return codes;
			}

			private static string RemapAnimFileName(string originalKanimFile, MinionIdentity identity)
			{
				if (identity != null
					&& TPersonalities.headKanims.TryGetValue(identity.personalityResourceId, out var anim)
					&& anim != null)
				{
					return anim;
				}

				return originalKanimFile;
			}

			// hacky temporary fix for https://forums.kleientertainment.com/klei-bug-tracker/oni/false-atmosuit-worn-by-dupes-who-were-stored-in-a-rocket-module-r42327/
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

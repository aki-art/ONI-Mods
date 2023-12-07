using FUtility;
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
				Log.Debug("TAGS " + __instance.nameStringKey);
				Log.Debug(__instance.GetComponent<KPrefabID>().tags.Join());
				if (__instance)
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
		}
	}
}

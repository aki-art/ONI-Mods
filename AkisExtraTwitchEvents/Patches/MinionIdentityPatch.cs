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
		[HarmonyPatch(typeof(MinionIdentity), "OnSpawn")]
		public class MinionIdentity_OnSpawn_Patch
		{
			public static void Prefix(MinionIdentity __instance)
			{
				var accessorizer = __instance.GetComponent<Accessorizer>();

				Log.Debuglog("Accessories for: " + __instance.name);

				foreach (var accessories in accessorizer.accessories)
				{
					var a = accessories.Get();
					Log.Debuglog(a == null ? "null" : a.Id);
				}

				var accessory = accessorizer.GetAccessory(Db.Get().AccessorySlots.HeadShape);
				if (accessory == null) Log.Warning("accessory is null");
				if (accessory.symbol == null) Log.Warning("accessory.symbol is null");

				var symbolName = HashCache.Get().Get(accessory.symbol.hash);
				Log.Debuglog("SYMBOL NAME: " + symbolName);

				string text = HashCache
					.Get()
					.Get(accessory.symbol.hash)
					.Replace("headshape", "cheek");
				Log.Debuglog("REPLCAED NAME: " + text);
			}

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
				Log.Debuglog("patching");
				Log.Assert("TPersonalities.headKanims", TPersonalities.headKanims);

				if (identity != null
					&& TPersonalities.headKanims.TryGetValue(identity.personalityResourceId, out var anim)
					&& anim != null)
				{
					Log.Debuglog("swapped head anim: " + anim);
					return anim;
				}

				return originalKanimFile;
			}
		}
	}
}

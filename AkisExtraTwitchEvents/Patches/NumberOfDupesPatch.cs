using Database;
using FUtility;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace Twitchery.Patches
{
	public class NumberOfDupesPatch
	{
		[HarmonyPatch(typeof(NumberOfDupes), "Success")]
		public class NumberOfDupes_Success_Patch
		{
			public static void Postfix(ref bool __result, NumberOfDupes __instance)
			{
				__result &= (Components.LiveMinionIdentities.Items.Count - Mod.doubledDupe.Count) >= __instance.numDupes;
			}
		}

		[HarmonyPatch(typeof(NumberOfDupes), "GetProgress")]
		public class NumberOfDupes_GetProgress_Patch
		{
			public static IEnumerable<CodeInstruction> Transpiler(ILGenerator _, IEnumerable<CodeInstruction> orig)
			{
				var codes = orig.ToList();

				var m_Count = AccessTools.PropertyGetter(typeof(List<MinionIdentity>), "Count");

				var index = codes.FindIndex(ci => ci.Calls(m_Count));

				if (index == -1)
				{
					Log.Warning($"Could not patch {nameof(NumberOfDupes)} GetProgress.");
					return codes;
				}

				var m_InjectedMethod = AccessTools.DeclaredMethod(typeof(NumberOfDupes_GetProgress_Patch), nameof(InjectedMethod));

				// inject right after the found index
				codes.InsertRange(index + 1, new[]
				{
					new CodeInstruction(OpCodes.Call, m_InjectedMethod)
				});

				return codes;
			}

			private static int InjectedMethod(int count) => count - Mod.doubledDupe.Count;
		}
	}
}

using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace PrintingPodRecharge.Patches
{
	public class MinionInstanceTargetPatch
	{
/*		[HarmonyPatch(typeof(MinionBrowserScreen.GridItem.MinionInstanceTarget), MethodType.Constructor)]
		public class MinionInstanceTarget_Ctor_Patch
		{
			public static void Prefix(MinionBrowserScreen.GridItem.MinionInstanceTarget __instance)
			{
				if (__instance.personality == null)
				{
					Log.Warning("MinionBrowserScreen.GridItem.MinionInstanceTarget personality was null " + __instance.GetName());
					__instance.personality = Db.Get().Personalities.GetRandom(true, false);
				}
			}
		}*/

		// temporary bandaid fix
		[HarmonyPatch(typeof(MinionBrowserScreen.GridItem), "Of", typeof(GameObject))]
		public class MinionBrowserScreen_GridItem_Of_Patch
		{
			public static IEnumerable<CodeInstruction> Transpiler(ILGenerator _, IEnumerable<CodeInstruction> orig)
			{
				var codes = orig.ToList();

				var p_personalityResourceId = typeof(MinionIdentity).GetProperty("personalityResourceId").GetGetMethod();
				var index = codes.FindIndex(ci => ci.Calls(p_personalityResourceId));

				if (index == -1)
					return codes;

				var m_InjectedMethod = AccessTools.DeclaredMethod(typeof(MinionBrowserScreen_GridItem_Of_Patch), nameof(ValidatePersonalityId));

				codes.Insert(index + 1, new CodeInstruction(OpCodes.Call, m_InjectedMethod));

				return codes;
			}

			private static HashedString ValidatePersonalityId(HashedString personalityId)
			{
				var personalities = Db.Get().Personalities;
				return personalities.TryGet(personalityId) == null
					? personalities.GetRandom(true, false).IdHash
					: personalityId;
			}
		}
	}
}

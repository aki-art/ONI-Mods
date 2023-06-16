using FUtility;
using HarmonyLib;
using UnityEngine;

namespace PrintingPodRecharge.Patches
{
	public class MinionInstanceTargetPatch
	{
		[HarmonyPatch(typeof(MinionBrowserScreen.GridItem.MinionInstanceTarget), MethodType.Constructor)]
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
		}

		// temporary fix that is very unfriendly to other mods
		[HarmonyPatch(typeof(MinionBrowserScreen.GridItem), "Of")]
		public class MinionBrowserScreen_GridItem_Of_Patch
		{
			public static bool Prefix(ref MinionBrowserScreen.GridItem.MinionInstanceTarget __result, GameObject minionInstance)
			{
				var component = minionInstance.GetComponent<MinionIdentity>();
				var personalities = Db.Get().Personalities;

				var personality = personalities.TryGet(component.personalityResourceId)
					?? personalities.GetRandom(true, false);

				__result = new MinionBrowserScreen.GridItem.MinionInstanceTarget()
				{
					minionInstance = minionInstance,
					minionIdentity = component,
					personality = personality
				};

				return false;
			}
		}
	}
}

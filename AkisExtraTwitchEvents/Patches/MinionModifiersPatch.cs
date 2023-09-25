using HarmonyLib;
using Klei.AI;
using Twitchery.Content;

namespace Twitchery.Patches
{
	// Do not trigger mourning for double troubles
	public class MinionModifiersPatch
	{
		[HarmonyPatch(typeof(MinionModifiers), "OnDeath")]
		public class MinionModifiers_OnDeath_Patch
		{
			public static bool Prefix(MinionModifiers __instance)
			{
				if(__instance.TryGetComponent(out Effects effects))
					return !effects.HasEffect(TEffects.DOUBLETROUBLE);

				return true;
			}
		}
	}
}

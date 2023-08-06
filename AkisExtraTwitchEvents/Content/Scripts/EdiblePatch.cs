using HarmonyLib;

namespace Twitchery.Content.Scripts
{
	public class EdiblePatch
	{
		[HarmonyPatch(typeof(Edible), "ApplySpiceEffects")]
		public class Edible_ApplySpiceEffects_Patch
		{
			public static void Postfix(Edible __instance, SpiceInstance spice)
			{
/*				if (spice.Id == TSpices.goldFlake.Id && __instance.TryGetComponent(out AETE_GoldFlakeable flakeable))
					flakeable.Foil();*/
			}
		}
	}
}

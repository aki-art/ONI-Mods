using Database;
using HarmonyLib;

namespace Patches
{
	public class SpicesPatch
	{
		[HarmonyPatch(typeof(Spices), MethodType.Constructor, [typeof(ResourceSet)])]
		public class Spices_Ctor_Patch
		{
			public static void Postfix(Spices __instance)
			{
				__instance.StrengthSpice.Image = "truespices_brawny";
				__instance.PilotingSpice.Image = "truespices_rocketeer";
				__instance.PreservingSpice.Image = "truespices_freshener";
				__instance.MachinerySpice.Image = "truespices_machinist";
			}
		}
	}
}

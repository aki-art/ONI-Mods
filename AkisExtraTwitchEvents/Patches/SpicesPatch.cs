using Database;
using HarmonyLib;
using Twitchery.Content;

namespace Twitchery.Patches
{
	public class SpicesPatch
	{
		[HarmonyPatch(typeof(Spices), MethodType.Constructor, typeof(ResourceSet))]
		public class Spices_TargetMethod_Patch
		{
			public static void Postfix(Spices __instance)
			{
				//TSpices.Register(__instance);
			}
		}
	}
}

using Database;
using HarmonyLib;

namespace Moonlet.Patches.Database
{
	public class SpicesPatch
	{
		[HarmonyPatch(typeof(Spices), MethodType.Constructor, [typeof(ResourceSet)])]
		public class Spices_Ctor
		{
			public static void Postfix(Spices __instance)
			{
				Mod.spicesLoader.ApplyToActiveTemplates(item => item.LoadContent(__instance));
			}
		}
	}
}

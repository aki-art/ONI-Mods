using Database;
using HarmonyLib;
using Twitchery.Content;

namespace Twitchery.Patches
{
	public class OrbitalCategoryTypesPatch
	{
		[HarmonyPatch(typeof(OrbitalTypeCategories), MethodType.Constructor, [typeof(ResourceSet)])]
		public class OrbitalTypeCategories_Ctor_Patch
		{
			public static void Postfix(OrbitalTypeCategories __instance)
			{
				TOrbitalTypeCategories.Register(__instance);
			}
		}
	}
}

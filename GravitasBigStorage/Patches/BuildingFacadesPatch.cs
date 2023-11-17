using Database;
using GravitasBigStorage.Content;
using HarmonyLib;

namespace GravitasBigStorage.Patches
{
	public class BuildingFacadesPatch
	{
		[HarmonyPatch(typeof(BuildingFacades), MethodType.Constructor, typeof(ResourceSet))]
		public class BuildingFacades_Ctor_Patch
		{
			public static void Postfix(BuildingFacades __instance)
			{
				GBSFacades.RegisterFacades(__instance);
			}
		}
	}
}

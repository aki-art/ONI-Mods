using Database;
using DecorPackB.Content.Db;
using HarmonyLib;

namespace DecorPackB.Patches
{
	public class ArtableStagesPatch
	{
		[HarmonyPatch(typeof(ArtableStages), MethodType.Constructor, typeof(ResourceSet))]
		public class ArtableStages_Ctor_Patch
		{
			public static void Postfix(ArtableStages __instance)
			{
				DPArtableStages.Register(__instance);
			}
		}
	}
}

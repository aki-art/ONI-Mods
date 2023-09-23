using Database;
using HarmonyLib;

namespace DecorPackA.Patches
{
	public class ArtableStagesPatch
	{
		[HarmonyPatch(typeof(ArtableStages), MethodType.Constructor, typeof(ResourceSet))]
		public class TargetType_TargetMethod_Patch
		{
			public static void Postfix(ArtableStages __instance)
			{
				ModDb.RegisterArtableStages(__instance);
			}
		}
	}
}

using Database;
using HarmonyLib;

namespace Moonlet.Patches
{
	public class ArtableStagesPatch
	{
		[HarmonyPatch(typeof(ArtableStages), MethodType.Constructor, typeof(ResourceSet))]
		public class TargetType_TargetMethod_Patch
		{
			public static void Postfix(ArtableStages __instance)
			{
				Mod.artablesLoader.ApplyToActiveTemplates(t => t.LoadContent(__instance));
			}
		}
	}
}

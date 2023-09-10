using Database;
using HarmonyLib;
using Twitchery.Content;

namespace Twitchery.Patches
{
	public class ChoreTypesPatch
	{
		[HarmonyPatch(typeof(ChoreTypes), MethodType.Constructor, typeof(ResourceSet))]
		public class TargetType_Ctor_Patch
		{
			public static void Postfix(ChoreTypes __instance)
			{
				TChoreTypes.Register(__instance);
			}
		}
	}
}

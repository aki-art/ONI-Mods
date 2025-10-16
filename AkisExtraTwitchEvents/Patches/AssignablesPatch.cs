using Database;
using HarmonyLib;
using Twitchery.Content;

namespace Twitchery.Patches
{
	public class AssignablesPatch
	{
		[HarmonyPatch(typeof(AssignableSlots), MethodType.Constructor)]
		public class AssignableSlots_Ctor_Patch
		{
			public static void Postfix(AssignableSlots __instance)
			{
				TAssignableSlots.Register(__instance);
			}
		}
	}
}

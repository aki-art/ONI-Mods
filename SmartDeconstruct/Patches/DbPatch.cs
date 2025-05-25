using HarmonyLib;

namespace SmartDeconstruct.Patches
{
	public class DbPatch
	{
		[HarmonyPatch(typeof(Db), "Initialize")]
		public class Db_Initialize_Patch
		{
			public static void Postfix(Db __instance)
			{
				SD_StatusItems.Register(__instance.MiscStatusItems);
			}
		}
	}
}

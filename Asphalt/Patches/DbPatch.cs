using HarmonyLib;

namespace Asphalt.Patches
{
	public class DbPatch
	{
		[HarmonyPatch(typeof(Db), nameof(Db.Initialize))]
		public static class Db_Initialize_Patch
		{
			public static void Prefix() => ModAssets.LateLoadAssets();
		}
	}
}

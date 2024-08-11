using HarmonyLib;

namespace GoldenThrone.Patches
{
	public class DbPatch
	{
		[HarmonyPatch(typeof(Db), "Initialize")]
		public class Db_Initialize_Patch
		{
			public static void Postfix()
			{
				ModAssets.LateLoadAssets();
			}
		}
	}
}

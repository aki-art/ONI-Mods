using HarmonyLib;

namespace DecorPackA.Patches
{
	public class DbPatch
	{
		[HarmonyPatch(typeof(Db), "Initialize")]
		public class Db_Initialize_Patch
		{
			public static void Prefix()
			{
				BuildingFacadesPatch.Patch(Mod.harmonyInstance);
			}

			public static void Postfix()
			{
				ModAssets.Load();
				ModDb.Initialize();
				Integration.TwitchMod.Initialize();
				Integration.Twitch.FloorUpgrader.OnDbInit();
			}
		}
	}
}

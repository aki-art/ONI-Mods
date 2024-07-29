using HarmonyLib;

namespace Moonlet.Patches.Database
{
	public class DbPatch
	{
		[HarmonyPatch(typeof(Db), nameof(Db.Initialize))]
		public class Db_Initialize_Patch
		{
			public static void Postfix(Db __instance)
			{
				//PlanScreen.iconNameMap.Add(HashCache.Get().Add(ModDb.BuildingCategories.POIS), "icon_errand_build");
				ModDb.OnDbInitialize(__instance);
			}

		}
	}
}

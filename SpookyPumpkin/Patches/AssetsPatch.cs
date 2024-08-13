using HarmonyLib;
using SpookyPumpkinSO.Content.GhostPip.Spawning;

namespace SpookyPumpkinSO.Patches
{
	public class AssetsPatch
	{
		[HarmonyPatch(typeof(Assets), "OnPrefabInit")]
		public class Assets_OnPrefabInit_Patch
		{
			public static void Prefix(Assets __instance)
			{
				FUtility.Assets.LoadSprites(__instance);
			}

			public static void Postfix()
			{
				Assets.GetBuildingDef("Headquarters").BuildingComplete.AddComponent<GhostPipSpawner>();
				if (DlcManager.IsExpansion1Active())
					Assets.GetBuildingDef("ExobaseHeadquarters").BuildingComplete.AddComponent<GhostPipSpawner>();
			}
		}
	}
}

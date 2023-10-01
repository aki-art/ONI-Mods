using HarmonyLib;
using SpookyPumpkinSO.Content.Buildings;
using SpookyPumpkinSO.Content.GhostPip;

namespace SpookyPumpkinSO.Patches
{
	public class DetailsScreenPatch
	{
		[HarmonyPatch(typeof(DetailsScreen), "OnPrefabInit")]
		public static class DetailsScreen_OnPrefabInit_Patch
		{
			public static void Postfix()
			{
				FUtility.FUI.SideScreen.AddCustomSideScreen<SpookyPumpkin_GhostSquirrelSideScreen>("SpookyPumpkin_GhostSquirrelSideScreen", ModAssets.Prefabs.sideScreenPrefab);

				FUtility.FUI.SideScreen.AddClonedSideScreen<SpookyPumpkin_CarvedPumpkinSideScreen>("SpookyPumpkin_CarvedPumpkinSideScreen", typeof(MonumentSideScreen));

			}
		}
	}
}

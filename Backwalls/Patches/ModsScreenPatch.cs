using Backwalls.UI;
using FUtility.FUI;
using HarmonyLib;

namespace Backwalls.Patches
{
	class ModsScreenPatch
	{
		[HarmonyPatch(typeof(ModsScreen), "BuildDisplay")]
		public static class ModsScreen_BuildDisplay_Patch
		{
			public static void Postfix(object ___displayedMods)
			{
				ModMenuButton.AddModSettingsButton(___displayedMods, "Backwalls", OpenModSettingsScreen);
			}

			private static void OpenModSettingsScreen()
			{
				Helper.CreateFDialog<ModSettingsDialog>(ModAssets.settingsDialogPrefab, "BackwallsSettings");
			}
		}
	}
}

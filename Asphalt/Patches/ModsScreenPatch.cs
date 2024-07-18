using Asphalt.Settings;
using FUtility.FUI;
using HarmonyLib;

namespace Asphalt.Patches
{
	public class ModsScreenPatch
	{
		[HarmonyPatch(typeof(ModsScreen), "BuildDisplay")]
		public static class ModsScreen_BuildDisplay_Patch
		{
			public static void Postfix(object ___displayedMods)
			{
				ModMenuButton.AddModSettingsButton(___displayedMods, "Asphalt", OpenModSettingsScreen);
			}

			private static void OpenModSettingsScreen() => Helper.CreateFDialog<SettingsScreen>(ModAssets.Prefabs.settingsDialog, "AsphaltSettings");
		}
	}
}

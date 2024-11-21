using DecorPackA.UI;
using FUtility.FUI;
using HarmonyLib;

namespace DecorPackA.Patches
{
	public class ModsScreenPatch
	{
		[HarmonyPatch(typeof(ModsScreen), "BuildDisplay")]
		public static class ModsScreen_BuildDisplay_Patch
		{
			public static void Postfix(object ___displayedMods)
			{
				ModMenuButton.AddModSettingsButton(___displayedMods, "DecorPackA", OpenModSettingsScreen);
			}

			private static void OpenModSettingsScreen() => Helper.CreateFDialog<SettingsScreen>(ModAssets.Prefabs.settingsWindow, "DecorPackISettings");
		}
	}
}

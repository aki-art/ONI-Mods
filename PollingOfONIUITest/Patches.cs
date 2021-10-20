using Harmony;
using UnityEngine;

namespace PollingOfONIUITest
{
	public class Patches
    {

        [HarmonyPatch(typeof(Db), "Initialize")]
        public static class Db_Initialize_Patch
        {
			public static void Postfix() => ModAssets.LateLoadAssets();
		}

		[HarmonyPatch(typeof(PauseScreen), "OnPrefabInit")]
		public static class PauseScreen_OnPrefabInit_Patch
		{
			public static void Postfix(ref KButtonMenu.ButtonInfo[] ___buttons)
			{
				___buttons = ___buttons.Append(new KButtonMenu.ButtonInfo("Polling of ONI Settings", Action.NumActions, OpenModSettingsScreen));
			}
		}

		private static void OpenModSettingsScreen()
		{
			if (ModAssets.settingsDialogPrefab == null)
			{
				Debug.LogWarning("Could not display UI: Mod Settings screen prefab is null.");
				return;
			}

			GameObject settingsScreen = Object.Instantiate(ModAssets.settingsDialogPrefab, GameScreenManager.Instance.ssOverlayCanvas.transform);
			settingsScreen.AddComponent<SettingsDemoDialog>().Show();
		}
	}
}

using FUtility.FUI;
using HarmonyLib;
using TrueTiles.Settings;
using static FUtility.FUI.Helper;

namespace TrueTiles.Patches
{
	public class ModsScreenPatch
	{
		[HarmonyPatch(typeof(ModsScreen), "BuildDisplay")]
		public static class ModsScreen_BuildDisplay_Patch
		{
			public static void Postfix(ModsScreen __instance)
			{
				//ModMenuButton.AddModSettingsButton(__instance.displayedMods, "TrueTiles", OpenModSettingsScreen);

				var globalMods = Global.Instance.modManager.mods;

				foreach (var displayedMod in __instance.displayedMods)
				{
					if (displayedMod.mod_index > (globalMods.Count - 1) || displayedMod.mod_index < 0)
						continue;

					var mod = globalMods[displayedMod.mod_index];
					if (Mod.moddedPacksPaths.Contains(mod.ContentPath))
					{
						PretendEnableMod(displayedMod, mod);
						if (displayedMod.rect_transform.TryGetComponent(out HierarchyReferences references))
						{
							var button = references.GetReference<KButton>("ManageButton").transform;
							var info = new ButtonInfo("Texture Packs", OpenModSettingsScreen, 12);
							MakeKButton(info, button.gameObject, button.parent.gameObject, button.GetSiblingIndex() - 1);
						}
					}
				}
			}

			private static void PretendEnableMod(ModsScreen.DisplayedMod displayedMod, KMod.Mod mod)
			{
				if (mod.available_content == 0 && mod.contentCompatability == KMod.ModContentCompatability.NoContent)
				{
					if (displayedMod.rect_transform.TryGetComponent(out HierarchyReferences references))
					{
						references.GetReference<LocText>("Title").text = $"{mod.title}\n<color=#e485e6><size=70%>Mod managed by True Tiles.</size></color>";
						references.GetReference<MultiToggle>("EnabledToggle").gameObject.SetActive(false);
						var bg = references.GetReference<KImage>("BG");
						bg.defaultState = KImage.ColorSelector.Inactive;
						bg.ColorState = KImage.ColorSelector.Inactive;
					}
				}
			}

			private static void OpenModSettingsScreen()
			{
				Helper.CreateFDialog<SettingsScreen>(ModAssets.Prefabs.settingsDialog, "TrueTilesSettings");
			}
		}
	}
}

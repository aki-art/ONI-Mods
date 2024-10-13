using HarmonyLib;
using Klei.CustomSettings;

namespace Moonlet.Patches
{
	internal class CustomGameSettingsPatch
	{

		[HarmonyPatch(typeof(CustomGameSettings), "OnPrefabInit")]
		public class CustomGameSettings_OnPrefabInit_Patch
		{
			public static void Prefix(CustomGameSettings __instance)
			{
				Mod.subworldMixingLoader.ApplyToActiveTemplates(loader =>
				{
					var setting = (SettingConfig)new SubworldMixingSettingConfig(loader.id, loader.GetWorldgenPath(), DlcManager.AVAILABLE_VANILLA_ONLY, DlcManager.VANILLA_ID);

					__instance.AddMixingSettingsConfig(setting);
					if (setting.coordinate_range < 0L)
						return;

					__instance.CoordinatedMixingSettings.Add(setting.id);
				});

				Mod.worldMixingLoader.ApplyToActiveTemplates(loader =>
				{
					var setting = (SettingConfig)new WorldMixingSettingConfig(loader.id, loader.GetWorldgenPath(), DlcManager.AVAILABLE_VANILLA_ONLY, DlcManager.VANILLA_ID);

					__instance.AddMixingSettingsConfig(setting);
					if (setting.coordinate_range < 0L)
						return;

					__instance.CoordinatedMixingSettings.Add(setting.id);
				});
			}
		}
	}
}

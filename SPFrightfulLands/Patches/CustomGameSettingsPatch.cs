using HarmonyLib;
using Klei.CustomSettings;

namespace SPFrightfulLands.Patches
{
    public class CustomGameSettingsPatch
    {
        // temp fix for a bug in base game
        [HarmonyPatch(typeof(CustomGameSettings), "AddMixingSettingsConfig")]
        public class CustomGameSettings_AddMixingSettingsConfig_Patch
        {
            public static void Prefix(SettingConfig config)
            {
                if (config.required_content == null)
                {
                    config.required_content = new string[] { DlcManager.VANILLA_ID };
                }
            }
        }
    }
}

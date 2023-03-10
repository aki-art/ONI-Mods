using HarmonyLib;
using Klei.CustomSettings;
using Randomizer.Content.Scripts;

namespace Randomizer.Patches
{
    public class OfflineWorldGenPatch
    {
        [HarmonyPatch(typeof(OfflineWorldGen), "DoWorldGenInitialize")]
        public class OfflineWorldGen_DoWorldGenInitialize_Patch
        {
            public static void Prefix()
            {
                string id = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout).id;

                if(id.StartsWith("clusters/Randomizer_PlaceHolder"))
                {
                    Log.Debuglog("redirecting");
                    CustomGameSettings.Instance.SetQualitySetting(CustomGameSettingConfigs.ClusterLayout, WorldRandomizerGenerator.currentCluster);
                    var settings = CustomGameSettings.Instance.QualitySettings[CustomGameSettingConfigs.ClusterLayout.id];
                    
                }

                Log.Debuglog("CURRENT: (offlineworld): " + CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout).id);
            }
        }
    }
}

using HarmonyLib;
using Twitchery.Content;

namespace Twitchery.Patches
{
    public class ElementsAudioPatch
    {
        [HarmonyPatch(typeof(ElementsAudio), "LoadData")]
        public class ElementsAudio_LoadData_Patch
        {
            public static void Postfix(ElementsAudio __instance, ref ElementsAudio.ElementAudioConfig[] ___elementAudioConfigs)
            {
                ___elementAudioConfigs = ___elementAudioConfigs.AddRangeToArray(Elements.CreateAudioConfigs(__instance));
            }
        }
    }
}

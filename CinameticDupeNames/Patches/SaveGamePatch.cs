using CinematicDupeNames.Content.Cmps;
using HarmonyLib;
namespace CinematicDupeNames.Patches
{
    public class SaveGamePatch
    {
        [HarmonyPatch(typeof(SaveGame), "OnPrefabInit")]
        public class SaveGame_OnPrefabInit_Patch
        {
            public static void Postfix(SaveGame __instance) => __instance.gameObject.AddOrGet<CDN_CinematicModeHandler>();
        }
    }
}

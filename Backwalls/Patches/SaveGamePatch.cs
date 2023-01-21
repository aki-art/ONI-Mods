using Backwalls.Cmps;
using HarmonyLib;

namespace Backwalls.Patches
{
    public class SaveGamePatch
    {
        [HarmonyPatch(typeof(SaveGame), "OnPrefabInit")]
        public class SaveGame_OnPrefabInit_Patch
        {
            public static void Postfix(SaveGame __instance)
            {
                //__instance.FindOrAdd<BackwallStorage>();
                var modStorage = __instance.gameObject.AddOrGet<ModStorage>();
                modStorage.ShowHSV = true;
                modStorage.ShowSwatches = true;
                modStorage.CopyColor = true;
                modStorage.CopyPattern = true;
            }
        }
    }
}

using HarmonyLib;
using Slag.Cmps;

namespace Slag.Patches
{
    public class SaveGamePatch
    {
        [HarmonyPatch(typeof(SaveGame), "OnSpawn")]
        public class SaveGame_OnSpawn_Patch
        {
            public static void Postfix(SaveGame __instance)
            {
                 __instance.gameObject.AddOrGet<ModSaveData>();
            }
        }
    }
}

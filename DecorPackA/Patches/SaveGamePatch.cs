using Buildings.StainedGlassTile;
using HarmonyLib;

namespace DecorPackA.Patches
{
    public class SaveGamePatch
    {
        [HarmonyPatch(typeof(SaveGame), "OnPrefabInit")]
        public class SaveGame_OnPrefabInit_Patch
        {
            public static void Postfix(SaveGame __instance)
            {
                if(Mod.addNeutroniumAlloyGlass)
                {
                    __instance.gameObject.AddOrGet<DecorPackAGlassShineColors>();
                }
            }
        }
    }
}

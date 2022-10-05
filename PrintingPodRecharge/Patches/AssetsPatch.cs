using HarmonyLib;

namespace PrintingPodRecharge.Patches
{
    public class AssetsPatch
    {
        [HarmonyPatch(typeof(Assets), "OnPrefabInit")]
        public static class Assets_OnPrefabInit_Patch
        {
            public static void Postfix()
            {
                CharacterSelectionControllerPatch.Patch(Mod.harmonyInstance);
            }
        }
    }
}

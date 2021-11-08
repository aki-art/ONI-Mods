using DecorPackA.Buildings.GlassSculpture;
using HarmonyLib;

namespace DecorPackA.Patches
{
    class ArtablePatch
    {
        [HarmonyPatch(typeof(Artable), "SetStage")]
        public static class Artable_SetStage_Patch
        {
            public static void Postfix(Artable __instance)
            {
                if (__instance.TryGetComponent(out Fabulous fabulous))
                {
                    fabulous.RefreshFab();
                }
            }
        }
    }
}

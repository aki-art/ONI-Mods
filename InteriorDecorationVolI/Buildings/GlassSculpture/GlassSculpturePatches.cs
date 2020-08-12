using Harmony;

namespace InteriorDecorationVolI.Buildings.GlassSculpture
{
    class GlassSculpturePatches
    {
        // For other mods that may alter sculpture states, such as ReSculpt
        [HarmonyPatch(typeof(Artable))]
        [HarmonyPatch("SetStage")]
        public static class Artable_SetStage_Patch
        {
            public static void Postfix(Artable __instance)
            {
                var fab = __instance.GetComponent<Fabulous>();
                if (fab != null)
                    fab.RefreshFab();
            }
        }
    }
}

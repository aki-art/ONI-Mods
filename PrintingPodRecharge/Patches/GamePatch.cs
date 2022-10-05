using HarmonyLib;

namespace PrintingPodRecharge.Patches
{
    public class GamePatch
    {
        [HarmonyPatch(typeof(Game), "Load")]
        public class Game_Load_Patch
        {
            public static void Postfix()
            {
                Integration.ArtifactsInCarePackages.SetData();
            }
        }
    }
}

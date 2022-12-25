using Database;
using DecorPackB.Content;
using HarmonyLib;

namespace DecorPackB.Patches
{
    public class ArtableStagesPatch
    {
        [HarmonyPatch(typeof(ArtableStages), MethodType.Constructor, typeof(ResourceSet))]
        public class TargetType_TargetMethod_Patch
        {
            public static void Postfix(ArtableStages __instance)
            {
                DPArtableStages.Register(__instance);
            }
        }
    }
}
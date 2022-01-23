using HarmonyLib;
using Terraformer.Screens;

namespace Terraformer.Patches
{
    public class DetailsScreenPatch
    {
        [HarmonyPatch(typeof(DetailsScreen), "OnPrefabInit")]
        public static class DetailsScreen_OnPrefabInit_Patch
        {
            public static void Postfix()
            {
                FUtility.FUI.SideScreen.AddClonedSideScreen<DestroyablePlanetSidescreen>(
                    "DestroyablePlanetSidescreen",
                    "Sealed Door Side Screen",
                    typeof(SealedDoorSideScreen));
            }
        }
    }
}

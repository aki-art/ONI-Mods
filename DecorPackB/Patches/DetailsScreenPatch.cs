using DecorPackB.Buildings.FossilDisplay;
using HarmonyLib;

namespace DecorPackB.Patches
{
    public class DetailsScreenPatch
    {
        [HarmonyPatch(typeof(DetailsScreen), "OnPrefabInit")]
        public static class DetailsScreen_OnPrefabInit_Patch
        {
            public static void Postfix()
            {
                FUtility.FUI.SideScreen.AddClonedSideScreen<ExhibitionInfoSidescreen>(
                    "Exhibition Side Screen",
                    "Sealed Door Side Screen",
                    typeof(SealedDoorSideScreen));
            }
        }
    }
}

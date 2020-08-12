/*using FUtility;
using Harmony;
using System.Collections.Generic;
using static DetailsScreen;

namespace InteriorDecorationVolI.Buildings.Aquarium
{
    class AquariumPatches
    {
        [HarmonyPatch(typeof(DetailsScreen), "OnPrefabInit")]
        public static class DetailsScreen_OnPrefabInit_Patch
        {
            public static void Postfix()
            {
                FUtility.FUI.SideScreen.AddClonedSideScreen<AquariumSideScreen>(
                    "Aquarium Side Screen", 
                    "Single Entity Receptacle Screen", 
                    typeof(ReceptacleSideScreen));
            }
        }
    }
}
*/
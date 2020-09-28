using FUtility;
using Harmony;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InteriorDecorationv1.Buildings.Aquarium
{
    class AquariumPatches
    {
        [HarmonyPatch(typeof(DetailsScreen), "OnPrefabInit")]
        public static class DetailsScreen_OnPrefabInit_Patch
        {
            public static void Postfix()
            {
/*                                FUtility.FUI.SideScreen.AddClonedSideScreen<FishSideScreen>(
                                    "Aquarium Side Screen", 
                                    "Single Entity Receptacle Screen", 
                                    typeof(ReceptacleSideScreen));
*/
/*                List<GameObject> prefabsWithTag = Assets.GetPrefabsWithTag(GameTags.SwimmingCreature);
                foreach (GameObject prefab in prefabsWithTag)
                    Debug.Log(prefab.name);*/
            }
        }


        [HarmonyPatch(typeof(ReceptacleSideScreen), "RequiresAvailableAmountToDeposit")]
        public static class ReceptacleSideScreen_RequiresAvailableAmountToDeposit_Patch
        {
            public static void Postfix(ref bool __result)
            {
                __result = false;
            }
        }

        [HarmonyPatch(typeof(ReceptacleSideScreen), "UpdateAvailableAmounts")]
        public static class ReceptacleSideScreen_UpdateAvailableAmounts_Patch
        {
            public static void Postfix(Dictionary<ReceptacleToggle, object> ___depositObjectMap, ReceptacleSideScreen __instance)
            {
                foreach (var keyValuePair in ___depositObjectMap)
                {
                    KToggle toggle = keyValuePair.Key.toggle;
                    toggle.GetComponent<ImageToggleState>().SetInactive();
                    toggle.gameObject.GetComponentInChildrenOnly<Image>().material = __instance.defaultMaterial;
                }
             }
        }
    }
}

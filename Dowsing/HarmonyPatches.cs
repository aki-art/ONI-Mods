using FUtility;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dowsing
{
    class HarmonyPatches
    {
        [HarmonyPatch(typeof(DetailsScreen), "OnPrefabInit")]
        public static class DetailsScreen_OnPrefabInit_Patch
        {
            public static void Postfix()
            {
                // FUtility.FUI.SideScreen.AddClonedSideScreen<DowsingSideScreen>( "Curtain Side Screen", "Single Entity Receptacle Screen", typeof(ReceptacleSideScreen));
                Debug.Log("null?");
                Debug.Log(ModAssets.sideScreenPrefab == null);
                FUtility.FUI.SideScreen.AddCustomSideScreen<DowsingSidescreen2>("DowsingSideSreen", ModAssets.sideScreenPrefab);
            }
        }
        [HarmonyPatch(typeof(Assets), "OnPrefabInit")]
        public static class Assets_OnPrefabInit_Patch
        {
            public static void Postfix()
            {
            }
        }
        [HarmonyPatch(typeof(Db), "Initialize")]
        public static class Db_Initialize_Patch
        {
            public static void Prefix()
            {
                ModAssets.LateLoadAssets();
            }

        }

        [HarmonyPatch(typeof(ReceptacleSideScreen), "Initialize")]
        public static class ReceptacleSideScreen_Initialize_Patch
        {
            public static void Postfix(GameObject ___entityToggle, GameObject ___requestObjectList, ReceptacleSideScreen __instance)
            {
                /*                Debug.Log("OBEJCT_____________________________________________");
                                Debug.Log("___entityToggle" + ___entityToggle.gameObject.name);
                                Debug.Log("___requestObjectList" + ___requestObjectList.gameObject.name);

                                Debug.Log("MATERIALS");
                                Debug.Log(__instance.defaultMaterial.name);
                                Debug.Log(__instance.desaturatedMaterial.name);*/
                

            }
        }


        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Postfix()
            {
                Buildings.RegisterSingleBuilding(typeof(DowsingRodConfig));
                //Buildings.RegisterSingleBuilding(typeof(permeabletile));
            }
        }

    }
}

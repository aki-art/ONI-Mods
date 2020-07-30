using FUtility;
using Harmony;
using Klei;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static FUtility.Buildings;
using static FUtility.FUI.SideScreen;

namespace Curtain
{
    class HarmonyPatches
    {
        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                Log.PrintVersion();
            }
        }

        [HarmonyPatch(typeof(DetailsScreen), "OnPrefabInit")]
        public static class DetailsScreen_OnPrefabInit_Patch
        {
            public static void Postfix()
            {
                AddSideScreen<CurtainSideScreen>("Curtain Side Screen", "Door Toggle Side Screen");
            }
        }

        [HarmonyPatch(typeof(Database.BuildingStatusItems), "CreateStatusItems")]
        public static class Database_BuildingStatusItems_CreateStatusItems_Patch
        {
            public static void Postfix()
            {
                ModAssets.MakeStatusItem();
            }
        }

        // Allowing curtains to have their settings copied to doors
        [HarmonyPatch(typeof(Door), "OnCopySettings")]
        public static class Door_OnCopySettings_Patch
        {
            public static void Postfix(object data, Door __instance)
            {
                var curtain = ((GameObject)data).GetComponent<Curtain>();
                if (curtain != null)
                {
                    __instance.QueueStateChange((Door.ControlState)curtain.RequestedState);
                    return;
                }
            }
        }

        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                Buildings2.RegisterSingleBuilding(typeof(PlasticCurtainConfig));

                LocString.CreateLocStringKeys(typeof(STRINGS.BUILDING));
                LocString.CreateLocStringKeys(typeof(STRINGS.BUILDINGS));
                LocString.CreateLocStringKeys(typeof(STRINGS.UI));
            }
        }
    }
}

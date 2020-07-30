using Harmony;
using System.Collections.Generic;
using FUtility;
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

        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                var buildings = new List<BuildingConfig>()
                {
                    new BuildingConfig(typeof(PlasticCurtainConfig))
                };

                RegisterAllBuildings(buildings);

                Strings.Add(
                    $"STRINGS.UI.UISIDESCREENS.CURTAIN_SIDE_SCREEN.TITLE", 
                    "Curtain test sidescreen");

                Strings.Add(
                    $"STRINGS.BUILDING.STATUSITEMS.CHANGECURTAINCONTROLSTATE.NAME", 
                    "Pending Curtain State Change: {CurrentState}");

                Strings.Add(
                    $"STRINGS.BUILDING.STATUSITEMS.CHANGECURTAINCONTROLSTATE.TOOLTIP", 
                    "Waiting for a Duplicant to change control state");
            }
        }
    }
}

using FUtility;
using HarmonyLib;
using SpookyPumpkin.Settings;
using System.Linq;

namespace SpookyPumpkin.GhostPip
{
    public class Patches
    {
        [HarmonyPatch(typeof(World), "OnSpawn")]
        public static class World_OnLoad_Patch
        {
            public static void Postfix()
            {
                if (ModSettings.Settings.SpawnGhostPip && !PipExists() && ShouldPipExist()) 
                    SpawnGhostPip();
            }

            private static void SpawnGhostPip()
            {
                var telepad = GameUtil.GetActiveTelepad();
                if (telepad == null)
                    Log.Warning("No Printing Pod, cannot spawn pip.");
                else
                    Utils.Spawn(GhostSquirrelConfig.ID, telepad);
            }

            private static bool PipExists() => Components.Capturables.Items.Any(c => c.PrefabID() == GhostSquirrelConfig.ID);

            private static bool ShouldPipExist()
            {
                string id = SaveLoader.Instance.GameInfo.colonyGuid.ToString();
                if (ModAssets.pipWorlds.TryGetValue(id, out bool shouldExist)) return shouldExist;
                else return true;
            }
        }

        [HarmonyPatch(typeof(DetailsScreen), "OnPrefabInit")]
        public static class DetailsScreen_OnPrefabInit_Patch
        {
            public static void Postfix()
            {
                FUtility.FUI.SideScreen.AddCustomSideScreen<GhostSquirrelSideScreen>("GhostSquirrelSideScreen", ModAssets.Prefabs.sideScreenPrefab);
            }
        }



        /*
        [HarmonyPatch(typeof(BuildingComplete), "OnSpawn")]
        public static class BuildingComplete_OnSpawn_Patch
        {
            public static void Postfix(BuildingComplete __instance)
            {
                KPrefabID kPrefabID = __instance.GetComponent<KPrefabID>();
                if (!kPrefabID.HasTag(GameTags.Bunker) &&
                    __instance.GetComponent<PrimaryElement>().ElementID == ModAssets.transparentAluminumHash)
                {
                    kPrefabID.AddTag(GameTags.Bunker);
                }
            }
        }*/
    }
}

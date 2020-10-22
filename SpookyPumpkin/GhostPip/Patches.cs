using FUtility;
using Harmony;
using SpookyPumpkin.Settings;
using System.Collections.Generic;

namespace SpookyPumpkin.GhostPip
{
    class Patches
    {
        public static List<string> spawnedWorlds = new List<string>();


        [HarmonyPatch(typeof(World), "OnSpawn")]
        public static class World_OnLoad_Patch
        {
            public static void Postfix()
            {
                string id = SaveLoader.Instance.GameInfo.colonyGuid.ToString();
                if ((spawnedWorlds == null || !spawnedWorlds.Contains(id)) && ModSettings.Settings.SpawnGhostPip)
                {
                    var telepad = GameUtil.GetTelepad();
                    if (telepad != null)
                    {
                        var prefab = Assets.GetPrefab(GhostSquirrelConfig.ID);
                        GameUtil.KInstantiate(prefab, telepad.transform.position, Grid.SceneLayer.Creatures).SetActive(true);
                        spawnedWorlds.Add(id);
                        ModAssets.WriteSettingsToFile(spawnedWorlds, "pipworlds");
                    }
                    else Log.Warning("No Printing Pod, cannot spawn pip.");
                }
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
    }
}

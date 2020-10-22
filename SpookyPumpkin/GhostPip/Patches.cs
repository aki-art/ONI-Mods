using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using TUNING;
using UnityEngine;
using static ComplexRecipe;
using SpookyPumpkin.GhostPip;
using SpookyPumpkin.Foods;
using Harmony;
using FUtility;

namespace SpookyPumpkin.GhostPip
{
    class Patches
    {
        static List<string> spawnedWorlds = new List<string>();

        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                spawnedWorlds = ModAssets.ReadPipWorlds();
            }
        }

        [HarmonyPatch(typeof(World), "OnSpawn")]
        public static class World_OnLoad_Patch
        {
            public static void Postfix()
            {
                string id = SaveLoader.Instance.GameInfo.colonyGuid.ToString();
                if (spawnedWorlds == null || !spawnedWorlds.Contains(id))
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

using Entropea.Gen;
using Harmony;
using Klei.CustomSettings;
using ProcGen;
using System;
using System.Collections.Generic;

namespace Entropea
{
    class WorldGenPatches
    {
        public static bool loaded = false;
        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                // just in case any was left over, eg. if the game crashed
                WorldGenerator.CleanTempFiles(); 
            }
        }

        [HarmonyPatch(typeof(ElementLoader), "Load")]
        public static class ElementLoader_Load_Patch
        {
            public static void Postfix()
            {
                Elements.Initialize();
            }
        }

        [HarmonyPatch(typeof(SettingsCache), "LoadFiles")]
        public static class AddFakeAsteroid
        {
            public static void Prefix()
            {
                if (loaded) return;
                WorldGenerator.CreateEmptyWorldFile(ModAssets.EpochTime());
                loaded = true;
            }
        }

        [HarmonyPatch(typeof(WorldGenScreen), "OnSpawn")]
        public static class GenerateRandomWorldData
        {
            public static void Prefix()
            {
                WorldGenerator.Generate();
            }
        }

        /*
                [HarmonyPatch(typeof(ColonyDestinationSelectScreen), "LaunchClicked")]
                public static class ColonyDestinationSelectScreen_LaunchClicked_Patch
                {
                    public static void Prefix(NewGameSettingsPanel ___newGameSettings)
                    {
                        if(___newGameSettings.GetSetting(CustomGameSettingConfigs.World) == "worlds/Entropea")
                        {
                            WorldGenerator.GenerateDefaults(ModAssets.EpochTime());
                            Debug.Log("setting here to " + WorldGenerator.path);
                            CustomGameSettingConfigs.World.levels.Add(new SettingLevel(WorldGenerator.path, "", ""));
                            ___newGameSettings.SetSetting(CustomGameSettingConfigs.World, WorldGenerator.path);
                        }
                    }

                }

                [HarmonyPatch(typeof(WorldGenSettings), MethodType.Constructor, new Type[] { typeof(string), typeof(List<string>), typeof(bool) })]
                public static class WorldGenSettings_Constructor_Patch
                {
                    public static void Prefix(string worldName)
                    {
                        Debug.Log("Trying to generate " + worldName);
                        Debug.Log(SettingsCache.worlds.HasWorld(worldName));
                        Debug.Log("my worlds name is " + WorldGenerator.path);
                        Debug.Log(SettingsCache.worlds.HasWorld(WorldGenerator.path));
                    }

                }*/

        /*        [HarmonyPatch(typeof(OfflineWorldGen), "DoWorldGenInitialize")]
                public static class OfflineWorldGen_Generate_Patch
                {
                    public static void Prefix(int ___seed)
                    {
                        if (CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.World).id == "worlds/Entropea")
                        {
                            WorldGenerator.Generate(___seed);
                            Debug.Log("Overwriting world to " + WorldGenerator.path);
                            CustomGameSettings.Instance.SetQualitySetting(CustomGameSettingConfigs.World, WorldGenerator.path);
                            Debug.Log("CurrentQualitySetting " + CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.World).id);
                        }
                    }
                }*/
    }
}

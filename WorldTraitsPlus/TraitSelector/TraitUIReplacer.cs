using FUtility.FUI;
using Harmony;
using Klei.CustomSettings;
using STRINGS;
using System.Collections.Generic;
using System.Linq;

namespace WorldTraitsPlus.TraitSelector
{
    class TraitUIReplacer
    {
        public static bool shuffling = false;
        public static int maxAttempts = 20000;
        public static List<string> lockedTraits = new List<string>();
        public static TraitSelector selector;
        public static List<AsteroidDescriptor> cachedTraits;
        public static List<AsteroidDescriptor> checkedTraits;

        // Only look if seed was shuffled, manually set seeds should not be overwritten
        [HarmonyPatch(typeof(ColonyDestinationSelectScreen), "ShuffleClicked")]
        public static class ColonyDestinationSelectScreen_ShuffleClicked_Patch
        {
            public static void Postfix()
            {
                shuffling = true;
            }
        }

        // Replace UI
        [HarmonyPatch(typeof(ColonyDestinationSelectScreen), "OnSpawn")]
        public static class ColonyDestinationSelectScreen_OnSpawn_Patch
        {
            public static void Postfix(ColonyDestinationSelectScreen __instance)
            {
                var panel = __instance.transform.Find("DestinationInfo/Content/InfoColumn/Horiz/Section - Start/Details");
                if (panel == null) return;

                var traitsPanel = UnityEngine.Object.Instantiate(ModAssets.traitsSelectorPrefab, panel.transform.parent);
                selector = traitsPanel.AddComponent<TraitSelector>();
            }
        }


        [HarmonyPatch(typeof(ColonyDestinationAsteroidData), "GenerateTraitDescriptors")]
        public static class ColonyDestinationAsteroidData_GenerateTraitDescriptors_Patch
        {
            public static void Postfix(List<AsteroidDescriptor> __result)
            {
                cachedTraits = __result;
            }
        }

        protected static bool HasAllTraits(List<AsteroidDescriptor> descriptors)
        {
            bool success = true;
            foreach (var lockedTrait in lockedTraits)
            {
                if (!descriptors.Any(a => a.text.Contains(lockedTrait)))
                {
                    success = false;
                    break;
                }
            }

            return success;
        }

        [HarmonyPatch(typeof(ColonyDestinationSelectScreen), "SettingChanged")]
        public static class ColonyDestinationSelectScreen_SettingChanged_Patch
        {
            public static void Postfix(NewGameSettingsPanel ___newGameSettings, 
                DestinationSelectPanel ___destinationMapPanel, ref System.Random ___random)
            {
                bool traitless = cachedTraits.Count == 0 || cachedTraits[0].text.Contains(WORLD_TRAITS.NO_TRAITS.NAME);
                bool screenNotLoaded = ___newGameSettings == null || ___destinationMapPanel == null;

                if (maxAttempts <= 0 || !shuffling || screenNotLoaded || traitless)
                    return;

                // testing data
                lockedTraits = new List<string>()
                {
                    ">Geoactive<",
                    ">Metal Rich<",
                    ">Frozen Core<",
                    ">Geodes<"
                };

                bool success = HasAllTraits(cachedTraits);
                if(success)
                {
                    Debug.Log("World already has all selected traits");
                    shuffling = false;
                    return;
                }

                while (!success && maxAttempts-- > 0)
                {
                    int num = ___random.Next();
                    checkedTraits = GetDescriptorsFromSeed(num, ___newGameSettings, ___destinationMapPanel);

                    if (HasAllTraits(checkedTraits))
                    {
                        success = true;
                        Debug.Log($"Found appropiate world in {20000 - maxAttempts} attempts");
                        Debug.Log($"Seed: " + num);
                        shuffling = false;
                        selector.Refresh(checkedTraits);
                        ___newGameSettings.SetSetting(CustomGameSettingConfigs.WorldgenSeed, num.ToString());
                    }
                }

                if(maxAttempts <= 0)
                {
                    shuffling = false;
                    Debug.Log("Tried to find a world with the selected traits, terminated after a 20000 unsuccessful attempts.");
                }

                shuffling = false;
                maxAttempts = 20000;
            }

            private static List<AsteroidDescriptor> GetDescriptorsFromSeed(int seed, NewGameSettingsPanel ___newGameSettings, DestinationSelectPanel ___destinationMapPanel)
            {
                string setting = ___newGameSettings.GetSetting(CustomGameSettingConfigs.World);
                //int.TryParse(___newGameSettings.GetSetting(CustomGameSettingConfigs.WorldgenSeed), out int seed);
                ColonyDestinationAsteroidData colonyDestinationAsteroidData = ___destinationMapPanel.SelectAsteroid(setting, seed);
                return colonyDestinationAsteroidData.GetTraitDescriptors();
            }
        }
    }
}

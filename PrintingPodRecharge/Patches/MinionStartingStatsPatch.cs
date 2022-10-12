using FUtility;
using HarmonyLib;
using PrintingPodRecharge.Cmps;
using TUNING;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PrintingPodRecharge.Patches
{
    public class MinionStartingStatsPatch
    {
        public static bool IsOverrideActive()
        {
            return ImmigrationModifier.Instance != null && ImmigrationModifier.Instance.IsOverrideActive;
        }

        [HarmonyPatch(typeof(MinionStartingStats), "CreateBodyData")]
        public class MinionStartingStats_CreateBodyData_Patch
        {
            public static void Postfix(Personality p, ref KCompBuilder.BodyData __result)
            {
                if (p.nameStringKey.StartsWith("shook_"))
                {
                    var hashCache = HashCache.Get();
                    __result.hair = hashCache.Add(hashCache.Get(__result.hair).Replace("hair", "hair_bleached"));
                }
            }
        }

        [HarmonyPatch(typeof(MinionStartingStats), "Deliver")]
        public class MinionStartingStats_Deliver_Patch
        {
            public static void Postfix(MinionStartingStats __instance, GameObject __result)
            {
                Log.Debuglog("MINION DELIVER");
                if (CustomDupe.rolledData.TryGetValue(__instance, out var data))
                {
                    var customDupe = __result.AddOrGet<CustomDupe>();
                    customDupe.hairColor = data.hairColor;
                    customDupe.dyedHair = true;
                    customDupe.hairID = __instance.personality.hair;
                    customDupe.runtimeHair = HashCache.Get().Add(string.Format("hair_bleached_{0:000}", __instance.personality.hair));

                    customDupe.descKey = data.descKey;

                    CustomDupe.rolledData.Remove(__instance);
                }
            }
        }


        [HarmonyPatch(typeof(MinionStartingStats), "Apply")]
        public class MinionStartingStats_Apply_Patch
        {
            public static void Postfix(MinionStartingStats __instance, GameObject go)
            {
                if (__instance.personality.nameStringKey.StartsWith("shook_"))
                {
                    var customDupe1 = go.GetComponent<CustomDupe>();

                    if(customDupe1 == null || !customDupe1.initialized)
                    {
                        var data = GenerateRandomDupe(__instance);

                        var customDupe = go.AddOrGet<CustomDupe>();
                        customDupe.hairColor = DupeGenHelper.GetRandomHairColor();
                        customDupe.dyedHair = true;
                        customDupe.hairID = __instance.personality.hair;
                        customDupe.runtimeHair = HashCache.Get().Add(string.Format("hair_bleached_{0:000}", __instance.personality.hair));

                        customDupe.descKey = data.descKey;
                    }
                }
            }
        }

        private static CustomDupe.MinionData GenerateRandomDupe(MinionStartingStats __instance)
        {
            if (Random.value < BundleLoader.bundleSettings.ActiveRando().ChanceForVacillatorTrait)
            {
                AddGeneShufflerTrait(__instance);
            }

            var name = DupeGenHelper.SetRandomName(__instance);
            var descKey = DupeGenHelper.GetRandomDescriptionKey();
            var hairColor = DupeGenHelper.GetRandomHairColor();

            var data = new CustomDupe.MinionData(hairColor, descKey);
            __instance.personality = DupeGenHelper.GetRandomPersonality(name, descKey);

            return data;
        }

        private static void AddGeneShufflerTrait(MinionStartingStats __instance)
        {
            DupeGenHelper.AddRandomTraits(__instance, 1, 1, DUPLICANTSTATS.GENESHUFFLERTRAITS);
        }


        [HarmonyPatch(typeof(MinionStartingStats), "GenerateTraits")]
        public class MinionStartingStats_GenerateTraits_Patch
        {
            public static void Prefix(MinionStartingStats __instance, ref bool __state)
            {
                // if the user set a chance to generatte random dupes, roll for one
                var randomReplaceChance = Mod.Settings.GetActualRandomReplaceChance();
                if (ImmigrationModifier.Instance.ActiveBundle == Bundle.Shaker || (randomReplaceChance > 0 && Random.value <= randomReplaceChance))
                {
                    CustomDupe.rolledData[__instance] = GenerateRandomDupe(__instance);
                    __state = true;
                }

                if (ImmigrationModifier.Instance.ActiveBundle == Bundle.SuperDuplicant)
                {
                    AddGeneShufflerTrait(__instance);
                }
            }


            // __result is pointsDelta
            public static void Postfix(MinionStartingStats __instance, ref int __result, ref bool __state)
            {
                if (ImmigrationModifier.Instance.ActiveBundle == Bundle.Shaker || __state)
                {
                    var settings = BundleLoader.bundleSettings.ActiveRando();

                    var value = Random.Range(settings.MinimumSkillBudgetModifier, settings.MaximumSkillBudgetModifier + 1);

                    __result += Mathf.FloorToInt(value);

                    if(__state && ImmigrationModifier.Instance.ActiveBundle == Bundle.SuperDuplicant)
                    {
                        __result += BundleLoader.bundleSettings.vacillating.ExtraSkillBudget;
                        __result = Mathf.Clamp(__result, 0, settings.MaximumTotalBudget + BundleLoader.bundleSettings.vacillating.ExtraSkillBudget / 2);
                    }
                    else
                    {
                        __result = Mathf.Clamp(__result, 0, settings.MaximumTotalBudget);
                    }

                    DupeGenHelper.AddRandomTraits(__instance, 0, settings.MaxBonusPositiveTraits, DUPLICANTSTATS.GOODTRAITS);
                    DupeGenHelper.AddRandomTraits(__instance, 0, settings.MaxBonusNegativeTraits, DUPLICANTSTATS.BADTRAITS);
                    DupeGenHelper.AddRandomTraits(__instance, 0, 1, DUPLICANTSTATS.NEEDTRAITS);
                }
                else if(ImmigrationModifier.Instance.ActiveBundle == Bundle.SuperDuplicant)
                {
                    __result += BundleLoader.bundleSettings.vacillating.ExtraSkillBudget;
                }
            }
        }
    }
}

using HarmonyLib;
using PrintingPodRecharge.Cmps;
using System.Collections.Generic;
using System.Linq;
using TUNING;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PrintingPodRecharge.Patches
{
    public class MinionStartingStatsPatch
    {
        public const int BALDIE = 9999;

        private static readonly int[] allowedHairIds = new[]
        {
            1,
            2,
            3,
            4,
            5,
            6,
            7,
            8,
            9,
            10,
            11,
            12,
            13,
            14,
            15,
            16,
            17,
            18,
            19,
            30,
            36,
            37,
            43,
            44,
            BALDIE
        };

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
                if (HairDye.rolledHairs.TryGetValue(__instance, out var hairColor))
                {
                    var hairDye = __result.AddOrGet<HairDye>();
                    hairDye.hairColor = hairColor;
                    hairDye.dyedHair = true;

                    HairDye.rolledHairs.Remove(__instance);
                }
            }
        }

        [HarmonyPatch(typeof(MinionStartingStats), "GenerateTraits")]
        public class MinionStartingStats_GenerateTraits_Patch
        {
            public static void Prefix(MinionStartingStats __instance)
            {
                if (ImmigrationModifier.Instance == null || !ImmigrationModifier.Instance.IsOverrideActive)
                {
                    // if the user set a chance to generatte random dupes, roll for one
                    if(Mod.Settings.RandomDupeReplaceChance > 0 && Random.value <= Mod.Settings.RandomDupeReplaceChance)
                    {
                        GenerateRandomDupe(__instance);
                    }

                    return;
                }

                // add a gene shuffler trait to super dupes
                if (ImmigrationModifier.Instance.selectedBundle == ImmigrationModifier.Bundle.SuperDuplicant)
                {
                    var trait = Db.Get().traits.Get(DUPLICANTSTATS.GENESHUFFLERTRAITS.GetRandom().id);
                    __instance.Traits.Add(trait);
                }
                // generate random dupe
                else if (ImmigrationModifier.Instance.selectedBundle == ImmigrationModifier.Bundle.Shaker)
                {
                    GenerateRandomDupe(__instance);
                }
            }

            private static void GenerateRandomDupe(MinionStartingStats __instance)
            {
                if (Random.value < 0.1f)
                {
                    AddRandomTraits(__instance, 1, 1, DUPLICANTSTATS.GENESHUFFLERTRAITS);
                }

                var skin = Random.Range(1, 5);
                var personality = new Personality(
                    "shook_",
                    "Doesntmatter",
                    Random.value > 0.5f ? "female" : "male",
                    "",
                    DUPLICANTSTATS.STRESSTRAITS.GetRandom().id,
                    DUPLICANTSTATS.JOYTRAITS.GetRandom().id,
                    "",
                    "",
                    skin,
                    skin,
                    1,
                    Random.Range(1, 6),
                    allowedHairIds.GetRandom(),
                    Random.Range(0, 5),
                    "desc",
                    false
                    );

                HairDye.rolledHairs[__instance] = Random.ColorHSV(0, 1, 0f, 0.9f, 0.1f, 1f);

                __instance.personality = personality;

                var name = GetRandomName();

                if (!name.IsNullOrWhiteSpace())
                {
                    __instance.Name = name;
                    var key = "PrintingPodRecharge.STRINGS.DUPLICANTS.PERSONALITIES." + name.ToUpperInvariant().Replace("-", "");
                    Strings.Add(key, name);
                    __instance.NameStringKey = key;
                }
            }

            private static void AddRandomTraits(MinionStartingStats __instance, int min, int max, List<DUPLICANTSTATS.TraitVal> pool)
            {
                var traitDb = Db.Get().traits;

                var list = new List<DUPLICANTSTATS.TraitVal>(pool);
                list.RemoveAll(l => __instance.Traits.Any(t => IsTraitExclusive(l, t.Id)));

                var traitPool = list
                    .Select(t => traitDb.Get(t.id))
                    .Except(__instance.Traits)
                    .ToList();

                traitPool.Shuffle();

                var randomExtra = Random.Range(min, max);
                randomExtra = Mathf.Min(traitPool.Count, randomExtra);

                for (var i = 0; i < randomExtra; i++)
                {
                    __instance.Traits.Add(traitPool[i]);
                }
            }

            private static bool IsTraitExclusive(DUPLICANTSTATS.TraitVal l, string trait)
            {
                if (l.mutuallyExclusiveTraits == null)
                {
                    return false;
                }
                foreach (var exclusiveTrait in l.mutuallyExclusiveTraits)
                {
                    if (exclusiveTrait == trait)
                    {
                        return true;
                    }
                }

                return false;
            }

            private static readonly HashSet<string> forbiddenNames = new HashSet<string>()
            {
                "Pener",
                "Pee"
            };

            private static string GetRandomName()
            {
                var name = "";
                var maxAttempts = 16;

                var prefixes = STRINGS.MISC.NAME_PREFIXES.text.Split(',');
                var suffixes = STRINGS.MISC.NAME_SUFFIXES.text.Split(',');

                if (prefixes.Length == 0 || suffixes.Length == 0)
                {
                    return null;
                }

                while ((name.IsNullOrWhiteSpace() || forbiddenNames.Contains(name)) && maxAttempts-- > 0)
                {
                    name = prefixes.GetRandom() + suffixes.GetRandom();
                }

                return name;
            }

            public static void Postfix(MinionStartingStats __instance, ref int __result)
            {
                if (ImmigrationModifier.Instance == null || !ImmigrationModifier.Instance.IsOverrideActive)
                {
                    return;
                }

                if (ImmigrationModifier.Instance.selectedBundle == ImmigrationModifier.Bundle.SuperDuplicant)
                {
                    __result += 8;
                }
                else if (ImmigrationModifier.Instance.selectedBundle == ImmigrationModifier.Bundle.Shaker)
                {
                    if (Random.value < 0.25f)
                    {
                        __instance.Traits.RemoveAll(t => !t.PositiveTrait);
                    }

                    //__result = Random.Range(0, 22);
                    __result = Mathf.FloorToInt(Util.GaussianRandom(0f, 1f) * 15f) + 7;
                    __result = Mathf.Clamp(__result, 0, 20);

                    var maxPositive = __result > 10 ? 0 : __result > 5 ? 2 : 4;
                    AddRandomTraits(__instance, 0, maxPositive, DUPLICANTSTATS.GOODTRAITS);

                    var maxNegative = __result < 10 ? 3 : __result < 5 ? 3 : 0;
                    AddRandomTraits(__instance, 0, maxNegative, DUPLICANTSTATS.BADTRAITS);

                    AddRandomTraits(__instance, 0, 1, DUPLICANTSTATS.NEEDTRAITS);
                }
            }

            // Mathf.Abs(Util.GaussianRandom(0f, 1f));
        }
    }
}

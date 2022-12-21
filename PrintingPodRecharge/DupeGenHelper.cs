using Database;
using FUtility;
using PrintingPodRecharge.Content.Cmps;
using System.Collections.Generic;
using System.Linq;
using TUNING;
using UnityEngine;
using static MathUtil;

namespace PrintingPodRecharge
{
    public class DupeGenHelper
    {
        private static readonly HashSet<string> forbiddenNames = new HashSet<string>()
        {
           "Pener",
           "Pee"
        };

        public static readonly int[] allowedHairIds = new[]
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
            44
        };

        public static CustomDupe.MinionData GenerateRandomDupe(MinionStartingStats __instance)
        {
            if (Random.value < BundleLoader.bundleSettings.ActiveRando(__instance).ChanceForVacillatorTrait)
            {
                AddGeneShufflerTrait(__instance);
            }

            if(Mod.otherMods.IsMeepHere)
            {
                var hairColor = Mod.Settings.ColoredMeeps ? GetRandomHairColor() : Color.white;
                return new CustomDupe.MinionData(
                    hairColor, 
                    "Meep", 
                    Mod.Settings.ColoredMeeps, 
                    "MEEP",
                    __instance.personality.hair);
            }
            else
            {
                var name = SetRandomName(__instance);

                // disallow modded personalities
                if(!NAMES.Contains(__instance.NameStringKey))
                {
                    __instance.personality = Db.Get().Personalities.GetPersonalityFromNameStringKey(GetRandomNameKey());
                }

                var descKey = __instance.NameStringKey;
                var hairColor = GetRandomHairColor();

                return new CustomDupe.MinionData(
                    hairColor,
                    name,
                    true,
                    descKey,
                    allowedHairIds.GetRandom());
            }
        }


        public static void AddGeneShufflerTrait(MinionStartingStats __instance)
        {
            AddRandomTraits(__instance, 1, 1, DUPLICANTSTATS.GENESHUFFLERTRAITS);
        }

        private static readonly List<string> NAMES = new List<string>()
        {
            "CATALINA",
            "NISBET",
            "ELLIE",
            "RUBY",
            "LEIRA",
            "BUBBLES",
            "MIMA",
            "NAILS",
            "MAE",
            "GOSSMANN",
            "MARIE",
            "LINDSAY",
            "DEVON",
            "REN",
            "FRANKIE",
            "BANHI",
            "ADA",
            "HASSAN",
            "STINKY",
            "JOSHUA",
            "LIAM",
            "ABE",
            "BURT",
            "TRAVALDO",
            "HAROLD",
            "MAX",
            "ROWAN",
            "OTTO",
            "TURNER",
            "NIKOLA",
            "MEEP",
            "ARI",
            "JEAN",
            "CAMILLE",
            "ASHKAN",
            "STEVE",
            "AMARI",
            "PEI",
            "QUINN",
        };

        public static string GetRandomNameKey()
        {
            if(Mod.otherMods.IsMeepHere)
            {
                return "MEEP";
            }

            return NAMES.GetRandom();
        }

        public static int AddRandomTraits(MinionStartingStats __instance, int min, int max, List<DUPLICANTSTATS.TraitVal> pool)
        {
            if (max <= 0)
            {
                return 0;
            }

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

            return randomExtra;
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

        public static string SetRandomName(MinionStartingStats __instance)
        {
            if(Mod.otherMods.IsMeepHere)
            {
                return __instance.Name;
            }

            var name = GetRandomName();
            if (!name.IsNullOrWhiteSpace())
            {
                __instance.Name = name;
                var key = "STRINGS.DUPLICANTS.PERSONALITIES." + name.ToUpperInvariant().Replace("-", "") + ".NAME";
                Strings.Add(key, name);
                //__instance.NameStringKey = PPersonalities.SHOOK_ID;

                return name;
            }

            return "Unknown";
        }

        public static string GetRandomName()
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

        public static Color GetRandomHairColor()
        {
            return Random.ColorHSV(0, 1, 0f, 0.9f, 0.1f, 1f);
        }

        public static void Wackify(MinionStartingStats stats, GameObject gameObject)
        {
            var goodTraits = Random.value < 0.66f; // we do be fudging

            var modifiers = gameObject.AddOrGet<MinionModifiers>();

            if (goodTraits)
            {
                stats.Traits.RemoveAll(trait => !trait.PositiveTrait && trait.Id != MinionConfig.MINION_BASE_TRAIT_ID);

                AddRandomTraits(stats, 4, 8, DUPLICANTSTATS.GOODTRAITS);
                AddRandomTraits(stats, 0, 2, DUPLICANTSTATS.BADTRAITS);
                AddRandomTraits(stats, 0, 1, DUPLICANTSTATS.NEEDTRAITS);
                AddRandomTraits(stats, 1, 2, DUPLICANTSTATS.GENESHUFFLERTRAITS);
            }
            else
            {
                stats.Traits.RemoveAll(trait => trait.PositiveTrait && trait.Id != MinionConfig.MINION_BASE_TRAIT_ID);
                AddRandomTraits(stats, 0, 2, DUPLICANTSTATS.GOODTRAITS);
                AddRandomTraits(stats, 4, 8, DUPLICANTSTATS.BADTRAITS);
                AddRandomTraits(stats, 1, 1, DUPLICANTSTATS.NEEDTRAITS);
            }

            var disabledChoreGroups = new List<ChoreGroup>();
            foreach (var trait in stats.Traits)
            {
                if (trait.disabledChoreGroups != null && trait.disabledChoreGroups.Length > 0)
                {
                    disabledChoreGroups.AddRange(trait.disabledChoreGroups);
                }
            }

            if (goodTraits)
            {
                RegenerateAptitudes(stats, 3, 6);
            }
            else
            {
                RegenerateAptitudes(stats, 1, 2);
            }

            RegenerateAttributes(stats, goodTraits ? 17 : 3);
        }

        private static void RegenerateAttributes(MinionStartingStats stats, int maxCost)
        {
            var list = new List<string>(DUPLICANTSTATS.ALL_ATTRIBUTES);
            var cost = 0;

            foreach (var attribute in list)
            {
                var value = Random.Range(-10, 20);

                if (attribute == Db.Get().Attributes.Athletics.Id)
                {
                    value = Mathf.Max(value, -5);
                }

                if (cost + value > maxCost)
                {
                    value = 0;
                }

                cost += value;

                stats.StartingLevels[attribute] = value;
            }
        }

        private static int RegenerateAptitudes(MinionStartingStats stats, int min, int max)
        {
            stats.skillAptitudes = new Dictionary<SkillGroup, float>();
            var count = Random.Range(min, max);

            var list = new List<SkillGroup>(Db.Get().SkillGroups.resources);
            list.Shuffle();

            for (int i = 0; i < count; i++)
            {
                stats.skillAptitudes.Add(list[i], DUPLICANTSTATS.APTITUDE_BONUS);
            }

            return count;
        }
    }
}

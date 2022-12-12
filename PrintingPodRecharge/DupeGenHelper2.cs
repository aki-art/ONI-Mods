using FUtility;
using PrintingPodRecharge.Cmps;
using System.Collections.Generic;
using UnityEngine;

namespace PrintingPodRecharge
{
   
    public class DupeGenHelper2
    {
        private static Dictionary<MinionStartingStats, DupeGenData> rolledData = new Dictionary<MinionStartingStats, DupeGenData>();

        public class DupeGenData
        {
            public Color hairColor;
            public int hairStyle;
            public string descKey;
            public DupeType type;
            public Accessorizer accessorizer;
            public KAnimHashedString hairOverride;
        }

        public enum DupeType
        {
            Shaker,
            Wacky,
            Meep
        }

        public static bool TryGetDataForStats(MinionStartingStats stats, out DupeGenData data)
        {
            return rolledData.TryGetValue(stats, out data);
        }

        public static void AddRandomizedData(MinionStartingStats stats, DupeType type)
        {
            if(type == DupeType.Meep)
            {
                rolledData[stats] = new DupeGenData()
                {
                    hairColor = Mod.Settings.ColoredMeeps ? GetRandomHairColor() : Color.white,
                    hairStyle = stats.personality.hair,
                    hairOverride = HashCache.Get().Add(string.Format("hair_{0:000}", stats.personality.hair)),
                    descKey = "MEEP",
                    type = type
                };

                return;
            }

            var hairColor = type == DupeType.Shaker ? GetRandomHairColor() : GetWackyRandomHairColor();
            var hair = allowedHairIds.GetRandom();

            rolledData[stats] = new DupeGenData()
            {
                hairColor = hairColor,
                hairStyle = hair,
                hairOverride = HashCache.Get().Add(string.Format("hair_bleached_{0:000}", hair)),
                descKey = NAMES.GetRandom(),
                type = type,
            };
            Log.Debuglog("Set name to " + stats.Name);
        }

        public static void AfterGenerateStats(MinionStartingStats stats)
        {
            if(TryGetDataForStats(stats, out var data) && data.type != DupeType.Meep)
            {
                stats.Name = GetRandomName();
            }
        }

        public static void SetRandomName(MinionStartingStats stats)
        {
            if (!Mod.IsMeepHere)
            {
                stats.Name = GetRandomName();
            }
        }

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

        public static void ApplyRandomization(MinionStartingStats startingStats, GameObject minionGo)
        {
            Log.Debuglog("applying rando " + startingStats.Name);
            var customDupe = minionGo.AddOrGet<CustomDupe>();

            if (customDupe.initialized)
            {
                return;
            }

            if(TryGetDataForStats(startingStats, out var data))
            {
                if(data.type == DupeType.Meep)
                {
                    customDupe.runtimeHair = null;
                }
                else
                {
                    customDupe.runtimeHair = HashCache.Get().Add(string.Format("hair_bleached_{0:000}", data.hairStyle));
                }

                customDupe.dyedHair = data.type != DupeType.Meep || Mod.Settings.ColoredMeeps;
                customDupe.hairColor = data.hairColor;
                customDupe.hairID = data.hairStyle;
                customDupe.descKey = data.descKey;
                customDupe.initialized = true;

                rolledData.Remove(startingStats);
            }
        }

        public static void AlterBodyData(MinionStartingStats stats, KCompBuilder.BodyData existingValue)
        {
            Log.Debuglog("APPLY MINION DATA stats");
            if (Mod.IsMeepHere && !Mod.Settings.ColoredMeeps)
            {
                return;
            }

            if (TryGetDataForStats(stats, out var data))
            {
                TurnHairBleached(existingValue, data.hairStyle);
            }
        }

        public static void AlterBodyData(Accessorizer accessorizer, KCompBuilder.BodyData bodyData)
        {
            Log.Debuglog("APPLY MINION DATA accessorizer");

            if (Mod.IsMeepHere && !Mod.Settings.ColoredMeeps)
            {
                return;
            }

            foreach(var data in rolledData.Values)
            {
                if(data.accessorizer == accessorizer)
                {
                    Log.Debuglog("found accessorizer");
                    accessorizer.RemoveAccessory(Db.Get().AccessorySlots.Hair.Lookup(bodyData.hair));
                    TurnHairBleached(bodyData, data.hairStyle);
                    var bleachedHair = Db.Get().AccessorySlots.Hair.Lookup(bodyData.hair);
                    Log.Assert("bleahced hair", bleachedHair);

                    accessorizer.AddAccessory(Db.Get().AccessorySlots.Hair.Lookup(bodyData.hair));


                    return;
                }
            }
        }

        public static void TurnHairBleached(KCompBuilder.BodyData bodyData, int hairStyle)
        {
            var hair = HashCache.Get().Add(string.Format("hair_bleached_{0:000}", hairStyle));
            bodyData.hair = hair;
        }

        private static Color GetRandomHairColor()
        {
            return Random.ColorHSV(0, 1, 0f, 0.9f, 0.1f, 1f);
        }

        private static Color GetWackyRandomHairColor()
        {
            return Random.ColorHSV(0, 1, 0.5f, 1f, 0.4f, 1f);
        }

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
            44
        };

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

        private static readonly HashSet<string> forbiddenNames = new HashSet<string>()
        {
           "Pener",
           "Pee"
        };
    }
}

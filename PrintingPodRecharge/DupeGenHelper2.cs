using Database;
using FUtility;
using HarmonyLib;
using PrintingPodRecharge.Content.Cmps;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static STRINGS.UI.DETAILTABS;

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
                var hairPrefix = Mod.Settings.ColoredMeeps ? "hair_bleached_{0:000}" : "hair_{0:000}";

                rolledData[stats] = new DupeGenData()
                {
                    hairColor = Mod.Settings.ColoredMeeps ? GetRandomHairColor() : Color.white,
                    hairStyle = stats.personality.hair,
                    hairOverride = HashCache.Get().Add(string.Format(hairPrefix, stats.personality.hair)),
                    descKey = "MEEP",
                    type = type
                };

                return;
            }

            var hairColor = type == DupeType.Shaker ? GetRandomHairColor() : GetWackyRandomHairColor();
            var hair = allowedHairIds.GetRandom();
            var descKey = NAMES.Contains(stats.NameStringKey) ? stats.NameStringKey : NAMES.GetRandom();

            rolledData[stats] = new DupeGenData()
            {
                hairColor = hairColor,
                hairStyle = hair,
                hairOverride = HashCache.Get().Add(string.Format("hair_bleached_{0:000}", hair)),
                descKey = descKey,
                type = type,
            };
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
            if (!Mod.otherMods.IsMeepHere)
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
            var customDupe = minionGo.AddOrGet<CustomDupe>();

            if (customDupe.initialized)
            {
                return;
            }

            if(TryGetDataForStats(startingStats, out var data))
            {
                if(data.type == DupeType.Meep && !Mod.Settings.ColoredMeeps)
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

/*            if(minionGo.TryGetComponent(out Accessorizer accessorizer))
            {
                if (startingStats.personality.outfitIds.TryGetValue(ClothingOutfitUtility.OutfitType.Clothing, out var outfitID))
                {
                    var outfit = ClothingOutfitTarget.TryFromId(outfitID);
                    if (outfit.HasValue)
                    {
                        accessorizer.ApplyClothingItems(outfit.Value.ReadItemValues());
                    }
                }
            }*/
        }

        public static void AlterBodyData(MinionStartingStats stats, KCompBuilder.BodyData existingValue)
        {
            if (Mod.otherMods.IsMeepHere && !Mod.Settings.ColoredMeeps)
            {
                return;
            }

            if (TryGetDataForStats(stats, out var data))
            {
                TurnHairBleached(existingValue, data.hairStyle);
            }
        }

        public static void ReplaceAccessory(Accessorizer accessorizer, AccessorySlot slot, HashedString newAccessoryID)
        {
            var previous = accessorizer.GetAccessory(slot);
            if (accessorizer.HasAccessory(previous))
            {
                accessorizer.RemoveAccessory(accessorizer.GetAccessory(slot));
            }

            accessorizer.AddAccessory(slot.Lookup(newAccessoryID));
        }

        public static void AlterBodyData(Accessorizer accessorizer, KCompBuilder.BodyData bodyData) //, List<ResourceRef<ClothingItemResource>> clothingItems)
        {
            if (Mod.otherMods.IsMeepHere && !Mod.Settings.ColoredMeeps)
            {
                return;
            }

            if(!accessorizer.TryGetComponent(out MinionIdentity identity) || identity.personalityResourceId== null)
            {
                return;
            }

            foreach(var data in rolledData.Values)
            {
                if(data.accessorizer == accessorizer)
                {
                    var hair = HashCache.Get().Add(string.Format("hair_bleached_{0:000}", data.hairStyle));

                    ReplaceAccessory(accessorizer, Db.Get().AccessorySlots.Eyes, bodyData.eyes);
                    ReplaceAccessory(accessorizer, Db.Get().AccessorySlots.Hair, hair);
                    ReplaceAccessory(accessorizer, Db.Get().AccessorySlots.HatHair, "hat_" + HashCache.Get().Get(bodyData.hair));
                    ReplaceAccessory(accessorizer, Db.Get().AccessorySlots.HeadShape, bodyData.headShape);
                    ReplaceAccessory(accessorizer, Db.Get().AccessorySlots.Mouth, bodyData.mouth);

                    bodyData.hair = hair;

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

        public static readonly List<string> NAMES = new List<string>()
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

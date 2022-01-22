using DecorPackB.Buildings.OilLantern;
using FUtility;
using Klei.AI;
using System.Collections.Generic;
using static DecorPackB.STRINGS.DUPLICANTS.STATUSITEMS.INSPIREDRESEARCHEFFICIENCYBONUS;

namespace DecorPackB
{
    public class ModAssets
    {
        public static class Materials
        {
            public static readonly string[] FOSSIL = new string[]
            {
                    SimHashes.Fossil.ToString()
            };
        }

        public static class StatusItems
        {
            public static StatusItem awaitingFuel;

            public static void Register()
            {
                awaitingFuel = new StatusItem(Mod.PREFIX + "AwaitingFuel", "BUILDING", string.Empty, StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, false);
                awaitingFuel.SetResolveStringCallback((str, obj) =>
                {
                    if (obj is OilLantern lantern)
                    {
                        ElementConverter elementConverter = lantern.GetComponent<ElementConverter>();
                        string fuel = elementConverter.consumedElements[0].tag.ProperName();
                        string formattedMass = GameUtil.GetFormattedMass(elementConverter.consumedElements[0].massConsumptionRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
                        return string.Format(str, fuel, formattedMass);
                    }

                    return "";
                });
            }
        }

        public static class Tags
        {
            // For rooms expanded, it uses this to recognize fossil buildings
            public static readonly Tag FossilBuilding = TagManager.Create("FossilBuilding");

            // items need a special tag that is marked as "buildable material" for the game
            public static readonly Tag FossilNodule = TagManager.Create(Mod.PREFIX + "FossilNodule");

            // using a custom tag so my other mod can add it's bones to this too
            public static readonly Tag Fossil = TagManager.Create(Mod.PREFIX + "Fossil");
        }

        public class Effects
        {
            public const string INSPIRED_LOW = "Inspired1";
            public const string INSPIRED_GOOD = "Inspired2";
            public const string INSPIRED_GIANT = "Inspired3";

            public static List<Effect> GetEffectsList()
            {
                return new List<Effect>
                {
                    new Effect(INSPIRED_LOW, NAME1, TOOLTIP, 60f, true, true, false)
                    {
                        SelfModifiers = new List<AttributeModifier>() {
                            new AttributeModifier(Db.Get().Attributes.Learning.Id, 2)
                        }
                    },
                    new Effect(INSPIRED_GOOD, NAME2, TOOLTIP, 60f, true, true, false)
                    {
                        SelfModifiers = new List<AttributeModifier>() {
                            new AttributeModifier(Db.Get().Attributes.Learning.Id, 6)
                        }
                    },
                    new Effect(INSPIRED_GIANT, NAME3, TOOLTIP, Consts.CYCLE_LENGTH, true, true, false)
                    {
                        SelfModifiers = new List<AttributeModifier>() {
                            new AttributeModifier(Db.Get().Attributes.Learning.Id, 10)
                        }
                    },
                };
            }
        }
    }
}

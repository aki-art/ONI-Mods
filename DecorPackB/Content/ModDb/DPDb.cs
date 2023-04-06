using ProcGen;
using System.Collections.Generic;

namespace DecorPackB.Content.ModDb
{
    public class DPDb
    {
        public static Dictionary<SimHashes, List<IWeighted>> treasureHunterLoottable = new Dictionary<SimHashes, List<IWeighted>>()
        {

        };

        public static class BuildLocationRules
        {
            public static BuildLocationRule OnAnyWall = (BuildLocationRule)(-1569291063);
        }

        public static class Materials
        {
            public static readonly string[] FOSSIL = new string[]
            {
                DPTags.trueFossilMaterial.ToString()
            };

            public static readonly string[] FOSSIL_LITE = new string[]
            {
                DPTags.liteFossilMaterial.ToString()
            };
        }

        public static class StatusItems
        {
            public static StatusItem awaitingFuel;

            public static void Register()
            {
                /*
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
                */
            }
        }
    }
}

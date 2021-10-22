using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwelveCyclesOfChristmas.Lang
{
    public class STRINGS
    {
        public static class BUILDINGS
        {
            public static class PREFABS
            {
                public static class TDOC_FANCYFARMPLOT
                {
                    public static LocString NAME = "Fancy Planter Box";
                    public static LocString DESC = "Can fit a plant while also looking spiffy.";
                    public static LocString EFFECT = $"Grows one {UI.FormatAsLink("Plant", "PLANTS")} from a {UI.FormatAsLink("Seed", "PLANTS")}\n" +
                        "Trees grown in this may be decorated by Duplicants.";
                    public static LocString POORQUALITYNAME = "Not So Fancy Planter Box";
                    public static LocString AVERAGEQUALITYNAME = "Mediocre Fancy Planter Box";
                    public static LocString EXCELLENTQUALITYNAME = "Posh Planter Box";
                }
            }
        }
    }
}
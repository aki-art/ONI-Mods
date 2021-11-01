using TwelveCyclesOfChristmas.Buildings.SnowSculpture;
using KUI = STRINGS.UI; // to avoid conflicts with Klei STRINGS UI and mine

namespace TwelveCyclesOfChristmas
{
    class STRINGS
    {
        public class BUILDINGS
        {
            public class PREFABS
            {
                public class TWELVECYCLESOFCHRISTMAS_SNOWSCULPTURE
                {
                    public static LocString NAME = KUI.FormatAsLink("Snow Block", SnowSculptureConfig.ID.ToUpperInvariant());
                    public static LocString DESC = "Duplicants who have learned art skills can produce more decorative sculptures.";
                    public static LocString EFFECT = "Majorly increases " + KUI.FormatAsLink("Decor", "DECOR") + ", contributing to " + KUI.FormatAsLink("Morale", "MORALE") + ".\n\nMust be sculpted by a Duplicant.";
                    public static LocString POORQUALITYNAME = "\"Abstract\" Snowman";
                    public static LocString AVERAGEQUALITYNAME = "Mediocre Snowman";
                    public static LocString EXCELLENTQUALITYNAME = "Genius Snowman";
                    public static LocString DOG = "Snowdog";
                }
            }
        }

        public class UI
        {
            public class USERMENUACTIONS
            {
                public class FABULOUS
                {
                    public class ENABLED
                    {
                        public static LocString NAME = "Fabulous On";
                        public static LocString TOOLTIP = "Bring the magic!";
                    }
                    public class DISABLED
                    {
                        public static LocString NAME = "Fabulous Off";
                        public static LocString TOOLTIP = "Take away the magic.";
                    }
                }
            }
        }
    }
}

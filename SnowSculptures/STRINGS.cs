using FUtility;
using rendering;
using SnowSculptures.Content.Buildings;
using static STRINGS.UI.CLUSTERMAP.ROCKETS;

namespace SnowSculptures
{
    public class STRINGS
    {
        public class BUILDINGS
        {
            public class PREFABS
            {
                public class SNOWSCULPTURES_SNOWSCULPTURE
                {
                    public static LocString NAME = Utils.FormatAsLink("Snow Pile", SnowSculptureConfig.ID);
                    public static LocString DESC = "Duplicants who have learned art skills can produce more decorative sculptures.";
                    public static LocString EFFECT = "Majorly increases " + Utils.FormatAsLink("Decor") + ", contributing to " + Utils.FormatAsLink("Morale") + ".\n\nMust be sculpted by a Duplicant.";
                    public static LocString POORQUALITYNAME = "\"Abstract\" Snowman";
                    public static LocString AVERAGEQUALITYNAME = "Mediocre Snowman";
                    public static LocString EXCELLENTQUALITYNAME = "Genius Snowman";
                    public static LocString SNOWDOG = "Snowdog";
                }
            }
        }

        public class UI
        {
            public class SNOWMACHINESIDESCREEN
            {
                public class CONTENTS
                {
                    public class DENSITY
                    {
                        public static LocString LABEL = "Density";
                        public static LocString TOOLTIP = "How snowy it should snow.";
                    }

                    public class SPEED
                    {
                        public static LocString LABEL = "Speed";
                        public static LocString TOOLTIP = "How fast the snow should fall.";
                    }

                    public class LIFETIME
                    {
                        public static LocString LABEL = "Lifetime";
                        public static LocString TOOLTIP = "Life length of a single particle.";
                    }

                    public class TURBULENCE
                    {
                        public static LocString LABEL = "Turbulence";
                        public static LocString TOOLTIP = "The higher the value, the more random and chaotic the snowfall is.";
                    }
                }
            }
        }
    }
}

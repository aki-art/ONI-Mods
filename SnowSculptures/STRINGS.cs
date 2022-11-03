using FUtility;
using SnowSculptures.Content.Buildings;

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
    }
}

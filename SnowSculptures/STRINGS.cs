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

                public class SNOWSCULPTURES_SNOWMACHINE
                {
                    public static LocString NAME = Utils.FormatAsLink("Snow Machine", SnowMachineConfig.ID);
                    public static LocString DESC = "Delights your duplicants with pretty snow."; 
                    public static LocString EFFECT = "Increases " + Utils.FormatAsLink("Decor") + ", contributing to " + Utils.FormatAsLink("Morale") + ".\n\nMust be powered.";
                }

                public class SNOWSCULPTURES_GLASSCASE
                {
                    public static LocString NAME = Utils.FormatAsLink("Glass Case", GlassCaseConfig.ID);
                    public static LocString DESC = "Protects Ice and Snow Sculptures.";
                    public static LocString EFFECT = "Thermally insulates Snow and Ice Sculptures, so they cannot melt or exchange temperature with their environment.";
                }
            }

            public class STATUSITEMS
            {
                public class SNOWSCULPTURES_SEALEDSTATUSITEM
                {
                    public static LocString NAME = "Vacuum sealed";
                    public static LocString TOOLTIP = "This building is thermally insulated, and cannot melt or exchange heat with it's surroundings.";
                }

                public class SNOWSCULPTURES_SOMEHOWSEALEDSTATUSITEM
                {
                    public static LocString NAME = "Somehow still sealed";
                    public static LocString TOOLTIP = "This building is thermally insulated, and cannot melt or exchange heat with it's surroundings.";
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

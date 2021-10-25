using KUI = STRINGS.UI; // to avoid conflicts with Klei STRINGS UI and mine

namespace DecorPackA
{
    class STRINGS
    {
        public class BUILDINGS
        {
            public class PREFABS
            {
                public class DP_GLASSSCULPTURE
                {
                    public static LocString NAME = KUI.FormatAsLink("Glass Block", DPBuilding.GlassSculpture.GlassSculptureConfig.ID.ToUpperInvariant());
                    public static LocString DESC = "Duplicants who have learned art skills can produce more decorative sculptures.";
                    public static LocString EFFECT = "Majorly increases " + KUI.FormatAsLink("Decor", "DECOR") + ", contributing to " + KUI.FormatAsLink("Morale", "MORALE") + ".\n\nMust be sculpted by a Duplicant.";
                    public static LocString POORQUALITYNAME = "\"Abstract\" Glass Sculpture";
                    public static LocString AVERAGEQUALITYNAME = "Mediocre Glass Sculpture";
                    public static LocString EXCELLENTQUALITYNAME = "Genius Glass Sculpture";
                }

                public class DP_MOODLAMP
                {
                    public static LocString NAME = KUI.FormatAsLink("Mood Lamp", DPBuilding.MoodLamp.MoodLampConfig.ID.ToUpperInvariant());
                    public static LocString DESC = "Light reduces Duplicant stress and is required to grow certain plants.";
                    public static LocString EFFECT = "Provides " + KUI.FormatAsLink("Light", "LIGHT") + " when " + KUI.FormatAsLink("Powered", "POWER") + ".\n\nDuplicants can operate buildings more quickly when the building is lit.";
                }

                public class DP_DEFAULTSTAINEDGLASSTILE
                {
                    public static LocString NAME = KUI.FormatAsLink("Stained Glass Tile", DPBuilding.StainedGlassTile.DefaultStainedGlassTileConfig.ID.ToUpperInvariant());
                    public static LocString DESC = "Stained glass tiles transparent tiles that provide a fashionable barrier against liquid and gas.";
                    public static LocString EFFECT = $"Used to build the walls and floors of rooms.\n\n" +
                        $"Allows {KUI.FormatAsLink("Light", "LIGHT")} and {KUI.FormatAsLink("Decor", "DECOR")} pass through.";
                }
            }
        }

        public class UI
        {
            public class UISIDESCREENS
            {
                public class MOODLAMP_SIDE_SCREEN
                {
                    public static LocString TITLE = "Lamp type";
                }
            }

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

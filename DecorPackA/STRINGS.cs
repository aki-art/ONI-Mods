using DecorPackA.Buildings.StainedGlassTile;
using KUI = STRINGS.UI;

namespace DecorPackA
{
    public class STRINGS
    {
        public class BUILDINGS
        {
            public class PREFABS
            {
                public class DECORPACKA_GLASSSCULPTURE
                {
                    public static LocString NAME = KUI.FormatAsLink("Glass Block", Buildings.GlassSculpture.GlassSculptureConfig.ID.ToUpperInvariant());
                    public static LocString DESC = "Duplicants who have learned art skills can produce more decorative sculptures.";
                    public static LocString EFFECT = "Majorly increases " + KUI.FormatAsLink("Decor", "DECOR") + ", contributing to " + KUI.FormatAsLink("Morale", "MORALE") + ".\n\nMust be sculpted by a Duplicant.";
                    public static LocString POORQUALITYNAME = "\"Abstract\" Glass Sculpture";
                    public static LocString AVERAGEQUALITYNAME = "Mediocre Glass Sculpture";
                    public static LocString EXCELLENTQUALITYNAME = "Genius Glass Sculpture";
                }

                public class DECORPACKA_MOODLAMP
                {
                    public static LocString NAME = KUI.FormatAsLink("Mood Lamp", Buildings.MoodLamp.MoodLampConfig.ID.ToUpperInvariant());
                    public static LocString DESC = "Light reduces Duplicant stress and is required to grow certain plants.";
                    public static LocString EFFECT = "Provides " + KUI.FormatAsLink("Light", "LIGHT") + " when " + KUI.FormatAsLink("Powered", "POWER") + ".\n\nDuplicants can operate buildings more quickly when the building is lit.";

                    public class VARIANT
                    {
                        public static LocString RANDOM = "Random";
                        public static LocString UNICORN = "Unicorn";
                        public static LocString MORB = "Morb";
                        public static LocString DENSE = "Dense Puft";
                        public static LocString MOON = "Moon";
                        public static LocString BROTHGAR = "Brothgar Logo";
                        public static LocString SATURN = "Saturn";
                        public static LocString PIP = "Pip";
                        public static LocString D6 = "D6";
                        public static LocString OGRE = "Shrumal Ogre";
                        public static LocString TESSERACT = "Tesseract";
                        public static LocString CAT = "Cat";
                        public static LocString OWO = "OwO Slickster";
                        public static LocString STAR = "Star";
                        public static LocString ROCKET = "Rocket";

                        // v 1.2
                        public static LocString LUMAPLAYS = "Luma Plays Logo";
                        public static LocString KONNY87 = "Konny87 Logo";
                        public static LocString REDSTONE_LAMP = "Redstone Lamp";
                        public static LocString ARCHIVE_TUBE = "Archive Tube";
                        public static LocString KLEI_MUG = "Klei Mug";
                        public static LocString DIAMONDHATCH = "Diamond Hatch";
                        public static LocString GLITTERPUFT = "Glitter Puft";
                        public static LocString AI = "AI in a jar";
                        public static LocString SLAGMITE = "Slagmite"; // Slag (material) + Mite (creature), also pun on stalagmite
                        public static LocString CUDDLE_PIP = "Cuddle Pip";
                    }
                }

                public class DECORPACKA_DEFAULTSTAINEDGLASSTILE
                {
                    public static LocString NAME = KUI.FormatAsLink("Stained Glass Tile", DefaultStainedGlassTileConfig.DEFAULT_ID.ToUpperInvariant());
                    public static LocString STAINED_NAME = "{element} " + KUI.FormatAsLink("Stained Glass Tile", DefaultStainedGlassTileConfig.DEFAULT_ID.ToUpperInvariant());
                    public static LocString DESC = $"Stained glass tiles are transparent tiles that provide a fashionable barrier against liquid and gas.";
                    public static LocString EFFECT = $"Used to build the walls and floors of rooms.\n\n" +
                        $"Allows {KUI.FormatAsLink("Light", "LIGHT")} and {KUI.FormatAsLink("Decor", "DECOR")} pass through.\n\n{KUI.FormatAsLink("Open Palette", PaletteCodexEntry.CATEGORY)}";
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

            public class BUILDINGEFFECTS
            {
                public static LocString THERMALCONDUCTIVITYCHANGE = "Thermal Conductivity: {0}";

                public class TOOLTIP
                {
                    public static LocString HIGHER = "higher";
                    public static LocString LOWER = "lower";

                    public static LocString THERMALCONDUCTIVITYCHANGE = "The dye {dyeElement} has {higherOrLower} thermal conductivity than {baseElement}, modifying it by {percent}.";
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

        public class MISC
        {
            public class TAGS
            {
                public static LocString DECORPACKA_STAINEDGLASSMATERIAL = "Glass Dye";
            }
        }
    }
}

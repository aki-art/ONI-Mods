using KUI = STRINGS.UI;

namespace DecorPackB
{
    public class STRINGS
    {
        public class BUILDINGS
        {
            public class PREFABS
            {
                public class DECORPACKB_FOSSILDISPLAY
                {
                    public static LocString NAME = KUI.FormatAsLink("Fossil Display", Buildings.FossilDisplay.FossilDisplayConfig.ID.ToUpperInvariant());
                    public static LocString DESC = "Duplicants who have learned research skills can produce more accurate reconstructions.";
                    public static LocString EFFECT = "Majorly increases " + KUI.FormatAsLink("Decor", "DECOR") + ", contributing to " + KUI.FormatAsLink("Morale", "MORALE") + ".\n\nMust be sculpted by a Duplicant.";
                    public static LocString POORQUALITYNAME = "\"Imaginative\" Fossil Display";
                    public static LocString AVERAGEQUALITYNAME = "Fossil Display";

                    public static LocString ASSEMBLEDBY = "Assembled by {duplicantName}";

                    public class VARIANT
                    {
                        public static class HUMAN
                        {
                            public static LocString NAME = "Questionable Human Skeleton";
                            public static LocString DESC = "Humans were a species that went extinct in a war against Dodos 4 million years ago. \n" +
                                "They had long icky arms with 5 fingers which they used to consume their only nutrition, Cheeto, a type of orange worm. \n" +
                                "\n" +
                                "This seems inaccurate.";
                        }

                        public static class TRILOBITE
                        {
                            public static LocString NAME = "Trilobite Fossil";
                            public static LocString DESC = "...";
                        }

                        public static class SPIDER
                        {
                            public static LocString NAME = "Questionable Spider Skeleton";
                            public static LocString DESC = "Spiders was a vertebrae creature obsessed with wrapping other creatures in warm and silky blankets, as a way to show it's friendly nature.\n" +
                                "Some species were well known for their incredible pastries.\n" +
                                "\n" +
                                "This seems inaccurate.";
                        }

                        public static class PACU
                        {
                            public static LocString NAME = "Pacumidae";
                            public static LocString DESC = "<i>Pacumidae</i> are a group of ancient fish that once lived on these Asteroids. If only such beautiful creatures could still grace our world...";
                        }

                        public static class PARASAUROLOPHUS
                        {
                            public static LocString NAME = "Tiny Parasaurolophus";
                            public static LocString DESC = "A regular Parasaurolophus skeleton which appears to have been hit by a shrink ray... or maybe it was a tiny dinosaur.";
                        }

                        public static class BEEFALO
                        {
                            public static LocString NAME = "Beefalo";
                            public static LocString DESC = "Ancient humans used to settle around Beefalo herds.";
                        }
                        
                        public static class HELLHOUND
                        {
                            public static LocString NAME = "Hell Hound";
                            public static LocString DESC = "...";
                        }

                        public static class CATCOON
                        {
                            public static LocString NAME = "Catcoon";
                            public static LocString DESC = "...";
                        }

                        public static class VOLGUS
                        {
                            public static LocString NAME = "Volgus";
                            public static LocString DESC = "...";
                        }
                    }
                }

                public class DECORPACKB_MASSIVE_FOSSILDISPLAY
                {
                    public static LocString NAME = KUI.FormatAsLink("Huge Fossil Display", "");
                    public static LocString DESC = "Duplicants who have learned research skills can produce more accurate reconstructions."; 
                    public static LocString EFFECT = "Majorly increases " + KUI.FormatAsLink("Decor", "DECOR") + ", contributing to " + KUI.FormatAsLink("Morale", "MORALE") + ".\n\nMust be sculpted by a Duplicant.";

                    public class VARIANT
                    {
                        public static class TREX
                        {
                            public static LocString NAME = "Tyrannosaurus Rex";
                            public static LocString DESC = "...";
                        }

                        public static class PTERODACTYL
                        {
                            public static LocString NAME = "Pterodactyl";
                            public static LocString DESC = "...";
                        }

                        public static class DEERCLOPS
                        {
                            public static LocString NAME = "Deerclops";
                            public static LocString DESC = "...";
                        }

                        public static class PUGALISK
                        {
                            public static LocString NAME = "Pugalisk";
                            public static LocString DESC = "...";
                        }
                    }
                }

                public class DECORPACKB_FOUNTAIN
                {
                    public static LocString NAME = KUI.FormatAsLink("Fountain Kit", "");
                    public static LocString DESC = "Duplicants who have learned art skills can produce more decorative sculptures.";
                    public static LocString EFFECT = "Majorly increases " + KUI.FormatAsLink("Decor", "DECOR") + ", contributing to " + KUI.FormatAsLink("Morale", "MORALE") + ".\n\nMust be sculpted by a Duplicant.";
                    public static LocString POORQUALITYNAME = "\"Abstract\" Fountain";
                    public static LocString AVERAGEQUALITYNAME = "Mediocre Fountain";
                    public static LocString GENIUSQUALITYNAME = "Genius Fountain";
                }

                public class DECORPACKB_OILLANTERN
                {
                    public static LocString NAME = KUI.FormatAsLink("Lantern", "");
                    public static LocString DESC = "Light reduces Duplicant stress and is required to grow certain plants.";
                    public static LocString EFFECT = "Provides " + KUI.FormatAsLink("Light", "LIGHT") + " when supplied with {Element}.\n\nDuplicants can operate buildings more quickly when the building is lit.";
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

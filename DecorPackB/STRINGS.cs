using DecorPackB.Content.Buildings;
using FUtility;

namespace DecorPackB
{
    public class STRINGS
    {
        public class ITEMS
        {
            public class DECORPACKB_FOSSILNODULE
            {
                public static LocString NAME = "Fossil Nodule";
                public static LocString DESC = "...";
            }
        }

        public class BUILDINGS
        {
            public class PREFABS
            {
                public class DECORPACKB_FOSSILDISPLAY
                {
                    public static LocString NAME = Utils.FormatAsLink("Fossil Display", FossilDisplayConfig.ID);
                    public static LocString DESC = "Duplicants who have learned research skills can produce more accurate reconstructions.";
                    public static LocString EFFECT = "Majorly increases " + Utils.FormatAsLink("Decor", "DECOR") + ", contributing to " + Utils.FormatAsLink("Morale", "MORALE") + ".\n\nMust be sculpted by a Duplicant.";

                    public static LocString ASSEMBLEDBY = "Reconstruction by {duplicantName}";

                    public class VARIANT
                    {
                        public static class HUMAN
                        {
                            public static LocString NAME = "\"Imaginative\" Human Skeleton";
                            public static LocString LORE = "Humans were a species that went extinct in a war against Dodos 4 million years ago. \n" +
                                "They had long icky arms with 5 fingers which they used to consume their only nutrition, Cheeto, a type of orange worm. \n" +
                                "\n" +
                                "This seems inaccurate.";
                        }

                        public static class SPIDER
                        {
                            public static LocString NAME = "\"Imaginative\" Spider Skeleton";
                            public static LocString LORE = "The Spider was a creature obsessed with wrapping other creatures in warm and silky blankets, as a way to show it's friendly nature.\n" +
                                "Some species were well known for their incredible pastries.\n" +
                                "\n" +
                                "This seems inaccurate.";
                        }

                        public static class UNICORN
                        {
                            public static LocString NAME = "\"Imaginative\" Unicorn Skeleton";
                            public static LocString LORE = "..." +
                                "\n" +
                                "This seems inaccurate.";
                        }

                        public static class PAWPRINT
                        {
                            public static LocString NAME = "Mud Prints";
                            public static LocString LORE = "Various imprints of paw and feet marks.";
                        }

                        public static class PACU
                        {
                            public static LocString NAME = "Pacumidae";
                            public static LocString LORE = "<i>Pacumidae</i> are a group of ancient fish that once lived on these Asteroids. If only such beautiful creatures could still grace our world...";
                        }

                        public static class DODO
                        {
                            public static LocString NAME = "Dodo";
                            public static LocString LORE = "...";
                        }

                        public static class TRILOBITE
                        {
                            public static LocString NAME = "Trilobites";
                            public static LocString LORE = "...";
                        }

                        public static class MINIPARA
                        {
                            public static LocString NAME = "Tiny Parasaurolophus";
                            public static LocString LORE = "A regular Parasaurolophus skeleton which appears to have been hit by a shrink ray... or maybe it was always just a tiny dinosaur.";
                        }

                        public static class BEEFALO
                        {
                            public static LocString NAME = "Beefalo";
                            public static LocString LORE = "Ancient humans used to settle around Beefalo herds.";
                        }

                        public static class HELLHOUND
                        {
                            public static LocString NAME = "Hell Hound";
                            public static LocString LORE = "...";
                        }

                        public static class CATCOON
                        {
                            public static LocString NAME = "Catcoon";
                            public static LocString LORE = "...";
                        }

                        public static class VOLGUS
                        {
                            public static LocString NAME = "Volgus";
                            public static LocString LORE = "...";
                        }

                        public static class MICRORAPTOR
                        {
                            public static LocString NAME = "Microraptor";
                            public static LocString LORE = "...";
                        }
                    }
                }

                public class DECORPACKB_GIANTFOSSILDISPLAY
                {
                    public static LocString NAME = Utils.FormatAsLink("Giant Fossil Display", GiantFossilDisplayConfig.ID);
                    public static LocString DESC = "Duplicants who have learned research skills can produce more accurate reconstructions.";
                    public static LocString EFFECT = "Majorly increases " + Utils.FormatAsLink("Decor", "DECOR") + ", contributing to " + Utils.FormatAsLink("Morale", "MORALE") + ".\n\nMust be sculpted by a Duplicant.";

                    public class VARIANT
                    {
                        public static class BRONTO
                        {
                            public static LocString NAME = "Brontosaurus";
                            public static LocString LORE = "...";
                        }

                        public static class TREX
                        {
                            public static LocString NAME = "Tyrannosaurus Rex";
                            public static LocString LORE = "...";
                        }

                        public static class TRICERATOPS
                        {
                            public static LocString NAME = "Triceratops";
                            public static LocString LORE = "...";
                        }

                        public static class PTERODACTYL
                        {
                            public static LocString NAME = "Pterodactyl";
                            public static LocString LORE = "...";
                        }

                        public static class SPINOSAURUS
                        {
                            public static LocString NAME = "Spinosaurus";
                            public static LocString LORE = "...";
                        }

                        public static class DEERCLOPS
                        {
                            public static LocString NAME = "Deerclops";
                            public static LocString LORE = "...";
                        }

                        public static class PUGALISK
                        {
                            public static LocString NAME = "Pugalisk";
                            public static LocString LORE = "...";
                        }

                        public static class LIVAYATAN
                        {
                            public static LocString NAME = "Livayatan";
                            public static LocString LORE = "Livayatan Melville was one of the largest predators to have every existed. They were a type of whale, named after the mythical Leviathan and the author of Moby Dick, Herman Melville. Livayatans most likely went extinct in the late Miocene due to globally declining temperatures.";
                        }
                    }
                }

                public class DECORPACKB_FOUNTAIN
                {
                    public static LocString NAME = Utils.FormatAsLink("Fountain Kit", "");
                    public static LocString DESC = "Duplicants who have learned art skills can produce more decorative sculptures.";
                    public static LocString EFFECT = "Majorly increases " + Utils.FormatAsLink("Decor", "DECOR") + ", contributing to " + Utils.FormatAsLink("Morale", "MORALE") + ".\n\nMust be sculpted by a Duplicant.";
                    public static LocString POORQUALITYNAME = "\"Abstract\" Fountain";
                    public static LocString AVERAGEQUALITYNAME = "Mediocre Fountain";
                    public static LocString GENIUSQUALITYNAME = "Genius Fountain";
                }

                public class DECORPACKB_OILLANTERN
                {
                    public static LocString NAME = Utils.FormatAsLink("Lantern", "");
                    public static LocString DESC = "Light reduces Duplicant stress and is required to grow certain plants.";
                    public static LocString EFFECT = "Provides " + Utils.FormatAsLink("Light", "LIGHT") + " when supplied with {Element}.\n\nDuplicants can operate buildings more quickly when the building is lit.";
                }
            }
        }

        public class DUPLICANTS
        {
            public class STATUSITEMS
            {
                public class INSPIREDRESEARCHEFFICIENCYBONUS
                {
                    public static LocString NAME1 = "Midly Curious";
                    public static LocString NAME2 = "Thirsting for knowledge";
                    public static LocString NAME3 = "Expanded mind";
                    public static LocString TOOLTIP = "This Duplicant can't wait to learn more about their World!";
                }
            }
        }

        public class MISC
        {
            public class TAGS
            {
                public static LocString FOSSILBUILDING = "Fossil";
                public static LocString FOSSIL = "Fossil";
                public static LocString FOSSILNODULE = "Fossil Nodule";
            }
        }

        public class UI
        {
            public static class TOOLTIPS
            {
                public static LocString HELP_BUILDLOCATION_ON_ANY_WALL = "Must be placed on the ground, ceiling, or next to a wall.";
            }
        }
    }
}

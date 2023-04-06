using DecorPackB.Content.Defs.Buildings;
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

        public static class EFFECTS
        {
            public static class DECORPACKB_INSPIRED_LOW
            {
                public static LocString NAME = "Midly Curious";
                public static LocString DESC = "This duplicant has seen a somewhat interesting thing.";
            }

            public static class DECORPACKB_INSPIRED_OKAY
            {
                public static LocString NAME = "Inspired";
                public static LocString DESC = "This duplicant has seen a decently interesting thing.";
            }

            public static class DECORPACKB_INSPIRED_GREAT
            {
                public static LocString NAME = "Awestruck";
                public static LocString DESC = "This duplicant has seen amazing discoveries and cannot wait to learn more about their world.";
            }

            public static class DECORPACKB_INSPIRED_GIANT
            {
                public static LocString NAME = "Expanded mind";
                public static LocString DESC = "This duplicant is greatly inspired by the wonders of this world.";
            }
        }

        public class TWITCH
        {
            public class LUCKY_POTS
            {
                public static LocString NAME = "Lucky Pots";
                public static LocString TOAST = "Pots Pots Pots Pots Pots Pots Pots Pots";
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
                            public static LocString NAME = Utils.FormatAsLink("\"Imaginative\" Human Skeleton", FossilDisplayConfig.ID);
                            public static LocString LORE = "Humans were a species that went extinct in a war against Dodos 4 million years ago. \n" +
                                "They had long icky arms with 5 fingers which they used to consume their only nutrition, Cheeto, a type of orange worm. \n" +
                                "\n" +
                                "This seems inaccurate.";
                        }

                        public static class DOGGY
                        {
                            public static LocString NAME = Utils.FormatAsLink("\"Imaginative\" Dog Skeleton", FossilDisplayConfig.ID);
                            public static LocString LORE = "" +
                                "\n" +
                                "This seems inaccurate.";
                        }


                        public static class SPIDER
                        {
                            public static LocString NAME = Utils.FormatAsLink("\"Imaginative\" Spider Skeleton", FossilDisplayConfig.ID);
                            public static LocString LORE = "The Spider was a creature obsessed with wrapping other creatures in warm and silky blankets, as a way to show it's friendly nature.\n" +
                                "Some species were well known for their incredible pastries.\n" +
                                "\n" +
                                "This seems inaccurate.";
                        }

                        public static class UNICORN
                        {
                            public static LocString NAME = Utils.FormatAsLink("\"Imaginative\" Unicorn Skeleton", FossilDisplayConfig.ID);
                            public static LocString LORE = "..." +
                                "\n" +
                                "This seems inaccurate.";
                        }

                        public static class PAWPRINT
                        {
                            public static LocString NAME = Utils.FormatAsLink("Mud Prints", FossilDisplayConfig.ID);
                            public static LocString LORE = "Various imprints of paw and feet marks.";
                        }

                        public static class PACU
                        {
                            public static LocString NAME = Utils.FormatAsLink("Three Pacus", FossilDisplayConfig.ID);
                            public static LocString LORE = "<i>Pacumidae</i> are a group of ancient fish that once lived on these Asteroids. " +
                                "If only such beautiful creatures could still grace our world...";
                        }

                        public static class DODO
                        {
                            public static LocString NAME = Utils.FormatAsLink("Dodo", FossilDisplayConfig.ID);
                            public static LocString LORE = "...";
                        }

                        public static class TRILOBITE
                        {
                            public static LocString NAME = Utils.FormatAsLink("Trilobites", FossilDisplayConfig.ID);
                            public static LocString LORE = "...";
                        }

                        public static class MINIPARA
                        {
                            public static LocString NAME = Utils.FormatAsLink("Tiny Parasaurolophus", FossilDisplayConfig.ID);
                            public static LocString LORE = "A regular Parasaurolophus skeleton which appears to have been hit by a shrink ray. " +
                                "That, or the artist misjudged the scale.";
                        }

                        public static class BEEFALO
                        {
                            public static LocString NAME = Utils.FormatAsLink("Beefalo", FossilDisplayConfig.ID);
                            public static LocString LORE = "Ancient humans used to settle around Beefalo herds.";
                        }

                        public static class HELLHOUND
                        {
                            public static LocString NAME = Utils.FormatAsLink("Hell Hound", FossilDisplayConfig.ID);
                            public static LocString LORE = "...";
                        }

                        public static class CATCOON
                        {
                            public static LocString NAME = Utils.FormatAsLink("Catcoon", FossilDisplayConfig.ID);
                            public static LocString LORE = "...";
                        }

                        public static class GLOMMER
                        {
                            public static LocString NAME = Utils.FormatAsLink("Oddly cute ancient insect", FossilDisplayConfig.ID);
                            public static LocString LORE = "...";
                        }


                        public static class VOLGUS
                        {
                            public static LocString NAME = Utils.FormatAsLink("Volgus", FossilDisplayConfig.ID);
                            public static LocString LORE = "...";
                        }

                        public static class MICRORAPTOR
                        {
                            public static LocString NAME = Utils.FormatAsLink("Microraptor", FossilDisplayConfig.ID);
                            public static LocString LORE = "...";
                        }

                        public static class AMMONITE
                        {
                            public static LocString NAME = Utils.FormatAsLink("Ammonite", FossilDisplayConfig.ID);
                            public static LocString LORE = "...";
                        }

                        public static class ANCIENTSPECIMENT
                        {
                            public static LocString NAME = Utils.FormatAsLink("Ancient Specimen", FossilDisplayConfig.ID);
                            public static LocString LORE = "...";
                        }

                        public static class PAWPRINTS
                        {
                            public static LocString NAME = Utils.FormatAsLink("Mudprints", FossilDisplayConfig.ID);
                            public static LocString LORE = "...";
                        }

                        public static class ANCIENTSPECIMENAMBER
                        {
                            public static LocString NAME = Utils.FormatAsLink("Ancient Amber Inclusion", FossilDisplayConfig.ID);
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
                            public static LocString NAME = Utils.FormatAsLink("Brontosaurus", GiantFossilDisplayConfig.ID);
                            public static LocString LORE = "...";
                        }

                        public static class TREX
                        {
                            public static LocString NAME = Utils.FormatAsLink("Tyrannosaurus Rex", GiantFossilDisplayConfig.ID);
                            public static LocString LORE = "...";
                        }

                        public static class TRICERATOPS
                        {
                            public static LocString NAME = Utils.FormatAsLink("Triceratops", GiantFossilDisplayConfig.ID);
                            public static LocString LORE = "...";
                        }

                        public static class PTERODACTYL
                        {
                            public static LocString NAME = Utils.FormatAsLink("Pterodactyl", GiantFossilDisplayConfig.ID);
                            public static LocString LORE = "...";
                        }

                        public static class SPINOSAURUS
                        {
                            public static LocString NAME = Utils.FormatAsLink("Spinosaurus", GiantFossilDisplayConfig.ID);
                            public static LocString LORE = "...";
                        }

                        public static class DEERCLOPS
                        {
                            public static LocString NAME = Utils.FormatAsLink("Deerclops", GiantFossilDisplayConfig.ID);
                            public static LocString LORE = "...";
                        }

                        public static class PUGALISK
                        {
                            public static LocString NAME = Utils.FormatAsLink("Pugalisk", GiantFossilDisplayConfig.ID);
                            public static LocString LORE = "...";
                        }

                        public static class LIVAYATAN
                        {
                            public static LocString NAME = Utils.FormatAsLink("Livayatan", GiantFossilDisplayConfig.ID);
                            public static LocString LORE = "Livayatan Melville was one of the largest predators to have every existed. They were a type of whale, named after the mythical Leviathan and the author of Moby Dick, Herman Melville. Livayatans most likely went extinct in the late Miocene due to globally declining temperatures.";
                        }
                    }
                }

                public class DECORPACKB_POT
                {
                    public class VARIANTS
                    {
                        public class ANGRYLETTUCE
                        {
                            public static LocString NAME = "Lettuce at peace";
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

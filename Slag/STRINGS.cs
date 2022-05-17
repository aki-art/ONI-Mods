using KUI = STRINGS.UI;

namespace Slag
{
    public class STRINGS
    {
        public class RESEARCH
        {
            public class TECHS
            {
                public class ADVANCEDINSULATION
                {
                    public static LocString NAME = "Advanced Insulation";
                    public static LocString DESC = "Advanced Insulation";
                }
            }
        }

        public class COMETS
        {
            public class EGGCOMET
            {
                // Mite + Meteor. It is actually a mite egg.
                public static LocString NAME = "Miteor";
            }
        }

        public class CREATURES
        {
            public class STATS
            {
                public class SHELLGROWTH
                {
                    public static LocString NAME = "Shell Growth";

                    public static LocString TOOLTIP = "The amount of time required for this critter to regrow its shell";
                }
            }

            public class STATUSITEMS
            {
                public class ELEMENT_GROWTH_GROWING
                {
                    public static LocString NAME = "Happy Critter";

                    public static LocString TOOLTIP = "This critter's Shell Growth rate is optimal\n\nPreferred food temperature range: {templo}-{temphi}";

                    public static LocString PREFERRED_TEMP = "Last eaten: {element} at {temperature}";
                }

                public class ELEMENT_GROWTH_STUNTED
                {
                    public static LocString NAME = "Picky Eater: {reason}";

                    public static LocString TOO_HOT = "Too Hot";

                    public static LocString TOO_COLD = "Too Cold";

                    public static LocString TOOLTIP = "";
                }
            }

            public class FAMILY
            {
                public static LocString MITE = KUI.FormatAsLink("Mite", "MITESPECIES");
            }
            public class FAMILY_PLURAL
            {
                public static LocString MITESPECIES = KUI.FormatAsLink("Mites", "MITESPECIES");
            }

            public class SPECIES
            {
                public class SLAGMITE
                {
                    public static LocString NAME = KUI.FormatAsLink("Slagmite", "SLAGMITE");
                    public static LocString DESC = $"Slagmites eat metal rich waste procucts.\n\nThe shells they leave behind after molting can be crushed into various ores.";
                    public static LocString EGG_NAME = KUI.FormatAsLink("Slagmite Egg", "SLAGMITE");
                    public class BABY
                    {
                        public static LocString NAME = KUI.FormatAsLink("Slagmite Larva", "SLAGMITE");
                        public static LocString DESC = $"A cute little { NAME }.\n\nIn time it will mature into a fully grown { SLAGMITE.NAME }.";
                    }
                }
                public class GLEAMITE
                {
                    public static LocString NAME = KUI.FormatAsLink("Gleamite", "GLEAMITE");
                    public static LocString DESC = $"Gleamites eat metal rich waste products.\n\nThe shells they leave behind after molting can be crushed into various refined metals.";
                    public static LocString EGG_NAME = KUI.FormatAsLink("Gleamite Egg", "GLEAMITE");
                    public class BABY
                    {
                        public static LocString NAME = KUI.FormatAsLink("Gleamite Larva", "GLEAMITE");
                        public static LocString DESC = $"A cute little { NAME }.\n\nIn time it will mature into a fully grown { GLEAMITE.NAME }.";
                    }
                }
                public class DRECKO
                {
                    public class VARIANT_ROCKWOOL
                    {
                        public static LocString NAME = KUI.FormatAsLink("Rockwool Drecko", "DRECKOROCKWOOL");
                        public static LocString DESC = $"Rockwool Dreckos are nonhostile critters that graze only on live Mealwood.\n\nTheir backsides are covered in thick woolly fibers that only grow in Carbon Dioxide climates.";
                        public static LocString EGG_NAME = KUI.FormatAsLink("Rockwool Drecklet Egg", "DRECKOROCKWOOL");

                        public class BABY
                        {
                            public static LocString NAME = KUI.FormatAsLink("Rockwool Drecklet", "DRECKOROCKWOOL");
                            public static LocString DESC = $"A little, bug-eyed {NAME}.\n\nIn time it will mature into a fully grown { VARIANT_ROCKWOOL.NAME }.";
                        }
                    }
                }
            }
        }

        public class ELEMENTS
        {
            // Metal rich slag, waste of IRL metal refinery processes
            public class SLAG
            {
                public static LocString NAME = KUI.FormatAsLink("Slag", "SLAG");
                public static LocString DESC = $"Slag is a byproduct of Metal refining processes.";
            }

            // Generic slag glass, not really based on any specific IRL variant
            public class SLAGGLASS
            {
                public static LocString NAME = KUI.FormatAsLink("Slag Glass", "SLAGGLASS");
                public static LocString DESC = $"Slag Glass is a brittle, semi-transparent substance formed from the byproducts of metal refining.";
            }

            public class MOLTENSLAGGLASS
            {
                public static LocString NAME = KUI.FormatAsLink("Slag Glass", "MOLTENSLAGGLASS");
                public static LocString DESC = $"Slag Glass is a brittle, semi-transparent substance formed from the byproducts of metal refining.";
            }
        }

        public class BUILDINGS
        {
            public class PREFABS
            {
                public class METALREFINERY
                {
                    public static LocString RECIPE_DESCRIPTION = "Extracts pure {0} from {1}, using {2} and {3}.";
                    public static LocString RECIPE_NAME = "<b>{Slag}</b> & {Ore} to {Metal}";
                }

                public class SLAG_SPINNER
                {
                    // "Centrifuge" also works
                    public static LocString NAME = "Spinner";
                    public static LocString DESC = $"Spinners use centrifugal force to spin various materials into fibers.";
                    public static LocString EFFECT = "Produces fibers from solids.\n\nDuplicants will not fabricate items unless recipes are queued.";
                }

                public class SLAG_DENSEINSULATIONTILES
                {
                    public static LocString NAME = KUI.FormatAsLink("Dense Insulated Tile", "DENSEINSULATIONTILE");
                    public static LocString DESC = "The even lower thermal conductivity of dense insulated tiles drastically slows any heat passing through them.";
                    public static LocString EFFECT = "Used to build the walls and floors of rooms.\n\nGreatly reduces " + KUI.FormatAsLink("Heat", "HEAT") + " transfer between walls, retaining ambient heat in an area.";
                }

                public class SLAG_INSULATEDWINDOWTILE
                {
                    public static LocString NAME = KUI.FormatAsLink("Insulated Window Tile", "INSULATEDWINDOWTILE");
                    public static LocString DESC = "These window tiles allow light and decor to pass through, but not so much the temperature.";
                    public static LocString EFFECT = "Used to build the walls and floors of rooms.\n\nModerately reduces " + KUI.FormatAsLink("Heat", "HEAT") + " transfer between walls, retaining ambient heat in an area.";
                }

                public class SLAG_INSULATEDMECHANIZEDAIRLOCK
                {
                    public static LocString NAME = "Insulated Mechanized Airlock";
                    public static LocString DESC = "";
                    public static LocString EFFECT = "";
                }

                public class SLAG_INSULATEDMANUALAIRLOCK
                {
                    public static LocString NAME = "Insulated Manual Airlock";
                    public static LocString DESC = "";
                    public static LocString EFFECT = "";
                }

                public class SLAG_FILTRATIONTILE
                {
                    public static LocString NAME = "Filtration Tile";
                    public static LocString DESC = "Filters stuff";
                    public static LocString EFFECT = "Desc.";
                }

                public class SLAG_INSULATEDSTORAGELOCKER
                {
                    public static LocString NAME = "Insulated Storage Compactor";
                    public static LocString DESC = "Insulated Storage Compactor";
                    public static LocString EFFECT = "Desc.";
                }
            }
        }

        public class ITEMS
        {
            public class FOOD
            {
                public class COTTON_CANDY
                {
                    public static LocString NAME = KUI.FormatAsLink("Cotton Candy", "COTTONCANDY");
                    public static LocString DESC = "Fruit flavored confection spun from sugars.\n\nThe fluffy texture makes for a perfect treat.";
                    public static LocString RECIPEDESC = "Fruit flavored confection spun from sugars.";
                }

                public class LICE_CREAM
                {
                    // Lice + Ice Cream
                    public static LocString NAME = KUI.FormatAsLink("Lice Cream", "LICECREAM");
                    public static LocString DESC = "Fruit flavored confection spun from sugars.\n\nThe fluffy texture makes for a perfect treat.";
                    public static LocString RECIPEDESC = "Fruit flavored confection spun from sugars.";
                }

                public class SPAGHETTI
                {
                    public static LocString NAME = KUI.FormatAsLink("Spaghetti", "SPAGHETTI");
                    public static LocString DESC = "";
                    public static LocString EFFECT = "";
                }

                public class DRY_NOODLES
                {
                    public static LocString NAME = KUI.FormatAsLink("Dry Noodles", "DRYNOODLES");
                    public static LocString DESC = "";
                    public static LocString EFFECT = "";
                }

                public class SEAFOOD_PASTA
                {
                    public static LocString NAME = KUI.FormatAsLink("Seafood pasta", "SEAFOODPASTA");
                    public static LocString DESC = "";
                    public static LocString EFFECT = "";
                }
            }

            public class INDUSTRIAL_INGREDIENTS
            {
                public class SLAG_WOOL
                {
                    public static LocString NAME = KUI.FormatAsLink("Slag Wool", "SLAGWOOL");
                    public static LocString DESC = $"Fibrous material spin from {ELEMENTS.SLAG.NAME}, used for insulation and filtration.";
                }

                public class MITE_MOLT
                {
                    private static string moltCommon = $"Molt shed by a Slagmite, contains various metal ores that can be exctracted by crushing..";
                    public class WORTHLESS
                    {
                        public static LocString NAME = KUI.FormatAsLink("Worthless Slagmite Molt", "MITEMOLT");
                        public static LocString DESC = moltCommon + "\nThis molt looks dull and unyielding.";
                    }
                    public class LACKLUSTER
                    {
                        public static LocString NAME = KUI.FormatAsLink("Lackluster Slagmite Molt", "MITEMOLT");
                        public static LocString DESC = moltCommon + "\nThis molt has very little value to it.";
                    }
                    public class MEDIOCRE
                    {
                        public static LocString NAME = KUI.FormatAsLink("Mediocre Slagmite Molt", "MITEMOLT");
                        public static LocString DESC = moltCommon + "\nThis molt looks to have average ore content.";
                    }
                    public class EXQUISITE
                    {
                        public static LocString NAME = KUI.FormatAsLink("Exquisite Slagmite Molt", "MITEMOLT");
                        public static LocString DESC = moltCommon + "\nThis molt is of exceptional high quality.";
                    }
                }
                public class GLEAMITE_MOLT
                {
                    private static string moltCommon = $"Molt shed by a Gleamite, contains various refined metals that can be exctracted by crushing..";
                    public class WORTHLESS
                    {
                        public static LocString NAME = KUI.FormatAsLink("Worthless Gleamite Molt", "MITEMOLT");
                        public static LocString DESC = moltCommon + "\nThis molt looks dull and unyielding.";
                    }
                    public class LACKLUSTER
                    {
                        public static LocString NAME = KUI.FormatAsLink("Lackluster Gleamite Molt", "MITEMOLT");
                        public static LocString DESC = moltCommon + "\nThis molt has very little value to it.";
                    }
                    public class MEDIOCRE
                    {
                        public static LocString NAME = KUI.FormatAsLink("Mediocre Gleamite Molt", "MITEMOLT");
                        public static LocString DESC = moltCommon + "\nThis molt looks to have average ore content.";
                    }
                    public class EXQUISITE
                    {
                        public static LocString NAME = KUI.FormatAsLink("Exquisite Gleamite Molt", "MITEMOLT");
                        public static LocString DESC = moltCommon + "\nThis molt is of exceptional high quality.";
                    }
                }

                public class MYSTERY_ORE
                {
                    public static LocString NAME = "Unknown ore";
                    public static LocString DESC = $"Holds many mysteries and secrets.";
                }
                public class MYSTERY_METAL
                {
                    public static LocString NAME = "Unknown metal";
                    public static LocString DESC = $"Holds many mysteries and secrets.</link>";
                }
            }
        }

        public class UI
        {
            public static class DIET
            {
                public static LocString EXTRA_PRODUCE = "Additional excretion: {slag}";
                public static LocString EXTRA_PRODUCE_TOOLTIP = "This critter will also \"produce\" {slag}, for {percent} of consumed mass.";
            }

            public static class MINEABLE
            {
                public static LocString TITLE = "Mineable";
                public static LocString TOOLTIP = "This critters outer can be mined and harvested, without causing any harm to the creature.";
            }
        }
    }
}

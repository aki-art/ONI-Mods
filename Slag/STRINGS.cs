using FUtility;
using Slag.Content;
using Slag.Content.Buildings;
using Slag.Content.Buildings.Spinner;
using Slag.Content.Critters.Slagmite;
using Slag.Content.Items;

namespace Slag
{
    public class STRINGS
    {
        public class CODEX
        {
            public class SLAGMITE
            {
                public static LocString SPECIES_TITLE = "Slagmites";
                public static LocString SPECIES_SUBTITLE = "Critter Species";
                public static LocString TITLE = "Slagmite";
                public static LocString SUBTITLE = "Domesticable Critter";

                public class BODY
                {
                    public static LocString CONTAINER1 = "Slagmite description";
                    public static LocString CONTAINER2 = "Slagmite description";
                    public static LocString CONTAINER3 = "Slagmite description";
                    public static LocString CONTAINER4 = "Slagmite description";
                }
            }
        }

        public class RESEARCH
        {
            public class TECHS
            {
                public class ADVANCEDINSULATION
                {
                    public static LocString NAME = Utils.FormatAsLink("Advanced Insulation", ModAssets.Techs.ADVANCED_INSULATION_ID);
                    public static LocString DESC = "Insulate many things.";
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
            public class MODIFIERS
            {
                public class SHELLGROWTH
                {
                    public static LocString NAME = "Shell Growth";

                    public static LocString TOOLTIP = "...";
                }
            }

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
                public static LocString SLAGMITE = Utils.FormatAsLink("Slagmite", "SLAGMITESPECIES");
            }

            public class FAMILY_PLURAL
            {
                public static LocString SLAGMITESPECIES = Utils.FormatAsLink("Slagmites", "SLAGMITESPECIES");
            }

            public class SPECIES
            {
                public class SLAGMITE
                {
                    // Slag + Mite (arachnid), but also a pun on Stalagmite, the rock formation which this creature resembles
                    public static LocString NAME = Utils.FormatAsLink("Slagmite", SlagmiteConfig.ID);
                    public static LocString DESC = $"Slagmites eat metal rich waste procucts.\n\nThe shells they leave behind after molting can be crushed into various ores.";
                    public static LocString EGG_NAME = Utils.FormatAsLink("Slagmite Egg", "SLAGMITE");

                    public class BABY
                    {
                        public static LocString NAME = Utils.FormatAsLink("Slagmitty", SlagmiteConfig.ID);
                        public static LocString DESC = $"A cute little Slagmite.\n\nIn time it will mature into a fully grown Slagmite.";
                    }
                }

                public class GLEAMITE
                {
                    // Gleam + Mite (arachnid), a "Gleaming Mite"
                    public static LocString NAME = Utils.FormatAsLink("Gleamite", "GLEAMITE");
                    public static LocString DESC = $"Gleamites eat metal rich waste products.\n\nThe shells they leave behind after molting can be crushed into various refined metals.";
                    public static LocString EGG_NAME = Utils.FormatAsLink("Gleamite Egg", "GLEAMITE");

                    public class BABY
                    {
                        public static LocString NAME = Utils.FormatAsLink("Gleamitty", "GLEAMITE");
                        public static LocString DESC = $"A cute little Gleamite.\n\nIn time it will mature into a fully grown Gleamite.";
                    }
                }

                public class DRECKO
                {
                    public class VARIANT_ROCKWOOL
                    {
                        // Slagwool is a type of rockwool, i just chose "rockwool" here because it sounded better with "drecko".
                        public static LocString NAME = Utils.FormatAsLink("Rockwool Drecko", "DRECKOROCKWOOL");
                        public static LocString DESC = $"Rockwool Dreckos are nonhostile critters that graze only on live Mealwood.\n\nTheir backsides are covered in thick woolly fibers that only grow in Carbon Dioxide climates.";
                        public static LocString EGG_NAME = Utils.FormatAsLink("Rockwool Drecklet Egg", "DRECKOROCKWOOL");

                        public class BABY
                        {
                            public static LocString NAME = Utils.FormatAsLink("Rockwool Drecklet", "DRECKOROCKWOOL");
                            public static LocString DESC = $"A little, bug-eyed {NAME}.\n\nIn time it will mature into a fully grown { VARIANT_ROCKWOOL.NAME }.";
                        }
                    }
                }

                public class MOLE
                {
                    public class VARIANT_BRITTLE
                    {
                        // Slagwool is a type of rockwool, i just chose "rockwool" here because it sounded better with "drecko".
                        public static LocString NAME = Utils.FormatAsLink("Brittle Drill", "...");
                        public static LocString DESC = $"...";
                        public static LocString EGG_NAME = Utils.FormatAsLink("Brittle Drill Egg", "...");

                        public class BABY
                        {
                            public static LocString NAME = Utils.FormatAsLink("Brittle Pup", "...");
                            public static LocString DESC = $"A tiny transparent Brittle Puppy.\n\nIn time it will mature into a fully grown Brittle Drill.";
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
                public static LocString NAME = Utils.FormatAsLink("Slag", Elements.Slag.ToString());
                public static LocString DESC = $"Slag is a byproduct of Metal refining processes.";
            }

            // Generic slag glass, not really based on any specific IRL variant
            public class SLAGGLASS
            {
                public static LocString NAME = Utils.FormatAsLink("Slag Glass", Elements.SlagGlass.ToString());
                public static LocString DESC = $"Slag Glass is a brittle, semi-transparent substance formed from the byproducts of metal refining.";
            }

            public class MOLTENSLAGGLASS
            {
                public static LocString NAME = Utils.FormatAsLink("Slag Glass", Elements.MoltenSlagGlass.ToString());
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
                    // "Centrifuge" or synonims also work
                    public static LocString NAME = Utils.FormatAsLink("Spinner", SpinnerConfig.ID);
                    public static LocString DESC = $"Spinners use centrifugal force to spin various materials into fibers.";
                    public static LocString EFFECT = "Produces fibers from solids.\n\nDuplicants will not fabricate items unless recipes are queued.";
                }

                public class SLAG_DENSEINSULATIONTILES
                {
                    public static LocString NAME = Utils.FormatAsLink("Dense Insulated Tile", "DENSEINSULATIONTILE");
                    public static LocString DESC = "The even lower thermal conductivity of dense insulated tiles drastically slows any heat passing through them.";
                    public static LocString EFFECT = "Used to build the walls and floors of rooms.\n\nGreatly reduces " + Utils.FormatAsLink("Heat", "HEAT") + " transfer between walls, retaining ambient heat in an area.";
                }

                public class SLAG_INSULATEDWINDOWTILE
                {
                    public static LocString NAME = Utils.FormatAsLink("Insulated Window Tile", InsulatedWindowTileConfig.ID);
                    public static LocString DESC = "These window tiles allow light and decor to pass through, but not so much the temperature.";
                    public static LocString EFFECT = "Used to build the walls and floors of rooms.\n\nModerately reduces " + Utils.FormatAsLink("Heat", "HEAT") + " transfer between walls, retaining ambient heat in an area.";
                }

                public class INSULATEDPRESSUREDOORCONFIG
                {
                    public static LocString NAME = Utils.FormatAsLink("Insulated Mechanized Airlock", InsulatedPressureDoorConfig.ID);
                    public static LocString DESC = "";
                    public static LocString EFFECT = "";
                }

                public class SLAG_INSULATEDMANUALPRESSUREDOOR
                {
                    public static LocString NAME = Utils.FormatAsLink("Insulated Manual Airlock", InsulatedManualPressureDoorConfig.ID);
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
                public class MITEMEAT
                {
                    public static LocString NAME = Utils.FormatAsLink("Mite Drippings", "MITEGOB");
                    public static LocString DESC = "A lump of congealed fatty bug juice and bits of chitin.\n\nUnappetizing, but think of the protein gains.";
                    public static LocString RECIPEDESC = "A lump of congealed fatty bug juice and bits of chitin.";
                }

                public class CRISPY_CRACKLINGS
                {
                    // Stone Cracking + Pork Cracklings. basically just crunchy mite legs
                    public static LocString NAME = Utils.FormatAsLink("Crispy Fire Crackling", "CRISPYCRACKLING");
                    public static LocString DESC = "Crunchy bite off a mite.\n\n "; // Just close your eyes and imagine it's roasted peanuts.
                    public static LocString RECIPEDESC = "Crunchy bite off a mite.";
                }

                public class COTTON_CANDY
                {
                    public static LocString NAME = Utils.FormatAsLink("Cotton Candy", CottonCandyConfig.ID);
                    public static LocString DESC = "Fruit flavored confection spun from sugars.\n\nThe fluffy texture makes for a perfect treat.";
                    public static LocString RECIPEDESC = "Fruit flavored confection spun from sugars.";
                }

                public class SPAGHETTI
                {
                    public static LocString NAME = Utils.FormatAsLink("Spaghetti", "SPAGHETTI");
                    public static LocString DESC = "";
                    public static LocString EFFECT = "";
                }

                public class DRY_NOODLES
                {
                    public static LocString NAME = Utils.FormatAsLink("Dry Noodles", "DRYNOODLES");
                    public static LocString DESC = "";
                    public static LocString EFFECT = "";
                }

                public class SEAFOOD_PASTA
                {
                    public static LocString NAME = Utils.FormatAsLink("Seafood pasta", "SEAFOODPASTA");
                    public static LocString DESC = "";
                    public static LocString EFFECT = "";
                }
            }

            public class INDUSTRIAL_INGREDIENTS
            {
                public class SLAG_WOOL
                {
                    public static LocString NAME = Utils.FormatAsLink("Slag Wool", SlagWoolConfig.ID);
                    public static LocString DESC = $"Fibrous material spun from Slag, used for insulation and filtration.";
                }

                public class MITE_MOLT
                {
                    private static string moltCommon = $"Molt shed by a Slagmite, contains various metal ores that can be exctracted by crushing..";

                    public class WORTHLESS
                    {
                        public static LocString NAME = Utils.FormatAsLink("Worthless Slagmite Molt", "MITEMOLT");
                        public static LocString DESC = moltCommon + "\nThis molt looks dull and unyielding.";
                    }

                    public class LACKLUSTER
                    {
                        public static LocString NAME = Utils.FormatAsLink("Lackluster Slagmite Molt", "MITEMOLT");
                        public static LocString DESC = moltCommon + "\nThis molt has very little value to it.";
                    }

                    public class MEDIOCRE
                    {
                        public static LocString NAME = Utils.FormatAsLink("Mediocre Slagmite Molt", "MITEMOLT");
                        public static LocString DESC = moltCommon + "\nThis molt looks to have average ore content.";
                    }
                    public class EXQUISITE
                    {
                        public static LocString NAME = Utils.FormatAsLink("Exquisite Slagmite Molt", "MITEMOLT");
                        public static LocString DESC = moltCommon + "\nThis molt is of exceptional high quality.";
                    }
                }

                public class GLEAMITE_MOLT
                {
                    private static string moltCommon = $"Molt shed by a Gleamite, contains various refined metals that can be exctracted by crushing..";
                    public class WORTHLESS
                    {
                        public static LocString NAME = Utils.FormatAsLink("Worthless Gleamite Molt", "MITEMOLT");
                        public static LocString DESC = moltCommon + "\nThis molt looks dull and unyielding.";
                    }
                    public class LACKLUSTER
                    {
                        public static LocString NAME = Utils.FormatAsLink("Lackluster Gleamite Molt", "MITEMOLT");
                        public static LocString DESC = moltCommon + "\nThis molt has very little value to it.";
                    }
                    public class MEDIOCRE
                    {
                        public static LocString NAME = Utils.FormatAsLink("Mediocre Gleamite Molt", "MITEMOLT");
                        public static LocString DESC = moltCommon + "\nThis molt looks to have average ore content.";
                    }
                    public class EXQUISITE
                    {
                        public static LocString NAME = Utils.FormatAsLink("Exquisite Gleamite Molt", "MITEMOLT");
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


using STRINGS;
namespace Slag
{
    class SlagStrings
    {
        public class SLAGSTRINGS
        {
            public class CREATURES
            {
                public class FAMILY
                {
                    public static LocString MITE = UI.FormatAsLink("Mite", "MITESPECIES");
                }
                public class FAMILY_PLURAL
                {
                    public static LocString MITESPECIES = UI.FormatAsLink("Mites", "MITESPECIES");
                }

                public class SPECIES
                {
                    public class SLAGMITE
                    {
                        static readonly string slag = ELEMENTS.SLAG.NAME;
                        static readonly string regolith = STRINGS.ELEMENTS.REGOLITH.NAME;

                        public static LocString NAME = UI.FormatAsLink("Slagmite", "SLAGMITE");
                        public static LocString DESC = $"Slagmites eat { slag } and { regolith }.\n\nThe shells they leave behind after molting can be crushed into various ores.";
                        public static LocString EGG_NAME = UI.FormatAsLink("Slagmite Egg", "SLAGMITE");
                        public class BABY
                        {
                            public static LocString NAME = UI.FormatAsLink("Slagmite Larva", "SLAGMITE");
                            public static LocString DESC = $"A cute little { NAME }.\n\nIn time it will mature into a fully grown { SLAGMITE.NAME }.";
                        }
                    }
                    public class GLEAMITE
                    {
                        static readonly string slag = ELEMENTS.SLAG.NAME;
                        static readonly string regolith = STRINGS.ELEMENTS.REGOLITH.NAME;

                        public static LocString NAME = UI.FormatAsLink("Gleamite", "GLEAMITE");
                        public static LocString DESC = $"Gleamites eat { slag } and { regolith }.\n\nThe shells they leave behind after molting can be crushed into various refined metals.";
                        public static LocString EGG_NAME = UI.FormatAsLink("Gleamite Egg", "GLEAMITE");
                        public class BABY
                        {
                            public static LocString NAME = UI.FormatAsLink("Gleamite Larva", "GLEAMITE");
                            public static LocString DESC = $"A cute little { NAME }.\n\nIn time it will mature into a fully grown { GLEAMITE.NAME }.";
                        }
                    }
                    public class DRECKO
                    {
                        public class VARIANT_ROCKWOOL
                        {
                            private static readonly string mealwood = UI.FormatAsLink("Mealwood Plants", "BASICSINGLEHARVESTPLANT");
                            private static readonly string carbondioxide = STRINGS.ELEMENTS.CARBONDIOXIDE.NAME;

                            public static LocString NAME = UI.FormatAsLink("Rockwool Drecko", "DRECKOROCKWOOL");
                            public static LocString DESC = $"Rockwool Dreckos are nonhostile critters that graze only on live { mealwood } .\n\nTheir backsides are covered in thick woolly fibers that only grow in { carbondioxide } climates.";
                            public static LocString EGG_NAME = UI.FormatAsLink("Rockwool Drecklet Egg", "DRECKOROCKWOOL");

                            public class BABY
                            {
                                public static LocString NAME = UI.FormatAsLink("Rockwool Drecklet", "DRECKOROCKWOOL");
                                public static LocString DESC = $"A little, bug-eyed {NAME}.\n\nIn time it will mature into a fully grown { VARIANT_ROCKWOOL.NAME }.";
                            }
                        }
                    }
                }
            }

            public class BUILDINGS
            {
                public class PREFABS
                {
                    public class METALREFINERY
                    {
                        public static LocString RECIPE_DESCRIPTION = "Extracts pure {0} from {1}, using {2} and {3}.";
                    }

                    public class SPINNER
                    {
                        public static LocString NAME = "Spinner";
                        public static LocString DESC = $"{ NAME }s use centrifugal force to spin various materials into fibers.";
                        public static LocString EFFECT = "Produces fibers from solids.\n\nDuplicants will not fabricate items unless recipes are queued.";
                    }
                    public class DENSE_INSULATION_TILES
                    {
                        public static LocString NAME = UI.FormatAsLink("Dense Insulated Tile", "DENSEINSULATIONTILE");
                        public static LocString DESC = "The even lower thermal conductivity of dense insulated tiles drastically slows any heat passing through them.";
                        public static LocString EFFECT = "Used to build the walls and floors of rooms.\n\nGreatly reduces " + UI.FormatAsLink("Heat", "HEAT") + " transfer between walls, retaining ambient heat in an area.";
                    }
                    public class INSULATED_MECHANIZED_AIRLOCK
                    {
                        public static LocString NAME = "Spinner";
                        public static LocString DESC = "";
                        public static LocString EFFECT = "";
                    }
                    public class INSULATED_MANUAL_AIRLOCK
                    {
                        public static LocString NAME = "Spinner";
                        public static LocString DESC = "";
                        public static LocString EFFECT = "";
                    }
                    public class FILTRATION_TILE
                    {
                        public static LocString NAME = "Filtration Tile";
                        public static LocString DESC = "Filters stuff";
                        public static LocString EFFECT = "Desc.";
                    }

                    public class INSULATED_STORAGE_LOCKER
                    {
                        public static LocString NAME = "Insulated Storage Compactor";
                        public static LocString DESC = "Insulated Storage Compactor";
                        public static LocString EFFECT = "Desc.";
                    }
                }
            }

            public class ELEMENTS
            {
                public class SLAG
                {
                    public static LocString NAME = "Slag";
                    public static LocString DESC = "Slag";
                }

                public class SLAGGLASS
                {
                    public static LocString NAME = "Slag Glass";
                    public static LocString DESC = "Slag";
                }
            }
            public class ITEMS
            {
                public class FOOD
                {
                    public class COTTON_CANDY
                    {
                        public static LocString NAME = "Spinner";
                        public static LocString DESC = "";
                        public static LocString EFFECT = "";
                    }
                    public class SPAGHETTI
                    {
                        public static LocString NAME = "Spinner";
                        public static LocString DESC = "";
                        public static LocString EFFECT = "";
                    }
                    public class DRY_NOODLES
                    {
                        public static LocString NAME = "Spinner";
                        public static LocString DESC = "";
                        public static LocString EFFECT = "";
                    }
                    public class SEAFOOD_PASTA
                    {
                        public static LocString NAME = "Spinner";
                        public static LocString DESC = "";
                        public static LocString EFFECT = "";
                    }
                }

                public class INDUSTRIAL_INGREDIENTS
                {
                    public class SLAG_WOOL
                    {
                        public static LocString NAME = "Spinner";
                        public static LocString DESC = "";
                        public static LocString EFFECT = "";
                    }
                }
            }
        }
    }
}

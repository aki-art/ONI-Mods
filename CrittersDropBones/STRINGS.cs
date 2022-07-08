using KUI = STRINGS.UI;

namespace CrittersDropBones
{
    public class STRINGS
    {
        public class TWITCH
        {
            public class MESSY_MESS_HALL
            {
                public static LocString NAME = "Messy Mess Hall";
            }
            public class SOUP_RAIN
            {
                public static LocString NAME = "Soup Rain";
            }
        }

        public class BUILDING
        {
            public class STATUSITEMS
            {
                public class CDB_NEEDSSTIRRINGSTATUS
                {
                    public static LocString NAME = "Needs stirring";
                    public static LocString TOOLTIP = ".";
                }
            }
        }

        public class BUILDINGS
        {
            public class PREFABS
            {
                public class CDB_SLOWCOOKER
                {
                    public static LocString NAME = KUI.FormatAsLink("Cooking Pot", Buildings.SlowCooker.SlowCookerConfig.ID.ToUpperInvariant());
                    public static LocString DESC = "Used to cook delicious soups.\n\n" +
                        "The pot needs occasional stirring, and emits Steam while active.\n" +
                        $"A {KUI.FormatAsLink("Range Hood", "")} can be built atop to catch the Steam output.";
                    public static LocString EFFECT = "Produces Soup while supplied with ingredients.";

                }
            }
        }

        public class ITEMS
        {
            public class FOOD
            {
                public class CDB_SURIMI
                {
                    public static LocString NAME = "Surimi";
                    public static LocString DESC = "This fishy paste tastes like it should have been made of Pokeshell meat.";
                }
                public class CDB_GRUBGRUB
                {
                    public static LocString NAME = "Grub Grub";
                    public static LocString DESC = "This fishy paste tastes like it should have been made of Pokeshell meat.";
                }
                public class CDB_SUPERHOTSOUP
                {
                    public static LocString NAME = "Super Hot Soup";
                    public static LocString DESC = "Soup that burns through all sickness.";
                }

                public class CDB_SOUPSTOCK
                {
                    public static LocString NAME = "Soup Stock";
                    public static LocString DESC = "This fishy paste tastes like it should have been made of Pokeshell meat.";
                }

                public class CDB_FISHSOUP
                {
                    public static LocString NAME = "Fish Soup";
                    public static LocString DESC = "This fishy paste tastes like it should have been made of Pokeshell meat.";
                }

                public class CDB_VEGETABLESOUP
                {
                    public static LocString NAME = "Vegetable Soup";
                    public static LocString DESC = "This fishy paste tastes like it should have been made of Pokeshell meat.";
                }

                public class CDB_EGGDROPSOUP
                {
                    public static LocString NAME = "Vegetable Soup";
                    public static LocString DESC = "This fishy paste tastes like it should have been made of Pokeshell meat.";
                }

                public class CDB_SLUDGE
                {
                    public static LocString NAME = "Vegetable Soup";
                    public static LocString DESC = "This fishy paste tastes like it should have been made of Pokeshell meat.";
                }
            }

            public class BONES
            {
                public class CDB_BONE
                {
                    public static LocString NAME = "Bone";
                    public static LocString DESC = "A bone from a creature.";
                }

                public class CDB_FISHBONE
                {
                    public static LocString NAME = "Fishbone";
                    public static LocString DESC = "Bone of a fish.";
                }
            }
        }

        public class MISC
        {
            public class TAGS
            {
                public static LocString CDB_BONE = "Bones";
            }
        }

        public class DUPLICANTS
        {
            public class STATUSITEMS
            {
                public class CDB_STAMINAREGENERATION
                {
                    public static LocString NAME = "Stamina Regeneration";
                    public static LocString TOOLTIP = ".";
                }

                public class CDB_UPSETSTOMACH
                {
                    public static LocString NAME = "Upset Stomach";
                    public static LocString TOOLTIP = ".";
                }

                public class CDB_CHILLFOOD
                {
                    public static LocString NAME = "Chill Food";
                    public static LocString TOOLTIP = ".";
                }

                public class CDB_SUPERHOTFOOD
                {
                    public static LocString NAME = "Super Hot Food";
                    public static LocString TOOLTIP = ".";
                }

                public class CDB_STRONGBONES
                {
                    public static LocString NAME = "Strong Bones";
                    public static LocString TOOLTIP = ".";
                }

                public class CDB_VITAMINLOADED
                {
                    public static LocString NAME = "Vitamin Loaded";
                    public static LocString TOOLTIP = ".";
                }
            }
        }
    }
}

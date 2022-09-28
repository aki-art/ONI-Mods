using CrittersDropBones.Buildings.SlowCooker;
using KUI = STRINGS.UI;
using KSTRINGS = STRINGS;

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
                public class CRITTERSDROPBONES_NEEDSSTIRRINGSTATUS
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
                public class CRITTERSDROPBONES_COOKER
                {
                    public static LocString NAME = KUI.FormatAsLink("Cooking Pot", SlowCookerConfig.ID.ToUpperInvariant());
                    public static LocString DESC = "Used to cook delicious soups.\n\n" +
                        "The pot needs occasional stirring, and emits Steam while active.\n" +
                        $"A {CRITTERSDROPBONES_HOOD.NAME} can be built atop to catch the Steam output.";
                    public static LocString EFFECT = "Produces Soup while supplied with ingredients.";
                }
                public class CRITTERSDROPBONES_HOOD
                {
                    public static LocString NAME = KUI.FormatAsLink("Range Hood", SlowCookerConfig.ID.ToUpperInvariant());
                    public static LocString DESC = $"Captures {KSTRINGS.ELEMENTS.STEAM.NAME}, and outputs the condensed water to a pipe.";
                    public static LocString EFFECT = "";
                }
            }
        }

        public class ITEMS
        {
            public class FOOD
            {
                public class CRITTERSDROPBONES_GELATINE
                {
                    public static LocString NAME = "Gelatine";
                    public static LocString DESC = "";
                }

                public class CRITTERSDROPBONES_SURIMI
                {
                    public static LocString NAME = "Surimi";
                    public static LocString DESC = "";
                }

                public class CRITTERSDROPBONES_PUMPKINSOUP
                {
                    public static LocString NAME = "Pumpkin Soup";
                    public static LocString DESC = "A slurry of delicious holiday flavors.";
                }

                public class CRITTERSDROPBONES_GRUBGRUB
                {
                    public static LocString NAME = "Grub Grub Grub";
                    public static LocString DESC = "Grub of a Grub Grub's grub.";
                }

                public class CRITTERSDROPBONES_SUPERHOTSOUP
                {
                    public static LocString NAME = "Super Hot Soup";
                    public static LocString DESC = "Soup that burns through all sickness.";
                }

                public class CRITTERSDROPBONES_SOUPSTOCK
                {
                    public static LocString NAME = "Soup Stock";
                    public static LocString DESC = "Bland and boring.";
                }

                public class CRITTERSDROPBONES_FISHSOUP
                {
                    public static LocString NAME = "Fish Soup";
                    public static LocString DESC = "This soup is really delicious. Too delicious... it's fishy.";
                }

                public class CRITTERSDROPBONES_VEGETABLESOUP
                {
                    public static LocString NAME = "Vegetable Soup";
                    public static LocString DESC = "Eat your greens!";
                }

                public class CRITTERSDROPBONES_EGGDROPSOUP
                {
                    public static LocString NAME = "Egg-Drop Soup";
                    public static LocString DESC = "This fishy paste tastes like it should have been made of Pokeshell meat.";
                }

                public class CRITTERSDROPBONES_FRUITSOUP
                {
                    public static LocString NAME = "Fruit Soup";
                    public static LocString DESC = "This is definitely <i>not</i> a dessert disguised as a full meal.";
                }

                public class CRITTERSDROPBONES_SLUDGE
                {
                    public static LocString NAME = "Stinky's Sludge";
                    public static LocString DESC = "This fishy paste tastes like it should have been made of Pokeshell meat.";
                }
            }

            public class BONES
            {
                public class CRITTERSDROPBONES_BONE
                {
                    public static LocString NAME = "Bone";
                    public static LocString DESC = "Bone of a creature.";
                }

                public class CRITTERSDROPBONES_FISHBONE
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
                public static LocString CRITTERSDROPBONES_BONE = "Bone";
            }
        }

        //public class DUPLICANTS
        //{
            public class EFFECTS
            {
                public class CRITTERSDROPBONES_STAMINAREGENERATION
                {
                    public static LocString NAME = "Stamina Regeneration";
                    public static LocString TOOLTIP = ".";
                }

                public class CRITTERSDROPBONES_UPSETSTOMACH
                {
                    public static LocString NAME = "Upset Stomach";
                    public static LocString TOOLTIP = ".";
                }

                public class CRITTERSDROPBONES_CHILLFOOD
                {
                    public static LocString NAME = "Chill Food";
                    public static LocString TOOLTIP = ".";
                }

                public class CRITTERSDROPBONES_HOTFOOD
                {
                    public static LocString NAME = "Super Hot Food";
                    public static LocString TOOLTIP = ".";
                }

                public class CRITTERSDROPBONES_STRONGBONES
                {
                    public static LocString NAME = "Strong Bones";
                    public static LocString TOOLTIP = ".";
                }

                public class CRITTERSDROPBONES_VITAMINLOADED
                {
                    public static LocString NAME = "Vitamin Loaded";
                    public static LocString TOOLTIP = ".";
                }
            }
        //}
    }
}

namespace TransparentAluminum
{
    public class STRINGS
    {
        public class MISC
        {
            public class TAGS
            {
                public static LocString TRANSPARENTALUMINUM_COATING = "Drill Coating";
            }
        }

        public class ELEMENTS
        {
            public class TRANSPARENTALUMINUM
            {
                public static LocString NAME = "Transparent Aluminum";
                public static LocString DESC = "Transparent Aluminum is a see-through ceramic composed of Aluminum and Oxylite.";
            }
        }

        public class BUILDINGS
        {
            public class PREFABS
            {
                public class TRANSPARENTALUMINUM_SOLARROAD
                {
                    public static LocString NAME = "Solar Road";
                    public static LocString DESC = "Solar roads are a space effective solution to renewable energy. Solar roads are upgradable.";
                    public static LocString EFFECT = "Absorbs sunlight and converts it to power. Blocks gases and liquids.";

                    public class TIERS
                    {
                        public static LocString TIER1 = "Rickety Solar Road";
                        public static LocString TIER2 = "Reinforced Solar Road";
                        public static LocString TIER3 = "Advanced Solar Road";
                        public static LocString TIER4 = "Hi-Tech Solar Road";
                    }
                }

                public class TRANSPARENTALUMINUM_REINFORCED_GLASS
                {
                    public static LocString NAME = "Reinforced Glass";
                    public static LocString DESC = "Window tiles that can take a beating.";
                    public static LocString EFFECT = "Blocks Gases, Liquids and Meteors, while allowing Light and Decor to pass through.";
                }

                public class TRANSPARENTALUMINUM_BORER
                {
                    public static LocString NAME = "Borer";
                    public static LocString DESC = "A type of auto-digger machine, drills a single wide hole until it hits Neutronium, falls out of the world or reaches it's maximum range.\n" +
                        "Reinforcing the drill cone with Transparent Aluminium will enable it to dig much longer.";
                    public static LocString EFFECT = "";
                }

                public class TRANSPARENTALUMINUM_ADVANCED_BORER
                {
                    public static LocString NAME = "Advanced Borer";
                    public static LocString DESC = "A type of auto-digger machine, drills a triple wide hole until it hits Neutronium, falls out of the world or reaches it's maximum range.\n" +
                        "Builds Walls on either side of it's tunnel if supplied with enough materials." +
                        "Reinforcing the drill cone with Transparent Aluminium will enable it to dig much longer.";
                    public static LocString EFFECT = "";
                }

                public class TRANSPARENTALUMINUM_SURVEYOR_MODULE
                {
                    public static LocString NAME = "Surveyor Module";
                    public static LocString DESC = "Launched from the ship while in space, this module will release a small surveyor drone that will map a sliver of the Star Map, revealing any planetoids in the chosen direction.";
                    public static LocString EFFECT = "";
                }
            }
        }

        public class BUILDING
        {
            public class STATUSITEMS
            {
                public class TRANSPARENTALUMINUM_DRILLING
                {
                    public static LocString NAME = "Drilling";
                    public static LocString TOOLTIP = "...";
                }
                public class TRANSPARENTALUMINUM_FALLING
                {
                    public static LocString NAME = "Falling";
                    public static LocString TOOLTIP = "...";
                }

                public class TRANSPARENTALUMINUM_STUCK
                {
                    public static LocString NAME = "Stuck";
                    public static LocString TOOLTIP = "This Borer has hit something imprenetable and cannot move forward.";
                }
                public class TRANSPARENTALUMINUM_INTEGRITY
                {
                    public static LocString NAME = "Integrity at {percent}";
                    public static LocString TOOLTIP = "This Borer has hit something imprenetable and cannot move forward.";
                }
            }
        }

        public class UI
        {
            public class BORERSIDESCREEN
            {
                public class HOT_LIQUID_TOGGLE
                {
                    public static LocString ON = "Ignore Hot Liquids";
                    public static LocString OFF = "Avoid Hot Liquids";

                    public static LocString TOOLTIP_ON = "The Borer will continue digging until it reaches Neutronium or falls out of the world.";
                    public static LocString TOOLTIP_OFF = "The Borer will stop digging one tile before reaching a Liquid with a tempterature of {temperature}.";
                }
            }
        }
    }
}

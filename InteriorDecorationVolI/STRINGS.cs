namespace InteriorDecorationv1
{
    class STRINGS
    {
        public class BUILDINGS
        {
            public class PREFABS
            {
                public class ID1_GLASS_SCULPTURE
                {
                    public static LocString NAME = global::STRINGS.UI.FormatAsLink("Glass Block", Buildings.GlassSculpture.GlassSculptureConfig.ID.ToUpperInvariant());
                    public static LocString DESC = "Duplicants who have learned art skills can produce more decorative sculptures.";
                    public static LocString EFFECT = "Majorly increases " + global::STRINGS.UI.FormatAsLink("Decor", "DECOR") + ", contributing to " + global::STRINGS.UI.FormatAsLink("Morale", "MORALE") + ".\n\nMust be sculpted by a Duplicant.";
                    public static LocString POORQUALITYNAME = "\"Abstract\" Glass Sculpture";
                    public static LocString AVERAGEQUALITYNAME = "Mediocre Glass Sculpture";
                    public static LocString EXCELLENTQUALITYNAME = "Genius Glass Sculpture";
                }

                public class ID1_MOODLAMP
                {
                    public static LocString NAME = global::STRINGS.UI.FormatAsLink("Mood Lamp", Buildings.MoodLamp.MoodLampConfig.ID.ToUpperInvariant());
                    public static LocString DESC = "Light reduces Duplicant stress and is required to grow certain plants.";
                    public static LocString EFFECT = "Provides " + global::STRINGS.UI.FormatAsLink("Light", "LIGHT") + " when " + global::STRINGS.UI.FormatAsLink("Powered", "POWER") + ".\n\nDuplicants can operate buildings more quickly when the building is lit.";
                }
                public class ID1_AQUARIUM
                {
                    public static LocString NAME = global::STRINGS.UI.FormatAsLink("Aquarium", Buildings.Aquarium.AquariumConfig.ID.ToUpperInvariant());
                    public static LocString DESC = "Light reduces Duplicant stress and is required to grow certain plants.";
                    public static LocString EFFECT = "Provides " + global::STRINGS.UI.FormatAsLink("Light", "LIGHT") + " when " + global::STRINGS.UI.FormatAsLink("Powered", "POWER") + ".\n\nDuplicants can operate buildings more quickly when the building is lit.";
                }

                public class ID1_STAINED_GLASS_TILE
                {
                    //public static LocString NAME = UI.FormatAsLink("Stained Glass Tile", Buildings.StainedGlassTiles.StainedGlassTileConfig.ID.ToUpperInvariant());
                    public static LocString DESC = "Window tiles provide a barrier against liquid and gas and are completely transparent.";
                    public static LocString EFFECT = $"Used to build the walls and floors of rooms.\n\nAllows {(global::STRINGS.UI.FormatAsLink("Light", "LIGHT"))} and {(global::STRINGS.UI.FormatAsLink("Decor", "DECOR"))} to pass through.";
                }

                public class ID1_FOSSIL_STAND
                {
                    //public static LocString NAME = UI.FormatAsLink("Fossil Display", Buildings.FossilDisplay.FossilStandConfig.ID.ToUpperInvariant());
                    public static LocString DESC = "Duplicants who have learned science skills can produce more believable reconstructions.";
                    public static LocString EFFECT = $"Majorly increases {(global::STRINGS.UI.FormatAsLink("Decor", "DECOR"))}, contributing to  + {(global::STRINGS.UI.FormatAsLink("Morale", "MORALE"))}.\n\nMust be sculpted by a Duplicant.";
                    public static LocString POORQUALITYNAME = "Questionable Fossil Display";
                    public static LocString AVERAGEQUALITYNAME = "Speculative Fossil Display";
                    public static LocString EXCELLENTQUALITYNAME = "Marvelous Fossil Display";
                }

                public class ID1_LANTERN
                {

                    //public static LocString NAME = UI.FormatAsLink("Lantern", Buildings.Lantern.LanternConfig.ID.ToUpperInvariant());
                    public static LocString DESC = "Light reduces Duplicant stress and is required to grow certain plants.";
                    public static LocString OILEFFECT = "Provides " + global::STRINGS.UI.FormatAsLink("Light", "LIGHT") + " when powered with " + global::STRINGS.UI.FormatAsLink("Crude Oil", "CRUDEOIL") + ".\n\nDuplicants can operate buildings more quickly when the building is lit.";
                    public static LocString PETROLEUMEFFECT = "Provides " + global::STRINGS.UI.FormatAsLink("Light", "LIGHT") + " when powered with " + global::STRINGS.UI.FormatAsLink("Petroleum", "PETROLEUM") + ".\n\nDuplicants can operate buildings more quickly when the building is lit.";
                }
            }
        }

        public class DUPLICANTS
        {
            public class STATUSITEMS
            {
                public class INSPIRED
                {
                    public static LocString NAME_LOW = "Mildly Curious";
                    public static LocString NAME_MEDIUM = "Curious";
                    public static LocString NAME_HIGH = "Super Curious";
                    public static LocString TOOLTIP = "This Duplicant can't wait to learn more about their World!";
                }
            }
        }

        public class UI
        {
            public class UISIDESCREENS
            {
                public class AQUARIUM_SIDE_SCREEN
                {
                    public static LocString TITLE = "Select Fish";
                }
                public class MOODLAMP_SIDE_SCREEN
                {
                    public static LocString TITLE = "Lamp type";
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

            public class IDMODSETTINGSSCREEN
            {
                public class PACUCYLCE
                {
                    public static LocString STASIS = "Stasis";
                    public static LocString STASISDESC = "Pacus in aquariums do not age or die.\nProduction and reproduction halted.";
                    public static LocString NORMAL = "Normal";
                    public static LocString NORMALDESC = "Pacus produce and die at a normal rate.";
                    public static LocString TENTIMES = "10x";
                    public static LocString TENTIMESDESC = "Pacus produce 10 times slower, but die much later.";
                }

                public class LANTERNCYCLE
                {
                    public static LocString OIL = "Oil";
                    public static LocString PETROLEUM = "Petroleum";
                }

                public class STAINEDSPEED
                {
                    public static LocString NOBONUS = "No bonus";
                    public static LocString SMALL = "Small Bonus";
                    public static LocString REGULAR = "Regular tiles";
                    public static LocString HASTY = "Hasty";
                    public static LocString METAL = "Metal tiles";
                    public static LocString FAST = "Fast";

                }
            }
        }
    }
}

using STRINGS;

namespace SpookyPumpkin
{
    public class STRINGS
    {
        public class DUPLICANTS
        {
            public class STATUSITEMS
            {
                public class SPOOKED
                {
                    public static LocString NAME = "Spooked!";
                    public static LocString TOOLTIP = "This Duplicant saw something super scary!";
                }
            }
        }

        public class ITEMS
        {
            public class FOOD
            {
                public class PUMPKIN
                {
                    public static LocString NAME = global::STRINGS.UI.FormatAsLink("Pumpkin", nameof(PUMPKIN));
                    public static LocString DESC = $"Bland tasting fruit of a {NAME} plant.";
                }

                public class PUMPKINPIE
                {
                    public static LocString NAME = "Pumpkin Pie";
                    public static LocString DESC = "A delicious seasonal treat.";
                }

                public class TOASTEDPUMPKINSEED
                {
                    public static LocString NAME = "Toasted Pumpkin Seed";
                    public static LocString DESC = "Tasty snack";
                }
            }
        }

        public class CREATURES
        {
            public class SPECIES
            {
                public class SP_PUMPKIN
                {
                    public static LocString NAME = global::STRINGS.UI.FormatAsLink("Pumpkin", PumpkinPlantConfig.ID);
                    public static LocString DESC = "desc";
                    public static LocString DOMESTICATEDDESC = "ddesc";
                }

                public class SEEDS
                {
                    static readonly LocString seeds = global::STRINGS.UI.FormatAsLink("Seed", "PLANTS");
                    public class SP_PUMPKIN
                    {
                        public static LocString NAME = global::STRINGS.UI.FormatAsLink("Pumpkin Seed", PumpkinPlantConfig.ID);
                        public static LocString DESC = $"The {seeds} of a {SPECIES.SP_PUMPKIN.NAME} plant.\n\nIt can be planted to grow more Pumpkins, or toasted for snacks.";
                    }
                }
            }
        }

        public class UI
        {
            public class UISIDESCREENS
            {
                public class GHOSTSIDESCREEN
                {
                    public static LocString TITLE = "Suspicious Pip";
                    public static LocString TREATBUTTON = "Give Treat";
                    public static LocString CANCELBUTTON = "Cancel";
                    public static LocString SHOOBUTTON = "Shoo Away";
                    public static LocString MESSAGE = "This suspicious pip looks like it wants some treats.";
                }
            }
        }
    }
}

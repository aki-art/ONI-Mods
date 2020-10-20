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
                    public static LocString NAME = "Pumpkin";
                    public static LocString DESC = "Raw pumkin";
                }

                public class PUMPKINPIE
                {
                    public static LocString NAME = "Pumpkin Pie";
                    public static LocString DESC = "Raw pumkin";
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
                    public static LocString NAME = UI.FormatAsLink("Pumpkin", PumpkinPlantConfig.ID);
                    public static LocString DESC = "desc";
                    public static LocString DOMESTICATEDDESC = "ddesc";
                }

                public class SEEDS
                {
                    public class SP_PUMPKIN
                    {
                        public static LocString NAME = UI.FormatAsLink("Pumpkin Seed", PumpkinPlantConfig.ID);
                        public static LocString DESC = "desc";
                    }
                }
            }
        }
    }
}

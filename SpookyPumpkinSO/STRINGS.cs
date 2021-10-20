using SpookyPumpkinSO.Foods;

namespace SpookyPumpkinSO
{
    // Translation support not yet implemented
    public class STRINGS
    {
        public static class BUILDINGS
        {
            public static class PREFABS
            {
                public static class SP_SPOOKYPUMPKIN
                {
                    public static LocString NAME = "Jack O' Lantern";
                    public static LocString DESC = "A spooky lamp for spooky times.";
                    public static LocString EFFECT = $"Provides Light and Decor. Spooks duplicants that come nearby.";
                }
            }
        }

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
                public class SP_PUMPKIN
                {
                    public static LocString NAME = global::STRINGS.UI.FormatAsLink("Pumpkin", PumpkinConfig.ID);
                    public static LocString DESC = $"Bland tasting fruit of a Pumpkin plant.";
                }

                public class SP_PUMPKINPIE
                {
                    public static LocString NAME = global::STRINGS.UI.FormatAsLink("Pumpkin Pie", PumkinPieConfig.ID);
                    public static LocString DESC = "A delicious seasonal treat.";
                }

                public class SP_TOASTEDPUMPKINSEED
                {
                    public static LocString NAME = global::STRINGS.UI.FormatAsLink("Roasted Pumpkin Seeds", ToastedPumpkinSeedConfig.ID);
                    public static LocString DESC = "Tasty snack.";
                }
            }
        }

        public class CREATURES
        {
            public class SPECIES
            {
                public class SP_GHOSTPIP
                {
                    public static LocString NAME = global::STRINGS.UI.FormatAsLink("Suspicious Pip", GhostPip.GhostSquirrelConfig.ID);
                    public static LocString DESC = "Seems suspicious. It looks like it would love some treats.\n\n" +
                        "<smallcaps>The Suspicious Pip will offer to trade for Pumpkin Seeds. The required item will change daily and after each trade.</smallcaps>";
                }
                public class SP_PUMPKIN
                {
                    public static LocString NAME = global::STRINGS.UI.FormatAsLink("Pumpkin", PumpkinPlantConfig.ID);
                    public static LocString DESC = $"Pumpkins are pretty neat. They grow enormous fruits also called Pumpkins.";
                    public static LocString DOMESTICATEDDESC = DESC;
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
                    public static LocString TREATBUTTON = "Deliver Treat";
                    public static LocString CANCELBUTTON = "Cancel";
                    public static LocString SHOOBUTTON = "Shoo Away";
                    public static LocString MESSAGE = "This suspicious pip looks like it wants some treats.";
                    public static LocString LABEL = "Wants one ";
                    public static LocString LABEL2 = "Delivering ";
                }
            }
        }
    }
}

using static STRINGS.UI;

namespace Curtain
{
    public class STRINGS
    {
        public class BUILDINGS
        {
            public class PREFABS
            {
                public class AC_PLASTICCURTAIN
                {
                    public static LocString NAME = "Plastic Door";
                    public static LocString DESC = "A transparent insulating door.";
                    public static LocString EFFECT = $"Quarters off dangerous areas and prevents gases" +
                        $" from seeping into the colony while closed, while allowing {FormatAsLink("Light", "LIGHT")}" +
                        $" and {FormatAsLink("Decor", "DECOR")} to pass through.\n\nDuplicants passing through will open the door for a short while, letting gases and liquids to pass through.";
                }
            }
        }

        public class BUILDING
        {
            public class STATUSITEMS
            {
                public class CHANGECURTAINCONTROLSTATE
                {
                    public static LocString NAME = "Pending Door State Change: {CurrentState}";
                    public static LocString TOOLTIP = "Waiting for a Duplicant to change control state";
                }
            }
        }
    }
}

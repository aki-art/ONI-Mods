using AsphaltStairs.Buildings;
using KUI = STRINGS.UI;

namespace AsphaltStairs
{
    internal class STRINGS
    {
        public class UI
        {
            public class NO_STAIRS_MOD
            {
                public static LocString BUTTON = "Stairs on Steam Workshop";
                public static LocString MESSAGE = "Asphalt Stairs relies on Stairs to work.\nPlease subscribe to and enable Stairs.";
            }
        }
        public class BUILDINGS
        {
            public class PREFABS
            {
                public class ASPHALTSTAIRS
                {
                    public static LocString NAME = KUI.FormatAsLink("Asphalt Stairs", AsphaltStairsConfig.ID.ToUpperInvariant());
                    public static LocString DESC = "\"Gravitas is not responsible for any injuries or damages caused when using the Asphalt Stairs.\"";
                    public static LocString EFFECT = $"Allows for diagonal movement. Majorly increases walking speed.\n\n <b><color=#F44A4A>Requires foundation tiles or support beneath!</b></color>";
                }
            }
        }
    }
}

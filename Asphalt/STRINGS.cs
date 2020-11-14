using static STRINGS.UI;

namespace Asphalt
{
    public class STRINGS
    {
        public class BUILDINGS
        {
            public class PREFABS
            {
                public class ASPHALTTILE
                {
                    public static LocString NAME = FormatAsLink("Asphalt Tile", "ID");
                    public static LocString DESC = "Asphalt tiles feel great to run on.";
                    public static LocString EFFECT = "Used to build the walls and floors of rooms.\n\nSubstantially increases Duplicant runspeed.";
                }

                public class AT_OILREFINERYALT
                {
                    public static LocString EFFECT = $"Converts {FormatAsLink("Crude Oil", "CRUDEOIL")} into {FormatAsLink("Petroleum", "PETROLEUM")}, " +
                        $"{FormatAsLink("Bitumen", "BITUMEN")} and {FormatAsLink("Natural Gas", "METHANE")}.";
                }
            }
        }

        public class UI
        {
            public class ASPHALTSETTINGSDIALOG
            {
                public class TITLEBAR
                {
                    public static LocString LABEL = "Asphalt Settings";
                }
                public class BUTTONS
                {
                    public static LocString VERSION = "v{version}";
                    public class CANCEL
                    {
                        public static LocString TEXT = "Cancel";
                    }
                    public class OK
                    {
                        public static LocString TEXT = "Apply";
                    }
                }

                public class CONTENT
                {
                    public class TOGGLEPANEL
                    {
                        public static LocString LABEL = "Use external save location";
                    }

                    public class SLIDERPANEL
                    {
                        public static LocString LABEL = "Speed: +{number}%";
                        public static LocString SPEEDLABEL = "{label}";
                        public static LocString TIER1_NOBONUS = "No bonus";
                        public static LocString TIER2_SMALLBONUS = "Small bonus";
                        public static LocString TIER3_REGULARTILE = "Regular Tiles";
                        public static LocString TIER4_SOMEBONUS = "Some bonus";
                        public static LocString TIER5_METALTILE = "Metal tiles";
                        public static LocString TIER6_FAST = "Fast";
                        public static LocString TIER7_DEFAULT = "Default";
                        public static LocString TIER8_GOFAST = "GO FAST";
                        public static LocString TIER9_LIGHTSPEED = "Light Speed";
                        public static LocString TIER10_RIDICULOUS = "Ridiculous";
                        public static LocString TIER11_LUDICROUS = "Ludicrous";
                    }

                    public class NUKEPANEL
                    {
                        public class NUKEBUTTON
                        {
                            public static LocString TEXT = "Nuke mod on next world load";
                            public static LocString CANCEL = "Cancel Nuking";
                        }
                    }
                }
            }
        }
    }
}
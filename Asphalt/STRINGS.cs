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
			}
		}

		public class MISC
		{
			public class TAGS
			{
				public static LocString ASPHALT_ROADSURFACEMATERIAL = "Road Surface";
			}
		}

		public class UI
		{
			public class SETTINGSDIALOG
			{
				public static LocString VERSIONLABEL = "v{number}";

				public class TITLE
				{
					public static LocString TITLETEXT = "Asphalt Settings";
				}

				public class BUTTONS
				{
					public class OK
					{
						public static LocString TEXT = "Apply";
					}

					public class CANCELBUTTON
					{
						public static LocString TEXT = "Cancel";
					}

					public class STEAMBUTTON
					{
						public static LocString TEXT = "Steam Workshop";
					}

					public class GITHUBBUTTON
					{
						public static LocString TEXT = "Github";
					}
				}

				public class SLIDERPANEL
				{
					public class SLIDER
					{
						public static LocString LABEL = "Speed: {number}x";
						public static LocString RANGELABEL = "{label}";
					}

					public static LocString TITLETEXT = "Run Speed Modifier";

					public class RANGES
					{
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
				}
			}
		}
	}
}
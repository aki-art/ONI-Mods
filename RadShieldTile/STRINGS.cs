using FUtility;
using RadShieldTile.Content;

namespace RadShieldTile
{
	public class STRINGS
	{
		public static class BUILDINGS
		{
			public static class PREFABS
			{
				public static class RADSHIELDTILE_RADSHIELDTILE
				{
					public static LocString NAME = Utils.FormatAsLink("Radiation Shield", RadShieldTileConfig.ID);
					public static LocString DESC = $"Z-Graded laminated radiation shield.";
					public static LocString EFFECT = $"Provides highly effective protection from radiation.";
				}
			}
		}

		public static class MISC
		{
			public static class TAGS
			{
				public static LocString RADSHIELDTILE_SHIELDMATERIAL = "Radiactive Shielding";
				public static LocString RADSHIELDTILE_SHIELDMATERIAL_DESC = "Materials with superb radiactive shielding ability.";
			}
		}

		public static class ELEMENTS
		{
			public static class RADSHIELDTILERADSHIELD
			{
				public static LocString NAME = Utils.FormatAsLink("Fiber Metal Laminate", RSTElements.RadShield.ToString());
				public static LocString DESC = "Layered metal materials of differing atomic numbers providing radiation.";
			}
		}

		public static class RADSHIELDTILE
		{
			public static class SETTINGS
			{
				public static class SHIELDING
				{
					public static LocString TITLE = "Shielding Factor";
					public static LocString TOOLTIP = "% Radiation shielding factor for a fully built tile of 800kg raw material.";
				}

				public static class MELTINGPOINT
				{
					public static LocString TITLE = "Melting Point";
					public static LocString TOOLTIP = "Melting point of the Radiation Shield material, in Kelvin.";
				}
			}
		}
	}
}

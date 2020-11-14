namespace TransparentAluminium
{
    public class STRINGS
    {
        public class ELEMENTS
        {
            public class TRANSPARENTALUMINUM
            {
                public static LocString NAME = "Transparent Aluminum";
                public static LocString DESC = "Aluminium oxynitride is a ceramic composed of aluminium, oxygen and nitrogen.";
            }
            public class MATERIAL_MODIFIERS
            {
                public static LocString ARMORED = global::STRINGS.UI.FormatAsLink("Armored", "HEAT");
                public class TOOLTIP
                {
                    public static LocString ARMORED = "Resistant to explosions and pressure.";
                }
            }
        }

        public class BUILDING
        {
            public class STATUSITEMS
            {
                public class UPGRADEABLESOLARPANELWATTAGE
                {
                    public static LocString NAME = "Current Wattage: {Wattage}";
                    public static LocString TOOLTIP = "This panel is generating " + global::STRINGS.UI.FormatAsPositiveRate("{Wattage}") + " of " +
                        global::STRINGS.UI.PRE_KEYWORD + "Power" + global::STRINGS.UI.PST_KEYWORD;

                }
            }
        }

        public class BUILDINGS
        {
            public class PREFABS
            {
                public class TAT_TRANSPARENTALUMINIUMTILE
                {
                    public static LocString NAME = "Transparent Aluminum Tile";
                    public static LocString DESC = $"Sturdy transparent window tiles that can withstand extreme pressures and impacts. Also make for excellent aquariums.";
                    public static LocString EFFECT = "Produces fibers from solids.\n\nDuplicants will not fabricate items unless recipes are queued.";
                }
            }
        }
    }
}
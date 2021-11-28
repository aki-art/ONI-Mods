using KUI = STRINGS.UI;


namespace BackgroundTiles
{
    public class STRINGS
    {
        public class BUILDINGS
        {
            public class PREFABS
            {
                public class BACKGROUNDTILES_WALL
                {
                    public static LocString NAME = KUI.FormatAsLink("Backwall ({originalTileName})", $"{Mod.ID}_{TileConfig.ID}Wall");
                    public static LocString DESC = "Backwalls can be used in conjunction with tiles to build airtight rooms on the surface.";
                }
            }
        }

        public class UI
        {
            public class BUILDCATEGORIES
            {
                public class BACKWALLS
                {
                    public static LocString NAME = "Backwalls";
                    public static LocString TOOLTIP = "Build tiles but on the walls.";
                }
            }
        }
    }
}

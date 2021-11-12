namespace BackgroundTiles
{
    public class ModAssets
    {
        public class Tags
        {
            // use to prevent a tile from generating a backwall version
            public static readonly Tag noBackwall = TagManager.Create("noBackwall");

            // used for already generated tiles, for replacement tags
            public static readonly Tag backWall = TagManager.Create("backWall");
        }
    }
}

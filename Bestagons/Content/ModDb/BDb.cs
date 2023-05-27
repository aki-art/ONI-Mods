namespace Bestagons.Content.ModDb
{
    public class BDb
    {
        public static HexTiles hexTiles;
        public static Currencies currencies;

        public static void Initialize()
        {
            hexTiles = new HexTiles();
            currencies = new Currencies();
        }
    }
}

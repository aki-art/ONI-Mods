namespace FUtility
{
    public class Tags
    {
        // TagManager.Create creates or fetches the already registered tag, so double "creating" for each futility is not a big deal

        public static Tag noPaint = TagManager.Create("NoPaint"); // MaterialColor mod uses this
        public static Tag noBackwall = TagManager.Create("NoBackwall"); // Background Tiles mod uses this
    }
}

using FLocalization;

namespace Bestagons
{
    public class STRINGS
    {
        public static class BESTAGONS
        {
            public static LocString UNLOCK_COST = "Unlock Cost";

            public static class CURRENCY
            {
                public static LocString BESTAGONS_METALBAR = "Metal Bar";
                public static LocString BESTAGONS_FOOD = "Food";
                public static LocString BESTAGONS_ROCK = "Rock";

            }
        }

        public static class CLUSTER_NAMES
        {
            public static class BESTAGONS
            {
                public static LocString NAME = "Bestagon";
                public static LocString DESCRIPTION = "A world with unusual forces preventing progression until proper price is paid.";
            }
        }

        public class NAMEGEN
        {
            public class WORLD
            {
                public class ROOTS
                {
                    [Note("The game will attach a suffix to one of these, like -lio or -io")]
                    public static LocString BESTAGONS = "Hexa\nCiv\nBest\nHive\nComb\nCell\nPrism\nCellur\n";
                }
            }
        }

        public static class WORLDS
        {
            public static class BESTAGONS
            {
                public static LocString NAME = "Bestagon";
                public static LocString DESCRIPTION = "A world with unusual forces preventing progression until proper price is paid.";
            }
        }
    }
}

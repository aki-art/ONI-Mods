using FUtility;
using ZipLine.Buildings.ZiplinePost;

namespace ZipLine
{
    public class STRINGS
    {
        public class BUILDINGS
        {
            public class PREFABS
            {
                public class ZIPLINE_ZIPLINEPOST
                {
                    public static LocString NAME = Utils.FormatAsLink("Zipline Anchor", ZiplinePostConfig.ID);
                    public static LocString DESC = "\"<i>Weeeeeeeeeeeeeeeeeeeeeee</i>\"";
                    public static LocString EFFECT = "Allows Duplicants to use a zipline and cross large distances to other Anchors.";
                }

                public class ZIPLINE_ZIPLINEGATEWAY
                {
                    public static LocString NAME = "Zipline Gateway";
                    public static LocString DESC = "A door for ziplines.";
                    public static LocString EFFECT = "Walls off rooms without blocking liquid and gas flow, but allowing ziplines to pass through.";
                }
            }
        }

        public class UI
        {
            public class ZIPLINE
            {
                public static LocString CONNECT = "Add connection";
                public static LocString CONNECT_TOOLTIP = "Place new anchors or connect to existing ones, connecting them with a rope and allowing traversal between them.";
                public static LocString TOO_LONG = "Zipline cannot be longer than {distance}.";
                public static LocString OBSTRUCTED = "Track must be free of obstacles.";
            }
        }
    }
}

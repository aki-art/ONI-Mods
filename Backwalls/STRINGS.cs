namespace Backwalls
{
    public class STRINGS
    {
        public class BUILDINGS
        {
            public class PREFABS
            {
                public class BACKWALL_DECORATIVEBACKWALL
                {
                    public static LocString NAME = "Decorative Backwall";
                    public static LocString DESC = "todo";
                    public static LocString EFFECT = "Does not prevent gas and liquid loss in Space.";
                }
                public class BACKWALL_SEALEDBACKWALL
                {
                    public static LocString NAME = "Sealed Backwall";
                    public static LocString DESC = "todo";
                    public static LocString EFFECT = global::STRINGS.BUILDINGS.PREFABS.EXTERIORWALL.EFFECT;
                }
            }
        }
        public class UI
        {
            public class WALLSIDESCREEN
            {
                public static LocString TITLE = "Backwall appearance";

                public class CONTENTS
                {
                    public class COPYTOGGLES
                    {
                        public class PATTERNTOGGLE
                        {
                            public class TOGGLE
                            {
                                public static LocString LABEL = "Copy Pattern";
                            }
                        }

                        public class COLORTOGGLE
                        {
                            public class TOGGLE
                            {
                                public static LocString LABEL = "Copy Color";
                            }
                        }

                        public class WARNING
                        {
                            public static LocString LABEL = "Nothing will be copied!";
                        }
                    }
                }
            }
        }
    }
}

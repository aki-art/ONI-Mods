using FUtility;
using GravitasBigStorage.Content;

namespace GravitasBigStorage
{
    public class STRINGS
    {
        public class GRAVITASBIGSTORAGE
        {
            public class SETTINGS
            {
                public class CAPACITY
                {
                    public static LocString TITLE = "Capacity (kg)";
                    public static LocString TOOLTIP = "How many corncobs a container could countain if a countainer could contain corncobs.";
                }
            }
        }

        public class BUILDINGS
        {
            public class PREFABS
            {
                public class GRAVITASBIGSTORAGE_CONTAINER
                {
                    public static LocString NAME = Utils.FormatAsLink("Gravitas Shipping Container", GravitasBigStorageConfig.ID);
                    public static LocString DESC = Utils.FormatAsLink("");

                    public class FACADES
                    {
                        public class ALIEN
                        {
                            public static LocString NAME = Utils.FormatAsLink("Alien", GravitasBigStorageConfig.ID);
                            public static LocString DESC = Utils.FormatAsLink("");
                        }

                        public class PADDED
                        {
                            public static LocString NAME = Utils.FormatAsLink("Padded", GravitasBigStorageConfig.ID);
                            public static LocString DESC = Utils.FormatAsLink("");
                        }

                        public class RED
                        {
                            public static LocString NAME = Utils.FormatAsLink("Red", GravitasBigStorageConfig.ID);
                            public static LocString DESC = Utils.FormatAsLink("");
                        }

                        public class RETRO
                        {
                            public static LocString NAME = Utils.FormatAsLink("Retro Wave", GravitasBigStorageConfig.ID);
                            public static LocString DESC = Utils.FormatAsLink("");
                        }

                        public class RUSTY
                        {
                            public static LocString NAME = Utils.FormatAsLink("Rusty", GravitasBigStorageConfig.ID);
                            public static LocString DESC = Utils.FormatAsLink("");
                        }

                        public class STARRY
                        {
                            public static LocString NAME = Utils.FormatAsLink("Starry", GravitasBigStorageConfig.ID);
                            public static LocString DESC = Utils.FormatAsLink("");
                        }
                    }
                }
            }
        }
    }
}

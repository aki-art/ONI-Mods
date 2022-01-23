namespace Terraformer
{
    internal class STRINGS
    {
        public static string FormatAsBad(string msg)
        {
            return "<b><color=#FF0000>" + msg + "</color></b>";
        }

        public static string FormatAsGood(string msg)
        {
            return "<color=#DDDDDD>" + msg + "</color>";
        }

        public class UI
        {
            public class WORLD_DESTRUCTION
            {
                public static LocString FAIL = "Cannot destroy this Asteroid for these reasons:";
                public static LocString ROCKET_NOT_PRESENT = "No rockets landed or heading here";
                public static LocString STARTING_WORLD = "Not the starting world";
                public static LocString DISCOVERED = "Planet discovered";
                public static LocString NO_CANNONS = "No Interplanetary Launchers aimed here";
                public static LocString NOT_TERRAFORMED = "Not under terraforming";

                public static LocString ALREADY_DESTROYED = "Problem: This world is already destroyed";

            }

            public class DESTROY_SCREEN
            {
                public static LocString PLZ_DO_BACKUP = "Please backup your save! \n\nDestroying a planet has irreversible changes, reloading saves will be the only way to return.";

                public class OPTIONS
                {
                    public class NOTHING
                    {
                        public static LocString TITLE = "Total annihilation";

                        public static LocString DESC = "Reduce this Asteroid into absolutely nothing.\n " +
                            "You will not be able to send rockets here, or restore this world in any capacity.\n\n" +
                            "<smallcaps>Removing the mod will NOT restore the world.</smallcaps>";
                    }

                    public class POI
                    {
                        public static LocString TITLE = "Crack open hidden resources";

                        public static LocString DESC = "Blow away most of this world, but leave behind a Space POI that can be farmed.\n" +
                            "The Space POI will be randomly chosen from all vanilla options.\n" +
                            "You will not be able to send rockets here, or restore this world in any capacity.\n\n" +
                            "<smallcaps>Removing the mod will NOT restore the world, and the POI will stay.</smallcaps>";
                    }
                }
            }
        }
    }
}

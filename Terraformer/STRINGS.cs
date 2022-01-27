namespace Terraformer
{
    public class STRINGS
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

            public class DIALOGS
            {
                public class DETONATIONCONFIRMDIALOG
                {
                    public class TITLE
                    {
                        public static LocString TITLETEXT = "Detonation Confirmation";
                    }

                    //public static LocString WARNINGDESC = "Please backup your save! \n\nDestroying a planet has irreversible changes, reloading saves will be the only way to return.";

                    public static LocString WARNINGLABEL = "Warning!";
                    public static LocString WARNINGDESC = "Once the big red button is pressed, planetoid {Name} will be permanently destroyed.";

                    public class CANCELBUTTON
                    {
                        public static LocString TEXT = "Cancel";
                    }

                    public class SAVE
                    {
                        public static LocString TEXT = "Save Backup";
                    }

                    public class OPTIONS
                    {
                        public class DEFAULT
                        {
                            public static LocString TITLE = "Default";

                            public static LocString DESC = "Safely remove all resources, buildings, geysers and entities from this asteroid. \n " +
                                "This planet will be ready to be terraformed at a later point.\n\n" +
                                "<smallcaps>Removing the Terraformer mod will restore the world's icon and inventory, but will <b>not</b> bring back lost resources.</smallcaps>";
                        }

                        public class NOTHING
                        {
                            public static LocString TITLE = "Total annihilation";

                            public static LocString DESC = "Reduce this Asteroid into absolutely nothing.\n " +
                                "You will not be able to send rockets here, or restore this world in any capacity. (No terraforming either).\n\n" +
                                "<smallcaps>Removing the Terraformer mod will <b>NOT</b> restore the world.</smallcaps>";
                        }

                        public class POI
                        {
                            public static LocString TITLE = "Crack open";

                            public static LocString DESC = "Blow away most of this world, but leave behind a Space POI that can be farmed.\n" +
                                "The Space POI will be randomly chosen from all vanilla options.\n" +
                                "You will not be able to restore this world as an asteroid. (No terraforming either).\n\n" +
                                "<smallcaps>Removing the Terraformer mod will <b>NOT</b> restore the world, and the spawned POI will remain.</smallcaps>";
                        }
                    }
                }

                public class RESULTDIALOG
                {
                    public class TITLE
                    {
                        public static LocString LABEL = "Detonation";
                    }

                    public static LocString SELECTLABEL = "Select what happens with {Name}";

                    public class CHOICESELECTOR
                    {
                        public static LocString DESCRIPTION = "Reduce this Asteroid into absolutely nothing. " +
                            "You will not be able to send rockets here, or restore this world in any capacity.\n" +
                            "<smallcaps> Removing the mod will NOT restore the world.</smallcaps>";

                        public static LocString LABEL = "Total Annihilation";
                    }

                    public class RESULTPREVIEW
                    {
                        public static LocString INTITLE = "{Name}";
                        public static LocString OUTTITLE = "Nothing";
                    }

                }
            }
        }
    }
}

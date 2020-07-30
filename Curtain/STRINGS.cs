namespace Curtain
{
    class STRINGS
    {
        public class BUILDINGS
        {
            public class PREFABS
            {
                public class AC_PLASTICCURTAIN
                {
                    public static LocString NAME = "Plastic Curtain";
                    public static LocString DESC = $"{ NAME }s use centrifugal force to spin various materials into fibers.";
                    public static LocString EFFECT = "Produces fibers from solids.\n\nDuplicants will not fabricate items unless recipes are queued.";
                }
            }
        }

        public class UI
        {
            public class UISIDESCREENS
            {
                public class CURTAINSIDESCREEN
                {
                    public static LocString TITLE = "Curtain test sidescreen";
                }
            }
        }

        public class BUILDING
        {
            public class STATUSITEMS
            {
                public class CHANGECURTAINCONTROLSTATE
                {
                    public static LocString NAME = "Pending Curtain State Change: {CurrentState}";
                    public static LocString TOOLTIP = "Waiting for a Duplicant to change control state";
                }
            }
        }
    }
}

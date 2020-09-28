using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bomb
{
    public class STRINGS
    {
        public class BUILDINGS
        {
            public class DAMAGESOURCES
            {
                public static LocString EXPLOSION = "Explosion";
            }

            public class PREFABS
            {
                public class AB_BOMB
                {
                    public static LocString NAME = "Bomb";
                    public static LocString DESCRIPTION = "";
                }

                public class AB_BIGBOMB
                {
                    public static LocString NAME = "Regular Frost Burger";
                    public static LocString DESCRIPTION = "Explodes in an enormous area, damaging the asteroid greatly.\n\n<smallcaps>BIG BADA BOOM</smallcaps>";
                }
            }
        }

        public class UI
        {
            public class GAMEOBJECTEFFECTS
            {
                public class DAMAGE_POPS
                {
                    public static LocString EXPLOSION = "Explosion Damage";
                }
            }

            public class BIGBOMB
            {
                public class SIDESCREEN
                {
                    public static LocString TITLE = "Detonate";
                    public static LocString MESSAGE = "";
                    public static LocString BUTTON = "Detonate";
                }
            }
        }

    }
}

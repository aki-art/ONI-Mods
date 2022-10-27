using DecorPackA.Buildings.MoodLamp;

namespace DecorPackA
{
    public class ModDb
    {
        public static LampVariants lampVariants;

        public static void Initialize()
        {
            lampVariants = new LampVariants();
        }
    }
}

using FUtility;
using HarmonyLib;

namespace TwelveCyclesOfChristmas.Lang
{
    class LocalizationPatches
    {
        [HarmonyPatch(typeof(Localization), "Initialize")]
        public class Localization_Initialize_Patch
        {
            public static void Postfix()
            {
                Loc.Translate(typeof(STRINGS));
            }
        }
    }
}

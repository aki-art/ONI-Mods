using FUtility;
using HarmonyLib;

namespace Twitchery.Patches
{
    public class LocalizationPatch
    {
        [HarmonyPatch(typeof(Localization), "Initialize")]
        public class Localization_Initialize_Patch
        {
            public static void Postfix()
            {
                Loc.Translate(typeof(STRINGS), true);

                // duplicate entry, but the game expects both keys to exist
                Strings.Add("STRINGS.ITEMS.FOOD.JELLO.NAME", STRINGS.ELEMENTS.JELLO.NAME);
                Strings.Add("STRINGS.ITEMS.FOOD.JELLO.DESC", STRINGS.ELEMENTS.JELLO.DESC);
            }
        }
    }
}

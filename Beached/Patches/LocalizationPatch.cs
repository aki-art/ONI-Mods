using FUtility;
using HarmonyLib;

namespace Beached.Patches
{
    public class LocalizationPatch
    {
        [HarmonyPatch(typeof(Localization), "Initialize")]
        public class Localization_Initialize_Patch
        {
            public static void Postfix()
            {
                Loc.Translate(typeof(STRINGS), true);

                Log.Debuglog("LOC HERE");
                Strings.Add("STRINGS.CREATURES.FAMILY_PLURAL.BEACHEDSNAILSPECIES", STRINGS.CREATURES.FAMILY_PLURAL.BEACHEDSNAILSPECIES);
            }
        }
    }
}

using FUtility;
using Harmony;

namespace ModularStorage
{
    class StringsPatches
    {
        [HarmonyPatch(typeof(Localization), "Initialize")]
        class Localization_Initialize_Patch
        {
            public static void Postfix()
            {
                Loc.Translate(typeof(STRINGS));
            }
        }
    }
}

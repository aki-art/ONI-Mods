using FUtility;
using Harmony;

namespace TransparentAluminium
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

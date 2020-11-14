using FUtility;
using Harmony;

namespace Asphalt
{
    public class StringsPatches
    {
        [HarmonyPatch(typeof(Localization), "Initialize")]
        public class Localization_Initialize_Patch
        {
            public static void Postfix()
            {
                Loc.Translate(typeof(STRINGS), true);
                Loc.AddOverride("STRINGS.BUILDINGS.PREFABS.OILREFINERY.EFFECT", STRINGS.BUILDINGS.PREFABS.AT_OILREFINERYALT.EFFECT);
            }
        }
    }
}

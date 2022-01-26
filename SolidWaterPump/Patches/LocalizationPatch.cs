using FUtility;
using HarmonyLib;

namespace SolidWaterPump.Patches
{
    public class LocalizationPatch
    {
        [HarmonyPatch(typeof(Localization), "Initialize")]
        public class Localization_Initialize_Patch
        {
            public static void Postfix()
            {
                Loc.Translate(typeof(STRINGS), true);

                Strings.Add("STRINGS.BUILDINGS.PREFABS.SOLIDWATERPUMP.DESC", global::STRINGS.BUILDINGS.PREFABS.LIQUIDPUMPINGSTATION.DESC);
                Strings.Add("STRINGS.BUILDINGS.PREFABS.SOLIDWATERPUMP.EFFECT", global::STRINGS.BUILDINGS.PREFABS.LIQUIDPUMPINGSTATION.EFFECT);
            }
        }
    }
}

using FUtility;
using HarmonyLib;

namespace SchwartzRocketEngine.Patches
{
    public class LocalizationPatch
    {
        [HarmonyPatch(typeof(Localization), "Initialize")]
        public class Localization_Initialize_Patch
        {
            public static void Postfix()
            {
                Loc.Translate(typeof(STRINGS), true);

                // This door mimics the original roket interior door, so the same strings are used
                //Strings.Add("STRINGS.BUILDINGS.PREFABS.SCHWARTZROCKETENGINE_FCLUSTERCRAFTINTERIORDOOR.NAME", global::STRINGS.BUILDINGS.PREFABS.CLUSTERCRAFTINTERIORDOOR.NAME);
                Strings.Add("STRINGS.BUILDINGS.PREFABS.SCHWARTZROCKETENGINE_FCLUSTERCRAFTINTERIORDOOR.NAME", "Its this one");
                Strings.Add("STRINGS.BUILDINGS.PREFABS.SCHWARTZROCKETENGINE_FCLUSTERCRAFTINTERIORDOOR.EFFECT", global::STRINGS.BUILDINGS.PREFABS.CLUSTERCRAFTINTERIORDOOR.EFFECT);
                Strings.Add("STRINGS.BUILDINGS.PREFABS.SCHWARTZROCKETENGINE_FCLUSTERCRAFTINTERIORDOOR.DESC", global::STRINGS.BUILDINGS.PREFABS.CLUSTERCRAFTINTERIORDOOR.DESC);
            }
        }
    }
}

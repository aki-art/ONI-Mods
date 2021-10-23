using FUtility;
using HarmonyLib;

namespace DecorPackA.Patches
{
    class LocalizationPatch
    {
        [HarmonyPatch(typeof(Localization), "Initialize")]
        public class Localization_Initialize_Patch
        {
            public static void Postfix()
            {
                Loc.Translate(typeof(STRINGS));

                // Add stained glass variants
                foreach (var tile in ModAssets.tiles)
                {
                    string key = $"STRINGS.BUILDINGS.PREFABS.{tile.Value.ToString().ToUpperInvariant()}";
                    Strings.Add(key + ".NAME", STRINGS.BUILDINGS.PREFABS.DP_DEFAULTSTAINEDGLASSTILE.NAME);
                    Strings.Add(key + ".DESC", STRINGS.BUILDINGS.PREFABS.DP_DEFAULTSTAINEDGLASSTILE.DESC);
                    Strings.Add(key + ".EFFECT", STRINGS.BUILDINGS.PREFABS.DP_DEFAULTSTAINEDGLASSTILE.EFFECT);
                }
            }
        }
    }
}

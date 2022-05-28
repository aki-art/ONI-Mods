using DecorPackA.Buildings.StainedGlassTile;
using FUtility;
using HarmonyLib;
using static DecorPackA.STRINGS.BUILDINGS.PREFABS.DECORPACKA_DEFAULTSTAINEDGLASSTILE;

namespace DecorPackA.Patches
{
    public class LocalizationPatch
    {
        [HarmonyPatch(typeof(Localization), "Initialize")]
        public class Localization_Initialize_Patch
        {
            public static void Postfix()
            {
                Loc.Translate(typeof(STRINGS), true);

                // Add stained glass variants
                foreach (var tile in StainedGlassTiles.tileInfos)
                {
                    var key = $"STRINGS.BUILDINGS.PREFABS.{tile.ID.ToString().ToUpperInvariant()}";
                    Strings.Add(key + ".NAME", NAME);
                    Strings.Add(key + ".DESC", DESC);
                    Strings.Add(key + ".EFFECT", EFFECT);
                }
            }
        }
    }
}

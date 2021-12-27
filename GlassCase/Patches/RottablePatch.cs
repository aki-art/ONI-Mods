using GlassCase.Buildings;
using HarmonyLib;

namespace GlassCase.Patches
{
    public class RottablePatch
    {
        [HarmonyPatch(typeof(Rottable), nameof(Rottable.AtmosphereQuality))]
        public static class Rottable_AtmosphereQuality_Patch
        {
            public static void Postfix(IRottable rottable, ref Rottable.RotAtmosphereQuality __result)
            {
                if(rottable.gameObject.GetComponent<Encased>() is object)
                {
                    __result = Rottable.RotAtmosphereQuality.Sterilizing;
                }
            }
        }
    }
}
